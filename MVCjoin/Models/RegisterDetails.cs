using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

namespace MVCjoin.Models
{
    public class RegisterDetails
    {
        public int RegisterId { get; set; }
        public string ClassId { get; set; }
        public string StudentId { get; set; }
        public List<SelectListItem> Classlist { get; set; }
        public List<SelectListItem> Studentlist { get; set; }
    }
}