using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
    public interface IEntity<T> where T: IComparable
    {
        public T Id { get; set; }

    }
}
