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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual IEnumerable<File> Files { get; set; }
        public virtual IEnumerable<Folder> Folders { get; set; }
        public virtual IEnumerable<FileShare> FileShares { get; set; }
        public virtual IEnumerable<FolderShare> FolderShares { get; set; }
    }
}
