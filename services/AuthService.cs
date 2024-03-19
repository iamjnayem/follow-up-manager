
using System.Security.Claims;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


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
            Name = signUpViewModel.FirstName  + " " + signUpViewModel.LastName,
            Email = signUpViewModel.Email,
            Password = signUpViewModel.Password,
            Status = 1,
            CreatedAt = DateTime.Now,
            UpdatedAt = null
        };
        
        await _dbContext.AddAsync(data);
        await _dbContext.SaveChangesAsync();
        return true;
    }

     public async Task<bool> LogIn(LoginViewModel loginViewModel, IHttpContextAccessor httpContextAccessor)
    {
        User user = await _dbContext.Users.Where(e => e.Email == loginViewModel.Email).SingleOrDefaultAsync();

        if(user == null)
        {
            return false;
        }

        bool isValid = user.Email == loginViewModel.Email && user.Password == loginViewModel.Password;

        if(!isValid)
        {
            return false;
        }

        var identity = new ClaimsIdentity(new [] {new Claim(ClaimTypes.Email, loginViewModel.Email)}, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await httpContextAccessor?.HttpContext?.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        httpContextAccessor?.HttpContext?.Session.SetString("Email", loginViewModel.Email);

        return true;
    }


    public async Task<bool> LogOut(IHttpContextAccessor httpContextAccessor)
    {
        await httpContextAccessor?.HttpContext?.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return true;
    }

    


}
