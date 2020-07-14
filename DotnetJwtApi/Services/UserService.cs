using DotnetJwtApi.Entities;
using DotnetJwtApi.Helpers;
using DotnetJwtApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DotnetJwtApi.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);

        IEnumerable<User> GetAll();
        User Get(int id);
    }

    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>
        {
            new User{ Id = 1, FirstName = "Test", LastName = "Test", UserName="test", Password = "test"},
            new User{ Id = 2, FirstName = "Test2", LastName = "Test2", UserName="test2", Password = "test2"},
            new User{ Id = 3, FirstName = "Test3", LastName = "Test3", UserName="test3", Password = "test3"}
        };

        private readonly AppSettings _appSettings;


        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);

            if (user == null)
                return null;

            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public User Get(int id)
        {
            var user = _users.SingleOrDefault(u => u.Id == id);

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        private string generateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}