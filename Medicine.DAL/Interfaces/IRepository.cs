using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.DAL.Entities;

namespace Medicine.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
       // IEnumerable<T> GetAll(string PatientId);
        IEnumerable<T> GetAll();
        void Create(T item);
        void Update(T item);
        void Delete(int id);
    }
}

