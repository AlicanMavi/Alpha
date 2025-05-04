using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class RegisterViewModel
{
    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "Full Name is required")]
    [RegularExpression(@"^[A-Za-zåäöÅÄÖ\s\-]{2,50}$", ErrorMessage = "Only letters, spaces and hyphens allowed")]
    public string FullName { get; set; } = null!;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password")]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Minimum 8 characters, one uppercase, one number")]
    public string Password { get; set; } = null!;

    [Display(Name = "Confirm Password")]
    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}


