using System.ComponentModel.DataAnnotations;

namespace Follow_Up_Manager.Models.ViewModels;

public class LoginViewModel
{

    public long? Id { get; set; }

    [Required(ErrorMessage = "* Email is Required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "* Password is Required")]
    public string Password {get; set;}

    public string? EmailErrorMessage{get;set;}
    public string? PasswordErrorMessage{get;set;}
}
