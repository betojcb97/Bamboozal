using Bamboo.DTO;
using Bamboo.Models;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Bamboo.Data;

namespace Bamboo.Services
{
    public class RegisterUser
    {
        private BambooContext db;
        private IMapper _mapper;
        private UserManager<User> _userManager;

        public RegisterUser(BambooContext context, IMapper mapper, UserManager<User> userManager)
        {
            db = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        internal async Task RegisterAsync(AddUserDto userDto)
        {
            User user = _mapper.Map<User>(userDto);
            IdentityResult result = await _userManager.CreateAsync(user, userDto.userPassword);
            if (!result.Succeeded)
            {
                throw new ApplicationException("Error registering user");
            }
        }
    }
}
