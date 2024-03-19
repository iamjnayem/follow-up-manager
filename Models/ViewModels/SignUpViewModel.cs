using System.ComponentModel.DataAnnotations;

namespace Follow_Up_Manager.Models.ViewModels;

public class SignUpViewModel
{
    [Required(ErrorMessage = "* First Name is Required")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "* Last Name is Required")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "* Email is Required")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "* Password is Required")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "* Password is Required")]
    public string RePassword{get; set;} = null!;


    public string? EmailValidationError{get;set;}

}
