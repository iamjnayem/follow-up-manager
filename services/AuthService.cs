
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.Models.ViewModels;


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

    


}
