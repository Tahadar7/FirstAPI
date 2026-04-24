using FirstAPI.DTO;
using FirstAPI.GenericResponse;
using FirstAPI.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDTO userdto)
        {
            try
            {
                var result = await _authService.LoginUser(userdto);

                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<string>.Failure(null, result.Item2));
                }
                if (result.Item1 == 1)
                {
                    return BadRequest(ResponseResult<string>.Failure(null, result.Item2));
                }

                if (result.Item1 == 4)
                {
                    return Unauthorized(ResponseResult<string>.Failure(null, result.Item2));
                }
                return Ok(ResponseResult<string>.Success(null, result.Item2));

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userdto)
        {
            try
            {
                var result = await _authService.RegisterUser(userdto);
                if (result.Item1 == 1)
                {
                    return BadRequest(ResponseResult<string>.Failure(null,result.Item2));
                }

                if (result.Item1 == 3)
                {
                    return Conflict(ResponseResult<string>.Failure(null,result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
