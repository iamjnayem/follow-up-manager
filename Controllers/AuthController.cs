using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.services;


namespace Follow_Up_Manager.Controllers;

public class AuthController : Controller
{ 
    private readonly IAuthService _authService;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public AuthController(IAuthService authService, IHttpContextAccessor httpContextAccessor)
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
    }

    public IActionResult SignUp()
    {
        return View("SignUp");
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
    {
       
        if(!ModelState.IsValid)
        {
            return View(signUpViewModel);
        }


        var isRegistered = await _authService.SignUp(signUpViewModel);
    
        if(!isRegistered)
        {

            if(signUpViewModel.EmailValidationError != "")
            {
                TempData["EmailValidationError"] = signUpViewModel.EmailValidationError;
            }

            return View(signUpViewModel);

        }

        // return View();

        return View("LogIn");
    }



    public IActionResult LogIn()
    {
        return View("LogIn");
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
    {

        if(!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var isLoggedIn = await _authService.LogIn(loginViewModel,_httpContextAccessor);

        if(!isLoggedIn)
        {
            if(loginViewModel.EmailErrorMessage != "")
            {
                TempData["EmailErrorMessage"] = loginViewModel.EmailErrorMessage;
            }

            if(loginViewModel.PasswordErrorMessage != "")
            {
                TempData["PasswordErrorMessage"] = loginViewModel.PasswordErrorMessage;
            }
            return View(loginViewModel);
        }

        return RedirectToAction("Index", "Home");

    }


    public async Task<IActionResult> LogOut()
    {
      await _authService.LogOut(_httpContextAccessor);  
      return View("LogIn");
    }

  
}
