using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class FileShare
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public Guid FileId { get; set; }
        public virtual File File { get; set; }
    }
}
