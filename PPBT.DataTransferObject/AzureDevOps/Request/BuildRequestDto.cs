using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPBT.DataTransferObject.AzureDevOps.Request
{  
    public class BuildRequestDto
    {
        [JsonProperty("value")]
        public List<BuildDto> BuildList { get; set; }

        public int count { get; set; }
    }

}
