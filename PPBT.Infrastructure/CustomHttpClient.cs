using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PPBT.Infrastructure
{
    public class CustomHttpClient
    {

        private HttpClient HttpClient { get; set; }

        public CustomHttpClient()
        {
            HttpClient = new HttpClient();

        }

        /// <summary>
        /// Set Authorization Header
        /// </summary>
        /// <param name="authenticationHeaderValue"></param>
        public void SetAuthorization(AuthenticationHeaderValue authenticationHeaderValue)
        {
            HttpClient.DefaultRequestHeaders.Authorization = authenticationHeaderValue;
        }

        /// <summary>
        /// Get Content
        /// </summary>
        public async Task<T> GetAsync<T>(string uri)
        {
            //Make Request
            var response = await HttpClient.GetAsync(uri);

            //Get Json
            string responseBody = await response.Content.ReadAsStringAsync();

            //Validate Success
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"GET:{Environment.NewLine} {uri} {Environment.NewLine} Failed: {Environment.NewLine} {responseBody}");
            }

            //Return Repository List
            return JsonConvert.DeserializeObject<T>(responseBody);
        }


        /// <summary>
        /// Post Content
        /// </summary>
        public async Task<T> PostAsync<T>(string uri, string json) 
        {
            // Build Json
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Post build
            var response = await HttpClient.PostAsync(uri, content);

            //Get Json
            string responseBody = await response.Content.ReadAsStringAsync();

            //Validate Success
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"POST: {Environment.NewLine} {uri} {Environment.NewLine} with Content: {Environment.NewLine} {json} {Environment.NewLine} Failed:{Environment.NewLine} {responseBody}");
            }

            //Return Response
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// Patch Content
        /// </summary>
        public async Task<T> PatchAsync<T>(string uri, string json)
        {
            // Build Json
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Post build
            var response = await HttpClient.PatchAsync(uri, content);

            //Get Json
            string responseBody = await response.Content.ReadAsStringAsync();

            //Validate Success
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"PATCH: {Environment.NewLine} {uri} {Environment.NewLine} with Content: {Environment.NewLine} {json} {Environment.NewLine} Failed:{Environment.NewLine} {responseBody}");
            }

            //Return Response
            return JsonConvert.DeserializeObject<T>(responseBody);
        }
    }
}
