using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;
namespace DAL.Entities
{
    public class FolderShare : ICompositeEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public Guid FolderId { get; set; }
        public Folder Folder { get; set; }

        public Guid ResourceId
        {
            get
            {
                return FolderId;
            }
            set
            {
                FolderId = value;
            }
        }
    }
}
