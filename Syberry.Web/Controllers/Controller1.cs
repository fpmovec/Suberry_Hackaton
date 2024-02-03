using Microsoft.AspNetCore.Mvc;

namespace Syberry.Web.Controllers;

public class Controller1 : Controller
{
    // GET
    public IActionResult Index()
    {
        return Ok();
    }
}