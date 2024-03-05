using Azure.Core;
using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Models.Dto;
using JobHunt.Services.AuthAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JobHunt.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        protected ResponseDto _response;

        public AuthController(UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _response = new();
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
                    _response.Message = "User Registration Successful";
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        _response.IsSuccess = false;
                        _response.Result = ModelState;
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
                    _response.IsSuccess = false;
                    _response.Result = ModelState;
                }
            }
            return Ok(_response);
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
                    var token = _tokenRepository.CreateJwtToken(user, roles.ToList());
                    var response = new LoginResponseDto()
                    {
                        UserId = user.Id,
                        FullName = user.FullName,
                        Email = request.Email,
                        Roles = roles.ToList(),
                        Token = token,
                    };

                    _response.Result = response;
                    _response.Message = "Login Successful";

                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Message = "Invalid Login Credentials";
                }
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Message = "User Does Not Exists. Please Register";

            return Ok(_response);
        }

        [HttpPost]
        [Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginRequstDto request)
        {
            if(request.Email == null)
            {
                _response.IsSuccess= false;
                _response.Message = "Empty Email";
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if(user == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "User does'nt exist. Please Register";
                }
                else
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var identityResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                    if(identityResult.Succeeded)
                    {
                        _response.Message = "Password Reset Successfull";
                    }
                    else
                    {
                        _response.IsSuccess = false;
                        _response.Message = "Password Reset Failed";
                    }
                }
            }
            return Ok(_response);
        }
    }
}
