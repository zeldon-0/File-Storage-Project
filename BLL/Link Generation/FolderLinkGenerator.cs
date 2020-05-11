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
    public class FolderLinkGenerator : ILinkGenerator<FolderDTO>
    {
        public IEnumerable<Link> GenerateAllLinks(ClaimsPrincipal user, FolderDTO resource)
        {
            List<Link> links = new List<Link>()
            {
                new Link($"https://localhost:5001/api/folders/{resource.Id}",
                "self", "GET"),
                new Link($"https://localhost:5001/api/folders/{resource.Id}/subfolders",
                "post_subfolder", "POST"),
                new Link($"https://localhost:5001/api/folders/{resource.Id}/copy",
                "copy_folder", "POST"),
                new Link($"https://localhost:5001/api/folders/{resource.Id}/move/folderId",
                "move_to_folder", "PUT"),
                new Link($"https://localhost:5001/api/folders/{resource.Id}",
                "delete_folder", "DELETE"),
                new Link($"https://localhost:5001/api/folders/{resource.Id}/move/folderId",
                "move_to_folder", "PUT"),
                new Link($"https://localhost:5001/api/folders/",
                "update_folder", "PUT"),
                new Link($"https://localhost:5001/api/folders/{resource.Id}/sharingInfo",
                "get_shared_users", "GET")
            };

            if (resource.ShareStatus == ShareStatusDTO.Private)
                links.Add(new Link($"https://localhost:5001/api/folders/{resource.Id}/share",
                "make_folder_shareable", "PUT"));
            else
                links.Add(new Link($"https://localhost:5001/api/folders/{resource.Id}/unshare",
                "make_folder_private", "PUT"));

            if (user.Claims.Where(c => c.Type == ClaimTypes.Role)
                .Any(r => r.Value == "Admin" || r.Value == "Corporate"))
            {
                links.Add(new Link($"https://localhost:5001/api/folders/{resource.Id}/share/userName",
                    "share_folder_with_user", "PUT"));
                links.Add(new Link($"https://localhost:5001/api/folders/{resource.Id}/unshare/userName",
                    "unshare_folder_with_user", "PUT"));
            }

            return links;
        }

        public IEnumerable<Link> GenerateRestrictedLinks(FolderDTO resource)
        {
            List<Link> links = new List<Link>()
            {
                new Link($"https://localhost:5001/api/folders/{resource.Id}",
                "self", "GET") 
            };
            return links;
        }
    }
}
