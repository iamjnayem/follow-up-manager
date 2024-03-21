using AspNetCoreHero.ToastNotification;
using Follow_Up_Manager.interfaces;
using Follow_Up_Manager.Models;
using Follow_Up_Manager.services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Quartz.AspNetCore;
using Quartz;
using Follow_Up_Manager.schedulers;

var Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .Build();

var builder = WebApplication.CreateBuilder(args);

string? showNotificationExpression = Configuration.GetValue<string>("schedulers:show_notification:cron_expression:show_notification");
bool? isShowNotificationEnabled = Configuration.GetValue<bool>("schedulers:show_notification:on_off_switch:show_notification");

builder.Services.AddQuartz(q =>
{
    if (isShowNotificationEnabled == true)
    {
        var jobKey = new JobKey("ShowNotificationJob");
        q.AddJob<ShowNotificationJob>(opts => opts.WithIdentity(jobKey));
        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("ShowNotificationJob-trigger")
            .WithCronSchedule(showNotificationExpression ?? string.Empty)
        );
    }
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);




// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddProgressiveWebApp();
builder.Services.AddHttpContextAccessor();

builder.Services.AddNotyf(config=> { config.DurationInSeconds = 3;config.IsDismissable = true;config.Position = NotyfPosition.TopRight; });

builder.Services.AddScoped<IFollowUpService, FollowUpService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddDbContext<FollowupContext>(options =>
{
    var config = Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlite(config);
    options.EnableSensitiveDataLogging();
}, ServiceLifetime.Transient);


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option => {
    option.ExpireTimeSpan = TimeSpan.FromHours(24 * 7);
    option.LoginPath = "/Auth/LogIn";
    option.AccessDeniedPath = "/Auth/LogIn";
});

builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromHours(24 * 7);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;

});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
