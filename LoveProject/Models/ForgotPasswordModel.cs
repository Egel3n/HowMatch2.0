using LoveProject.Identity;
using System.ComponentModel.DataAnnotations;

namespace LoveProject.Models
{
    public class ForgotPasswordModel
    {

        public string UserId { get; set; }
        public string Token { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        
       
    }
}
