using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.Models.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Follow_Up_Manager.services;

public class FollowUpService:IFollowUpService
{
    private readonly FollowupContext _dbContext;
    public FollowUpService(FollowupContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<List<FollowUpViewModel>> GetAllFollowUps(){
        List<FollowUp> data = await _dbContext.FollowUps.ToListAsync();
        List<FollowUpViewModel> dataList = new List<FollowUpViewModel>();

        foreach(var followUp in data)
        {
            FollowUpViewModel model = new FollowUpViewModel();
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
                FollowUp followUp = new FollowUp
                {
                    Name = followUpViewModel.Name,
                    Task = followUpViewModel.Task,
                    Project = followUpViewModel.Project,
                    StartDate = followUpViewModel.StartDate,
                    FollowUpDate = followUpViewModel.FollowUpDate,
                    Status = 0
                };
                
                await _dbContext.AddAsync(followUp);
                await _dbContext.SaveChangesAsync();
                
                ActivityLog activityLog = new ActivityLog
                {
                    FollowUpId = followUp.Id,
                    UserId = 1, //should be auth user id
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
}
