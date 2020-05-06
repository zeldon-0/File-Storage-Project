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

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        
        public  AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided user model is not valid.");

            UserDTO user =  await _accountService.Register(signUpModel);

            return CreatedAtAction(nameof(SignUp), user);
            
        }
        [HttpPost("signin")]
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

            return Ok();
        }

        [HttpPut("revert_upgrade")]
        [Authorize(Roles ="Corporate")]
        public async Task<IActionResult> RevertAccountUpgrade()
        {
            var currentUser = this.User;
            await _accountService.RemoveFromRole(
                currentUser.Claims.FirstOrDefault
                (c => c.Type == ClaimTypes.Email).Value, "Corporate");

            return Ok();
        }


    }
}