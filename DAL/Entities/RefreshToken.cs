using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using DAL.Interfaces;

namespace DAL.Entities
{
    public class RefreshToken : IEntity<int>
    {
        public int Id {get; set;}
        public string Token {get; set;}
        public DateTime Expiration {get; set;}
        public int UserId {get; set;}
        public User User {get; set;}
    }
}
