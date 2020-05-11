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
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly ILinkGenerator<PrivateUserDTO> _linkGenerator;
        public UsersController(IUserService userService,
            IAccountService accountService,
            ILinkGenerator<PrivateUserDTO> linkGenerator)
        {
            _userService = userService;
            _accountService = accountService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<PrivateUserDTO>> GetUserByName(string userName)
        {
            PrivateUserDTO user = await _userService.FindByUserName(userName);

            user.Links = _linkGenerator.GenerateAllLinks(User, user);
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
        [HttpDelete("{userName}")]
        public async Task<IActionResult> DeleteUser(string userName)
        {
            await _userService.DeleteUser(userName);
            return NoContent();
        }
        [HttpPut("{userName}/upgrade")]
        public async Task<IActionResult> UpgradeAccount(string userName)
        {

            await _accountService.AddAccountToRole(userName, "Corporate");

            return NoContent();
        }

        [HttpPut("{userName}/revertUpgrade")]
        public async Task<IActionResult> RevertAccountUpgrade(string userName)
        {

            await _accountService.RemoveAccountFromRole(userName, "Corporate");

            return NoContent();
        }
    }
}
