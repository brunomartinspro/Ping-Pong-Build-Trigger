using Newtonsoft.Json;
using PPBT.DataTransferObject;
using PPBT.DataTransferObject.AzureDevOps;
using PPBT.DataTransferObject.AzureDevOps.Request;
using PPBT.DataTransferObject.Options;
using PPBT.Infrastructure;
using PPBT.Infrastructure.IO;
using PPBT.Logic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PPBT.Logic.Platform
{
    public class AzureDevOpsLogic : IPlatform
    {
        private readonly OptionsDto _optionsDto;
        private readonly CustomHttpClient _httpClient;
        private readonly FileBuildOrderLogic _fileBuildOrderLogic;

        private int buildOrder { get; set; }
        private int counter { get; set; }


        public AzureDevOpsLogic(OptionsDto optionsDto)
        {
            _optionsDto = optionsDto;
            _httpClient = new CustomHttpClient();
            _fileBuildOrderLogic = new FileBuildOrderLogic(optionsDto);

            //Define PAT token
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_optionsDto.ApiKey}"));

            //Define Authorization
            _httpClient.SetAuthorization(new AuthenticationHeaderValue("Basic", authToken));

        }

        /// <summary>
        /// Let's play ping pong!
        /// </summary>
        /// <returns></returns>
        public async Task<int> PingPong()
        {
            int result = -1;

            try
            {
                var buildOrderList = _fileBuildOrderLogic.OpenFile();

                var buildList = await GetBuilds();

                var updatedBuildList = await TriggerBuilds(buildOrderList, buildList);

                buildOrderList = updatedBuildList.Select(x => new BuildOrderDto { Name = x.name, Order = x.BuildSuccessOrder }).ToList();

                _fileBuildOrderLogic.CreateOrModifyFile(buildOrderList);

                result = 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: { ex.ToString() }");
            }

            return result;
        }

        /// <summary>
        /// List all builds
        /// </summary>
        public async Task<List<BuildDto>> GetBuilds()
        {
            //Setup resources
            string uri = $"{ _optionsDto.SourceUri }/_apis/build/definitions?name=*{ _optionsDto.ProjectName }*";

            //Make Request
            var response = await _httpClient.GetAsync<BuildRequestDto>(uri);

            //Return Build List
            return response?.BuildList;
        }

        /// <summary>
        /// Get Build
        /// </summary>
        /// <param name="buildId"></param>
        /// <returns></returns>
        public async Task<BuildDto> GetBuild(int buildId)
        {
            var uri = $"{ _optionsDto.SourceUri }/_apis/build/builds/{buildId}";

            //Make Request
            var response = await _httpClient.GetAsync<BuildDto>(uri);

            //Return Repository List
            return response;
        }

        /// <summary>
        /// Post Build
        /// </summary>
        /// <param name="build"></param>
        /// <returns></returns>
        public async Task<BuildDto> PostBuild(BuildDto build)
        {
            DefinitionRequestDto definitionRequestDto = new DefinitionRequestDto
            {
                Definition = build,
                SourceBranch = _optionsDto.SourceBranch
            };

            string json = JsonConvert.SerializeObject(definitionRequestDto);

            // Setup resources
            string uri = $"{ _optionsDto.SourceUri }/_apis/build/builds?api-version=5.0";

            // Post build
            var response = await _httpClient.PostAsync<BuildDto>(uri, json);

            //Return Repository List
            return response;
        }

        /// <summary>
        /// Order list 
        /// </summary>
        /// <returns></returns>
        public List<BuildDto> OrderBuildListByBuildOrder(List<BuildOrderDto> buildOrderDtoList, List<BuildDto> buildList)
        {
            List<BuildDto> orderedBuildList = new List<BuildDto>();

            foreach (var buildOrder in buildOrderDtoList)
            {
                var build = buildList.Where(x => x.name == buildOrder.Name).FirstOrDefault();

                if (build != null)
                {
                    build.BuildSuccessOrder = buildOrder.Order;
                    orderedBuildList.Add(build);
                }
            }

            var missingOrdered = buildList.Except(orderedBuildList).ToList();

            orderedBuildList.AddRange(missingOrdered);

            orderedBuildList = orderedBuildList.OrderBy(x => x.BuildSuccessOrder).ToList();

            return orderedBuildList;
        }

        /// <summary>
        /// Trigger all builds
        /// </summary>
        public async Task<List<BuildDto>> TriggerBuilds(List<BuildOrderDto> buildOrderList, List<BuildDto> buildList)
        {
            bool keepLooping = true;
            List<BuildDto> buildCompletedList = new List<BuildDto>();

            buildList = OrderBuildListByBuildOrder(buildOrderList, buildList).ToList();

            foreach (var build in buildList.Where(x => x.BuildSuccessOrder > 0).ToList())
            {
                await TriggerBuild(build);

                if (build.BuildSuccessOrder > 0)
                {
                    buildCompletedList.Add(build);
                }
            }

            var brokenBuildList = buildList.Where(x => x.BuildSuccessOrder < 1).ToList();

            while (brokenBuildList.Any() && keepLooping)
            {
                counter = 0;

                Parallel.ForEach(brokenBuildList, async (build) =>
                {
                    if (build.BuildSuccessOrder < 1)
                    {
                        await TriggerBuild(build);
                    }
                });

                while ((counter != brokenBuildList.Where(x => x.BuildSuccessOrder < 1).ToList().Count) && keepLooping)
                {

                    if ((_optionsDto.InfiniteCycles.HasValue && !_optionsDto.InfiniteCycles.Value) || !_optionsDto.InfiniteCycles.HasValue)
                    {
                        keepLooping = !brokenBuildList.Any(x => x.BuildFailedCounter >= (_optionsDto.MaxErrorCycles ?? 10));

                        if (!keepLooping)
                        {
                            break;
                        }
                    }
                    Thread.Sleep(5000);
                }
            }

            buildCompletedList.AddRange(brokenBuildList);

            return buildCompletedList;
        }

        /// <summary>
        /// Trigger a build
        /// </summary>
        /// <param name="build"></param>
        /// <returns></returns>
        private async Task TriggerBuild(BuildDto build)
        {
            var createdBuild = await PostBuild(build);

            while (createdBuild.status != "completed")
            {
                Thread.Sleep(5000);

                createdBuild = await GetBuild(createdBuild.id);
            }

            if (createdBuild.result == "succeeded")
            {
                buildOrder++;
                build.BuildSuccessOrder = buildOrder;
            }
            else
            {
                counter++;
                build.BuildFailedCounter++;
                build.BuildSuccessOrder = 0;
            }
        }
    }
}
