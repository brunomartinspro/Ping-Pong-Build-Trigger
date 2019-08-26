using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PPBT.Infrastructure.Git
{
    public class GitWrapper
    {
        private readonly PowerShellWrapper _powerShellWrapper;

        public GitWrapper()
        {
            _powerShellWrapper = new PowerShellWrapper();
        }

        private async Task RunCommand(string command)
        {
           await _powerShellWrapper.InvokeScript(command);
        }


    }
}
