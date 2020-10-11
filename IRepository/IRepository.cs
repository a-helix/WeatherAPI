using System;
using System.Collections.Generic;

namespace IRepo
{
    public interface IRepository<T>
        where T : class
    {
        T Get(string location);
        void Create(T coordinates);
        void Update(string geocoordinates, string newArea);
        void Delete(string location);
        bool Contains(string location);
    }
}
