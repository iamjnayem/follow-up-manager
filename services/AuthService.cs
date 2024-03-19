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
using Microsoft.AspNetCore.Mvc;


namespace Follow_Up_Manager.services;

public class AuthService : IAuthService
{
    private readonly FollowupContext _dbContext;
    public AuthService(FollowupContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SignUp(SignUpViewModel signUpViewModel)
    {
        var data = new User()
        {
            Name = signUpViewModel.Name,
            Email = signUpViewModel.Email,
            Password = signUpViewModel.Password,
            Status = 1,
            CreatedAt = DateTime.Now
        };

        await _dbContext.AddAsync(data);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    


}
