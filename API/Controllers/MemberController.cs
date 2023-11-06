using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private IMemberService _svc;
        public MemberController(IMemberService svc)
        {
            _svc = svc;
        }

        /// <summary>
        /// 取得一筆會員
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("GetMemberByUserId")]
        public async Task<ActionResult<MemberVM?>> GetMemberByUserId(string id)
        {
            return Ok(await _svc.GetMemberByUserId(id));
        }

        /// <summary>
        /// 取得會員
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllMembers")]
        public async Task<ActionResult<IQueryable<MemberVM?>>> GetAllMembers()
        {
            return Ok(await _svc.GetMembers());
        }

        /// <summary>
        /// 更新會員
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateMember")]
        public async Task<IActionResult> UpdateMember(MemberVM model)
        {
            var result = await _svc.UpdateMember(model);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();

            }
        }
    }
}