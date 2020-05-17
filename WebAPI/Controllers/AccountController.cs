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
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILinkGenerator<PrivateUserDTO> _linkGenerator;
        public AccountController(IAccountService accountService,
            ILinkGenerator<PrivateUserDTO> linkGenerator)
        {
            _accountService = accountService;
            _linkGenerator = linkGenerator;
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
        public async Task<IActionResult> SignIn([FromBody] SignInDTO signInModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided user model is not valid.");
            
            
            JwtSecurityToken token = await _accountService.Authenticate(signInModel);           

            string tokenString =
                new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new {
                Id = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                Username =  token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value,
                Roles = token.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .Select(r => r.Value).ToArray(),
                Token = tokenString
            });

        }

        [HttpGet]
        public async Task<ActionResult<PrivateUserDTO>> GetOwnInfo()
        {
            PrivateUserDTO info = await _accountService.GetOwnInfo
                (Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
            info.Links = _linkGenerator.GenerateAllLinks(User, info);

            return Ok(info);
        }
        
        [HttpPut("upgrade")]
        [Authorize(Roles ="User, Admin")]
        public async Task<IActionResult> UpgradeAccount()
        {
            var currentUser = this.User;
            await _accountService.AddAccountToRole(
                currentUser.Claims.FirstOrDefault
                (c => c.Type ==ClaimTypes.Name).Value, "Corporate");

            return NoContent();
        }

        [HttpPut("revertUpgrade")]
        [Authorize(Roles ="Corporate")]
        public async Task<IActionResult> RevertAccountUpgrade()
        {
            var currentUser = this.User;
            await _accountService.RemoveAccountFromRole(
                currentUser.Claims.FirstOrDefault
                (c => c.Type == ClaimTypes.Name).Value, "Corporate");

            return NoContent();
        }


        [HttpPut("edit")]
        public async Task<IActionResult> EditAccount([FromBody] UserDTO userInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest("You did not fill all the required fields");
            await _accountService.Edit(userInfo);
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