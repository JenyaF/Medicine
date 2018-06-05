using System.Collections.Generic;
using System.Web.Mvc;
using Medicine.WEB.Models;
using Medicine.BLL.DTO;
using Medicine.BLL.Interfaces;
using AutoMapper;

namespace Medicine.WEB.Controllers
{
    public class RecipeController : Controller
    {

        public IRecipeService RecipeService { get; set; }
        public RecipeController()
        {   }

        public RecipeController(IRecipeService service)
        {
            RecipeService = service;
        }

        public ActionResult Index(string id)
        {
            return RedirectToAction($"GetListOfRecipes/{id}");
        }

        [Authorize(Roles = "admin, doctor, patient")]
        public ActionResult GetListOfRecipes(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RecipeDTO, RecipeView>()).CreateMapper();
            var items = mapper.Map<IEnumerable<RecipeDTO>, List<RecipeView>>(RecipeService.GetAll(id));
            HttpContext.Session["patientId"] = id;
            return View(items);
        }

        [Authorize(Roles = "admin, doctor")]
        public ActionResult Update(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RecipeDTO, RecipeView>()).CreateMapper();
            var item = mapper.Map<RecipeDTO, RecipeView>(RecipeService.Find(id));
            return View(item);
        }
        [HttpPost]
        public ActionResult Update(RecipeView model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RecipeView, RecipeDTO>()).CreateMapper();
                var item = mapper.Map<RecipeView, RecipeDTO>(model);
                RecipeService.Update(item);
                return RedirectToAction($"GetListOfRecipes/{item.PatientId}");
            }
            return Update(model);
        }

        public ActionResult Delete(int id)
        {
            RecipeDTO recipe=RecipeService.Find(id);
            RecipeService.Delete(id);
            return RedirectToAction($"GetListOfRecipes/{recipe.PatientId}");
        }

        [Authorize(Roles = "admin, doctor")]
        public ActionResult Create()
        {
            var patientId = HttpContext.Session["patientId"].ToString(); 
            return View(new RecipeView() {MedicamentName="medicament1", AmountPerDay = 2, FinishDate = "2018-08-13", StartDate ="2018-07-13", Volume = 1, PatientId = patientId });
        }

        [HttpPost]
        public ActionResult Create(RecipeView model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RecipeView, RecipeDTO>()).CreateMapper();
                var item = mapper.Map<RecipeView, RecipeDTO>(model);
                RecipeService.Create(item);
                return RedirectToAction($"GetListOfRecipes/{item.PatientId}");
            }
            return Create(model);
        }
    }
}