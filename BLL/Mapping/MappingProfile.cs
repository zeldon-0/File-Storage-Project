using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using DAL.Entities;
using BLL.Models;
using System.Linq;
namespace BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ReverseMap();
            CreateMap<User, SignInDTO>()
                .ReverseMap();
            CreateMap<ShareStatus, ShareStatusDTO>()
                .ReverseMap();

            CreateMap<FileDTO, File>()
                .ForMember(dest => dest.FileShares,
                            opt => opt.MapFrom(dto => dto.UsersWithAccess))
                            .AfterMap((dto, entity) =>
                            {
                                foreach (var fileShare in entity.FileShares)
                                    fileShare.File = entity;
                            });

            CreateMap<File, FileDTO>()
                .ForMember(dest => dest.UsersWithAccess,
                            opt => opt.MapFrom(entity => entity.FileShares
                            .Select(fs => fs.User).ToList()));

            CreateMap<UserDTO, FileShare>()
                .ForMember(dest => dest.User,
                            opt => opt.MapFrom(user => user));

            CreateMap<FolderDTO, Folder>()
                .ForMember(dest => dest.FolderShares,
                opt => opt.MapFrom(dto => dto.UsersWithAccess))
                .AfterMap((dto, entity) =>
                {
                    foreach (var folderShare in entity.FolderShares)
                        folderShare.Folder = entity;
                });

            CreateMap<Folder, FolderDTO>()
                .ForMember(dest => dest.UsersWithAccess,
                            opt => opt.MapFrom(entity => entity.FolderShares
                            .Select(fs => fs.User).ToList()));

            CreateMap<UserDTO, FolderShare>()
                .ForMember(dest => dest.User,
                            opt => opt.MapFrom(user => user));



        }
    }
}
