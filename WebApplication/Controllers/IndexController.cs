using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers;

[ApiController]
public class IndexController : ControllerBase
{

    [Route("/")]
    [HttpGet]
    public ActionResult<string> Hello()
    {
        return "Hello World!!!";
    }
}
