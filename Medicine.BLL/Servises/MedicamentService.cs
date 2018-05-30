using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.BLL.DTO;
using Medicine.DAL.Entities;
using Medicine.BLL.Interfaces;
using Medicine.DAL.Interfaces;

namespace Medicine.BLL.Services
{
   public  class MedicamentService:IMedicametService
    {
        public IUnitOfWork Database { get; set; }

        public MedicamentService (IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<MedicamentDTO> GetAll()
        {
            return Database.Medicaments.GetAll().Select(x => new MedicamentDTO() { Id = x.Id, Name = x.Name });
        }
        public void Create(MedicamentDTO item)
        {
            Database.Medicaments.Create(new Medicament() { Id = item.Id, Name = item.Name });
            Database.Save();
        }
        public void Update(MedicamentDTO item)
        {
            Database.Medicaments.Update(new Medicament() { Id = item.Id, Name = item.Name });
            Database.Save();
        }
       public void Delete(int id)
        {
            Database.Medicaments.Delete(id);
            Database.Save();
        }
        public MedicamentDTO Find(int id)
        {
            Medicament item= Database.Medicaments.Find(id);
            return new MedicamentDTO() { Id = item.Id, Name = item.Name };
        }
    }
}
