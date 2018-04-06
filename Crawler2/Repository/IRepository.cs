﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crawler2
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object Id);
        T Insert(T obj);
        void Delete(object Id);
        T Update(T obj);
        void Save();
    }
}
