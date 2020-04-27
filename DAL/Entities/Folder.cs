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
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public Guid? ParentId { get; set; }
        public virtual Folder Parent { get; set; }
        public virtual IEnumerable<Folder> Subfolders { get; set; }
        public virtual IEnumerable<File> Files { get; set; }
        public virtual IEnumerable<FolderShare> FolderShares { get; set; }
    }
}
