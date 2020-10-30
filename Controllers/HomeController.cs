using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PackWalkLicenta.Models;

namespace PackWalkLicenta.Controllers
{
    public class HomeController : Controller
    {
        private PackWalkDbEntities db = new PackWalkDbEntities();
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User us)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(us);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User us)
        {
            if (ModelState.IsValid)
            {
                var details = (from userlist in db.Users
                               where userlist.Username == us.Username && userlist.Parola == us.Parola
                               select new
                               {
                                   userlist.UserId,
                                   userlist.Username
                               }).ToList();
                if (details.FirstOrDefault() != null)
                {
                    Session["UserId"] = details.FirstOrDefault().UserId;
                    Session["Username"] = details.FirstOrDefault().Username;
                    if (details.FirstOrDefault().Username == "admin")
                    { return RedirectToAction("ViewAdmin", "Home"); }
                    else
                    {
                        return RedirectToAction("Create", "Rezervares");
                    }


                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Credentials");
            }
            return View(us);

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
       public ActionResult ViewAdmin()
        {
            return View();
        }



    }
}