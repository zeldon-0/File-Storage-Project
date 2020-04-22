using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface ICompositeEntity
    {
        Guid ResourceId { get; set; }
        int UserId { get; set; }
    }
}
