using FirstAPI.DTO;
using FirstAPI.GenericResponse;
using FirstAPI.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        [HttpGet("employees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            try
            {
                var result = await employeeService.AllEmployees();

                if (!result.Item2.Any())
                {
                    return Ok(ResponseResult<List<EmployeeDTO>>.Failure(null, "No employees found."));
                }

                return Ok(ResponseResult<List<EmployeeDTO>>.Success(result.Item2, "Employees retrieved successfully."));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO employeedto)
        {
            try
            {
                var result = await employeeService.CreateEmployee(employeedto);
                if (result.Item1 == 1)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                if (result.Item1 == 3)
                {
                    return Conflict(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> EditEmployee([FromBody] EmployeeDTO employeedto)
        {
            try
            {
                var result = await employeeService.UpdateEmployee(employeedto);
                if (result.Item1 == 1)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            try
            {
                var result = await employeeService.RemoveEmployee(id);
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("search/{id}")]
        public async Task<IActionResult> GetEmployeeData([FromRoute] Guid id)
        {
            try
            {
                var result = await employeeService.GetEmployeeById(id);
                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, "Employee not found."));
                }
                return Ok(ResponseResult<EmployeeDTO>.Success(result.Item2, "Employee retrieved successfully."));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}