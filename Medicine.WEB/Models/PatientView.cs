using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Medicine.WEB.Models
{
    public class PatientView
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [MaxLength(200)]
        public string historyOfTreatment { get; set; }
        [Required]
        public string DoctorId { get; set; }
    }
}