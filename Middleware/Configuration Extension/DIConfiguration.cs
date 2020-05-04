﻿using DAL.UOW;
using DAL.Interfaces;
using BLL.Interfaces;
using DAL.Entities;
using BLL.Services;
using BLL.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using AutoMapper;


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

            MapperConfiguration mappingConfig = new MapperConfiguration(mc=>
                    mc.AddProfile(new MappingProfile())
            );
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
