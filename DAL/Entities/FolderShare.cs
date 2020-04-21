using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class FolderShare
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public Guid FolderId { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
