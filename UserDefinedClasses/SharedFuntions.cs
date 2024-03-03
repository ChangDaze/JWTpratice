using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JWTpratice.UserDefinedClasses
{
    public class SharedFuntions
    {
        private readonly IConfiguration _configuration;
        public SharedFuntions(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateJWTToken(string account, string password)
        {
            if(account!= _configuration["DefaultAccount"] || password != _configuration["DefaultPassword"])
            {
                return null!;
            }
            List<Claim> claims = new List<Claim>()
            {
                new Claim("account", account)
            };            

            //1字元8Byte，Sha256要32字元
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));            
            var jwt = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddSeconds(30),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return token;
        }
    }
}
