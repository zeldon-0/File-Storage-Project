using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class FolderShare
    {
        public virtual User User { get; set; }
        public virtual Folder Folder { get; set; }
    }
}
