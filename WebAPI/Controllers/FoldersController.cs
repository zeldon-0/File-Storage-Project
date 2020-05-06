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
using DAL.Entities;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FoldersController : ControllerBase
    {
        private IFolderService _folderService;

        public FoldersController(IFolderService folderService)
        {
            _folderService = folderService;
        }

        [HttpPost]
        public async Task<ActionResult<FolderDTO>> Post([FromBody] FolderDTO folder)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided folder model is not valid.");
            FolderDTO created = await _folderService.CreateAtRoot(folder,
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            return CreatedAtAction(nameof(Post), created);
        }
        [HttpPost("{id}")]
        public async Task<ActionResult<FolderDTO>> PostSubFolder(Guid id, [FromBody] FolderDTO subFolder)
        {
            if (!ModelState.IsValid)
                return BadRequest("The provided folder model is not valid.");
            FolderDTO created = await _folderService.CreateAtFolder(subFolder, id,
                User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);
            return CreatedAtAction(nameof(Post), created);
        }

    }
}