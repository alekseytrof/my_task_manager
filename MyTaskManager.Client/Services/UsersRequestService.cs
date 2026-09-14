using MyTaskManager.Client.Models;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace MyTaskManager.Client.Services
{
    public class UsersRequestService
    {
        private const string HOST = "http://localhost:5052/api/";
        private string _usersConrollerUrl = HOST + "users";

        public AuthToken GetToken(string userName, string password)
        {
            string url = HOST + "account/token";

            var responseStr = GetDataByUrl(HttpMethod.Post, url, null, userName, password);
            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(responseStr);

            return token;
        }

        public HttpStatusCode CreateUser(AuthToken token, UserDto user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendDataByUrl(HttpMethod.Post, _usersConrollerUrl, token, userJson);
            return result;
        }

        public List<UserDto> GetAllUsers(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, _usersConrollerUrl, token);
            List<UserDto> users = JsonConvert.DeserializeObject<List<UserDto>>(response);
            return users;
        }

        public HttpStatusCode DeleteUser(AuthToken token, int userId)
        {
            var result = DeleteDataByUrl(_usersConrollerUrl + $"/{userId}", token);
            return result;
        }

        public HttpStatusCode CreateMultiplyUser(AuthToken token, List<UserDto> users)
        {
            string userJson = JsonConvert.SerializeObject(users);
            var result = SendDataByUrl(HttpMethod.Post, _usersConrollerUrl + "/all", token, userJson);
            return result;
        }

        public HttpStatusCode UpdateUser(AuthToken token, UserDto user)
        {
            string userJson = JsonConvert.SerializeObject(user);
            var result = SendDataByUrl(HttpMethod.Patch, _usersConrollerUrl + $"/{user.Id}", token, userJson);
            return result;
        }

        private string GetDataByUrl(HttpMethod method, string url, AuthToken token, string userName = null, string password = null)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = method.Method;
            if (userName != null && password != null)
            {
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);
            }
            else if (token != null)
            {
                request.Headers.Add("Authorization", "Bearer " + token.access_token);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                string responseStr = reader.ReadToEnd();
                result = responseStr;
            }

            return result;
        }

        private HttpStatusCode SendDataByUrl(HttpMethod method, string url, AuthToken token, string data)
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

        private HttpStatusCode DeleteDataByUrl(string url, AuthToken token)
        {
            var result = new HttpResponseMessage();
            var client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.access_token);
            result = client.DeleteAsync(url).Result;

            return result.StatusCode;
        }
    }
}
