using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPBT.DataTransferObject.AzureDevOps.Request
{
    public class RepositoryRequestDto
    {
        [JsonProperty("value")]
        public List<RepositoryDto> RepositoryList { get; set; }
        public int count { get; set; }
    }
}
