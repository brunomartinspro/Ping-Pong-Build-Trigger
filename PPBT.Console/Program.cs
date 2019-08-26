using CommandLine;
using PPBT.DataTransferObject;
using System;
using System.Linq;

namespace PPBT.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<OptionsDto>(args).WithParsed(option =>
            {
                switch (option.Mode)
                {
                    case "AzureDevOps":
                        {
                            break;
                        }
                }
            });
        }
    }
}
