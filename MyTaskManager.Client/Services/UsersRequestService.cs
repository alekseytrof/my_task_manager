using MyTaskManager.Client.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;

namespace MyTaskManager.Client.Services
{
    public class UsersRequestService
    {
        private const string HOST = "http://localhost:5052/api/";

        public AuthToken GetToken(string userName, string password)
        {
            string url = HOST + "account/token";

            var responseStr = GetDataByUrl(url, userName, password);
            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(responseStr);

            return token;
        }

        private string GetDataByUrl(string url, string userName, string password)
        {
            string result = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            if (userName != null && password != null)
            {
                string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(userName + ":" + password));
                request.Headers.Add("Authorization", "Basic " + encoded);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string responseStr = reader.ReadToEnd();
                    result = responseStr;
                }
            }
            return result;
        }
    }
}
