using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TIEB.MainController;
using TIEB.Models;

namespace TIEB.Controllers
{
    // Ana bir controller tarafından inherit edilmiştir giriş yapılınca devreye girer
    public class CreditCartTypesController : BaseController
    {
        private TIEB_BudgetDBEntities db = new TIEB_BudgetDBEntities();

        // GET: CreditCartTypes
        // CreditCartType tablosundaki verileri PersonID ye göre listeler
        public ActionResult Index()
        {
            var creditCartType = db.CreditCartType.Include(c => c.Persons);
            return View(creditCartType.ToList());
        }

        // GET: CreditCartTypes/Details/5
        // Belirtilen id değerine uygun kişinin Kredi kartı bilgilerini getirir
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCartType creditCartType = db.CreditCartType.Find(id);
            if (creditCartType == null)
            {
                return HttpNotFound();
            }
            return View(creditCartType);
        }

        // GET: CreditCartTypes/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name");
            return View();
        }

        // POST: CreditCartTypes/Create
        // View/Create üzerinden Post edilen verileri alır ve Databaseye kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CreditCartID,PersonID,CreditCartName")] CreditCartType creditCartType)
        {
            if (ModelState.IsValid)
            {
                db.CreditCartType.Add(creditCartType);
                db.SaveChanges();
                return RedirectToAction("Create"); // Veri kaydedildikten sonra tekrardan Create ekranına yönlendirir.
            }

            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", creditCartType.PersonID);
            return View(creditCartType);
        }

        // GET: CreditCartTypes/Edit/5
        // Belirtilen id ile kişinin Database deki Kredi kartı bilgilerini Edit ekranındaki nesnelere doldurur
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCartType creditCartType = db.CreditCartType.Find(id);
            if (creditCartType == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", creditCartType.PersonID);
            return View(creditCartType);
        }

        // POST: CreditCartTypes/Edit/5
        // View/CreditCartTypes/Crate üzerinden Post edilen bilgileri Database üzerine tekrardan kaydeder yani değiştirir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CreditCartID,PersonID,CreditCartName")] CreditCartType creditCartType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(creditCartType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", creditCartType.PersonID);
            return View(creditCartType);
        }

        // GET: CreditCartTypes/Delete/5
        // id si belli olan veriyi View/CreditCartTypes/Delete ekranına gösterir ve silip silmeyeceğini sorar
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreditCartType creditCartType = db.CreditCartType.Find(id);
            if (creditCartType == null)
            {
                return HttpNotFound();
            }
            return View(creditCartType);
        }

        // POST: CreditCartTypes/Delete/5
        // View/CreditCartTypes/Delete ekranında sil butonuna basılıp Post edilen bilgi id si ile Database den silinir
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CreditCartType creditCartType = db.CreditCartType.Find(id);
            db.CreditCartType.Remove(creditCartType);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // Dispose methodu ile Database bağlantısı kesilir ve Bellekte şişmeler engellenir
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
