using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.BLL.DTO;

namespace Medicine.BLL.Interfaces
{
    public interface IRecipeService
    {
        IEnumerable<RecipeDTO> GetAll(string PatientId);
        void Create(RecipeDTO item);
        void Update(RecipeDTO item);
        void Delete(int id);
        RecipeDTO Find(int id);
    }
}
