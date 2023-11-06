using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private IEventService _svc;
        public EventController(IEventService svc)
        {
            _svc = svc;
        }

        /// <summary>
        /// 新增活動
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateEvent")]
        public async Task<IActionResult> CreateEvent(EventVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await this._svc.CreateEvent(model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// 取得活動
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("GetEventById")]
        public async Task<ActionResult<EventVM?>> GetEventById(Guid id)
        {
            try
            {
                return Ok(await this._svc.GetEventById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        /// <summary>
        /// 取得所有活動
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllEvents")]
        public async Task<ActionResult<IEnumerable<EventVM?>>> GetAllEvents()
        {
            try
            {
                return Ok(await this._svc.GetEvents());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        /// <summary>
        /// 更新活動
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent(EventVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await this._svc.UpdateEvent(model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }


        /// <summary>
        /// 新增活動參與者
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        [HttpPost("AddEventJoinUsers")]
        public async Task<IActionResult> AddEventJoinUsers(AddEventJoinUserVM users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await this._svc.AddEventJoinUsers(users);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
    }
}