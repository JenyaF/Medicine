using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Medicine.Models
{
    class MyContextInitilizer:DropCreateDatabaseAlways<MedicineContext>
    {
        protected override void Seed(MedicineContext context)
        {
            List<Medicament> medicines1 = new List<Medicament>() {new Medicament() { Name="Parazol", AmountPerDay=2, Volume=0.1},
                                                            new Medicament() { Name="Analgin", AmountPerDay=2, Volume=0.5},
                                                            new Medicament() { Name="Noksprey", AmountPerDay=3, Volume=0.1}};
            List<Medicament> medicines2 = new List<Medicament>() { new Medicament() { Name="Karvalol", AmountPerDay=2, Volume=0.1}};

            List<Patient> patients1 = new List<Patient>(){new Patient(){ Name="Ivan",Surname="Braun",DateOfBirth=new DateTime(1970,08,07),Phone="3044459671",Password="1", Medicaments=medicines1},
                                                            new Patient(){ Name="Maria",Surname="Braun",DateOfBirth=new DateTime(1970,08,07),Phone="3044459672",Password="1"},
                                                            new Patient(){ Name="Lena",Surname="Braun",DateOfBirth=new DateTime(1970,08,07),Phone="3044459673",Password="1",Medicaments=medicines2} };
            List<Patient> patients2 = new List<Patient>(){new Patient(){ Name="Vova",Surname="Sokol",DateOfBirth=new DateTime(1970,08,07),Phone="3044459674",Password="1"},
                                                            new Patient(){ Name="Misha",Surname="Sokol",DateOfBirth=new DateTime(1970,08,07),Phone="3044459675",Medicaments=medicines2,Password="1"} };

            List<Doctor> doctors = new List<Doctor>() { new Doctor() { Name = "Jon", Surname = "Swith", DateOfBirth = new DateTime(1970, 08, 07), Phone = "3044459676", Qualification = "Hight category",Password="1", Patients = patients1 },
                                                        new Doctor() { Name="Jey",Surname="Smith",DateOfBirth=new DateTime(1970,08,07),Phone="3044459677",Qualification="Hight category",Password="1",Patients=patients2},
                                                        new Doctor() { Name="Jim",Surname="Smith",DateOfBirth=new DateTime(1970,08,07),Phone="3044459678",Qualification="Hight category", Password="1"} };
            using (MedicineContext db = new MedicineContext())
            {
                db.Doctors.AddRange(doctors);
                db.SaveChanges();
            }
        }
    }
}
