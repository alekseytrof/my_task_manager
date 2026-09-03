using MyTaskManager.Api.Models.Abstractions;
using MyTaskManager.Api.Models.Data;
using MyTaskManager.Common.Models;
using System.Security.Claims;
using System.Text;

namespace MyTaskManager.Api.Models.Services
{
    public class UsersService : AbstractionService, ICommonService<UserDto>
    {
        private readonly ApplicationContext _db;

        public UsersService(ApplicationContext db)
        {
            _db = db;
        }

        public Tuple<string, string> GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            string userName = "";
            string userPass = "";
            string authHeader = request.Headers["Authorization"].ToString();

            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserPass = authHeader.Replace("Basic ", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");

                string[] namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserPass)).Split(':');
                userName = namePassArray[0];
                userPass = namePassArray[1];
            }
            return new Tuple<string, string>(userName, userPass);
        }

        public User GetUser(string login, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login && u.Password == password);
            return user;
        }

        public User GetUser(string login)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login);
            return user;
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User currentUser = GetUser(username, password);
            if (currentUser != null)
            {
                currentUser.LastLoginDate = DateTime.Now;
                _db.Users.Update(currentUser);
                _db.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUser.Status.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        public bool Create(UserDto model)
        {
            return DoAction(delegate
            {
                User newUser = new User(model.FirstName, model.LastName, model.Email,
                    model.Password, model.Status, model.Phone, model.Photo);
                _db.Users.Add(newUser);
                _db.SaveChanges();
            });
        }

        public bool Update(int id, UserDto model)
        {
            User userForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
            if (userForUpdate != null)
            {
                return DoAction(delegate
                {
                    userForUpdate.Email = model.Email;
                    userForUpdate.Password = model.Password;
                    userForUpdate.Status = model.Status;
                    userForUpdate.Phone = model.Phone;
                    userForUpdate.FirstName = model.FirstName;
                    userForUpdate.LastName = model.LastName;
                    userForUpdate.Photo = model.Photo;

                    _db.Users.Update(userForUpdate);
                    _db.SaveChanges();
                });
            }
            return false;
        }

        public bool Delete(int id)
        {
            User userForDelete = _db.Users.FirstOrDefault(u => u.Id == id);
            if (userForDelete != null)
            {
                return DoAction(delegate
                {
                    _db.Users.Remove(userForDelete);
                    _db.SaveChanges();
                });
            }
            return false;
        }

        public bool CreateMultiplyUsers(List<UserDto> users)
        {
            return DoAction(delegate
            {
                var newUsers = users.Select(u => new User(u));
                _db.Users.AddRange(newUsers);
                _db.SaveChangesAsync();
            });
        }
    }
}
