using RealEstate.Domain.Common;
using RealEstate.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Entities
{
    public class Property : AuditableEntity
    {
        public Property(int id) : base(id)
        {
        }

        public string? Title { get; private set; }

        public string? Description { get; private set; }

        public PropertyType? PropertyType { get; private set; }

        public string? Location { get; private set; }

        public decimal? Price { get; private set; }

        public User? User { get; set; } 


        public IEnumerable<PropertyImg>? PropertyImgs { get; set; } = new List<PropertyImg>();

    }
}
