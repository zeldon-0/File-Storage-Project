using System;
using System.Collections.Generic;
using System.Text;
using BLL.Models;
using BLL.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
namespace BLL.Link_Generation
{
    public class FileLinkGenerator : ILinkGenerator<FileDTO>
    {
        public IEnumerable<Link> GenerateAllLinks(ClaimsPrincipal user, FileDTO resource)
        {
            List<Link> links = new List<Link>()
            {
                new Link($"https://localhost:5001/api/files/{resource.Id}",
                "self", "GET"),
                new Link($"https://localhost:5001/api/files/{resource.Id}/copy",
                "copy_file", "POST"),
                new Link($"https://localhost:5001/api/files/{resource.Id}/move/folderId",
                "move_to_folder", "PUT"),
                new Link($"https://localhost:5001/api/files/{resource.Id}",
                "delete_folder", "DELETE"),
                new Link($"https://localhost:5001/api/files/",
                "update_folder", "PUT"),
                new Link($"https://localhost:5001/api/files/{resource.Id}/sharingInfo",
                "get_shared_users", "GET")
            };

            if (resource.ShareStatus == ShareStatusDTO.Private)
                links.Add(new Link($"https://localhost:5001/api/files/{resource.Id}/share",
                "make_file_shareable", "PUT"));
            else
                links.Add(new Link($"https://localhost:5001/api/files/{resource.Id}/unshare",
                "make_file_private", "PUT"));

            if (user.Claims.Where(c => c.Type == ClaimTypes.Role)
                .Any(r => r.Value == "Admin" || r.Value == "Corporate"))
            {
                links.Add(new Link($"https://localhost:5001/api/files/{resource.Id}/share/userName",
                    "share_file_with_user", "PUT"));
                links.Add(new Link($"https://localhost:5001/api/files/{resource.Id}/unshare/userName",
                    "unshare_file_with_user", "PUT"));
            }

            return links;
        }

        public IEnumerable<Link> GenerateRestrictedLinks(FileDTO resource)
        {
            List<Link> links = new List<Link>()
            {
                new Link($"https://localhost:5001/api/files/{resource.Id}",
                "self", "GET")
            };
            return links;
        }
    }
}

