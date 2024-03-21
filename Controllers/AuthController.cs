using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.services;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Follow_Up_Manager.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    //  public INotyfService _notifyService { get; }
    public INotyfService _notifyService { get; }
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(
        IAuthService authService,
        IHttpContextAccessor httpContextAccessor,
        INotyfService notifyService
    )
    {
        _authService = authService;
        _httpContextAccessor = httpContextAccessor;
        _notifyService = notifyService;
    }

    public IActionResult SignUp()
    {
        return View("SignUp");
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel signUpViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Invalid Input");
                return View(signUpViewModel);
            }

            var isRegistered = await _authService.SignUp(signUpViewModel);

            if (!isRegistered)
            {
                if (signUpViewModel.EmailValidationError != "")
                {
                    _notifyService.Error("This email is already used");
                    TempData["EmailValidationError"] = signUpViewModel.EmailValidationError;
                }
                return View(signUpViewModel);
            }

            // return View();

            _notifyService.Success("Registered Successfully. Login with Credentials");

            return View("LogIn");
        }
        catch (Exception e)
        {
            _notifyService.Error("Something went wrong");
            return RedirectToAction("SignUp");
        }
    }

    public IActionResult LogIn()
    {
        return View("LogIn");
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LoginViewModel loginViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _notifyService.Error("Invalid Credentials");
                return View(loginViewModel);
            }

            var isLoggedIn = await _authService.LogIn(loginViewModel, _httpContextAccessor);

            if (!isLoggedIn)
            {
                if (loginViewModel.EmailErrorMessage != "")
                {
                    TempData["EmailErrorMessage"] = loginViewModel.EmailErrorMessage;
                }

                if (loginViewModel.PasswordErrorMessage != "")
                {
                    TempData["PasswordErrorMessage"] = loginViewModel.PasswordErrorMessage;
                }
                _notifyService.Error("Invalid credentials");
                return View(loginViewModel);
            }

            _notifyService.Success("Login Successfull. Welcome Back!!!");
            return RedirectToAction("Index", "FollowUp");
        }
        catch (Exception e)
        {
            _notifyService.Error("Something went wrong");
            return RedirectToAction("LogIn");
        }
    }

    public async Task<IActionResult> LogOut()
    {
        await _authService.LogOut(_httpContextAccessor);
        return View("LogIn");
    }
}
