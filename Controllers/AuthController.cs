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
            //return view with error message
            return View(signUpViewModel);
        }

        var isRegistered = await _authService.SignUp(signUpViewModel);
    
        if(!isRegistered)
        {
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

        return RedirectToAction("Index", "Home");

    }


    public async Task<IActionResult> LogOut()
    {
      await _authService.LogOut(_httpContextAccessor);  
      return View("LogIn");
    }

    // public IActionResult Create()
    // {
    //     return View();
    // }
    
    // [HttpPost]
    // public async Task<IActionResult> Create(FollowUpViewModel followUpViewModel)
    // {
    //     await _followUpService.StoreFollowUp(followUpViewModel);
    //     return RedirectToAction("Index");
    // }

    // public async Task<IActionResult> Edit(int id)
    // {
    //     var followUpViewModel = await _followUpService.GetFollowUpById(id);
    //     if(followUpViewModel.Id == 0)
    //     {
    //         // add toaster that no follow up found 
    //         return RedirectToAction("Index");
    //     }

    //     ViewBag.FollowUpViewModel = followUpViewModel;
    //     return View();
    // }

    // [HttpPost]
    // public async Task<IActionResult> Edit(FollowUpViewModel followUpViewModel)
    // {

    //     await _followUpService.UpdateFollowUp(followUpViewModel);
    //     return RedirectToAction("Index");
    // }


    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
