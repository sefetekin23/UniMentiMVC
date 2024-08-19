using System.ComponentModel.DataAnnotations;

namespace UniMenti.ViewModels;

public class RegisterVM
{
    [Required(ErrorMessage = "Name is required.")]
    public string? Name { get; set; }
    [Required(ErrorMessage = "UserName is required.")]
    public string? UserName { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [Compare("Password",ErrorMessage ="Passwords don't match.")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }


}
