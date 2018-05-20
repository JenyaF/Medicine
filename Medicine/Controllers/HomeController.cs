using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Medicine;
using Medicine.Models;
using System.Web.Security;
using System.Data.Entity;
using System.Reflection;
namespace Medicine.Controllers
{
    public class HomeController : Controller
    {
        MedicineContext db = new MedicineContext();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EnterAsDoctor([Bind(Include = "Password, Phone")] Doctor doctor)
        {
            Doctor d = db.Doctors.FirstOrDefault(x => x.Phone == doctor.Phone && x.Password == doctor.Password);
            if (d != null)
            {
                // HttpContext.Response.Cookies["Id"].Value = d.Id.ToString();
                Session["id"] = d.Id.ToString();
                return View(d);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult GetLitOfPatient()
        {
            Doctor doctor = GetDoctor();
            if (doctor != null)
            {
                // db.Entry(d).Collection("Patient").Load();
                return View(doctor.Patients.ToList());
            }
            return new HttpUnauthorizedResult();
        }

        public ActionResult EditPatient(int id)
        {
            Patient patient = GetPatient(id);
            if (patient != null)
            {
                return View(patient);
            }

            return new HttpUnauthorizedResult();
        }

        [HttpPost]
        public ActionResult SaveChangePatient(Patient patient)
        {
            Patient newPatient = GetPatient(patient.Id);
            newPatient.Name = patient.Name;
            newPatient.Surname = patient.Surname;
            newPatient.Phone = patient.Phone;
            newPatient.Password = patient.Password;
            newPatient.DateOfBirth = patient.DateOfBirth;
            newPatient.historyOfTreatment = patient.historyOfTreatment;
            if (newPatient != null)
            {
                db.Entry(newPatient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetLitOfPatient");
            }

            return new HttpUnauthorizedResult();
        }

        [HttpPost]
        public ActionResult SavePatient(Patient patient)
        {
            if (patient != null)
            {
                patient.DoctorId = GetDoctor().Id;
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("GetLitOfPatient");
            }

            return new HttpUnauthorizedResult();
        }
        public ActionResult AddPatient()
        {
            Doctor doctor = GetDoctor();
            if (doctor != null)
            {
                return View();
            }

            return new HttpUnauthorizedResult();
        }
        public ActionResult DeletePatient(int id)
        {
            Patient patient = GetPatient(id);
            if (patient != null)
            {
                db.Entry(patient).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("GetLitOfPatient");
            }
            return new HttpUnauthorizedResult();
        }

        public ActionResult DetailsPatient(int id)
        {
            Patient patient = GetPatient(id);
            if (patient != null)
            {
                return View(patient);
            }
            return new HttpUnauthorizedResult();
        }

        public ActionResult GetListOfMedicaments(int id)
        {
            Patient patient = GetPatient(id);
            if (patient != null)
            {
                ViewData["patientId"] = id;
                return View(patient.Medicaments.ToList());
            }
            return new HttpUnauthorizedResult();
        }

        public ActionResult EditMedicament(int id)
        {
            Medicament medicament = GetMedicament(id);
            if (medicament != null)
            {
                return View(medicament);
            }
            return new HttpUnauthorizedResult();
        }
        [HttpPost]
        public ActionResult SaveChangeMedicament(Medicament medicament)
        {
            if (medicament != null)
            {
                Medicament newMedicament = GetMedicament(medicament.Id);
                if (newMedicament != null)
                {
                    newMedicament.Name = medicament.Name;
                    newMedicament.Volume = medicament.Volume;
                    newMedicament.AmountPerDay = medicament.AmountPerDay;
                    db.Entry(newMedicament).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("GetLitOfPatient");
                }
            }
            return new HttpUnauthorizedResult();
        }

        public ActionResult DeleteMedicament(int id)
        {
            Medicament medicament = GetMedicament(id);
            if (medicament != null)
            {
                db.Entry(medicament).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("GetLitOfPatient");
            }
            return new HttpUnauthorizedResult();
        }

        public ActionResult DetailsMedicament(int id)
        {
            Medicament medicament = GetMedicament(id);
            if (medicament != null)
            {
                return View(medicament);
            }
            return new HttpUnauthorizedResult();
        }

        [HttpPost]
        public ActionResult SaveMedicament(Medicament medicament, int idPatient)
        {
            if (medicament != null)
            {
                Patient patient = GetPatient(idPatient);
                patient?.Medicaments.Add(medicament);
                db.SaveChanges();
                return RedirectToAction($"GetListOfMedicaments/{idPatient}");
            }

            return new HttpUnauthorizedResult();
        }
        public ActionResult AddMedicament(int id)
        {
            Patient patient = GetPatient(id);
            if (patient != null)
            {
                ViewData["patientId"] = id;
                return View();
            }
            return new HttpUnauthorizedResult();
        }

        private Doctor GetDoctor()
        {
            int id;
            //  if (HttpContext.Response.Cookies.Count>0&&int.TryParse(HttpContext.Response.Cookies[0].Value,out id))
            if (Session.Count > 0 && int.TryParse(Session["id"].ToString(), out id))
            {
                return db.Doctors.FirstOrDefault(x => x.Id == id);
            }
            return null;
        }
        private Patient GetPatient(int id)
        {
            Doctor doctor = GetDoctor();
            if (doctor != null)
            {
                return doctor.Patients.FirstOrDefault(x => x.Id == id);
            }
            return null;
        }
        private Medicament GetMedicament(int id)
        {
            Doctor doctor = GetDoctor();
            if (doctor != null)
            {
                return db.Medicaments.FirstOrDefault(x => x.Id == id);
            }
            return null;
        }
    }
}