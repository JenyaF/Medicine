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

namespace Medicine.WEB.Controllers
{
    public class MedicamentController : Controller
    {
        public IMedicametService MedicametService { get; set; }
        public MedicamentController()
        {   }

        public MedicamentController(IMedicametService service)
        {
            MedicametService = service;
        }

        public ActionResult Index()
        {
            return RedirectToAction("GetListOfMedicaments");
        }
        [Authorize(Roles = "admin")]
        public ActionResult GetListOfMedicaments()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MedicamentDTO, MedicamentView>()).CreateMapper();
            var items = mapper.Map<IEnumerable<MedicamentDTO>, List<MedicamentView>>(MedicametService.GetAll());
            return View(items);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Update(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MedicamentDTO, MedicamentView>()).CreateMapper();
            var item = mapper.Map<MedicamentDTO, MedicamentView>(MedicametService.Find(id));
            return View(item);
        }

        [HttpPost]
        public ActionResult Update(MedicamentView model)
        {
            if (ModelState.IsValid)
            {
                MedicametService.Update(new MedicamentDTO() { Id = model.Id, Name = model.Name });
                return RedirectToAction("GetListOfMedicaments");
            }
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            MedicametService.Delete(id);
            return RedirectToAction("GetListOfMedicaments");
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            return View(new MedicamentView() { Name="med1" });
        }

        [HttpPost]
        public ActionResult Create(MedicamentView model)
        {
            if (ModelState.IsValid)
            {
                MedicametService.Create(new MedicamentDTO() { Name = model.Name });
                return RedirectToAction("GetListOfmedicaments");
            }
            return Create(model);
        }
    }
}