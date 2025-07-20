using System.ComponentModel.DataAnnotations;

namespace HomeZone.Services.AuthAPI.Models.Dto
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [Required]
        public string? Role { get; set; }
    }
}
