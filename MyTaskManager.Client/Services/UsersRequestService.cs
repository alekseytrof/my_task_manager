using MyTaskManager.Client.Models;
using MyTaskManager.Common.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace MyTaskManager.Client.Services
{
    public class UsersRequestService : CommonRequestService
    {
        private string _usersConrollerUrl = HOST + "users";

        public AuthToken GetToken(string userName, string password)
        {
            string url = HOST + "account/token";

            var responseStr = GetDataByUrl(HttpMethod.Post, url, null, userName, password);
            AuthToken token = JsonConvert.DeserializeObject<AuthToken>(responseStr);

            return token;
        }

        public UserDto GetCurrentUser(AuthToken token)
        {
            string response = GetDataByUrl(HttpMethod.Get, HOST + "account/info", token);
            UserDto user = JsonConvert.DeserializeObject<UserDto>(response);
            return user;
        }

        public UserDto GetUserById(AuthToken token, int? userId)
        {
            string response = GetDataByUrl(HttpMethod.Get, _usersConrollerUrl + $"/{userId}", token);
            UserDto user = JsonConvert.DeserializeObject<UserDto>(response);
            return user;
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

        public HttpStatusCode CreateMultiplyUsers(AuthToken token, List<UserDto> users)
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
    }
}
