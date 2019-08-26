using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPBT.Infrastructure.Git
{
    public class PowerShellWrapper
    {
        public async Task<PSDataCollection<PSObject>> InvokeScript(string script)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript(script);

                var outputCollection = new PSDataCollection<PSObject>();

                outputCollection.DataAdded += OutputCollection_DataAdded;
                ps.Streams.Error.DataAdded += Error_DataAdded;

                var result = await Task.Run(() => ps.BeginInvoke<PSObject, PSObject>(null, outputCollection));

                while(!result.IsCompleted)
                {
                    Console.WriteLine("PowerShell: Executing Script..");
                    Thread.Sleep(100);
                }

                foreach(var item in ps.Streams.Error)
                {
                    Console.WriteLine($"PowerShell Error: {item.ToString()}");

                }

                Console.WriteLine($"PowerShell: Finished.");

                return outputCollection;
            }
        }

        private void OutputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            Console.WriteLine("PowerShell: Object Added to output.");
        }

        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            Console.WriteLine("PowerShell: Error");
        }
    }
}
