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
    public class BankCartTypesController : BaseController  
    {
        private TIEB_BudgetDBEntities db = new TIEB_BudgetDBEntities(); 

        // GET: BankCartTypes
        // BankCartType tablosundaki verileri PersonID ye göre listeler
        public ActionResult Index()
        {
            var bankCartType = db.BankCartType.Include(b => b.Persons); 
            return View(bankCartType.ToList());
        }

        // GET: BankCartTypes/Details/5
        // Belirtilen id değerine uygun kişinin Banka bilgilerini getirir
        public ActionResult Details(int? id) 
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankCartType bankCartType = db.BankCartType.Find(id);
            if (bankCartType == null)
            {
                return HttpNotFound();
            }
            return View(bankCartType);
        }

        // GET: BankCartTypes/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name");
            return View();
        }

        // POST: BankCartTypes/Create
        // View/BankCartTypes/Create üzerinden Post edilen verileri alır ve Databaseye kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BankCartID,PersonID,BankCartName")] BankCartType bankCartType)
        {
            if (ModelState.IsValid)
            {
                db.BankCartType.Add(bankCartType);
                db.SaveChanges();
                return RedirectToAction("Create"); // Veri kaydedildikten sonra tekrardan Create ekranına yönlendirir.
            }

            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", bankCartType.PersonID);
            return View(bankCartType);
        }

        // GET: BankCartTypes/Edit/5
        // Belirtilen id ile kişinin Database deki Banka bilgilerini Edit ekranındaki nesnelere doldurur
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankCartType bankCartType = db.BankCartType.Find(id);
            if (bankCartType == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", bankCartType.PersonID);
            return View(bankCartType);
        }

        // POST: BankCartTypes/Edit/5
        // View/BankCartType/Crate üzerinden Post edilen bilgileri Database üzerine tekrardan kaydeder yani değiştirir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BankCartID,PersonID,BankCartName")] BankCartType bankCartType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankCartType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", bankCartType.PersonID);
            return View(bankCartType);
        }

        // GET: BankCartTypes/Delete/5
        // id si belli olan veriyi View/BankCartTypes/Delete ekranına gösterir ve silip silmeyeceğini sorar
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankCartType bankCartType = db.BankCartType.Find(id);
            if (bankCartType == null)
            {
                return HttpNotFound();
            }
            return View(bankCartType);
        }

        // POST: BankCartTypes/Delete/5
        // View/BankCartTypes/Delete ekranında sil butonuna basılıp Post edilen bilgi id si ile Database den silinir
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankCartType bankCartType = db.BankCartType.Find(id);
            db.BankCartType.Remove(bankCartType);
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
