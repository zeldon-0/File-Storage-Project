using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using BLL.Models;

namespace BLL.Interfaces
{
    public interface ILinkGenerator<T>
    {
        IEnumerable<Link> GenerateRestrictedLinks(T resource);
        IEnumerable<Link> GenerateAllLinks(ClaimsPrincipal user, T resource);
    }
}
