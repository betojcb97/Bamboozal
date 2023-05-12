using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Bamboo.Data;
using Microsoft.AspNetCore.Mvc;
using Azure.Core;
using Castle.Core.Smtp;
using System.Text.Encodings.Web;
using System;

namespace Bamboo.Services
{
    public class UserService : Controller
    {
        private BambooContext db;
        private IMapper _mapper;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private TokenService _tokenService;

        public UserService(BambooContext context, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, TokenService tokenService)
        {
            db = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(AddUserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            IdentityResult result = await _userManager.CreateAsync(user, userDto.userPassword);
            if (!result.Succeeded)
            {
                throw new ApplicationException("Error registering user");
            }
        }

        public async Task<string> LoginAsync(LoginUserDto userDto)
        {
            var result = await _signInManager.PasswordSignInAsync(userDto.userName, userDto.userPassword, false, false);
            if (!result.Succeeded) { throw new ApplicationException("Login error for user"); }

            var user = _signInManager.UserManager.Users.FirstOrDefault(u => u.NormalizedUserName.Equals(userDto.userName.ToUpper()));

            var token = _tokenService.GenerateToken(user);
            return token;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> LogoffAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }
}
