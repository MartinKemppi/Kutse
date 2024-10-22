using Kutse_App.Models;
using Kutsung.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;

namespace Kutsung.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuestContext guestDb;
        private readonly ApplicationDbContext appDb;
        public HomeController(GuestContext guestDb, ApplicationDbContext appDb)
        {
            this.guestDb = guestDb;
            this.appDb = appDb;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Teie taotluse kirjelduse lehekülg.";

            return View();
        }
        [Authorize]
        public ActionResult Roll()
        {
            IList<string> roles = new List<string> { "Roll ei ole maaratud" };
            ApplicationUserManager userManager = HttpContext.GetOwinContext()
                                                        .GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.FindByEmail(User.Identity.Name);

            if (user != null)
                roles = userManager.GetRoles(user.Id);

            return View(roles);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Teie kontaktleht.";

            return View();
        }
        public ActionResult Kutse()
        {
            int tund = DateTime.Now.Hour;
            if(tund < 10)
            {
                ViewBag.Greeting = "Tere hommikust";
                ViewBag.Message = "Ootan sind oma peole! Tule kindlasti!!! Ootan sind!";
            }
            else if(tund < 16)
            {
                ViewBag.Greeting = "Tere päevast";
                ViewBag.Message = "Ootan sind oma peole! Tule kindlasti!!! Ootan sind!";
            }
            else if(tund < 22)
            {
                ViewBag.Greeting = "Tere õhtust";
                ViewBag.Message = "Ootan sind oma peole! Tule kindlasti!!! Ootan sind!";
            }
            else
            {
                ViewBag.Greeting = "Tere ööst";
                ViewBag.Message = "Ootan sind oma peole! Tule kindlasti!!! Ootan sind!";
            }
            

            return View();
        }
        [HttpGet]
        public ActionResult Ankeet()
        {
            return View();
        }
        public ActionResult Thanks()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Tuleb()
        {
            IEnumerable<Kylaline> guests = guestDb.Kylalised.Where(g => g.Tuleb == true);
            return View(guests);
        }
        [HttpGet]
        public ActionResult EiTule()
        {
            IEnumerable<Kylaline> guests = guestDb.Kylalised.Where(g => g.Tuleb == false);
            return View(guests);
        }
        public ActionResult Rohkem()
        {
            IEnumerable<Kylaline> guests = guestDb.Kylalised;
            return View(guests);
        }
        public ActionResult Rohkem_regpeod()
        {
            IEnumerable<KylalinePeole> kylalised = guestDb.KylalisedPeole.Include(kp => kp.Kylaline).ToList();

            return View(kylalised);
        }
        [HttpPost]
        public ViewResult Ankeet(Kylaline guest)
        {
            E_mail_K(guest);
            E_mail_O(guest);
            if (ModelState.IsValid)
            {
                guestDb.Kylalised.Add(guest);
                guestDb.SaveChanges();
                return View("Thanks",guest);
            }
            else
            {
                return View();
            }
        }
        public void E_mail_O(Kylaline guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "dusperin1234@gmail.com";
                WebMail.Password = "uguy pvvq prlx xjkr";
                WebMail.From = "dusperin1234@gmail.com";
                WebMail.Send("dusperin1234@gmail.com", "Vastus kutsele", $"{guest.Nimi} vastas, et {(guest.Tuleb ?? false ? " tuleb peole." : " ei tule peole.")} {guest.Nimi} telefoninumber on {guest.Tel}");
                ViewBag.Message = "Kiri on saatnud!";
            }
            catch (Exception)
            {
                ViewBag.Message = "Mul on kahju!Ei saa kirja saada!!!";
            }
        }
        public void E_mail_K(Kylaline guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "dusperin1234@gmail.com";
                WebMail.Password = "uguy pvvq prlx xjkr";
                WebMail.From = "dusperin1234@gmail.com";
                WebMail.Send(guest.Email, "Vastus kutsele", $"{guest.Nimi} vastas, et {(guest.Tuleb ?? false ? " tuleb peole." : " ei tule peole.")}");
                ViewBag.Message = "Kiri on saatnud!";
            }
            catch (Exception)
            {
                ViewBag.Message = "Mul on kahju!Ei saa kirja saada!!!";
            }
        }
        [HttpPost]
        public ActionResult Tuletameelde(Kylaline guest)
        {
            try
            {
                E_mail_Meeldetuletus(guest);

                ViewBag.ImagePath = guest.Tuleb == true ? "~/Images/roomusnagu.jpg" : "~/Images/kurbnagu.jpg";
                ViewBag.Message = guest.Tuleb == true ? "Ootan sind, " + guest.Nimi + ", peole!" : "Väga kurb uudis. Näeme hiljem!";

                ViewBag.Message = "Meeldetuletus on saadetud!";
            }
            catch (Exception)
            {
                ViewBag.Message = "Meeldetuletuse saatmine ebaõnnestus!";
                ViewBag.ImagePath = "~/Images/kurbnagu.jpg";
            }

            return View("Thanks", guest);
        }
        public void E_mail_Meeldetuletus(Kylaline guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "dusperin1234@gmail.com";
                WebMail.Password = "uguy pvvq prlx xjkr";
                WebMail.From = "dusperin1234@gmail.com";

                string subject = "Meeldetuletus peole tulemiseks";
                string message = "Ära unusta tulla peole, " + guest.Nimi + "!";

                WebMail.Send(guest.Email, subject, message);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        public ActionResult Guest()
        {
            IEnumerable<Kylaline> guests = guestDb.Kylalised;
            return View(guests);
        }
        [Authorize]
        public ActionResult Peod()
        {
            IEnumerable<Peo> peod = guestDb.Peod;
            return View(peod);
        }
        [Authorize]
        public ActionResult RegistreeritudKylalisedPeol()
        {
            IEnumerable<KylalinePeole> kylalised = guestDb.KylalisedPeole.Include(kp => kp.Kylaline).ToList();

            foreach (var kylalinePeole in kylalised)
            {
                SaadaMeeldetuletuseEmail(kylalinePeole.Kylaline.Email, kylalinePeole.Peo.Pidu, kylalinePeole.Peo.Kuupaev);
            }

            return View(kylalised);
        }
        /*
         * See kõik mis on all on mõeldud Regkylalise jaoks
         */
        [HttpGet]
        public ActionResult Create_reg()
        {
            ViewBag.KylalineId = new SelectList(guestDb.Kylalised, "Id", "Nimi");
            ViewBag.PeoId = new SelectList(guestDb.Peod, "Id", "Pidu");
            return View();
        }
        [HttpPost]
        public ActionResult Create_reg(KylalinePeole kylalinepeole)
        {
            if (ModelState.IsValid)
            {
                guestDb.KylalisedPeole.Add(kylalinepeole);
                guestDb.SaveChanges();
                return RedirectToAction("RegistreeritudKylalisedPeol");
            }

            ViewBag.KylalineId = new SelectList(guestDb.Kylalised, "Id", "Nimi", kylalinepeole.KylalineId);
            ViewBag.PeoId = new SelectList(guestDb.Peod, "Id", "Pidu", kylalinepeole.PeoId);
            return View(kylalinepeole);
        }
        [HttpGet]
        public ActionResult Delete_reg(int id)
        {
            KylalinePeole k = guestDb.KylalisedPeole.Find(id);
            if (k == null)
            {
                return HttpNotFound();
            }
            return View(k);
        }
        [HttpPost, ActionName("Delete_reg")]
        public ActionResult DeleteConfirmed_reg(int id)
        {
            KylalinePeole k = guestDb.KylalisedPeole.Find(id);
            if (k == null)
            {
                return HttpNotFound();
            }
            guestDb.KylalisedPeole.Remove(k);
            guestDb.SaveChanges();
            return RedirectToAction("RegistreeriKylalinePeole");
        }
        [HttpGet]
        public ActionResult Edit_reg(int? id)
        {
            KylalinePeole k = guestDb.KylalisedPeole.Find(id);
            if (k == null)
            {
                return HttpNotFound();
            }
            ViewBag.KylalineId = new SelectList(guestDb.Kylalised, "Id", "Nimi", k.KylalineId);
            ViewBag.PeoId = new SelectList(guestDb.Peod, "Id", "Pidu", k.PeoId);
            return View(k);
        }

        [HttpPost, ActionName("Edit_reg")]
        public ActionResult EditConfirmed_reg(KylalinePeole kylalinepeole)
        {
            if (ModelState.IsValid)
            {
                guestDb.Entry(kylalinepeole).State = EntityState.Modified;
                guestDb.SaveChanges();
                return RedirectToAction("RegistreeritudKylalisedPeol");
            }

            ViewBag.KylalineId = new SelectList(guestDb.Kylalised, "Id", "Nimi", kylalinepeole.KylalineId);
            ViewBag.PeoId = new SelectList(guestDb.Peod, "Id", "Pidu", kylalinepeole.PeoId);
            return View(kylalinepeole);
        }

        private void SaadaMeeldetuletuseEmail(string email, string peoNimi, DateTime kuupaev)
        {
            try
            {
                if (DateTime.Today == kuupaev.Date)
                {
                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.SmtpPort = 587;
                    WebMail.EnableSsl = true;
                    WebMail.UserName = "dusperin1234@gmail.com";
                    WebMail.Password = "uguy pvvq prlx xjkr";
                    WebMail.From = "dusperin1234@gmail.com";

                    string subject = "Meeldetuletus peole tulemiseks";
                    string message = $"Ära unusta tulla peole! {peoNimi} toimub {kuupaev}";

                    WebMail.Send(email, subject, message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /*
         * See kõik mis on all on mõeldud Kylalise jaoks
         */
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Kylaline guest)
        {
            guestDb.Kylalised.Add(guest);
            guestDb.SaveChanges();
            return RedirectToAction("Guest");
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Kylaline g = guestDb.Kylalised.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }
            return View(g);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Kylaline g = guestDb.Kylalised.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }
            guestDb.Kylalised.Remove(g);
            guestDb.SaveChanges();
            return RedirectToAction("Guest");
        }
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            Kylaline g = guestDb.Kylalised.Find(id);
            if (g == null)
            {
                return HttpNotFound();
            }
            return View(g);
        }
        [HttpPost, ActionName("Edit")]
        public ActionResult EditConfirmed(Kylaline guest)
        {
            guestDb.Entry(guest).State = EntityState.Modified;
            guestDb.SaveChanges();
            return RedirectToAction("Guest");
        }
        /*
         * See kõik mis on all on mõeldud Peo jaoks
         */
        [HttpGet]        
        public ActionResult Loo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Loo(Peo peod)
        {
            guestDb.Peod.Add(peod);
            guestDb.SaveChanges();
            return RedirectToAction("Peod");
        }
        [HttpGet]
        public ActionResult Kustuta(int id)
        {
            Peo p = guestDb.Peod.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }
        [HttpPost, ActionName("Kustuta")]
        public ActionResult Kustutakinnitatud(int id)
        {
            Peo p = guestDb.Peod.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            guestDb.Peod.Remove(p);
            guestDb.SaveChanges();
            return RedirectToAction("Peod");
        }
        [HttpGet]
        public ActionResult Redigeeri(int? id)
        {
            Peo p = guestDb.Peod.Find(id);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }
        [HttpPost, ActionName("Redigeeri")]
        public ActionResult Redigeerikinnitatud(Peo peod)
        {
            guestDb.Entry(peod).State = EntityState.Modified;
            guestDb.SaveChanges();
            return RedirectToAction("Peod");
        }        
    }
}