using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ShopOnline.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
        {
           var result = await authRepository.RegisterAsync(new User { Email = request.Email }, request.Password);
            if(result.Success)
                return Ok(result);
            else
                return BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
        {
            var response = await authRepository.LoginAsync(request.Email, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody]string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
           var response = await authRepository.ChangePasswordAsync(int.Parse(userId), newPassword);
            if(!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
