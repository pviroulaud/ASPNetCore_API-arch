using System.Net.Http;
using System.Text.Json;
using System.Text;
using fileDTO;

namespace documentAPI.SyncServices
{
    public class HttpLocalClient : IHttpLocalClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpLocalClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<bool> checkUserPermit(int userId, int permitId)
        {
            //var httpContent = new StringContent(
            //    JsonSerializer.Serialize(plat),
            //Encoding.UTF8,
            //"application/json");

            string url = $"{_configuration["userService"]}/userPermit/check/{userId}/{permitId}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string respContent = await response.Content.ReadAsStringAsync();
                try
                {
                    return Convert.ToBoolean(respContent);
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<userFileDTO?> getFile(int fileId)
        {
            //var httpContent = new StringContent(
            //    JsonSerializer.Serialize(plat),
            //Encoding.UTF8,
            //"application/json");

            string url = $"{_configuration["storageService"]}/{fileId}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string respContent = await response.Content.ReadAsStringAsync();
                    userFileDTO uFile= JsonSerializer.Deserialize<userFileDTO>(respContent)?? new userFileDTO();
                
                    return uFile;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
