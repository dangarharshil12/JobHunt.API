﻿using JobHunt.Services.AuthAPI.Models;
using JobHunt.Services.AuthAPI.Models.Dto;
using JobHunt.Services.AuthAPI.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            _response = new ResponseDto();
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            // Checking whether user already exists or not
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _response.IsSuccess = false;
                _response.Message = "User with this email already exist. Please Login";
            }
            else
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
                        _response.Result = user;
                    }
                    else
                    {
                        if (identityResult.Errors.Any())
                        {
                            foreach (var error in identityResult.Errors)
                            {
                                _response.Message = error.Description;
                            }
                            _response.IsSuccess = false;
                        }
                    }
                }
                else
                {
                    if (identityResult.Errors.Any())
                    {
                        foreach (var error in identityResult.Errors)
                        {
                            _response.Message = error.Description;
                        }
                        _response.IsSuccess = false;
                    }
                }
            }
            return Ok(_response);
        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginRequstDto request)
        {
            if (request.Email.IsNullOrEmpty() || request.Password.IsNullOrEmpty())
            {
                _response.IsSuccess = false;
                _response.Message = "Incomplete Credentials (Either Email or Password or both are Empty)";
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "User does not exist. Please Register";
                }
                else
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var identityResult = await _userManager.ResetPasswordAsync(user, token, request.Password);
                    if (identityResult.Succeeded)
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
