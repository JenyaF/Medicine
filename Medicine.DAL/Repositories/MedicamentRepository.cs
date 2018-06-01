using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.DAL.Interfaces;
using Medicine.DAL.Entities;
using Medicine.DAL.EF;
using System.Data.Entity;
namespace Medicine.DAL.Repositories
{
    public class MedicamentRepository:IMedicamentManeger
    {
        private ApplicationContext db;
        public MedicamentRepository(ApplicationContext context)
        {
            this.db = context;
        }
        public void Create(Medicament medicament)
        {
            db.Medicaments.Add(medicament);
        }
        public IEnumerable<Medicament> GetAll()
        {
            return db.Medicaments.ToList();
        }
        public void Delete(int id)
        {
            var item = db.Medicaments.Find(id);
            if (item != null)
            {
                db.Medicaments.Remove(item);
            }
        }
        public void Update(Medicament item)
        {
           var newItem= db.Medicaments.Find(item.Id);
            newItem.Name = item.Name;
            db.Entry(newItem).State = EntityState.Modified;
        }
        public Medicament Find(int id)
        {
          return db.Medicaments.Find(id);
        }
        public Medicament Find(string name)
        {
            return db.Medicaments.FirstOrDefault(x=>x.Name==name);
        }
    }
}
