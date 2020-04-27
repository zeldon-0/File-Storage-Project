﻿using System;
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
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public Guid? FolderId { get; set; }
        public virtual Folder Folder { get; set; }
        public virtual IEnumerable<FileShare> FileShares { get; set; }
    }
}
