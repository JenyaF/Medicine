using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Medicine.WEB.Models;
using Medicine.BLL.DTO;
using System.Security.Claims;
using Medicine.BLL.Interfaces;
using Medicine.BLL.Infrastructure;
using AutoMapper;
using System.Linq;
using System;
namespace Medicine.WEB.Controllers
{
    public class RecipeController : Controller
    {

        public IRecipeService RecipeService { get; set; }
        public RecipeController()
        {

        }
        public RecipeController(IRecipeService service)
        {
            RecipeService = service;
        }

        public ActionResult Index(string id)
        {
            return RedirectToAction($"GetListOfRecipes/{id}");
        }
        public ActionResult GetListOfRecipes(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RecipeDTO, RecipeView>()).CreateMapper();
            var items = mapper.Map<IEnumerable<RecipeDTO>, List<RecipeView>>(RecipeService.GetAll(id));
            HttpContext.Session["patientId"] = id;
            return View(items);
        }

        public ActionResult Update(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RecipeDTO, RecipeView>()).CreateMapper();
            var item = mapper.Map<RecipeDTO, RecipeView>(RecipeService.Find(id));
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(RecipeView model)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RecipeView,RecipeDTO >()).CreateMapper();
            var item = mapper.Map<RecipeView,RecipeDTO >(model);
            RecipeService.Update(item);
            return RedirectToAction("GetListOfRecipes");
        }

        public ActionResult Delete(int id)
        {
            RecipeDTO recipe=RecipeService.Find(id);
            RecipeService.Delete(id);
            return RedirectToAction($"GetListOfRecipes/{recipe.PatientId}");
        }
        public ActionResult Create()
        {
            var patientId = HttpContext.Session["patientId"].ToString();
           
            return View(new RecipeView() {MedicamentName="medicament1", AmountPerDay = 2, FinishDate = DateTime.Now.Date.AddMonths(2).ToShortDateString(), StartDate = DateTime.Now.Date.ToShortDateString(), Volume = 1, PatientId = patientId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecipeView model,string nameOFMedicament)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<RecipeView, RecipeDTO>()).CreateMapper();
            var item = mapper.Map<RecipeView, RecipeDTO>(model);
          //  RecipeService
            RecipeService.Create(item);
            return RedirectToAction("GetListOfRecipes");
        }
    }
}