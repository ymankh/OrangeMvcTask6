using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication31.Models;

namespace WebApplication31.Controllers
{
    public class notUsersController : Controller
    {
        private TestAuthEntities db = new TestAuthEntities();    

        
       



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(notUser user, string confermPassword)
        {
            if (user.password.Length < 8 || user.email.Length < 5 || user.password != confermPassword)
                return View();
            db.notUsers.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            if (Session["isLoggedIn"] == "true")
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public ActionResult Login(notUser user)
        {
            var theUser = db.notUsers.SingleOrDefault(u => u.email == user.email);
            if (theUser != null && user.password == theUser.password)
            {
                Session["isLoggedIn"] = "true";
                Session["id"] = theUser.id;
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        public ActionResult Profile()
        {
            if (Session["isLoggedIn"] != "true")
                return RedirectToAction("Index", "Home");
            notUser user = db.notUsers.Find(Session["id"]);
            return View(user);
        }

        [HttpPost]
        public ActionResult Profile(notUser user)
        {
            notUser theUser = db.notUsers.Find(Session["id"]);
            if (Session["isLoggedIn"] != "true")
                return RedirectToAction("Index", "Home");
            theUser.name = user.name;
            theUser.email = user.email;
            theUser.img = user.img;
            db.notUsers.AddOrUpdate(theUser);
            db.SaveChanges();
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Remove("id");
            Session.Remove("isLoggedIn");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult changePassword()
        {
            if (Session["isLoggedIn"] != "true")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public ActionResult changePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            var user = db.notUsers.Find(Session["id"]);
            if (user.password == oldPassword && newPassword == confirmNewPassword)
            {
                user.password = newPassword;
                db.notUsers.AddOrUpdate(user);
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View();
        }



    }

}
