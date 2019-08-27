using CommandLine;
using System;

namespace PPBT.DataTransferObject
{
    public class OptionsDto
    {
        [Option('m', "mode", Required = true, HelpText = "Mode, AzureDevOps, github etc")]
        public string Mode { get; set; }

        [Option('s', "source", Required = true, HelpText = "Project Uri")]
        public string SourceUri { get; set; }

        [Option('k', "api-key", Required = true, HelpText = "Api Key")]
        public string ApiKey { get; set; }

        [Option('l', "last-known-file", Required = false, HelpText = "Last Known File")]
        public string LastKnownFile { get; set; }

        [Option('p', "project-name", Required = true, HelpText = "Project Name")]
        public string ProjectName { get; set; }

        [Option('b', "source-branch", Required = true, HelpText = "Source Branch")]
        public string SourceBranch { get; set; }

        [Option('c', "cycles", Required = false, HelpText = "Maximum of Error Cycles Allowed")]
        public int? MaxErrorCycles { get; set; }

        [Option('i', "infinite-cycles", Required = false, HelpText = "Runs forever until everything succeeds")]
        public bool? InfiniteCycles { get; set; }

    }
}
