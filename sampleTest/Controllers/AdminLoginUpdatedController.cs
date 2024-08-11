using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using sampleTest.Models;
using sampleTest.Repository;

namespace sampleTest.Controllers
{
    public class AdminLoginUpdatedController : Controller
    {
        private readonly IAdminRepository _AdminRepository;
        ModelEntity _dbContext = new ModelEntity();


        public AdminLoginUpdatedController()
        {
            _AdminRepository = new AdminRepository(new ModelEntity());
        }

        public ActionResult AdminLogin()
        {
            AdminLogin model = new AdminLogin();
            return View(model);
        }

        [HttpPost]
        public ActionResult AdminLogin(AdminLogin mod)
        {
            string message = string.Empty;
            try
            {
                AdminLogin usersEntities = new AdminLogin();

                usersEntities = _dbContext.AdminLogins.FirstOrDefault(x => x.Username == mod.Username && x.Password == mod.Password);
                if (usersEntities != null)
                {


                    switch (usersEntities.ID)
                    {
                        case -1:
                            message = "Username and/or password is incorrect.";
                            break;
                        case -2:
                            message = "Account has not been activated.";
                            break;
                        default:
                           
                            FormsAuthentication.SetAuthCookie(Convert.ToString(usersEntities.ID), false);
                            Session["AdminID"] = usersEntities.ID;

                            return RedirectToAction("Profile");
                    }
                }
                else
                {
                    message = "Username and/or password is incorrect.";
                }
            }
            catch (Exception ex)
            {
                _AdminRepository.SaveErrorLogs("Admin", "AdminLogin", "Error", mod.Username + "," + mod.Password, ex.Message + ex.StackTrace, 0, "");

            }

            ViewBag.Message = message;
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("AdminLogin");
        }

        public ActionResult ForgotPassword()
        {

            AdminLogin model = new AdminLogin();

            int LUserid = Convert.ToInt32(Session["AdminID"]);

            AdminLogin UserDetails = _AdminRepository.AdminUserDetails(LUserid);
            model.Username = UserDetails.Username;
            model.Password = " ";

            if (TempData["StatusMess"] == null)
            {
                TempData["StatusMess"] = string.Empty;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ForgotPassword(AdminLogin objAdmin)
        {
            Response1 ObjRes = new Response1();
            AdminLogin model = new AdminLogin();
            AdminLogin CheckAdmin = _AdminRepository.GetAdminModelList().FirstOrDefault(x => x.Username == objAdmin.Username);

            if (CheckAdmin != null && objAdmin.Username != null && objAdmin.Password != null)
            {
                model = _AdminRepository.GetUserByUserName(objAdmin.Username);
                if (ModelState.IsValid)
                {
                    model.Username = objAdmin.Username;
                    model.Password = objAdmin.Password;
                    _AdminRepository.UpdatePassword(model);
                    ObjRes.Success = true;
                    ObjRes.Message = "Updated successfully";
                }
                else
                {
                    ObjRes.Message = "Invalid data";
                }
            }
            else
            {
                ObjRes.Message = "Invalid data";
            }

            if (TempData["StatusMess"] == null)
            {
                TempData["StatusMess"] = string.Empty;
            }
            TempData["StatusMess"] = ObjRes.Message;

            if (ObjRes.Success == true)
            {
                return RedirectToAction("ForgotPassword", "AdminLoginUpdated");
            }
            else
            {
                return View(objAdmin);
            }
        }

        public ActionResult Profile()
        {
            return View();
        }


        public ActionResult Index()
        {
            return View();
        }
    }
}