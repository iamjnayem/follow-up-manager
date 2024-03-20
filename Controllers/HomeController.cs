using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Follow_Up_Manager.Models;
using Microsoft.AspNetCore.Authorization;

namespace Follow_Up_Manager.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IConfiguration _configuration;

    public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        
    }

    public IActionResult Index()
    {
        ViewBag.applicationServerKey = _configuration["VAPID:publicKey"];
        return View();
    }

    [HttpPost]
    public IActionResult Index(string client, string endpoint, string p256dh, string auth)
    {
        
        
        return Ok("hello");
        
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
