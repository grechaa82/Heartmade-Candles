using HeartmadeCandles.API.Contracts.Requests;
using HeartmadeCandles.Auth.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HeartmadeCandles.API.Controllers.Auth
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _userService;
        private readonly JwtOptions _jwtOptions;

        public AuthController(IOptions<JwtOptions> jwtOptions, IAuthService userService)
        {
            _userService = userService;
            _jwtOptions = jwtOptions.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var isValidUser = _userService.IsValidUser(request.Login, request.Password);

            if (isValidUser)
            {
                var token = GenerateJwtToken();

                SetTokenInCookie(token);

                Console.WriteLine(token.ToString());

                return Ok(new { token });
            }

            return Unauthorized();
        }

        private void SetTokenInCookie(string token)
        {
            Response.Cookies.Append("token", token, new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(1),
                Path = "/",
                HttpOnly = true,
                Secure = true
            });
        }

        private string GenerateJwtToken()
        {
            /* var secretKey = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "Admin")
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token)*/
            ;

            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpiryInMinutes),
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
