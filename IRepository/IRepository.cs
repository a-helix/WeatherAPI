using System;
using System.Collections.Generic;

namespace Repository
{
    public interface IRepository<T>
        where T : class
    {
        T Get(string location);
        void Insert(T coordinates);
        void Update(string geocoordinates, string newArea);
        void Delete(string location);
    }
}
