using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using PackWalkLicenta.Models;
using PackWalkLicenta.Models.ViewModels;

namespace PackWalkLicenta.Controllers
{
    public class RezervaresController : Controller
    {
        private PackWalkDbEntities db = new PackWalkDbEntities();

        // GET: Rezervares
        public ActionResult Index()
        {
            return View(db.Rezervares.ToList());
        }

        // GET: Rezervares/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rezervare rezervare = db.Rezervares.Find(id);
            if (rezervare == null)
            {
                return HttpNotFound();
            }
            return View(rezervare);
        }

        // GET: Rezervares/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rezervares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RezervareId,Adresa_de_Ridicare,Numarul_persoanei_care_trimite,Adresa_de_Predare,Numarul_persoanei_care_primeste,Cantitate_aproximativa,Data_Trimiterii")] Rezervare rezervare)
        {
            if (ModelState.IsValid)
            {
                rezervare.UserId = int.Parse(Session["UserId"].ToString());
                db.Rezervares.Add(rezervare);
                db.SaveChanges();
                return RedirectToAction("RezervareSucces");
            }

            return View(rezervare);
        }
        public ActionResult RezervareSucces()
        {
            return View();
        }
    
        // GET: Rezervares/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rezervare rezervare = db.Rezervares.Find(id);
            if (rezervare == null)
            {
                return HttpNotFound();
            }
            return View(rezervare);
        }

        // POST: Rezervares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RezervareId,Adresa_de_Ridicare,Numarul_persoanei_care_trimite,Adresa_de_Predare,Numarul_persoanei_care_primeste,Cantitate_aproximativa,Data_Trimiterii")] Rezervare rezervare)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rezervare).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rezervare);
        }

        // GET: Rezervares/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rezervare rezervare = db.Rezervares.Find(id);
            if (rezervare == null)
            {
                return HttpNotFound();
            }
            return View(rezervare);
        }

        // POST: Rezervares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rezervare rezervare = db.Rezervares.Find(id);
            db.Rezervares.Remove(rezervare);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult SendEmail(int? id)
        {
            Rezervare rezervare = db.Rezervares.Find(id);

            return View(new EmailContent {RezervareID=rezervare.RezervareId,Destinatar=rezervare.User.Email,Subiect=$"Informare cu privire la rezervarea din data {rezervare.Data_Trimiterii}" });
        }
        [HttpPost]
        public ActionResult SendEmail(EmailContent email)
        {
            try
            {
                if (ModelState.IsValid)
                { 
                    var senderEmail = new MailAddress("pachete.studentesti12@gmail.com", "PackWalk-Plimbarea Pachetelor");
                    var receiverEmail = new MailAddress(email.Destinatar, "Receiver");
                    var password = "Licenta1234";
                    var sub = email.Subiect;
                    var body = email.Mesaj;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return RedirectToAction("EmailSucces");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Ceva a fost în neregulă! Ne pare rău, mail-ul nu s-a putut trimite. Încercați din nou!";
            }
            return View();
        }
        public ActionResult EmailSucces()
        {
            return View();
        }
    }
}
