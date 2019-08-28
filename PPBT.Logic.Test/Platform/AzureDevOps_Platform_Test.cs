using PPBT.DataTransferObject;
using PPBT.DataTransferObject.Options;
using PPBT.Infrastructure.IO;
using PPBT.Logic.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PPBT.Logic.Test.Platform
{
    public class AzureDevOps_Platform_Test
    {
        public readonly OptionsDto _options;
        public readonly AzureDevOpsLogic _azureDevOps;
        public readonly FileBuildOrderLogic _fileBuildOrderLogic;

        public AzureDevOps_Platform_Test()
        {
            _options = new OptionsDto
            {
                Mode = "AzureDevOps",
                ApiKey = "cqjk3s2pkv5x5o57feaw6j2qs4id2thfsw3w544hiqgt347gfoja",
                SourceUri = "http://kamina.azuredevops.local/DefaultCollection/Kamina",
                ProjectName = "Tengen Toppa Gurren Lagann",
                SourceBranch = "develop",
                LastKnownFile = "Input/BuildOrder.json",
                MaxErrorCycles = 6
            };

            _azureDevOps = new AzureDevOpsLogic(_options);
            _fileBuildOrderLogic = new FileBuildOrderLogic(_options);
        }

        [Fact]
        public async Task GetBuilds()
        {
            await _azureDevOps.GetBuilds();
        }

        [Fact]
        public async Task DeleteAllBuilds()
        {
            await _azureDevOps.DeleteAllBuilds();
        }


        [Fact]
        public async Task TriggerBuilds()
        {
            var buildOrderList = _fileBuildOrderLogic.OpenFile();

            var buildList = await _azureDevOps.GetBuilds();

            var updatedBuildList = await _azureDevOps.TriggerBuilds(buildOrderList, buildList);

            buildOrderList = updatedBuildList.Select(x => new BuildOrderDto { Name = x.name, Order = x.BuildSuccessOrder }).ToList();

            _fileBuildOrderLogic.CreateOrModifyFile(buildOrderList);
        }
    }
}
