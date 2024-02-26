using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var user = new ApplicationUser
            {
                FullName = request.FirstName + " " + request.LastName,
                UserName = request.Email?.Trim(),
                Email = request.Email?.Trim(),
                PhoneNumber = request.PhoneNumber?.Trim(),
            };

            var identityResult = await _userManager.CreateAsync(user, request.Password);
            if (identityResult.Succeeded)
            {
                identityResult = await _userManager.AddToRoleAsync(user, request.Role);
                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return ValidationProblem(ModelState);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequstDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
                if (checkPassword)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var response = new LoginResponseDto()
                    {
                        UserId = user.Id,
                        FullName = user.FullName,
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = "token",
                    };

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Invalid Login Credentials");

            return ValidationProblem(ModelState);
        } 
    }
}
