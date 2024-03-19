using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Follow_Up_Manager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Follow_Up_Manager.interfaces;

public interface IAuthService
{
    Task<bool> SignUp(SignUpViewModel signUpViewModel);
}
