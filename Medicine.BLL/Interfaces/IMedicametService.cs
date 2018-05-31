using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.BLL.DTO;

namespace Medicine.BLL.Interfaces
{
    public interface IMedicametService
    {
        IEnumerable<MedicamentDTO> GetAll();
        void Create(MedicamentDTO item);
        void Update(MedicamentDTO item);
        void Delete(int id);
        MedicamentDTO Find(int id);
        MedicamentDTO Find(string name);
    }
}
