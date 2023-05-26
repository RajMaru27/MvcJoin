using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;

namespace MVCjoin.Models
{
    public class ExamDetails
    {
        public int ExamId { get; set; }
        public string SubjectId { get; set; }
        public string RegisterId { get; set; }
        public int marks { get; set; }
        public int OutOf { get; set; }
        public List<SelectListItem> sublist { get; set; }
        public List<SelectListItem> reglist { get; set; }
    }
}