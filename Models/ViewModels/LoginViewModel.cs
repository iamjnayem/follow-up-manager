using System.ComponentModel.DataAnnotations;

namespace Follow_Up_Manager.Models.ViewModels;

public class LoginViewModel
{

    [Required]
    public string Email { get; set; }

    [Required]
    public string Password {get; set;}
}
