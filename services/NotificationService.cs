
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.Models;

namespace Follow_Up_Manager.services;

public class NotificationService:INotificationService
{

    private readonly FollowupContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;


     public NotificationService(FollowupContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<bool> Subscribe(string endpoint, string p256dh, string auth)
    {

        try{
            Notification notification = new Notification();
            notification.Status = 0;
            notification.Auth = auth;
            notification.P256dh = p256dh;
            notification.Endpoint = endpoint;

            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();
            
            return true;

        }catch(Exception e)
        {
            return false;

        }

    }
}