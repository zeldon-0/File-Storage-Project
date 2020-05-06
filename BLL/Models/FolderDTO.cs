using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace BLL.Models
{
    public class FolderDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ShareStatusDTO ShareStatus { get; set; }
        public int OwnerId { get; set; }
        public  UserDTO Owner { get; set; }
        public Guid? ParentId { get; set; }
        public IEnumerable<FolderDTO> Subfolders { get; set; }
        public IEnumerable<FileDTO> Files { get; set; }
        public IEnumerable<UserDTO> UsersWithAccess { get; set; }
    }
}
