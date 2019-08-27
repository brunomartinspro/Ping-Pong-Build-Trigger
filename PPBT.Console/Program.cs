using CommandLine;
using PPBT.DataTransferObject;
using PPBT.Infrastructure.Git;
using PPBT.Logic.Platform;
using System;
using System.Linq;

namespace PPBT.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            int output = 0;

            Parser.Default.ParseArguments<OptionsDto>(args).WithParsed(option =>
            {
                switch (option.Mode)
                {
                    case "AzureDevOps":
                        {                           
                            AzureDevOpsLogic azureDevOpsLogic = new AzureDevOpsLogic(option);

                            output = azureDevOpsLogic.PingPong().Result;

                            break;

                        }
                }
            });

            return output;
        }
    }
}
