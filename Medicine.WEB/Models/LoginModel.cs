using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Medicine.WEB.Models
{
    public class LoginModel
    {
       [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
     
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}