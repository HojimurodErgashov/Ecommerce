using Ecommerce.V1.Commons.Exceptions;
using Ecommerce.V1.Commons.TokenGenerator.Interface;
using Ecommerce.V1.Roles.Entities;
using Ecommerce.V1.Users.Entities;
using Ecommerce.V1.Users.Interfaces;
using Ecommerce.V1.Users.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Security.Claims;

namespace Ecommerce.V1.Users.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;//
        private readonly SignInManager<User> _signInManager;//
        private readonly RoleManager<Role> _roleManager;//
        private readonly ITokenService _tokenService;
        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager = null , ITokenService tokenService = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="EcommerceException"></exception>

        public async ValueTask<string> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email); 
            
            if (user == null)
            {
                throw new EcommerceException(400, "User not found");
            }

            

            // Check the password
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPassword)
            {
                throw new EcommerceException(401, "Email or password is incorrect");
            }

            // Sign in the user
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var claims =  new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.MobilePhone , user.PhoneNumber)
                    };

                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                Console.WriteLine($"Welcome {user.UserName}");

                return _tokenService.WriteToken(claims);
            }

           

            if (!result.Succeeded)
            {
                throw new EcommerceException(401, "Email or password is incorrect");
            }

            //_tokenGenerator.WriteToken(user); // write user token generator 
            return "";
        }


        /// <summary>
        /// Registrate
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="EcommerceException"></exception>
        public async ValueTask<UserModel> Registration(RegisterModel user)
        {
            User newUser = new User()
            {
                UserName = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };

            var registerUser = await _userManager.CreateAsync(newUser , user.Password);

            if (!registerUser.Succeeded)
            {
                throw new EcommerceException(400, string.Join(", ", registerUser.Errors.Select(x => x.Description)));
            }

            var adminRole = await _roleManager.FindByNameAsync("Admin");

            if (adminRole == null)
            {
                // Create "Admin" role if it doesn't exist
                adminRole = new Role()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };
                await _roleManager.CreateAsync(adminRole);
            }

            await _userManager.AddToRoleAsync(newUser, "Admin");

            return new UserModel().MapFromEntity(newUser);

        }
    }
}
