
using Follow_Up_Manager.interfaces;

namespace Follow_Up_Manager.services;

public class NotificationService:INotificationService
{
    public Task<bool> Subscribe()
    {
        return Task.FromResult(true);
    }
}