using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using DAL.Interfaces;

namespace DAL.Entities
{
    public class User : IdentityUser<int> , IEntity<int>
    {

        public IEnumerable<File> Files { get; set; }
        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<FileShare> FileShares { get; set; }
        public IEnumerable<FolderShare> FolderShares { get; set; }
    }
}
