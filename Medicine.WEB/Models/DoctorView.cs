using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Medicine.WEB.Models
{
    public class DoctorView
    {

        public string Id { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }       
        public string Qualification { get; set; }
        [DataType(DataType.Date)]
        public string DateOfBirth { get; set; }
    }
}