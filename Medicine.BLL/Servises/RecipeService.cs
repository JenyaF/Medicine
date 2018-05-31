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
            return Database.Recipes.GetAll(patientId).Select(x=>new RecipeDTO() { AmountPerDay = x.AmountPerDay, Id = x.Id, MedicamentId = x.MedicamentId, PatientId = x.PatientId, Volume = x.Volume,FinishDate=x.FinishDate,StartDate=x.StartDate });
        }
       public  void Create(RecipeDTO item)
        {
             Medicament medicament= Database.Medicaments.Find(item.MedicamentName);
            if (medicament != null)
            {
                Database.Recipes.Create(new Recipe() { AmountPerDay = (int)item.AmountPerDay, Id = (int)item.Id, MedicamentId = medicament.Id, PatientId = item.PatientId, Volume = item.Volume, FinishDate = item.FinishDate, StartDate = item.StartDate });
                Database.Save();
            }
        }

        public void Update(RecipeDTO item)
        {
            Medicament medicament = Database.Medicaments.Find(item.MedicamentName);
            if (medicament != null)
            {
                Database.Recipes.Update(new Recipe() { AmountPerDay = (int)item.AmountPerDay, Id = (int)item.Id, MedicamentId = (int)medicament.Id, PatientId = item.PatientId, Volume = item.Volume, FinishDate = item.FinishDate, StartDate = item.StartDate });
                Database.Save();
            }
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
            item.MedicamentName= Database.Medicaments.Find((int)item.MedicamentId)?.Name;
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
