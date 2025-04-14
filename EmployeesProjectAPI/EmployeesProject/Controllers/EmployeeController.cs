using EmployeesProject.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _employeeService;

    public EmployeeController(EmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [HttpPost("upload")]
    public IActionResult UploadCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        try
        {
            using var stream = file.OpenReadStream();
            var records = _employeeService.ParseCsv(stream);
            var result = _employeeService.CalculatePairs(records);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
