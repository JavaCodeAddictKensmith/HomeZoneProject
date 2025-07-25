using HomeZone.Services.AuthAPI.Models;
using HomeZone.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Org.BouncyCastle.Crypto.Generators;
using System.Text.RegularExpressions;

namespace HomeZone.Services.AuthAPI.Service
{
    public class TransactionPinService : ITransactionPinService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionPinService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> SetTransactionPinAsync(string userId, string pin)
        {
            if (!Regex.IsMatch(pin, @"^\d{4}$"))
                return "PIN must be exactly 4 digits.";

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return "User not found.";

            var userTransPin = user.TransactionPinHash;
            if (userTransPin != null)
            {
                return "User has pin which already exists";
            }

            user.TransactionPinHash = BCrypt.Net.BCrypt.HashPassword(pin);
            var result = await _userManager.UpdateAsync(user);


            return result.Succeeded ? "" : "Failed to save PIN.";
        }

        public async Task<bool> ValidateTransactionPinAsync(string userId, string pin)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || string.IsNullOrWhiteSpace(user.TransactionPinHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(pin, user.TransactionPinHash);
        }
    }

}
