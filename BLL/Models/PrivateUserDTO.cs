using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BLL.Models
{
    public class PrivateUserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public IEnumerable<string> Roles { get; set; }
    }
}
