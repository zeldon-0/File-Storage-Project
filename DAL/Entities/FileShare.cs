using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;

namespace DAL.Entities
{
    public class FileShare : ICompositeEntity 
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid FileId { get; set; }
        public File File { get; set; }

        public Guid ResourceId { 
            get
            {
                return FileId;
            }
            set
            {
                FileId = value;
            }
        }
    }
}
