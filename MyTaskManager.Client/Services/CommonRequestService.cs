using DryIoc;
using MyTaskManager.Client.Models;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MyTaskManager.Client.Services
{
    public abstract class CommonRequestService
    {
        public const string HOST = "http://localhost:5052/api/";
        private CommonViewService _viewService;

        public CommonRequestService()
        {
            _viewService = new CommonViewService();
        }

        protected string GetDataByUrl(HttpMethod method, string url, AuthToken token, string userName = null, string password = null, Dictionary<string, string> parameters = null)
        {
            WebClient client = new WebClient();
            if (userName != null && password != null)
            {
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + password));
                client.Headers.Add("Authorization", "Basic " + encoded);
            }
            else if (token != null)
            {
                client.Headers.Add("Authorization", "Bearer " + token.access_token);
            }

            if (parameters != null)
            {
                foreach (var key in parameters.Keys)
                {
                    client.QueryString.Add(key, parameters[key]);
                }
            }

            byte[] data = Array.Empty<byte>();

            if (method == HttpMethod.Post)
            {
                data = client.UploadValues(url, method.Method, client.QueryString);
            }
            else if (method == HttpMethod.Get)
            {
                try
                {
                    data = client.DownloadData(url);
                }
                catch (Exception ex)
                {
                    _viewService.ShowMessage(ex.Message);
                }
            }

            return UnicodeEncoding.UTF8.GetString(data);
        }

        protected HttpStatusCode SendDataByUrl(HttpMethod method, string url, AuthToken token, string data)
        {
            var result = new HttpResponseMessage();
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);

            var content = new StringContent(data, Encoding.UTF8, "application/json");

            if (method == HttpMethod.Post)
            {
                result = client.PostAsync(url, content).Result;
            }
            if (method == HttpMethod.Patch)
            {
                result = client.PatchAsync(url, content).Result;
            }

            return result.StatusCode;
        }

        protected HttpStatusCode DeleteDataByUrl(string url, AuthToken token)
        {
            var result = new HttpResponseMessage();
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
            result = client.DeleteAsync(url).Result;

            return result.StatusCode;
        }
    }
}
