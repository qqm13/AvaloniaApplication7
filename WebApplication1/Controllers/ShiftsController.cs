using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]

public class ShiftsController  : ControllerBase
{
    private readonly ILogger<ShiftsController> _logger;
    
    public ShiftsController(ILogger<ShiftsController> logger)
    {
        _logger = logger;
    }
}