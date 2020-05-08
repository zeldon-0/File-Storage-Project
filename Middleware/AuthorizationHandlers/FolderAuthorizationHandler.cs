using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BLL.AuthorizationHandlers
{
    public class FolderAuthorizationHandler
        : AuthorizationHandler<OperationAuthorizationRequirement, FolderDTO>
    {
        private readonly IUnitOfWork _uow;
        public FolderAuthorizationHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }
        protected override  Task HandleRequirementAsync(
             AuthorizationHandlerContext context, 
            OperationAuthorizationRequirement requirement, FolderDTO resource)
        {
            int userId = Int32.Parse(
                context.User.Claims.FirstOrDefault
                (c => c.Type == ClaimTypes.NameIdentifier).Value);
            if (requirement.Name == Operations.Read.Name)
            {
                if (userId == resource.OwnerId
                    || resource.ShareStatus == ShareStatusDTO.Shareable
                    || Task.Run(() => _uow.FolderShares.FolderShareExists(resource.Id, userId)).Result
                    || context.User.Claims.Where(c => c.Type == ClaimTypes.Role)
                        .Any(c => c.Value == "Admin"))
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                if (userId == resource.OwnerId
                    || context.User.Claims.Where(
                        c => c.Type == ClaimTypes.Role)
                        .Any( c =>c.Value == "Admin"))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
    public class FolderAccessRequirement : IAuthorizationRequirement { }

}
