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


        public FilesController(IFileService fileService,
            IFolderService folderService,
            IAuthorizationService authorizationService,
            ISharingService sharingService)
        {
            _fileService = fileService;
            _folderService = folderService;
            _authorizationService = authorizationService;
            _sharingService = sharingService;
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


    }
}
