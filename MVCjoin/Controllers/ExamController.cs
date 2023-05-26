using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using MVCjoin.Models;

namespace MVCjoin.Controllers
{
    public class ExamController : Controller
    {
        // GET: Exam
        public DataSet ExecuteQuery(string query)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string Command = query;
                using (SqlCommand sqlCommand = new SqlCommand(Command,sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlConnection.Open();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(ds);
                }
            }
            return ds;
        }
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var exm = new ExamDetails();
            string SelectCommand = "select * From Subject";
            DataSet ds =ExecuteQuery(SelectCommand);

            exm.sublist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var sb = new SelectListItem();
                sb.Text = Convert.ToString(dr["SubjectName"]);
                sb.Value = Convert.ToString(dr["SubjectId"]);
                exm.sublist.Add(sb);
            }

            string SelectCommand1 = "Select r.RegisterId, c.Standard, s.StudentName from Register as r join Class as c on c.ClassId = r.ClassId join Student as s on s.StudentId = r.StudentId";
            DataSet ds1 = ExecuteQuery(SelectCommand1);

            exm.reglist = new List<SelectListItem>();
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                var sb1 = new SelectListItem();
                sb1.Text = Convert.ToString(dr1["Standard"]) + " " + Convert.ToString(dr1["StudentName"]);
                sb1.Value = Convert.ToString(dr1["RegisterId"]);
                exm.reglist.Add(sb1);
            }
            return View(exm);
        }
        [HttpPost]
        public ActionResult AddForm(ExamDetails ed)
        {
            string InsertCommand = "insert into Exam ";
            InsertCommand += "Values ('" + ed.SubjectId + "', '" + ed.RegisterId + "', '" + ed.marks + "', '" + ed.OutOf + "')";
            DataSet ds = ExecuteQuery(InsertCommand);
            TempData["Add"] = "Data Added Successfully";

            return RedirectToAction("Listing");
        }

        public ActionResult Listing()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ExamList examlist = new ExamList();
            string SelectCommand = "Select e.ExamId, c.Standard, s.StudentName,sub.SubjectName,e.marks,OutOf from Exam as e join Register as r on r.RegisterId = e.RegisterId join Class as c on c.ClassId = r.ClassId join Student as s on s.StudentId = r.StudentId join Subject as sub on sub.SubjectId = e.SubjectId";
            DataSet ds = ExecuteQuery(SelectCommand);
            examlist.etable = ds.Tables[0];

            return View(examlist);  
        }

        public ActionResult Delete(string id)
        {
            string DeleteCommand = "Delete from Exam where ExamId=" + id;
            DataSet ds = ExecuteQuery(DeleteCommand);
            TempData["Delete"] = "Data Deleted Successfully";

            return RedirectToAction("Listing");
        }

        public ActionResult Edit(string id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var exm = new ExamDetails();
            //ExamList examList = new ExamList();
            string SelectCommand = "select * From Subject";
            DataSet ds = ExecuteQuery(SelectCommand);

            exm.sublist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var sb = new SelectListItem();
                sb.Text = Convert.ToString(dr["SubjectName"]);
                sb.Value = Convert.ToString(dr["SubjectId"]);
                exm.sublist.Add(sb);
            }

            string SelectCommand1 = "Select r.RegisterId, c.Standard, s.StudentName from Register as r join Class as c on c.ClassId = r.ClassId join Student as s on s.StudentId = r.StudentId";
            DataSet ds1 = ExecuteQuery(SelectCommand1);

            exm.reglist = new List<SelectListItem>();
            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                var sb1 = new SelectListItem();
                sb1.Text = Convert.ToString(dr1["Standard"]) + " " + Convert.ToString(dr1["StudentName"]);
                sb1.Value = Convert.ToString(dr1["RegisterId"]);
                exm.reglist.Add(sb1);
            }

            string SelectCommand2 = "Select * from Exam where ExamId=" + id;
            DataSet ds2 = ExecuteQuery(SelectCommand2);

            DataRow dr2 = ds2.Tables[0].Rows[0];
            exm.ExamId = Convert.ToInt32(dr2["ExamId"]);
            exm.SubjectId = Convert.ToString(dr2["SubjectId"]);
            exm.RegisterId = Convert.ToString(dr2["RegisterId"]);
            exm.marks = Convert.ToInt32(dr2["marks"]);
            exm.OutOf = Convert.ToInt32(dr2["OutOf"]);

            return View(exm);
        }
        [HttpPost]
        public ActionResult Edit(ExamDetails ed)
        {
            string EditCommand = "Update Exam set SubjectId='" + ed.SubjectId + "',RegisterId= '" + ed.RegisterId + "',marks='" + ed.marks + "',OutOf='" + ed.OutOf + "' where ExamId='" + ed.ExamId + "'";
            DataSet ds = ExecuteQuery(EditCommand);
            TempData["Edit"] = "Data Edited Successfully";

            return RedirectToAction("Listing");
        }

        public ActionResult Report()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            ExamList examList = new ExamList();
            string SelectCommand = "Select c.Standard, s.StudentName,sum(marks) as Totalmarks,sum(OutOf) as Total,Convert(Decimal(4,2) ,(sum(marks)/sum(OutOf) * 100)) as percentage  from Exam as e join Register as r on r.RegisterId = e.RegisterId join Class as c on c.ClassId = r.ClassId join Student as s on s.StudentId = r.StudentId group by  c.Standard,s.StudentName";
            DataSet ds = ExecuteQuery(SelectCommand);
            examList.etable = ds.Tables[0];

            return View(examList);
        }

       
    }
}