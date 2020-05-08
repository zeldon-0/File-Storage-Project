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
using System.Security.Claims;
using System.Diagnostics;
using System;
using Microsoft.Extensions.Logging;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public  AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided user model is not valid.");

            UserDTO user =  await _accountService.Register(signUpModel);

            return CreatedAtAction(nameof(SignUp), user);
            
        }
        [HttpPost("signIn")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> SignIn([FromBody] SignInDTO signInModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided user model is not valid.");
            string token = await _accountService.Authenticate(signInModel);
            return Ok(token);

        }
        
        [HttpPut("upgrade")]
        [Authorize(Roles ="User, Admin")]
        public async Task<IActionResult> UpgradeAccount()
        {
            var currentUser = this.User;
            await _accountService.AddAccountToRole(
                currentUser.Claims.FirstOrDefault
                (c => c.Type ==ClaimTypes.Email).Value, "Corporate");

            return NoContent();
        }

        [HttpPut("revertUpgrade")]
        [Authorize(Roles ="Corporate")]
        public async Task<IActionResult> RevertAccountUpgrade()
        {
            var currentUser = this.User;
            await _accountService.RemoveFromRole(
                currentUser.Claims.FirstOrDefault
                (c => c.Type == ClaimTypes.Email).Value, "Corporate");

            return NoContent();
        }


        [HttpPut("edit")]
        public async Task<IActionResult> EditAccount([FromBody] UserDTO userInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest("You did not fill all the required fields");
            await _accountService.Edit(userInfo,
                User.Claims.FirstOrDefault
                (c => c.Type == ClaimTypes.Email).Value);
            return NoContent();
        }

        [HttpPut("updatePassword")]
        public async Task<IActionResult> ChangePassword([FromQuery] string oldPassword, string newPassword)
        {
            SignInDTO credentials = new SignInDTO()
            {
                Login = User.Claims.FirstOrDefault
                    (c => c.Type == ClaimTypes.Email).Value,
                Password = oldPassword
            };

            await _accountService.ChangePassword(credentials, newPassword);
            return NoContent();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount([FromBody] SignInDTO credentials)
        {
            await _accountService.Delete(credentials);
            return NoContent();
        }
    }
}