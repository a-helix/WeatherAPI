using System;
using System.Collections.Generic;

namespace IRepository
{
    interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> getList();
        T get(int id);
        void create(T item);
        void update(T item);
        void delete(int id);
        void save();
        bool contains(T item);
    }
}
