using System.Collections.Generic;

namespace PetoncleDb;

public interface IPetoncleEnumerable<T> : IEnumerable<T> {
    
    int Count { get; }
    
}