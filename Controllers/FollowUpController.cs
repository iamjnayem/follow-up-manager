using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.services;
using Microsoft.AspNetCore.Authorization;


namespace Follow_Up_Manager.Controllers;
[Authorize]
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

    public async Task<IActionResult> Edit(int id)
    {
        var followUpViewModel = await _followUpService.GetFollowUpById(id);
        if(followUpViewModel.Id == 0)
        {
            // add toaster that no follow up found 
            return RedirectToAction("Index");
        }

        ViewBag.FollowUpViewModel = followUpViewModel;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(FollowUpViewModel followUpViewModel)
    {

        await _followUpService.UpdateFollowUp(followUpViewModel);
        return RedirectToAction("Index");
    }


    public async Task<IActionResult> View(FollowUpViewModel followUpViewModel)
    {
        // int id = Convert.ToInt32(followUpViewModel.Id);
        var followUp = await _followUpService.GetFollowUpById(Convert.ToInt32(followUpViewModel.Id));
        var activityLogs = await _followUpService.GetActivityLogsByFollowUpId(Convert.ToInt32(followUpViewModel.Id));

        FollowUpViewModel model = new FollowUpViewModel();

        model.Id = followUp.Id;
        model.Name = followUp.Name;
        model.UserId = followUp.UserId;
        model.StartDate = followUp.StartDate;
        model.FollowUpDate =followUp.FollowUpDate;
        model.Project =followUp.Project;
        model.Task = followUp.Task;

        ViewBag.FollowUpViewModel = model;
        ViewBag.activityLogs = activityLogs;
        return View();

    }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
