using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicine.BLL.Interfaces;
using Medicine.DAL.Interfaces;
using Medicine.BLL.DTO;
using Medicine.DAL.Entities;
using AutoMapper;
namespace Medicine.BLL.Services
{
    public class RecipeService:IRecipeService
    {
        public IUnitOfWork Database { get; set; }

        public RecipeService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public IEnumerable<RecipeDTO> GetAll(string patientId)
        {
            return Database.Recipes.GetAll(patientId).Select(x=>new RecipeDTO() { AmountPerDay = x.AmountPerDay, Id = x.Id, MedicamentId = x.MedicamentId, PatientId = x.PatientId, Volume = x.Volume });
        }
       public  void Create(RecipeDTO item)
        {
            Database.Recipes.Create(new Recipe() { AmountPerDay = item.AmountPerDay, Id = item.Id, MedicamentId = item.MedicamentId, PatientId = item.PatientId, Volume = item.Volume });
            Database.Save();
        }

        public void Update(RecipeDTO item)
        {
            Database.Recipes.Update(new Recipe() { AmountPerDay = item.AmountPerDay, Id = item.Id, MedicamentId = item.MedicamentId, PatientId = item.PatientId, Volume = item.Volume });
            Database.Save();
        }

        public void Delete(int id)
        {
            Database.Recipes.Delete(id);
            Database.Save();
        }

        public RecipeDTO Find(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Recipe,RecipeDTO >()).CreateMapper();
            var item = mapper.Map<Recipe,RecipeDTO >(Database.Recipes.Find(id));
            return item;
        }
        public RecipeDTO Find(string nameOfMedicament,string patientId)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Recipe, RecipeDTO>()).CreateMapper();
            var item = mapper.Map<Recipe, RecipeDTO>(Database.Recipes.Find(nameOfMedicament, patientId));
            return item;
        }
    }
}
