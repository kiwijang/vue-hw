using API.Models;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(
            IUserService userService,
            IJwtService jwtService,
            ILogger<UserController> logger)
        {
            _logger = logger;
            _userService = userService;
            _jwtService = jwtService;
        }

        /// <summary>
        /// 取得使用者資料
        /// </summary>
        /// <returns></returns> 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,User")]
        [HttpPost("GetUser")]
        public async Task<ActionResult<GetUserVM>> GetUser(string email)
        {
            var token = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token"); ;
            var isJwtValid = _jwtService.ValidateToken(token, out Exception? ex);
            // jwt 驗證 / 只能改自己的(驗證 viewModel 和 identity 是否相同)
            if (!isJwtValid || email != User.Identity.Name)
            {
                return BadRequest();
            }
            return Ok(await _userService.GetUser(email));
        }

        /// <summary>
        /// 修改使用者資料
        /// </summary>
        /// <returns></returns> 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,User")]
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserVM model)
        {
            var token = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token"); ;
            var isJwtValid = _jwtService.ValidateToken(token, out Exception? ex);
            // jwt 驗證 / 只能改自己的(驗證 viewModel 和 identity 是否相同)
            if (!isJwtValid || model.Email != User.Identity.Name)
            {
                return BadRequest();
            }
            var result = await _userService.UpdateUser(model);

            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}