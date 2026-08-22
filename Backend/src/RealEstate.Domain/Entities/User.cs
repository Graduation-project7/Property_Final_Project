using RealEstate.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Entities
{
    public class User : AuditableEntity
    {
        public User(int id) : base(id)
        {
        }

        public string? FirstName { get; private set; }

        public string? LastName { get; private set; }

        public string? Email { get; private set; }

        public string? Password { get; set; }

        public IEnumerable<Property>? Properties { get; set; } = new List<Property>();

    }
}
