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
using BLL.Link_Generation;
using DAL.Entities;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoldersController : ControllerBase
    {
        private readonly IFolderService _folderService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ISharingService _sharingService;
        private readonly ILinkGenerator<FolderDTO> _linkGenerator;

        public FoldersController(IFolderService folderService,
                    IAuthorizationService authorizationService,
                    ISharingService sharingService,
                    ILinkGenerator<FolderDTO> linkGenerator)
        {
            _folderService = folderService;
            _authorizationService = authorizationService;
            _sharingService = sharingService;
            _linkGenerator = linkGenerator;
        }

        [HttpPost]
        public async Task<ActionResult<FolderDTO>> Post([FromBody] FolderDTO folder)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided folder model is not valid.");
            FolderDTO created = await _folderService.CreateAtRoot(folder,
                  Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
            return CreatedAtAction(nameof(Post), created);
        }

        [HttpPost("{id}/subfolders")]
        public async Task<ActionResult<FolderDTO>> PostSubFolder(Guid id, [FromBody] FolderDTO subFolder)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided folder model is not valid.");
            FolderDTO parentFolder = await _folderService.GetFolderById(id);

            if (!(await _authorizationService.AuthorizeAsync(
                    User, parentFolder, Operations.Create)).Succeeded)
            {
                return Forbid( "You are not authorized to create subfolders for this folder.");
            }

            FolderDTO created = await _folderService.CreateAtFolder(subFolder, id,
                Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
            return CreatedAtAction(nameof(PostSubFolder), created);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FolderDTO>> GetFolder(Guid id)
        {
            FolderDTO folder = await _folderService.GetFolderById(id);

            if (folder == null)
                return NotFound();

            if (!(await _authorizationService.AuthorizeAsync(
                User, folder, Operations.Read)).Succeeded)
            {
                return Forbid("You are not authorized to access this folder.");
            }
            if ((await _authorizationService.AuthorizeAsync(
                User, folder, Operations.Create)).Succeeded)
            {
                folder.Links = _linkGenerator.GenerateAllLinks(User, folder);
            }
            else
            {
                folder.Links = _linkGenerator.GenerateRestrictedLinks(folder);
            }
            return Ok(folder);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FolderDTO>>> GetAvailableFolders()
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault
                (c => c.Type == ClaimTypes.NameIdentifier).Value);
            IEnumerable<FolderDTO> ownFolders = await _folderService.GetUserFolders(userId);

            if (!ownFolders.Any())
                return NoContent();
            return Ok(ownFolders);

        }

        [HttpPost("{id}/copy")]
        public async Task<ActionResult<FolderDTO>> CopyFolder(Guid id)
        {
            FolderDTO folder = await _folderService.GetFolderById(id);
            if (!(await _authorizationService.AuthorizeAsync(
            User, folder, Operations.Create)).Succeeded)
            {
                return Forbid("You are not authorized to create copies of this folder.");
            }
            FolderDTO copy = await _folderService.CopyFolder(id);
            return CreatedAtAction(nameof(CopyFolder), copy);
        }

        [HttpPut("{id}/move/{folderId}")]
        public async Task<IActionResult> MoveFolder(Guid id, Guid folderId)
        {

            FolderDTO folderToMove = await _folderService.GetFolderById(id);

            if (!(await _authorizationService.AuthorizeAsync(
                    User, folderToMove, Operations.Update)).Succeeded)
            {
                return Forbid("You are not authorized to move this folder.");
            }

            FolderDTO folderToMoveTo = await _folderService.GetFolderById(folderId);

            if (!(await _authorizationService.AuthorizeAsync(
                    User, folderToMoveTo, Operations.Update)).Succeeded)
            {
                return Forbid("You are not authorized to edit the targer parent folder.");
            }
            await _folderService.MoveToFolder(id, folderId);

            return NoContent();

        }

        [HttpPut("{id}/move/")]
        public async Task<IActionResult> MoveFolderToRoot(Guid id)
        {

            FolderDTO folderToMove = await _folderService.GetFolderById(id);

            if (!(await _authorizationService.AuthorizeAsync(
                    User, folderToMove, Operations.Update)).Succeeded)
            {
                return Forbid("You are not authorized to move this folder.");
            }

            await _folderService.MoveToRoot(id);

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            FolderDTO folder = await _folderService.GetFolderById(id);
            

            if (!(await _authorizationService.AuthorizeAsync(
                    User, folder, Operations.Delete)).Succeeded)
            {
                return Forbid("You are not authorized to delete this folder.");
            }
            await _folderService.Delete(id);
            return NoContent();

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FolderDTO newFolder)
        {
            FolderDTO currentFolder = await _folderService.GetFolderById(newFolder.Id);

            if (!(await _authorizationService.AuthorizeAsync(
                    User, currentFolder, Operations.Update)).Succeeded)
            {
                return Forbid("You are not authorized to edit this folder.");
            }
            await _folderService.Update(newFolder);
            return NoContent();
        }

    }
}