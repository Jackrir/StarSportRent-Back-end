using BusinessLogicLayer.API.Responses;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Models.Entyties;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class AuthService
    {
        private readonly IRepository repository;

        public AuthService(IRepository repository)
        {
            this.repository = repository;
        }

        public string GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();
            byte[] byteHash = CSP.ComputeHash(bytes);
            string hash = string.Empty;
            foreach (byte b in byteHash)
            {
                hash += string.Format("{0:x2}", b);
            }
            return hash;
        }

        public async Task<AuthentificationResult> GenerateAuthenticationResult(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("w6D4a77ZRrlv9rTCPnYGXq2sCxqL9vLnqgktceiUxwgrQZVwfK6775lytEck8Pyk");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("username", user.Name)
                }),
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse("24:00:00")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            string refreshToken = GetHashString(user.UserId + DateTime.UtcNow.ToString() + user.Name);
            Token oldToken = await this.repository.GetAsync<Token>(true, x => x.UserId == user.UserId);

            oldToken.JWT = tokenHandler.WriteToken(token);
            oldToken.RefreshToken = refreshToken;
            oldToken.Time = DateTime.UtcNow;

            await this.repository.UpdateAsync<Token>(oldToken);

            return new AuthentificationResult
            {
                Token = oldToken.JWT,
                RefreshToken = oldToken.RefreshToken
            };
        }

        public async Task<(bool,AuthentificationResult)> RefreshToken(string rT)
        {
            Token oldToken = await this.repository.GetAsync<Token>(true, x => x.RefreshToken == rT);
            if(oldToken != null)
            {
                User user = await this.repository.GetAsync<User>(true, x => x.UserId == oldToken.UserId);
                AuthentificationResult result = await GenerateAuthenticationResult(user);
                return (true, result);
            } 
            else
            {
                return (false, new AuthentificationResult());
            }
        }

        public async Task<User> GetUser(string rt)
        {
            Token token = await this.repository.GetAsync<Token>(true, x => x.JWT == rt);
            return await this.repository.GetAsync<User>(true, x => x.UserId == token.UserId);
        }

        public async Task<(bool,string)> CheckToken(string token)
        {
            Token data = await this.repository.GetAsync<Token>(true, x => x.JWT == token);
            if(data != null)
            {
                if (DateTime.UtcNow.Subtract(data.Time).TotalMinutes > 10000000)
                {
                    return (false,"");
                }
                else
                {
                    User user = await this.repository.GetAsync<User>(true, x => x.UserId == data.UserId);
                    return (true,user.Role);
                }
            }
            else
            {
                return (false, "");
            }
        }
    }
}
