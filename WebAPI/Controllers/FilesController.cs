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
using System.Security.Policy;
using Microsoft.AspNetCore.Routing;

namespace WebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]

    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IFolderService _folderService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ISharingService _sharingService;
        private readonly ILinkGenerator<FileDTO> _linkGenerator;

        public FilesController(IFileService fileService,
            IFolderService folderService,
            IAuthorizationService authorizationService,
            ISharingService sharingService,
            ILinkGenerator<FileDTO> linkGenerator)
        {
            _fileService = fileService;
            _folderService = folderService;
            _authorizationService = authorizationService;
            _sharingService = sharingService;
            _linkGenerator = linkGenerator;
        }

        [HttpPost("files")]
        public async Task<ActionResult<FileDTO>> Post([FromBody] FileDTO file)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided folder model is not valid.");
            FileDTO created = await _fileService.CreateAtRoot(file,
                Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
            return CreatedAtAction(nameof(Post), created);
        }
        [Route("folders/{id}/files")]
        [HttpPost]
        public async Task<ActionResult<FileDTO>> PostAtFolder(Guid id, [FromBody] FileDTO file)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided folder model is not valid.");

            FolderDTO folder = await _folderService.GetFolderById(id);

            if (!(await _authorizationService.AuthorizeAsync(
                User, folder, Operations.Create)).Succeeded)
            {
                return Unauthorized("You are not authorized to post files at this folder.");
            }

            FileDTO created = await _fileService.CreateAtFolder(file, id,
                Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value));
            return CreatedAtAction(nameof(PostAtFolder), created);
        }

        [HttpGet("files/{id}")]
        public async Task<ActionResult<FileDTO>> GetById(Guid id)
        {
            FileDTO file = await _fileService.GetFileById(id);
            if (!(await _authorizationService.AuthorizeAsync(
                User, file, Operations.Read)).Succeeded)
            {
                return Unauthorized("You are not authorized to access the file.");
            }
            if ((await _authorizationService.AuthorizeAsync(
                User, file, Operations.Create)).Succeeded)
            {
                file.Links = _linkGenerator.GenerateAllLinks(User, file);
            }
            else
            {
                file.Links = _linkGenerator.GenerateRestrictedLinks(file);
            }

            return Ok(file);
        }
        [HttpPost("files/{id}/copy")]
        public async Task<ActionResult<FileDTO>> CopyFile(Guid id)
        {
            FileDTO file = await _fileService.GetFileById(id);
            if (!(await _authorizationService.AuthorizeAsync(
                User, file, Operations.Create)).Succeeded)
            {
                return Unauthorized("You are not authorized to access the file.");
            }

            FileDTO fileCopy = await _fileService.CopyFile(id);
            return CreatedAtAction(nameof(CopyFile) , fileCopy);
        }

        [HttpPut("files/{fileId}/move/{folderId}")]
        public async Task<IActionResult> MoveFile(Guid fileId, Guid folderId)
        {
            FileDTO file = await _fileService.GetFileById(fileId);
            if (!(await _authorizationService.AuthorizeAsync(
                User, file, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to access the file.");
            }

            FolderDTO folder = await _folderService.GetFolderById(folderId);
            if (!(await _authorizationService.AuthorizeAsync(
                User, folder, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to access the folder.");
            }

            await _fileService.MoveToFolder(fileId, folderId);
            return NoContent();
        }

        [HttpGet("files")]
        public async Task<ActionResult<IEnumerable<FolderDTO>>> GetAvailableFiles()
        {
            int userId = Int32.Parse(User.Claims.FirstOrDefault
                (c => c.Type == ClaimTypes.NameIdentifier).Value);
            IEnumerable<FileDTO> ownFiles = await _fileService.GetUserFiles(userId);
            /*IEnumerable<FileDTO> sharedFiles = await _sharingService.GetSharedFiles(userId);
            List<FileDTO> allFiles = ownFiles.Concat(sharedFiles).ToList();
            */
            if (!ownFiles.Any())
                return NoContent();
            return Ok(ownFiles);
        }
        [HttpPut("files")]
        public async Task<IActionResult> UpdateFile([FromBody] FileDTO file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("The folder model is not valid.");
            }
            FileDTO currentFile = await _fileService.GetFileById(file.Id);
            if(!(await _authorizationService.AuthorizeAsync(
                User, currentFile, Operations.Update)).Succeeded)
            {
                return Unauthorized("You are not authorized to edit the file.");
            }

            await _fileService.Update(file);
            return NoContent();
        }

        [HttpDelete("files/{id}")]
        public async Task<IActionResult> DeleteFile(Guid id)
        {
            FileDTO file = await _fileService.GetFileById(id);
            if (!(await _authorizationService.AuthorizeAsync(
                User, file, Operations.Delete)).Succeeded)
            {
                return Unauthorized("You are not authorized to delete the file.");
            }

            await _fileService.Delete(id);
            return NoContent();
        }
    }
}
