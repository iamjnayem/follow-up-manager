using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Follow_Up_Manager.services;

public class FollowUpService : IFollowUpService
{
    private readonly FollowupContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public FollowUpService(FollowupContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<List<FollowUpViewModel>>? GetAllFollowUps()
    {
        try
        {
            var userId = _httpContextAccessor?.HttpContext?.Session.GetString("UserId");

            List<FollowUp> data = await _dbContext.FollowUps
                                .Where(f => f.UserId == long.Parse(userId))
                                .OrderBy(f => f.FollowUpDate) 
                                .ToListAsync();

            List<FollowUpViewModel> dataList = new List<FollowUpViewModel>();

            foreach (var followUp in data)
            {
                FollowUpViewModel model = new FollowUpViewModel();

                model.Id = followUp.Id;
                model.Task = followUp.Task;
                model.StartDate = followUp.StartDate;
                model.FollowUpDate = followUp.FollowUpDate;
                model.Name = followUp.Name;
                model.Project = followUp.Project;
                model.Status = Convert.ToInt32(followUp.Status);
                dataList.Add(model);
            }
            return dataList;

        }
        catch (Exception e)
        {
            List<FollowUpViewModel> dataList = new List<FollowUpViewModel>();
            return dataList;
        }

    }

    public async Task<bool> StoreFollowUp(FollowUpViewModel followUpViewModel)
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                var userId = _httpContextAccessor?.HttpContext?.Session.GetString("UserId");
                FollowUp followUp = new FollowUp
                {
                    Name = followUpViewModel.Name,
                    Task = followUpViewModel.Task,
                    Project = followUpViewModel.Project,
                    StartDate = followUpViewModel.StartDate,
                    FollowUpDate = followUpViewModel.FollowUpDate,
                    Status = 0,
                    UserId = long.Parse(userId)
                };

                await _dbContext.AddAsync(followUp);
                await _dbContext.SaveChangesAsync();

                List<string> descriptionList = new List<string>();
                descriptionList.Add("Follow Up Created");

                ActivityLog activityLog = new ActivityLog
                {
                    FollowUpId = followUp.Id,
                    UserId = long.Parse(userId),
                    Description = JsonSerializer.Serialize(descriptionList),
                    CreatedAt = DateTime.Now
                };

                await _dbContext.AddAsync(activityLog);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return false;
            }
        }
    }


    public async Task<FollowUpViewModel> GetFollowUpById(int id)
    {

        try
        {
            FollowUpViewModel followUpViewModel = new FollowUpViewModel();

            var followUp = await _dbContext.FollowUps.FirstOrDefaultAsync(f => f.Id == id);
            Console.WriteLine("Maruf");

            if (followUp == null)
            {
                return followUpViewModel;
            }

            followUpViewModel.Id = followUp.Id;
            followUpViewModel.Name = followUp.Name;
            followUpViewModel.Task = followUp.Task;
            followUpViewModel.StartDate = followUp.StartDate;
            followUpViewModel.FollowUpDate = followUp.FollowUpDate;
            followUpViewModel.Project = followUp.Project;
            followUpViewModel.Status = Convert.ToInt32(followUp.Status);

            return followUpViewModel;


        }
        catch (Exception ex)
        {
            FollowUpViewModel followUpViewModel = new FollowUpViewModel();

            return followUpViewModel;
        }

    }

    public async Task<bool> UpdateFollowUp(FollowUpViewModel followUpViewModel)
    {
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                var userId = _httpContextAccessor?.HttpContext?.Session.GetString("UserId");
                var followUp = await _dbContext.FollowUps.FirstOrDefaultAsync(f => f.Id == followUpViewModel.Id);

                if (followUp == null)
                {
                    return false;
                }
                List<string> descriptionList = new List<string>();


                if (followUpViewModel.Name != followUp.Name)
                {
                    descriptionList.Add("Updated Name from " + followUp.Name + " to " + followUpViewModel.Name);
                    followUp.Name = followUpViewModel.Name;
                }

                if (followUpViewModel.StartDate != followUp.StartDate)
                {
                    descriptionList.Add("Updated StartDate from " + followUp.StartDate + " to " + followUpViewModel.StartDate);
                    followUp.StartDate = followUpViewModel.StartDate;
                }

                if (followUpViewModel.FollowUpDate != followUp.FollowUpDate)
                {
                    descriptionList.Add("Updated Follow Up Date  from " + followUp.FollowUpDate + " to " + followUpViewModel.FollowUpDate);
                    followUp.FollowUpDate = followUpViewModel.FollowUpDate;
                }

                if (followUpViewModel.Project != followUp.Project)
                {
                    descriptionList.Add("Updated Project  from " + followUp.Project + " to " + followUpViewModel.Project);
                    followUp.Project = followUpViewModel.Project;
                }

                await _dbContext.SaveChangesAsync();



                ActivityLog activityLog = new ActivityLog
                {
                    FollowUpId = followUp.Id,
                    UserId = long.Parse(userId),
                    Description = JsonSerializer.Serialize(descriptionList),
                    CreatedAt = DateTime.Now
                };

                await _dbContext.AddAsync(activityLog);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();

                return true;


            }
            catch (Exception e)
            {
                transaction.Rollback();
                return false;
            }
        }


    }


    public async Task<List<ActivityLog>>? GetActivityLogsByFollowUpId(int id)
    {


        var userId = _httpContextAccessor?.HttpContext?.Session.GetString("UserId");
        List<ActivityLog> data = await _dbContext.ActivityLogs
                            .Where(f => f.UserId == int.Parse(userId))
                            .Where(f => f.FollowUpId == id)
                            .OrderByDescending(f => f.CreatedAt).ToListAsync();

        return data;


    }

    public async Task<bool> MarkFollowUp(int id)
    {

        try
        {
            var followUp = await _dbContext.FollowUps.FirstOrDefaultAsync(f => f.Id == id);
            Console.WriteLine(followUp == null);
            Console.WriteLine("followup " + followUp);
            Console.WriteLine("status is ");
            followUp.Status = 1;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }

    }


}
