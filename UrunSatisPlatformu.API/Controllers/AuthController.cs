using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrunSatisPlatformu.Entity.DTOs;

namespace UrunSatisPlatformu.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        // Identity kütüphanesinin hazır kullanıcı yöneticisini çağırıyoruz
        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        // 1. KAYIT OLMA METODU
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var user = new IdentityUser { UserName = dto.Username, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                return Ok(new { message = "Kullanıcı başarıyla oluşturuldu." });

            return BadRequest(result.Errors); // Şifre çok kısaysa vs. hata döner
        }

        // 2. GİRİŞ YAPMA VE TOKEN ÜRETME METODU
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            // Kullanıcı var mı ve şifresi doğru mu kontrolü
            if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                // Şifre doğruysa kullanıcının kimliğini oluşturuyoruz
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                // appsettings.json'daki gizli şifremizi alıyoruz
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

                // Bilekliği (Token) hazırlıyoruz
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddHours(3), // Token 3 saat geçerli olsun
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                // Token'ı kullanıcıya yolluyoruz
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            return Unauthorized(new { message = "Kullanıcı adı veya şifre hatalı!" });
        }
    }
}