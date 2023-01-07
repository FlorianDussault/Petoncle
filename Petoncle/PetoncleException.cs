using System;

namespace PetoncleDb;

public class PetoncleException : Exception
{
    public PetoncleException(string message) : base(message)
    {
        
    }
    
    public PetoncleException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}