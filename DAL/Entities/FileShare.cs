using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities
{
    public class FileShare
    {
        public virtual User User { get; set; }
        public virtual File File { get; set; }
    }
}
