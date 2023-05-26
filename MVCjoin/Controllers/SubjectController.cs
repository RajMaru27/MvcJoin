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
    public class SubjectController : Controller
    {
        // GET: Subject

        public DataSet ExecuteQuery(string query)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string Command = query;
                using (SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection))
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
            var sub = new SubjectDetails();
            return View(sub);
        }
        [HttpPost]

        public ActionResult AddForm(SubjectDetails sd)
        {
            string InsertCommand = "Insert into Subject ";
            InsertCommand += "Values ('" + sd.SubjectName + "')";
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
            SubjectList subjectList = new SubjectList();
            string SelectCommand = "Select sub.SubjectId,sub.SubjectName,Convert (Decimal(4,2),(sum(marks)/count(1))) as AvgMarks from Exam as e join Subject as sub on sub.SubjectId = e.SubjectId group by sub.SubjectName,sub.SubjectId";
            DataSet ds = ExecuteQuery(SelectCommand);
            subjectList.Stable = ds.Tables[0];

            return View(subjectList);
        }

        public ActionResult Delete(string id)
        {
            string DeleteCommand = "Delete From Subject where SubjectId=" + id;
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
            var sub = new SubjectDetails();
            SubjectList subjectList = new SubjectList();
            string SelectCommand = "Select * from Subject where SubjectId=" + id;
            DataSet ds = ExecuteQuery(SelectCommand);
            subjectList.Stable = ds.Tables[0];

            DataRow dr = subjectList.Stable.Rows[0];
            sub.SubjectId = Convert.ToInt32(dr["SubjectId"]);
            sub.SubjectName = Convert.ToString(dr["SubjectName"]);

            return View(sub);
        }
        [HttpPost]

        public ActionResult Edit(SubjectDetails sd)
        {
            string EditCommand = "Update Subject set SubjectName = '" + sd.SubjectName + "' where SubjectId = '" + sd.SubjectId + "'";
            DataSet ds = ExecuteQuery(EditCommand);
            TempData["Edit"] = "Data Edited Successfully";

            return RedirectToAction("Listing"); 
        }

        public ActionResult Avg(string id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            SubjectList subjectList = new SubjectList();
            string SelectCommand = "Select sub.SubjectName,Convert (Decimal(4,2),(sum(marks)/count(1))) as Avg from Exam as e join Subject as sub on sub.SubjectId = e.SubjectId where sub.SubjectId = '" + id + "' group by sub.SubjectName";
            DataSet ds = ExecuteQuery(SelectCommand);
            subjectList.Stable = ds.Tables[0];

            return View(subjectList);   
        }
    }
}