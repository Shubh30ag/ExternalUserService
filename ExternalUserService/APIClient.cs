using ExternalUserService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;

namespace ExternalUserService
{
    public class APIClient : IAPIClient
    {
        private readonly IConfiguration _configuration;
        private readonly string baseUrl;


        public APIClient(IConfiguration configuration)
        {
            _configuration = configuration;
            baseUrl = _configuration.GetSection("BsaeUrl").Value;
            baseUrl = baseUrl.TrimEnd('/');
        }

        public async virtual Task<User> GetUserByIdAsync(int userid)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{baseUrl}/users/{userid}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("User Not Found");
            }

            else if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API error: {response.StatusCode}");
            }

            var result = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<User>(result);
            }

            catch (JsonException ex)
            {
                throw new Exception("Failed to parse response");
            }

            finally
            {
                httpClient.Dispose();
               
            }
        }

        public async virtual Task<PagedResponse> GetAllUserByPageAsync(int page)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{baseUrl}/users?page={page}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"API error: {response.StatusCode}");
            }

            var result = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonConvert.DeserializeObject<PagedResponse>(result);
            }

            catch (JsonException ex)
            {
                throw new Exception("Failed to parse response");
            }

            finally
            {
                httpClient.Dispose();

            }
        }
    }
}
