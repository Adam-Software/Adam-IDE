using AdamController.WebApi.Client.v1;
using System.Net.Http.Headers;
using System.Text;

namespace AdamController.WebApi.Client
{
    public class ApiClient
    {
        private static readonly HttpClient mClient = new();
        private static readonly MediaTypeWithQualityHeaderValue mMediaTypeHeader = new("application/json");
        private const string cDefaultApiPath = "api/";

        static ApiClient()
        {
            mClient.DefaultRequestHeaders.Accept.Clear();
            mClient.DefaultRequestHeaders.Add("X-Version", "1");
            mClient.DefaultRequestHeaders.Accept.Add(mMediaTypeHeader);
        }

        internal static void SetAuthenticationHeaderValue(string login, string password)
        {
            AuthenticationHeaderValue defaultAutentificationHeader = new("Basic", 
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}")));

            mClient.DefaultRequestHeaders.Authorization = defaultAutentificationHeader;
        }

        internal static void SetUri(Uri uri)
        {
            mClient.BaseAddress = uri;
        }

        internal static void SetUri(string uri)
        {
            mClient.BaseAddress = new($"{uri}");
        }

        internal static void SetUri(string ip, int port)
        {
            mClient.BaseAddress = new($"http://{ip}:{port}");
        }

        internal static async Task<HttpResponseMessage?> Put(string path)
        {
            HttpResponseMessage? responseMessage = await mClient.PutAsync($"{cDefaultApiPath}{path}", null);
            responseMessage.EnsureSuccessStatusCode();
            
            return responseMessage;
        }

        internal static async Task<HttpResponseMessage?> Get(string path)
        {
            HttpResponseMessage? responseMessage = await mClient.GetAsync($"{cDefaultApiPath}{path}");
            responseMessage.EnsureSuccessStatusCode();

            return responseMessage;
        }

        internal static async Task<HttpResponseMessage?> Post(string path, string command)
        {
            StringContent content = new(command);
            var result = await mClient.PostAsync($"{cDefaultApiPath}{path}", content);
            return result;
        }

        internal static async Task<HttpResponseMessage?> Post(string path, v1.RequestModel.PythonCommand command)
        {
            var result = await mClient.PostAsJsonAsync($"{cDefaultApiPath}{path}", command);
            return result;
        }

        internal static async Task<HttpResponseMessage?> Post(string path, string filePath, string name, string fileName)
        {

            using (var content = new MultipartFormDataContent("Upload--------------------" + DateTime.Now.ToString()))
            {
                var fileStreamContent = new StreamContent(File.OpenRead(filePath));
                content.Add(fileStreamContent, name, fileName);
                var result = await mClient.PostAsync($"{cDefaultApiPath}{path}", content);

                return result;
            }

        }
    }
}