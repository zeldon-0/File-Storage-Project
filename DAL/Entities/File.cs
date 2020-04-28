using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;

namespace DAL.Entities
{
    public class File : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public ShareStatus ShareStatus { get; set; }
        public int OwnerId { get; set; }
        public  User Owner { get; set; }
        public Guid? FolderId { get; set; }
        public  Folder Folder { get; set; }
        public  IEnumerable<FileShare> FileShares { get; set; }
    }
}
