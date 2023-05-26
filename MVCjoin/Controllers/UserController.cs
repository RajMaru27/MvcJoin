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
    public class UserController : Controller
    {
        public DataSet ExecuteQuery(string query)
        {
            string Connectionstring = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            using (SqlConnection sqlConnection = new SqlConnection(Connectionstring))
            {
                string Command = query;
                using (SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlConnection.Open();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(ds);

                    return ds;
                }
            }
        }
        // GET: User
        public ActionResult Index()
        {       
            var U = new UserDetails();
            return View(U);
        }

        [HttpPost]
        public ActionResult AddForm(UserDetails ud)
        {
           
            if (ud.UserName == null || ud.Password == null || ud.ConfirmPassword == null)
            {
                TempData["Insert"] = "UserName and Password Cannot be Empty";
                return RedirectToAction("Index");
            }
            if (ud.UserName == ud.UserName)
            {
                TempData["Taken"] = "UserName Already Taken";
                return RedirectToAction("Index");
            }
            if (ud.Password != ud.ConfirmPassword)
            {
                TempData["NotMatch"] = "Password and ConfirmPassword Does not Match";
                return RedirectToAction("Index");
            }
            string InsertCommand = "Insert into [User] ";
            InsertCommand += "Values ('" + ud.UserName + "','" + ud.Password + "','" + ud.ConfirmPassword + "')";
            DataSet ds = ExecuteQuery(InsertCommand);

            TempData["Add"] = "Data Added Successfully";

            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            var L = new LoginDetails();
            return View(L);
        }

        [HttpPost]
        public ActionResult Submit(LoginDetails ld)
        {
            string SelectCommand = "Select * from [User] where UserName= '" + ld.UserName + "' and Password='" + ld.Password + "'";
            DataSet ds = ExecuteQuery(SelectCommand);
            var detail = ds.Tables[0].Rows.Count;

            if (detail > 0)
            {
                TempData["Login"] = "Login Successfull";
                Session["UserName"] = ld.UserName;
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Login");
        }

        public ActionResult Reset()
        {
            var R = new PasswordReset();
            return View(R);
        }

        [HttpPost]
        public ActionResult ResetForm(PasswordReset pr)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Login");
            }
            if (pr.Password != pr.ConfirmPassword)
            {
                TempData["NotMatch"] = "Password And ConfirmPassword Does Not Match";
                return RedirectToAction("Login");
            }
            string UpdateCommand = "Update [User] set Password='" + pr.Password + "',ConfirmPassword='" + pr.ConfirmPassword + "' where UserName='" + pr.UserName + "'";
            DataSet ds = ExecuteQuery(UpdateCommand);
            var res = ds.Tables[0].Rows.Count;

            if (res > 0)
            {
                TempData["Reset"] = "Password Reset Successfull";
                return RedirectToAction("Login");
            }

            return RedirectToAction("Reset");
        }

        public ActionResult Logout(UserDetails ud)
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}