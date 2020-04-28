using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class AuthUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
