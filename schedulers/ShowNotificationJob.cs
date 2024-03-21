using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.Models;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Quartz.Xml.JobSchedulingData20;
using WebPush;

namespace Follow_Up_Manager.schedulers;

public class ShowNotificationJob : IJob
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly INotificationService _notficationSevice;
    private readonly FollowupContext _dbContext;

    private readonly IConfiguration _configuration;

    public ShowNotificationJob(
        IHttpContextAccessor httpContextAccessor,
        INotificationService notificationService,
        FollowupContext dbContext,
        IConfiguration configuration
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _notficationSevice = notificationService;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            var clientList = await _dbContext.Notifications.ToListAsync();

            foreach (var client in clientList)
            {
                var subscription = new PushSubscription(
                    client.Endpoint,
                    client.P256dh,
                    client.Auth
                );
                var subject = _configuration["VAPID:subject"];
                var publicKey = _configuration["VAPID:publicKey"];
                var privateKey = _configuration["VAPID:privateKey"];

                var vapidDetails = new VapidDetails(subject, publicKey, privateKey);
                var webPushClient = new WebPushClient();

                var followUpCount = await this.GetMessage(Convert.ToInt32(client.Client));
                if(followUpCount > 0)
                {
                    await webPushClient.SendNotificationAsync(subscription, "You have " + followUpCount + " follow ups today", vapidDetails);
                }
                
            }
        }
        catch (Exception ex)
        {
            Console.Write("Some thing occurred");
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<int> GetMessage(int id)
    {


        var date = DateTime.Today.ToString("dd-MM-yyyy");
        Console.WriteLine(date);
        var targetList = await _dbContext.FollowUps.Where(e => e.UserId == id)
                            .Where(e => e.FollowUpDate == date)
                            .Where(e => e.Status == 0 )
                            .ToListAsync();
        
        if(targetList == null)
        {
            return 0;
        }


        return targetList.Count;
                                
    }

}
