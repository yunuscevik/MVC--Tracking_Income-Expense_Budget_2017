using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TIEB.Models;

namespace TIEB.Controllers
{
    public class PersonsController : Controller
    {
        private TIEB_BudgetDBEntities db = new TIEB_BudgetDBEntities();

        // GET: Persons
        public ActionResult Index()
        {
            return View();
        }
        // Persons tablosundaki verileri PersonID ye göre listeler ayrıca giriş yapılan kişinin profilini gösterir
        public ActionResult List()
        {
            var persons = db.Persons.Include(p => p.BankCartType).Include(p => p.CreditCartType).Include(p => p.Expense).Include(p => p.Income);
            return View(persons.ToList());
        }

        // GET: Persons/Details/5
        // Belirtilen id değerine uygun kişinin Banka bilgilerini getirir
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persons persons = db.Persons.Find(id);
            if (persons == null)
            {
                return HttpNotFound();
            }
            return View(persons);
        }

        // GET: Persons/Create
        public ActionResult Create()
        {
            ViewBag.PersonID = new SelectList(db.BankCartType, "BankCartID", "BankCartName");
            ViewBag.PersonID = new SelectList(db.CreditCartType, "CreditCartID", "CreditCartName");
            ViewBag.PersonID = new SelectList(db.Expense, "ExpenseID", "ExpenseID");
            ViewBag.PersonID = new SelectList(db.Income, "IncomeID", "IncomeID");
            return View();
        }

        // POST: Persons/Create
        // View/Persons/Create üzerinden Post edilen verileri alır ve Databaseye kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonID,Name,SurName,UserName,Password,Email,Gender,BrithDate")] Persons persons)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in db.Persons)
                {
                    if (persons.UserName == item.UserName) // Kişi eklerken Kullanıcı adı eğer Database de mevcutsa hata verir 
                    {
                        ViewBag.HATA = "Bu Kullanıcı Adı Daha Önceden Alınmış";
                        return View("Create");
                    }
                    if (persons.Email == item.Email) // Kişi eklerken mail adresi eğer Database de mevcutsa hata verir
                    {
                        ViewBag.HATA = "Bu Mail Adresi Daha Önceden Alınmış";
                        return View("Create");
                    }
                }
                db.Persons.Add(persons);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.PersonID = new SelectList(db.BankCartType, "BankCartID", "BankCartName", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.CreditCartType, "CreditCartID", "CreditCartName", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.Expense, "ExpenseID", "ExpenseID", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.Income, "IncomeID", "IncomeID", persons.PersonID);
            return View(persons);
        }

        // GET: Persons/Edit/5
        // Belirtilen id ile kişinin Database deki Banka bilgilerini Edit ekranındaki nesnelere doldurur
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persons persons = db.Persons.Find(id);
            if (persons == null)
            {
                return HttpNotFound();
            }
            ViewBag.PersonID = new SelectList(db.BankCartType, "BankCartID", "BankCartName", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.CreditCartType, "CreditCartID", "CreditCartName", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.Expense, "ExpenseID", "ExpenseID", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.Income, "IncomeID", "IncomeID", persons.PersonID);
            return View(persons);
        }

        // POST: Persons/Edit/5
        // View/Persons/Edit üzerinden Post edilen bilgileri Database üzerine tekrardan kaydeder yani değiştirir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonID,Name,SurName,UserName,Password,Email,Gender,BrithDate")] Persons persons)
        {
            if (ModelState.IsValid)
            {
                db.Entry(persons).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PersonID = new SelectList(db.BankCartType, "BankCartID", "BankCartName", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.CreditCartType, "CreditCartID", "CreditCartName", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.Expense, "ExpenseID", "ExpenseID", persons.PersonID);
            ViewBag.PersonID = new SelectList(db.Income, "IncomeID", "IncomeID", persons.PersonID);
            return View(persons);
        }

        // GET: Persons/Delete/5
        // id si belli olan veriyi View/BankCartTypes/Delete ekranına gösterir ve silip silmeyeceğini sorar
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persons persons = db.Persons.Find(id);
            if (persons == null)
            {
                return HttpNotFound();
            }
            return View(persons);
        }

        // POST: Persons/Delete/5
        // View/Persons/Delete ekranında sil butonuna basılıp Post edilen bilgi id si ile Database den silinir
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Persons persons = db.Persons.Find(id);
            db.Persons.Remove(persons);
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
