using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace BLL.Models
{
    class FileDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public string URL { get; set; }
        [Required]
        public int OwnerId { get; set; }
        public UserDTO Owner { get; set; }
        public Guid? FolderId { get; set; }
        public FolderDTO Folder { get; set; }
        public IEnumerable<UserDTO> UsersWithAccess { get; set; }
    }
}
