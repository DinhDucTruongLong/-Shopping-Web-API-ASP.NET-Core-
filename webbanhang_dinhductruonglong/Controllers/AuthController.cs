using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using webbanhang_dinhductruonglong.DTO_Models;
using webbanhang_dinhductruonglong.Models;
using BCrypt.Net;


namespace webbanhang_dinhductruonglong.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;


        public AuthController(UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration configuration, ApplicationDbContext context, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if(await _context.Users.AnyAsync(u => u.Username == user.Username))
                return BadRequest("Username already exists");
            // sử dụng hàm để bắm password mã hóa ngẫu nhiên
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == login.Username);
            // hằm băm mật khẩu
            if (user == null || !BCrypt.Net.BCrypt.Verify(login.PasswordHash, user.PasswordHash)) 
                return Unauthorized("Invalid username or password");

            // Tạo token
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
           


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Lưu token vào DB
            _context.Tokens.Add(new Token
            {
                AccessToken = tokenString,
                Expiration = token.ValidTo,
                UserId = user.Id
            });
            await _context.SaveChangesAsync();

            return Ok(new
            {
                token = tokenString,
                expiration = token.ValidTo,
                role = user.Role
            });
        }
    }

}
