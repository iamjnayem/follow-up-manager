using System.ComponentModel.DataAnnotations;

namespace Follow_Up_Manager.Models.ViewModels;

public class SignUpViewModel
{
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string RePassword{get; set;} = null!;

}
