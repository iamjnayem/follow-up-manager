using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.services;


namespace Follow_Up_Manager.Controllers;

public class FollowUpController : Controller
{ 
    private readonly IFollowUpService _followUpService;

    public FollowUpController(IFollowUpService followUpService)
    {
        _followUpService = followUpService;
    }

    public async Task<IActionResult> Index()
    {        
        ViewBag.dataList = await _followUpService.GetAllFollowUps();
        return View();
    }


    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(FollowUpViewModel followUpViewModel)
    {
        await _followUpService.StoreFollowUp(followUpViewModel);
        return RedirectToAction("Index");
    }
    public IActionResult Edit()
    {
        Console.WriteLine("");
        return View();
    }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
