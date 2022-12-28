using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("[controller]")]
public class DinnersController : ApiController
{
    [HttpGet]
    async public Task<IActionResult> ListDinners()
    {
        var dinners = Array.Empty<string>();
        return await Task.FromResult(Ok(dinners));
    }
}