using Microsoft.AspNetCore.Mvc;
using Follow_Up_Manager.Models.ViewModels;
using Follow_Up_Manager.interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using AspNetCoreHero.ToastNotification.Abstractions;


namespace Follow_Up_Manager.Controllers;
[Authorize]
public class FollowUpController : Controller
{
    private readonly IFollowUpService _followUpService;
    public INotyfService _notifyService { get; }

    public FollowUpController(IFollowUpService followUpService, INotyfService notifyService)
    {
        _followUpService = followUpService;
        _notifyService = notifyService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var dataList = await _followUpService.GetAllFollowUps();

            var marked = new List<FollowUpViewModel>();
            var notMarked = new List<FollowUpViewModel>();

            foreach(var data in dataList)
            {
                if(data.Status == 0)
                {
                    notMarked.Add(data);
                }
                else{
                    marked.Add(data);
                }
            }

            ViewBag.marked = marked;
            ViewBag.notMarked = notMarked;

            return View();
        }
        catch (Exception e)
        {
            _notifyService.Error("Something Went Wrong");
            return View();
        }

    }


    public IActionResult Create()
    {
        try
        {
            return View();
        }
        catch (Exception e)
        {
            _notifyService.Error("Something Went Wrong");
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(FollowUpViewModel followUpViewModel)
    {
        try
        {
            string dateString1 = followUpViewModel.StartDate;
            string dateString2 = followUpViewModel.FollowUpDate;

            DateTime date1 = DateTime.ParseExact(dateString1, "dd-MM-yyyy", null);
            DateTime date2 = DateTime.ParseExact(dateString2, "dd-MM-yyyy", null);


            if (date1 >= date2)
            {
                TempData["start_date"] = "Start date should be less than followup date";
                TempData["follow_up_date"] = "Follow Up date should be greater than start date";
                _notifyService.Error("Invalid Start Date & Follow Up Date");
                return RedirectToAction("Create");
            }

            var isStored = await _followUpService.StoreFollowUp(followUpViewModel);
            if (!isStored)
            {
                _notifyService.Error("Follow Up Save Failed");
                return RedirectToAction("Index");
            }
            _notifyService.Success("Follow Up Saved Successfully!");
            return RedirectToAction("Index");
        }
        catch (FormatException ex)
        {
            _notifyService.Error("Something went wrong");
            return RedirectToAction("Index");
        }


    }

    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var followUpViewModel = await _followUpService.GetFollowUpById(id);
            if (followUpViewModel.Id == 0)
            {
                _notifyService.Error("Invalid Follow up to Edit");
                return RedirectToAction("Index");
            }

            ViewBag.FollowUpViewModel = followUpViewModel;
            return View();

        }
        catch (Exception e)
        {
            _notifyService.Error("Something went wrong");
            return RedirectToAction("Index");
        }

    }

    [HttpPost]
    public async Task<IActionResult> Edit(FollowUpViewModel followUpViewModel)
    {
        try
        {
            string dateString1 = followUpViewModel.StartDate;
            string dateString2 = followUpViewModel.FollowUpDate;

            DateTime date1 = DateTime.ParseExact(dateString1, "dd-MM-yyyy", null);
            DateTime date2 = DateTime.ParseExact(dateString2, "dd-MM-yyyy", null);


            if (date1 >= date2)
            {
                TempData["start_date"] = "Start date should be less than followup date";
                TempData["follow_up_date"] = "Follow Up date should be greater than start date";
                _notifyService.Error("Invalid startdate & followup date");
                return RedirectToAction("Edit");
            }


            await _followUpService.UpdateFollowUp(followUpViewModel);
            _notifyService.Success("follow up updated");
            return RedirectToAction("Index");

        }
        catch (Exception e)
        {
            _notifyService.Error("Something went wrong");
            return RedirectToAction("Index");

        }

    }


    public async Task<IActionResult> View(FollowUpViewModel followUpViewModel)
    {
        try
        {
            var followUp = await _followUpService.GetFollowUpById(Convert.ToInt32(followUpViewModel.Id));
            var activityLogs = await _followUpService.GetActivityLogsByFollowUpId(Convert.ToInt32(followUpViewModel.Id));

            List<ActivityLogViewModel> viewModels = new List<ActivityLogViewModel>();

            foreach (var activityLog in activityLogs)
            {
                ActivityLogViewModel viewModel = new ActivityLogViewModel();
                viewModel.Id = activityLog.Id;
                viewModel.UserId = activityLog.UserId;
                viewModel.FollowUpId = activityLog.FollowUpId;
                viewModel.Description = activityLog.Description;
                List<string>? deserializedList = JsonSerializer.Deserialize<List<string>>(activityLog.Description);
                viewModel.HistoryList = deserializedList;
                viewModel.CreatedAt = activityLog.CreatedAt;
                viewModel.UpdatedAt = activityLog.UpdatedAt;
                viewModels.Add(viewModel);
            }
            FollowUpViewModel model = new FollowUpViewModel();

            model.Id = followUp.Id;
            model.Name = followUp.Name;
            model.UserId = followUp.UserId;
            model.StartDate = followUp.StartDate;
            model.FollowUpDate = followUp.FollowUpDate;
            model.Project = followUp.Project;
            model.Task = followUp.Task;

            ViewBag.FollowUpViewModel = model;
            ViewBag.activityLogs = viewModels;
            return View();
        }
        catch (Exception e)
        {
            _notifyService.Error("Something went wrong");
            return RedirectToAction("Index");
        }


    }


    public async Task<IActionResult> Mark(int id)
    {
        try{

            var followUpViewModel = await _followUpService.GetFollowUpById(id);
            
            if (followUpViewModel.Id == 0)
            {
                _notifyService.Error("Invalid Follow up to Edit");
                return RedirectToAction("Index");
            }

            var isMarked = await _followUpService.MarkFollowUp(id);
            if(!isMarked)
            {
                _notifyService.Error("Marking Failed");
                return RedirectToAction("Index");
            }

            _notifyService.Success("Marked followup succesfully!!!");
            return RedirectToAction("Index");

        }catch(Exception e)
        {
            _notifyService.Error("Something went wrong");
            return RedirectToAction("Index");
        }

    }





}
