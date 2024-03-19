using System.ComponentModel.DataAnnotations;

namespace Follow_Up_Manager.Models.ViewModels;

public class SignUpViewModel
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

}
