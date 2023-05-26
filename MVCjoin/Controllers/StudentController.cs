using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MVCjoin.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

namespace MVCjoin.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student

        public DataSet ExecuteQuery(string Query)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string Command = Query;
                using (SqlCommand sqlCommand = new SqlCommand(Command,sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlConnection.Open();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(ds);

                }
                return ds;
            }
        }
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var S = new StudentDetails();            
            return View(S);
        }
        [HttpPost]

        public ActionResult AddForm(StudentDetails sd)
        {
            string InsertCommand = "Insert into Student ";
            InsertCommand += "Values ('" + sd.StudentName + "', '" + sd.Email + "', '" + sd.Birthdate + "')";
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
            StudentList std = new StudentList();
            string SelectCommand = "Select * from Student";
            DataSet ds = ExecuteQuery(SelectCommand);
            std.Stable = ds.Tables[0];
            
            return View(std);
        }

        public ActionResult Delete(string id)
        {
            string DeleteCommand = "Delete From Student where StudentId= " + id;
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
            var edt = new StudentDetails();
            StudentList studentlist = new StudentList();
            string SelectCommand = "Select * from Student where StudentId= " + id;
            DataSet ds = ExecuteQuery(SelectCommand);
            studentlist.Stable = ds.Tables[0];

            DataRow dr = ds.Tables[0].Rows[0];
            edt.StudentId = Convert.ToInt32(dr["StudentId"]);
            edt.StudentName = Convert.ToString(dr["StudentName"]);
            edt.Email = Convert.ToString(dr["Email"]);
            edt.Birthdate = Convert.ToString(dr["Birthdate"]);

            return View(edt);
        }
        [HttpPost]

        public ActionResult Edit(StudentDetails sd)
        {
            string EditCommand = "Update Student set StudentName ='" + sd.StudentName + "',Email ='" + sd.Email + "',Birthdate='" + sd.Birthdate + "' where StudentId='" + sd.StudentId + "'";
            DataSet ds = ExecuteQuery(EditCommand);
            TempData["Edit"] = "Data Edited Successfully";

            return RedirectToAction("Listing");
        }
    }
}