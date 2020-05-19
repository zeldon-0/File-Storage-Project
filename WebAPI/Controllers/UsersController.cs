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
        private readonly IFileService _fileService;
        private readonly IFolderService _folderService;

        private readonly ILinkGenerator<PrivateUserDTO> _linkGenerator;
        public UsersController(IUserService userService,
            IAccountService accountService,
            IFileService fileService,
            IFolderService folderService,
            ILinkGenerator<PrivateUserDTO> linkGenerator)
        {
            _userService = userService;
            _accountService = accountService;
            _fileService = fileService;
            _folderService = folderService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<PrivateUserDTO>> GetUserById(string userId)
        {
            PrivateUserDTO user = await _userService.FindById(userId);

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
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            await _userService.DeleteUser(userId);
            return NoContent();
        }
        [HttpPut("{userId}/upgrade")]
        public async Task<IActionResult> UpgradeAccount(string userId)
        {

            await _accountService.AddAccountToRole(userId, "Corporate");

            return NoContent();
        }

        [HttpPut("{userId}/revertUpgrade")]
        public async Task<IActionResult> RevertAccountUpgrade(string userId)
        {

            await _accountService.RemoveAccountFromRole(userId, "Corporate");

            return NoContent();
        }
        [HttpGet("{userId}/files")]
        public async Task<ActionResult<IEnumerable<FileDTO>>> GetUserFiles(string userId)
        {

            IEnumerable<FileDTO> files =  
                await _fileService.GetUserFiles(Int32.Parse(userId));

            return Ok(files);
        }


        [HttpGet("{userId}/folders")]

        public async Task<ActionResult<IEnumerable<FolderDTO>>> GetUserFolders(string userId)
        {

            IEnumerable<FolderDTO> folders =  
                await _folderService.GetUserFolders(Int32.Parse(userId));

            return Ok(folders);
        }
    }
}
