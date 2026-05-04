using Microsoft.AspNetCore.Mvc;

namespace PermissionsAttributeTask.Main.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}