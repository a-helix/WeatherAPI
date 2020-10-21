using System;
using System.Collections.Generic;

namespace Repository
{
    public interface IRepository<T>
        where T : class
    {
        void Create(T coordinates);
        T Read(string location);
        void Update(string oldArea, string newArea);
        void Delete(string location);
    }
}
