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
        {

        }
        public MedicamentController(IMedicametService service)
        {
            MedicametService = service;
        }
        // GET: Medicament
        public ActionResult Index()
        {
            return RedirectToAction("GetListOfMedicaments");
        }
        public ActionResult GetListOfMedicaments()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MedicamentDTO, MedicamentView>()).CreateMapper();
            var items = mapper.Map<IEnumerable<MedicamentDTO>, List<MedicamentView>>(MedicametService.GetAll());
            return View(items);
        }

        public ActionResult Update(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MedicamentDTO, MedicamentView>()).CreateMapper();
            var item = mapper.Map<MedicamentDTO, MedicamentView>(MedicametService.Find(id));
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(MedicamentView model)
        {
            MedicametService.Update(new MedicamentDTO() { Id = model.Id, Name = model.Name });
            return RedirectToAction("GetListOfMedicaments");
        }

        public ActionResult Delete(int id)
        {
            MedicametService.Delete(id);
            return RedirectToAction("GetListOfMedicaments");
        }
        public ActionResult Create()
        {
            return View(new MedicamentView() { Name="med1" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MedicamentView model)
        {
          MedicametService.Create(new MedicamentDTO() { Name = model.Name });
          return RedirectToAction("GetListOfmedicaments");
        }
    }
}