using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PPBT.DataTransferObject.AzureDevOps.Request
{  
    public class DefinitionRequestDto
    {
        public BuildDto Definition { get; set; }
        public string SourceBranch { get; set; }
    }

}
