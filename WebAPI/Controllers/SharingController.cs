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



        [HttpPut("files/{id}/share")]
        public async Task<IActionResult> MakeFileShareable(Guid id)
        {
            FileDTO file = await _fileService.GetFileById(id);
            if (!(await _authorizationService.AuthorizeAsync
                (User, file, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to edit this file.");
            }
            await _shareStatusService.MakeFileShareable(file);
            return NoContent();

        }

        [HttpPut("files/{id}/unshare")]
        public async Task<IActionResult> MakeFileUnshareable(Guid id)
        {
            FileDTO file = await _fileService.GetFileById(id);
            if (!(await _authorizationService.AuthorizeAsync
                (User, file, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to edit this file.");
            }
            await _shareStatusService.MakeFileUnshareable(file);
            return NoContent();
        }

        [HttpPut("files/{fileId}/share/{email}")]
        [Authorize(Roles = "Corporate, Admin")]
        public async Task<IActionResult> ShareFileWithUser(Guid fileId, string email)
        {
            FileDTO file = await _fileService.GetFileById(fileId);
            if (!(await _authorizationService.AuthorizeAsync
                (User, file, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to edit this file.");
            }
            await _sharingService.ShareFile(fileId, email);
            return NoContent();
        }

        [HttpPut("files/{fileId}/unshare/{email}")]
        [Authorize(Roles = "Corporate, Admin")]
        public async Task<IActionResult> UnshareFileWithUser(Guid fileId, string email)
        {
            FileDTO file = await _fileService.GetFileById(fileId);
            if (!(await _authorizationService.AuthorizeAsync
                (User, file, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to edit this file.");
            }
            await _sharingService.UnshareFile(fileId, email);
            return NoContent();
        }

        [HttpGet("filess/{fileId}/sharingInfo")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetFileSharedUsers(Guid fileId)
        {
            FileDTO file = await _fileService.GetFileById(fileId);
            if (!(await _authorizationService.AuthorizeAsync
                (User, file, Operations.Read)).Succeeded)
            {
                return Unauthorized("You are not authorized to view this file's information.");
            }

            IEnumerable<UserDTO> users =
                await _sharingService.GetSharedFileUserList(fileId);
            return Ok(users);
        }

    }
}
