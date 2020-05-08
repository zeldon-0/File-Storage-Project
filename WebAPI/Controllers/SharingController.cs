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

namespace WebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class SharingController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly ISharingService _sharingService;
        private readonly IShareStatusService _shareStatusService;
        private readonly IAuthorizationService _authorizationService;


        public SharingController (IFolderService folderService,
            IFileService fileService, ISharingService sharingService, 
            IShareStatusService shareStatusService, IAuthorizationService authorizationService)
        {
            _folderService = folderService;
            _fileService = fileService;
            _sharingService = sharingService;
            _shareStatusService = shareStatusService;
            _authorizationService = authorizationService;
        }

        [HttpPut("folders/{id}/share")]
        public async Task<IActionResult> MakeFolderShareable(Guid id)
        {
            FolderDTO folder = await _folderService.GetFolderById(id);
            if (!(await _authorizationService.AuthorizeAsync
                (User, folder, Operations.Update )).Succeeded)
            {
                return Unauthorized("You are not authorized to edit this folder.");
            }
            await _shareStatusService.MakeFolderShareable(folder);
            return NoContent();

        }

        [HttpPut("folders/{id}/unshare")]
        public async Task<IActionResult> MakeFolderUnshareable(Guid id)
        {
            FolderDTO folder = await _folderService.GetFolderById(id);
            if (!(await _authorizationService.AuthorizeAsync
                (User, folder, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to edit this folder.");
            }
            await _shareStatusService.MakeFolderUnshareable(folder);
            return NoContent();
        }

        [HttpPut("folders/{folderId}/share/{email}")]
        [Authorize(Roles ="Corporate, Admin")]
        public async Task<IActionResult> ShareFolderWithUser(Guid folderId, string email)
        {
            FolderDTO folder = await _folderService.GetFolderById(folderId);
            if (!(await _authorizationService.AuthorizeAsync
                (User, folder, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to edit this folder.");
            }
            await _sharingService.ShareFolder(folderId, email);
            return NoContent();
        }

        [HttpPut("folders/{folderId}/unshare/{email}")]
        [Authorize(Roles ="Corporate, Admin")]
        public async Task<IActionResult> UnshareFolderWithUser(Guid folderId, string email)
        {
            FolderDTO folder = await _folderService.GetFolderById(folderId);
            if (!(await _authorizationService.AuthorizeAsync
                (User, folder, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to edit this folder.");
            }
            await _sharingService.UnshareFolder(folderId, email);
            return NoContent();
        }

        [HttpGet("folders/{folderId}/sharingInfo")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFolderSharedUsers(Guid folderId)
        {
            FolderDTO folder = await _folderService.GetFolderById(folderId);
            if (!(await _authorizationService.AuthorizeAsync
                (User, folder, Operations.Read)).Succeeded)
            {
                return Unauthorized("You are not authorized to view this folder's information.");
            }

            IEnumerable<UserDTO> users =
                await _sharingService.GetSharedFolderUserList(folderId);
            return Ok(users);
        }


    }
}
