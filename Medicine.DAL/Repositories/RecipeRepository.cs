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
    public class RecipeRepository:IRecipeManeger
    {
        private ApplicationContext db;
        public RecipeRepository (ApplicationContext context)
        {
            this.db = context;
        }
        public void Create(Recipe recipe)
        {
            db.Recipes.Add(recipe);
        }
        public IEnumerable<Recipe> GetAll(string patientId)
        {
            return db.Recipes.Where(x=>x.PatientId==patientId).ToList();
        }
        public void Delete(int id)
        {
            var item = db.Recipes.Find(id);
            if (item != null)
            {
                db.Recipes.Remove(item);
            }
        }
        public void Update(Recipe item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
