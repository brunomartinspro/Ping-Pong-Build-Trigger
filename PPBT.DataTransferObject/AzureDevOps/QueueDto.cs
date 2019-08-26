using System;
using System.Collections.Generic;
using System.Text;

namespace PPBT.DataTransferObject.AzureDevOps
{
    public class QueueDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public PoolDto pool { get; set; }
    }
}
