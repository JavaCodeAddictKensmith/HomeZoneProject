using HomeZone.Services.AuthAPI.Data;
using HomeZone.Services.AuthAPI.Models;
using HomeZone.Services.AuthAPI.Models.Dto;
using HomeZone.Services.AuthAPI.Service.IService;
using HomeZone.Services.AuthAPI.Utility;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace HomeZone.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;

        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //if user was found , Generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDto userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            // Validate password strength
            string password = registrationRequestDto.Password;
            if (!Regex.IsMatch(password, @"[A-Z]") ||          // at least one uppercase
                !Regex.IsMatch(password, @"[a-z]") ||          // at least one lowercase
                !Regex.IsMatch(password, @"[\W_]"))            // at least one special character
            {
                return "Password must contain at least one uppercase letter, one lowercase letter, and one special character.";
            }

            // Validate role before user creation
            string roleName = registrationRequestDto.Role?.ToUpper();
            if (roleName != SD.RoleAdmin && roleName != SD.RoleCustomer)
            {
                return "Role must be either 'Admin' or 'Customer'.";
            }

            try
            {
                // Ensure role exists in the system
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        return "Failed to create role.";
                    }
                }

                // Create the user
                ApplicationUser user = new()
                {
                    UserName = registrationRequestDto.Email,
                    Email = registrationRequestDto.Email,
                    NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                    Name = registrationRequestDto.Name,
                    PhoneNumber = registrationRequestDto.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return result.Errors.FirstOrDefault()?.Description ?? "User creation failed.";
                }

                // Assign role to user
                var roleAssignResult = await _userManager.AddToRoleAsync(user, roleName);
                if (!roleAssignResult.Succeeded)
                {
                    return "User was created but role assignment failed.";
                }

                return ""; // success
            }
            catch (Exception ex)
            {
                // You should log the exception in real-world scenarios
                return $"Error encountered: {ex.Message}";
            }
        }


        //public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        //{

        //    // Password validation
        //    string password = registrationRequestDto.Password;
        //    if (!Regex.IsMatch(password, @"[A-Z]") ||          // at least one uppercase
        //        !Regex.IsMatch(password, @"[a-z]") ||          // at least one lowercase
        //        !Regex.IsMatch(password, @"[\W_]"))            // at least one non-alphanumeric
        //    {
        //        return "Password must contain at least one uppercase letter, one lowercase letter, and one special character.";
        //    }

        //    ApplicationUser user = new()
        //    {
        //        UserName = registrationRequestDto.Email,
        //        Email = registrationRequestDto.Email,
        //        NormalizedEmail = registrationRequestDto.Email.ToUpper(),
        //        Name = registrationRequestDto.Name,
        //        PhoneNumber = registrationRequestDto.PhoneNumber
        //    };

        //    try
        //    {


        //        // Validate and assign role
        //        string roleName = registrationRequestDto.Role?.ToUpper();
        //        if (roleName != SD.RoleAdmin && roleName != SD.RoleCustomer)
        //        {
        //            //roleName = SD.RoleCustomer; // default role
        //            return "the role must either Admin or Customer, no other role is allowed";
        //        }

        //        if (!await _roleManager.RoleExistsAsync(roleName))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole(roleName));
        //        }

        //        await _userManager.AddToRoleAsync(user, roleName);
        //        var result = await _userManager.CreateAsync(user, password);
        //        if (!result.Succeeded)
        //        {
        //            return result.Errors.FirstOrDefault()?.Description ?? "User creation failed.";
        //        }
        //        return "";
        //    }
        //    catch (Exception)
        //    {
        //        return "Error Encountered";
        //    }
        //}






        //public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        //{
        //    ApplicationUser user = new()
        //    {
        //        UserName = registrationRequestDto.Email,
        //        Email = registrationRequestDto.Email,
        //        NormalizedEmail = registrationRequestDto.Email.ToUpper(),
        //        Name = registrationRequestDto.Name,
        //        PhoneNumber = registrationRequestDto.PhoneNumber
        //    };

        //    try
        //    {
        //        var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
        //        if (result.Succeeded)
        //        {
        //            var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

        //            UserDto userDto = new()
        //            {
        //                Email = userToReturn.Email,
        //                ID = userToReturn.Id,
        //                Name = userToReturn.Name,
        //                PhoneNumber = userToReturn.PhoneNumber,

        //            };

        //            return "";

        //        }
        //        else
        //        {
        //            return result.Errors.FirstOrDefault().Description;
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return "Error Encountered";
        //}
    }
}
