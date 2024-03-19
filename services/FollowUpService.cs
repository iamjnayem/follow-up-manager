using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
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


    public async Task<List<FollowUpViewModel>> GetAllFollowUps()
    {
        var userId = _httpContextAccessor?.HttpContext?.Session.GetString("UserId");
        Console.WriteLine(userId);

        List<FollowUp> data = await _dbContext.FollowUps
                            .Where(f => f.Id == long.Parse(userId))
                            .OrderBy(f => f.FollowUpDate).ToListAsync();

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
            dataList.Add(model);
        }
        return dataList;
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

                ActivityLog activityLog = new ActivityLog
                {
                    FollowUpId = followUp.Id,
                    UserId = long.Parse(userId),
                    Description = "Follow Up Created",
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
                throw;
            }
        }
    }


    public async Task<FollowUpViewModel> GetFollowUpById(int id)
    {

        try
        {
            FollowUpViewModel followUpViewModel = new FollowUpViewModel();

            var followUp = await _dbContext.FollowUps.FirstOrDefaultAsync(f => f.Id == id);

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

            return followUpViewModel;


        }
        catch (Exception ex)
        {
            throw;
        }

    }

    public async Task<bool> UpdateFollowUp(FollowUpViewModel followUpViewModel)
    {
        try
        {
            var followUp = await _dbContext.FollowUps.FirstOrDefaultAsync(f => f.Id == followUpViewModel.Id);

            if (followUp == null)
            {
                return false;
            }


            if (followUpViewModel.Name != null)
            {
                followUp.Name = followUpViewModel.Name;
            }

            if (followUpViewModel.StartDate != null)
            {
                followUp.StartDate = followUpViewModel.StartDate;
            }

            if (followUpViewModel.FollowUpDate != null)
            {
                followUp.FollowUpDate = followUpViewModel.FollowUpDate;
            }

            if (followUpViewModel.Project != null)
            {
                followUp.Project = followUpViewModel.Project;
            }

            await _dbContext.SaveChangesAsync();

            return true;

        }
        catch (Exception e)
        {
            throw;
        }

    }
}
