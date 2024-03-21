using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Follow_Up_Manager.Models;
using Microsoft.AspNetCore.Authorization;
using WebPush;
using Follow_Up_Manager.interfaces;

namespace Follow_Up_Manager.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INotificationService _notificationService;
    private readonly IConfiguration _configuration;

    private readonly IHttpContextAccessor _httpContextAccessor;


    public HomeController(ILogger<HomeController> logger, IConfiguration configuration, INotificationService notificationService, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _configuration = configuration;
        _notificationService = notificationService;
        _httpContextAccessor = httpContextAccessor;

    }

    public IActionResult Index()
    {
        ViewBag.userId = _httpContextAccessor?.HttpContext?.Session.GetString("UserId");
        ViewBag.applicationServerKey = _configuration["VAPID:publicKey"];
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(string client, string endpoint, string p256dh, string auth)
    {
        
        // receive the data;
        // store the notification table
        // send the response
        Console.WriteLine(endpoint);
        Console.WriteLine(p256dh);
        Console.WriteLine(auth);
        var subscription = new PushSubscription(endpoint, p256dh, auth);
        Console.WriteLine(subscription);


        var isSubscribed = await _notificationService.Subscribe(endpoint, p256dh, auth);
        return View();

        
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
