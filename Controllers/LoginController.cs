using JWTpratice.UserDefinedClasses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTpratice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SharedFuntions _sharedFuntions;
        public LoginController(IConfiguration configuration, SharedFuntions sharedFuntions)
        {
            _configuration = configuration;
            _sharedFuntions = sharedFuntions;
        }
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login(LoginParam param)
        {
            string jwtToken = _sharedFuntions.CreateJWTToken(param.account, param.password);

            if (jwtToken is null) return new OkObjectResult(new { Code = "9999", Message = "帳密錯誤" });

            return new OkObjectResult(new
            {
                Code = "0000",
                JWTToken = jwtToken
            });
        }
    }
}
