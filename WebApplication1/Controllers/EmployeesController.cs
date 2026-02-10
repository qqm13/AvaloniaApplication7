using Microsoft.AspNetCore.Mvc;
using WebApplication1.DB;
using WebApplication1.DTO;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]

public class EmployeesController  : ControllerBase
{
    private readonly ILogger<EmployeesController> _logger;
    private readonly ZzzadaniePrContext _context;
    
    public EmployeesController(ILogger<EmployeesController> logger, ZzzadaniePrContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet(Name = "api/employees")]
    public List<EmployeeDTO> Sotrydniki()
    {
        var listDTO = new List<EmployeeDTO>();
        
        var listEmployees = _context.Employees.ToList();

        foreach (var item in listEmployees)
        {
            var dto = new EmployeeDTO
            {
                Id = item.Id,
                IsActive = item.IsActive,
                FirstName = item.FirstName,
                LastName = item.LastName,
            };
            listDTO.Add(dto);
        }
        
        return listDTO;
    }
}