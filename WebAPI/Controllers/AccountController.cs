using System;
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
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;
        
        public  AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDTO signUpModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided user model is not valid.");

            UserDTO user =  await _accountService.Register(signUpModel);

            return CreatedAtAction(nameof(SignUp), user);
            
        }
        [HttpPost("signin")]
        public async Task<ActionResult<string>> SignIn([FromBody] SignInDTO signInModel)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided user model is not valid.");
            string token = await _accountService.Authenticate(signInModel);
            return Ok(token);

        }
        
    }
}