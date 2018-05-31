using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.DAL.Entities;

namespace Medicine.DAL.Interfaces
{
    public interface IMedicamentManeger
    {
        void Create(Medicament item);
        void Update(Medicament item);
        void Delete(int id);
        IEnumerable<Medicament> GetAll();
        Medicament Find(int id);
        Medicament Find(string name);

    }
}
