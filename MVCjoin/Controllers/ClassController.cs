using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using MVCjoin.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace MVCjoin.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            var C = new ClassDetails();
          
            return View(C);
        }
        [HttpPost]

        public ActionResult AddForm(ClassDetails cd)
        {
            string InsertCommand = "insert into Class ";
            InsertCommand += "Values ('" + cd.Standard + "')";
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

            ClassList classlist = new ClassList();
            string SelectCommand = "select * from Class";
            DataSet ds = ExecuteQuery(SelectCommand);
            classlist.Ctable = ds.Tables[0];
                
            
            return View(classlist);
        }

        public ActionResult Delete(string id)
        {
            string DeleteCommand = "Delete From Class where ClassId= " + id;
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
            var Cls = new ClassDetails();
            ClassList classList = new ClassList();
            string SelectCommand = "Select * from Class where ClassId= " + id;
            DataSet ds = ExecuteQuery(SelectCommand);
            classList.Ctable = ds.Tables[0];

            DataRow dr = classList.Ctable.Rows[0];
            Cls.ClassId = Convert.ToInt32(dr["ClassId"]);
            Cls.Standard = Convert.ToString(dr["Standard"]);

            return View(Cls);
        }
        [HttpPost]
          
        public ActionResult Edit(ClassDetails cd)
        {
            string EditCommand = "Update Class set standard = '" + cd.Standard + "' where ClassId = '" + cd.ClassId + "'";
            DataSet ds = ExecuteQuery(EditCommand);

            TempData["Edit"] = "Data Edited Successfully";

            return RedirectToAction("Listing");
        }

        public DataSet ExecuteQuery(string Query)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                string SelectCommand = Query;
                using (SqlCommand sqlCommand = new SqlCommand(SelectCommand, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlConnection.Open();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(ds);
                }
            }

            return ds;
        }

        public ActionResult Logout()
        {

            return RedirectToAction("Login","User");
        }
    }
}