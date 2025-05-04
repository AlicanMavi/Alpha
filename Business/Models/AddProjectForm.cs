using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class AddProjectForm
{
    [Display(Name = "Project Name", Prompt = "Enter project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Project Name cannot be empty")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Enter client name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Client Name cannot be empty")]
    public string ClientName { get; set; } = null!;


    [Display(Name = "Description", Prompt = "Type something")]
    [DataType(DataType.MultilineText)]
    public string? Description { get; set; }


    [Display(Name = "Start Date", Prompt = "Enter start date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public string StartDate { get; set; } = null!;


    [Display(Name = "End Date", Prompt = "Enter end date")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public string EndDate { get; set; } = null!;


    [Display(Name = "Budget", Prompt = "0")]
    [DataType(DataType.Currency)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Budget cannot be empty")]
    public string Budget { get; set; } = null!;
}