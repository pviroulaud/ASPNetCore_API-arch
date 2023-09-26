using System.Net.Http;
using System.Text.Json;
using System.Text;
using fileDTO;
using System.Net.Http.Headers;

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

            string url = $"{_configuration["storageService"]}/userFile/{fileId}";

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

        public async Task<int?> PostWithFile(syncDocumentInfoDTO msg, IFormFile file)
        {
            HttpResponseMessage response;

            var formData = new MultipartFormDataContent();

            byte[] arr= new byte[file.Length];
            file.OpenReadStream().Read(arr, 0, arr.Length);
            MemoryStream ms = new MemoryStream(arr);

            HttpContent c = new StreamContent(ms);
            c.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

            formData.Add(c, file.FileName, file.FileName);
            formData.Add(new StringContent(msg.requireSign.ToString()), "requireSign");
            formData.Add(new StringContent(msg.documentId.ToString()), "documentId");
            formData.Add(new StringContent(msg.userId.ToString()), "userId");
            formData.Add(new StringContent(msg.cipher.ToString()), "cipher");

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string url = $"{_configuration["storageService"]}/userFile";

            response = _httpClient.PostAsync(url, formData).Result;

            
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string respContent = await response.Content.ReadAsStringAsync();
                    int userFileId = Convert.ToInt32(respContent);


                    return userFileId;
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
