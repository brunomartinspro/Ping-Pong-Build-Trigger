using System;
using System.Collections.Generic;
using System.Text;

namespace PPBT.DataTransferObject.AzureDevOps
{
    public class RepositoryDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string defaultBranch { get; set; }
        public long size { get; set; }
        public string remoteUrl { get; set; }
        public string sshUrl { get; set; }
    }
}