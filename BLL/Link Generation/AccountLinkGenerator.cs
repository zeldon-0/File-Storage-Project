using System;
using System.Collections.Generic;
using System.Text;
using BLL.Models;
using BLL.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace BLL.Link_Generation
{
    public class AccountLinkGenerator : ILinkGenerator<PrivateUserDTO>
    {
        public IEnumerable<Link> GenerateAllLinks(ClaimsPrincipal user, PrivateUserDTO resource)
        {
            List<Link> links = new List<Link>()
            {
                new Link($"https://localhost:5001/api/account/{resource.Id}",
                "self", "GET"),
                new Link($"https://localhost:5001/api/account/",
                "update_account", "PUT"),
                new Link($"https://localhost:5001/api/account/{resource.Id}",
                "delete_account", "DELETE"),
            };

            if (user.Claims.Where(c => c.Type == ClaimTypes.Role).Any(r => r.Value == "Corporate"))
            {
                links.Add(new Link($"https://localhost:5001/api/account/{resource.Id}/revertUpgrade",
                "downgrade_from_corporate", "PUT"));
            }
            else
            {
                links.Add(new Link($"https://localhost:5001/api/account/{resource.Id}/upgrade",
                "upgrade_to_corporate", "PUT"));
            }
            if(user.Claims.FirstOrDefault
                (c => c.Type == ClaimTypes.NameIdentifier).Value 
                == resource.Id.ToString())
            {
                links.Add(new Link($"https://localhost:5001/api/account/{resource.Id}/updatePassword?oldPassword=old&newPassword=new",
                    "update_password", "PUT"));
            }

            return links;
        }

        public IEnumerable<Link> GenerateRestrictedLinks(PrivateUserDTO resource)
        {
            List<Link> links = new List<Link>()
            {
                new Link($"https://localhost:5001/api/account/{resource.Id}",
                "self", "GET")
            };
            return links;
        }
    }
}

