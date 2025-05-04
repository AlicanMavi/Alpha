using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class LoginViewModel
{
    [Display(Name = "Email")]
    [DataType(DataType.EmailAddress)]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}
