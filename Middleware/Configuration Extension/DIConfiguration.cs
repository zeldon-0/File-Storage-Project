﻿using DAL.UOW;
using DAL.Interfaces;
using BLL.Interfaces;
using DAL.Entities;
using BLL.Services;
using BLL.Mapping;
using BLL.AuthorizationHandlers;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using BLL.Link_Generation;
using BLL.Models;

namespace Middleware.Configuration_Extension
{
    public static class DIConfiguration
    {
        public static void ConfigureDI(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFolderService, FolderService>();
            services.AddScoped<IShareStatusService, ShareStatusService>();
            services.AddScoped<ISharingService, SharingService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthorizationHandler, FolderAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, FileAuthorizationHandler>();

            services.AddScoped<ILinkGenerator<FolderDTO>, FolderLinkGenerator>();
            services.AddScoped<ILinkGenerator<FileDTO>, FileLinkGenerator>();
            services.AddScoped<ILinkGenerator<PrivateUserDTO>, AccountLinkGenerator>();

            MapperConfiguration mappingConfig = new MapperConfiguration(mc=>
                    mc.AddProfile(new MappingProfile())
            );
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
