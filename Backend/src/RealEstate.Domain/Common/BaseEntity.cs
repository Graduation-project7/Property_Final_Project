using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }

        protected BaseEntity(int id)
        {
            Id = id;
        }
    }
}
