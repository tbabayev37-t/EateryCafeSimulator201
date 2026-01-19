using System.ComponentModel.DataAnnotations;

namespace EateryCafeSimulator201.ViewModels.UserViewModel
{
    public class RegisterVM
    {
        [Required,MaxLength(64),MinLength(3)]
        public string Fullname {  get; set; }=string.Empty;
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, MaxLength(64), MinLength(3)]
        public string Username { get; set; } = string.Empty;
        [Required,DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
