using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.DAL.Entities;

namespace Medicine.DAL.Interfaces
{
    public interface IRecipeManeger
    {
        void Create(Recipe item);
        void Update(Recipe item);
        void Delete(int id);
        IEnumerable<Recipe> GetAll(string patientId);
    }
}
