using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetJwtApi.Models;
using DotnetJwtApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotnetJwtApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Authenticate))]
        public IActionResult Authenticate([FromBody] AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { Mesage = "Username of password is incorrect" });

            return Ok(response);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!int.TryParse(id, out int i))
                return BadRequest(new { Message = "Id is not valid." });

            var user = _userService.Get(i);

            if (user == null)
                return NotFound(new { Message = "Not found" });

            return Ok(user);
        }
    }
}
