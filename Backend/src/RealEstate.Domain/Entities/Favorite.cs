using RealEstate.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Entities
{
    public class Favorite: AuditableEntity
    {
        public Favorite(int id) : base(id)
        {
        }

        public int UserId { get; set; }
        public int PropertyId { get; set; }


    }
}
