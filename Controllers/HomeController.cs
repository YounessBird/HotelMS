using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HotelMS.Models;
using HotelMS.Data;

namespace HotelMS.Controllers;

[Route("[controller]")]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }
    [Route("")]
    [Route("~/")]
    [Route("[action]")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("[action]")]
    public IActionResult Privacy()
    {
        return View();
    }

    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
    [Route("[action]")]

    public IActionResult Login()
    {

        return View();
    }
}
