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

    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
            //sent some error message;
        }
        
        //Redirect to login page;
        return View("");
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
