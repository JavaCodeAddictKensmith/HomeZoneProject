using System.ComponentModel.DataAnnotations;

namespace HomeZone.Services.AuthAPI.Models.Dto
{
    public class SetPinDto
    {
        //[Required]
        //public string userId;
        [Required]
        public string Pin { get; set; }
        

       

    }
}
