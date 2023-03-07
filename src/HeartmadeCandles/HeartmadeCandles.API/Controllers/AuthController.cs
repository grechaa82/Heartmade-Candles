using HeartmadeCandles.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HeartmadeCandles.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly string _securityKey;
        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _securityKey = configuration.GetSection("SecurityKey").Value;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var isVetify = await _authService.IsVerifyAsync(email, password);
            if (!isVetify)
            {
                return BadRequest();
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(_securityKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, email),
                }),
                Expires = DateTime.UtcNow.AddDays(28),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return Ok(token);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(string nickName, string email, string password)
        {
            var isRegister = await _authService.RegisterUserAsync(nickName, email, password);

            if (!isRegister)
            {
                return BadRequest();
            };

            return Ok();
        }
    }
}