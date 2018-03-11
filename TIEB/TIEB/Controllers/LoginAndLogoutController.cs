using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TIEB.Models;

namespace TIEB.Controllers
{
    public class LoginAndLogoutController : Controller
    {

        private TIEB_BudgetDBEntities db = new TIEB_BudgetDBEntities();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Persons Model)
        {
            // Login işleminde Model den gelen UserName ve Password verileri Persons tablosundaki UserName ve Password verileriyle 
            // eşlelişir ise User değişkenine atılır.
            var User = db.Persons.FirstOrDefault(x => x.UserName == Model.UserName && x.Password == Model.Password);

            if (User != null)
            {
                Session["UserName"] = User; // Session methoduna eşleşen veriler atılır ve çıkış yapılana kadar orda saklanır.

                return RedirectToAction("Index", "Home");

            }
            else
                ViewBag.HATA="KULLANICIADI VEYA ŞİFRE YANLIŞ!!!";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Session verileri sıfırlanır
            return View();

        }
    }
}