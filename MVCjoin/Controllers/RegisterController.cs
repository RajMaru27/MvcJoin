using MVCjoin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace MVCjoin.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register

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
            var reg = new RegisterDetails();
            string SelectCommand = "Select * from Class";
            DataSet ds = ExecuteQuery(SelectCommand);

            reg.Classlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var Cls = new SelectListItem();
                Cls.Text = Convert.ToString(dr["Standard"]);
                Cls.Value = Convert.ToString(dr["ClassId"]);
                reg.Classlist.Add(Cls);
            }

            string SelectCommand1 = "Select * from Student";
            DataSet data = ExecuteQuery(SelectCommand1);

            reg.Studentlist = new List<SelectListItem>();
            foreach (DataRow dr1 in data.Tables[0].Rows)
            {
                var std = new SelectListItem();
                std.Text = Convert.ToString(dr1["StudentName"]);
                std.Value = Convert.ToString(dr1["StudentId"]);
                reg.Studentlist.Add(std);
            }

            return View(reg);
        }
        [HttpPost]

        public ActionResult AddForm(RegisterDetails rd)
        {
            string InsertCommand = "Insert into Register ";
            InsertCommand += "Values ('" + rd.ClassId + "', '" + rd.StudentId + "')";
            DataSet ds = ExecuteQuery(InsertCommand);
            TempData["Add"] = "Data Added Successfully";

            return RedirectToAction("Listing");
        }

        public ActionResult Listing(string id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            RegisterList reglist = new RegisterList();
            string SelectCommand = "select r.RegisterId,c.Standard,s.StudentName from Register as r join Class as c on c.ClassId = r.ClassId join Student as s on s.StudentId = r.StudentId";
            DataSet ds = ExecuteQuery(SelectCommand);
            reglist.Rtable = ds.Tables[0];

            return View(reglist);
        }

        public ActionResult Delete(string id)
        {
            string DeleteCommand = "Delete From Register Where RegisterId = " + id;
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
            var abc = new RegisterDetails();
            RegisterList reglist = new RegisterList();
            string SelectCommand = "Select * from Class";
            DataSet ds = ExecuteQuery(SelectCommand);

            abc.Classlist = new List<SelectListItem>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var xyz = new SelectListItem();
                xyz.Text = Convert.ToString(dr["Standard"]);
                xyz.Value = Convert.ToString(dr["ClassId"]);
                abc.Classlist.Add(xyz);
            }

            string SelectCommand1 = "Select * from Student";
            DataSet data = ExecuteQuery(SelectCommand1);

            abc.Studentlist = new List<SelectListItem>();
            foreach (DataRow row in data.Tables[0].Rows)
            {
                var xyz = new SelectListItem();
                xyz.Text = Convert.ToString(row["StudentName"]);
                xyz.Value = Convert.ToString(row["StudentId"]);
                abc.Studentlist.Add(xyz);
            }

            string SelectCommand2 = "Select * from Register where RegisterId= " + id;
            DataSet dataSet = ExecuteQuery(SelectCommand2);
            reglist.Rtable = dataSet.Tables[0];

            DataRow dr1 = reglist.Rtable.Rows[0];
            abc.RegisterId = Convert.ToInt32(dr1["RegisterId"]);
            abc.ClassId = Convert.ToString(dr1["ClassId"]);
            abc.StudentId = Convert.ToString(dr1["StudentId"]);

            return View(abc);
        }
        [HttpPost]

        public ActionResult Edit(RegisterDetails rd)
        {
            string EditCommand = "Update Register set ClassId = '" + rd.ClassId + "',StudentId = '" + rd.StudentId + "' where RegisterId = '" + rd.RegisterId + "'";
            DataSet ds = ExecuteQuery(EditCommand);
            TempData["Edit"] = "Data Edited Successfully";

            return RedirectToAction("Listing");
        }

        public ActionResult Details(string id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            RegisterList reg = new RegisterList();
            string SelectCommand = "Select c.Standard, s.StudentName,sum(marks) as Totalmarks,sum(OutOf) as Total,Convert(Decimal(4,2) ,(sum(marks)/sum(OutOf) * 100)) as percentage  from Exam as e join Register as r on r.RegisterId = e.RegisterId join Class as c on c.ClassId = r.ClassId join Student as s on s.StudentId = r.StudentId Where r.RegisterId = '" + id + "'  group by  c.Standard,s.StudentName";
            DataSet ds = ExecuteQuery(SelectCommand);
            reg.Rtable = ds.Tables[0];

            return View(reg);

        }


    }
}