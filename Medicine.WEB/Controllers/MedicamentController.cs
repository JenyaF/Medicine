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
            return View();
        }
    }
}