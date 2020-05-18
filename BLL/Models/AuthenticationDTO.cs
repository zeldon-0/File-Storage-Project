using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BLL.Models
{
    public class AuthenticationDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public IEnumerable<Link> Links { get; set; }
        public string Token {get; set;}
        public string RefreshToken {get;set;}
    }
}
