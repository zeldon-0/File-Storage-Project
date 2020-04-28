using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;

namespace DAL.Entities
{
    public class Folder : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ShareStatus ShareStatus { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public Guid? ParentId { get; set; }
        public Folder Parent { get; set; }
        public IEnumerable<Folder> Subfolders { get; set; }
        public IEnumerable<File> Files { get; set; }
        public IEnumerable<FolderShare> FolderShares { get; set; }
    }
}
