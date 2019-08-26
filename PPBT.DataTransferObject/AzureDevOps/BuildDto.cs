using System;
using System.Collections.Generic;
using System.Text;

namespace PPBT.DataTransferObject.AzureDevOps
{
    public class BuildDto
    {
        public string quality { get; set; }
        public object[] drafts { get; set; }
        public QueueDto queue { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string uri { get; set; }
        public string path { get; set; }
        public string status { get; set; }
        public string result { get; set; }

        public string type { get; set; }
        public string queueStatus { get; set; }
        public int revision { get; set; }
        public DateTime createdDate { get; set; }
        public int BuildSuccessOrder { get; set; }
        public int BuildFailedCounter { get; set; }
    }
}
