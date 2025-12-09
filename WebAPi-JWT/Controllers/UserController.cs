using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPi_JWT.Infrastructure;
using WebAPi_JWT.WebApi.Core.Domian;

namespace WebAPi_JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataAccess _dataAccess;
        public UserController(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }
        [HttpGet("Get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _dataAccess.Users.ToListAsync();
            return Ok(users);
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update(User user)
        {
            _dataAccess.Users.Update(user);
            await _dataAccess.SaveChangesAsync();
            return Ok(user);
        }
    }
}
