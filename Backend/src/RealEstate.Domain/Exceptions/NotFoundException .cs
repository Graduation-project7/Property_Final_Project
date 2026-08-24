using System;

namespace RealEstate.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with Id ({key}) was not found.")
    {
    }
}
