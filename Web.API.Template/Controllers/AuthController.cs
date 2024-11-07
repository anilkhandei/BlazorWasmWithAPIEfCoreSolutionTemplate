using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web.API.Template.Data.Authentication;

namespace Web.API.Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;
        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _config = configuration;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailId = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                };

                var result = await _userManager.CreateAsync(user,model.Password);

                if (result.Succeeded)
                {

                    user.TwoFactorEnabled = false;
                    await _userManager.UpdateAsync(user);
                    return Ok("User registered successfully");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest("invalid model");

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        var claims = new[]
                        {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JwtSettings:Key").Value ?? string.Empty));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: _config.GetSection("JwtSettings:Issuer").Value ?? string.Empty,
                            audience: _config.GetSection("JwtSettings:Audience").Value ?? string.Empty,
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: creds
                        );

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                }

                return Unauthorized();
            }

            return BadRequest("Invalid model");

        }
    }
}
