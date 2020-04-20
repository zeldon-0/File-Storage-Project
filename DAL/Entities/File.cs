using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class File
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public virtual User Owner { get; set; }
        public virtual Folder Folder { get; set; }
        public virtual IEnumerable<FileShare> FileShares { get; set; }
    }
}
