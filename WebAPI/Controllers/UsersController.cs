using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using BLL.Interfaces;
using BLL.Models;
using BLL.AuthorizationHandlers;
using System.Security.Claims;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IAccountService _accountService;
        public UsersController(IUserService userService,
            IAccountService accountService)
        {
            _userService = userService;
            _accountService = accountService;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<PrivateUserDTO>> GetUserByEmail(string email)
        {
            PrivateUserDTO user = await _userService.FindByEmail(email);
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            IEnumerable<UserDTO> users = await _userService.GetAllUsers();
            return Ok(users);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided user model is invalid");
            await _userService.EditUser(userModel);

            return NoContent();
        }
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            await _userService.DeleteUser(email);
            return NoContent();
        }
        [HttpPut("{email}/upgrade")]
        public async Task<IActionResult> UpgradeAccount(string email)
        {

            await _accountService.AddAccountToRole(email, "Corporate");

            return NoContent();
        }

        [HttpPut("{email}/revertUpgrade")]
        public async Task<IActionResult> RevertAccountUpgrade(string email)
        {

            await _accountService.RemoveAccountFromRole(email, "Corporate");

            return NoContent();
        }
    }
}
