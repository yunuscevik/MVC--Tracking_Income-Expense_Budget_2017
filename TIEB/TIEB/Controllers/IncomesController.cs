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
using PagedList;

namespace TIEB.Controllers
{
    // Ana bir controller tarafından inherit edilmiştir giriş yapılınca devreye girer
    public class IncomesController : BaseController
    {
        private TIEB_BudgetDBEntities db = new TIEB_BudgetDBEntities();

        //Incomes/Index Gelirlerin listelendiği sayfadır Search ve Sort işlemlerini de bu Method yapar.
        public ActionResult Index(string SearchString1, string SearchString2, string date1, string date2, string amount1, string amount2,
            string Sorting_Order, string sortOrder,string currentFilter, string currentFilter1, string currentFilter2, int? page)
        {
            ViewBag.SortingAmount = String.IsNullOrEmpty(Sorting_Order) ? "Amount" : "";
            ViewBag.SortingDate = String.IsNullOrEmpty(Sorting_Order) ? "Date" : "";
            ViewBag.SortingIncomeName = String.IsNullOrEmpty(Sorting_Order) ? "IncomeName" : "";
            ViewBag.SortingExplanation = String.IsNullOrEmpty(Sorting_Order) ? "Explanation" : "";

            var name = Session["UserName"] as Persons;
            if (name == null) { return View(); }

            if (Sorting_Order == null)
                Sorting_Order = sortOrder;
            ViewBag.CurrentSort = Sorting_Order;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (SearchString1 != null || SearchString2 != null || date1 != null || date2 != null || amount1 != null || amount2 != null)
            {
                page = 1;
                
                TempData["temp1"] = date1;
                TempData.Keep();
                TempData["temp2"] = date2;
                TempData.Keep();
                TempData["temp3"] = amount1;
                TempData.Keep();
                TempData["temp4"] = amount2;
                TempData.Keep();
                TempData["SearchString1"] = SearchString1;
                TempData.Keep();
                TempData["SearchString2"] = SearchString2;
                TempData.Keep();
            }
            else
            {
                string temp1 = "", temp2 = "", temp3 = "", temp4 = "";
                string tempSearchString1 = "", tempSearchString2 = "";


                temp1 = Convert.ToString(TempData["temp1"]);
                TempData.Keep();
                temp2 = Convert.ToString(TempData["temp2"]);
                TempData.Keep();
                temp3 = Convert.ToString(TempData["temp3"]);
                TempData.Keep();
                temp4 = Convert.ToString(TempData["temp4"]);
                TempData.Keep();
                tempSearchString1 = Convert.ToString(TempData["SearchString1"]);
                TempData.Keep();
                tempSearchString2 = Convert.ToString(TempData["SearchString2"]);
                TempData.Keep();

                
                

                if (temp1 != "")
                    date1 = currentFilter1;
                if (temp2 != "")
                    date2 = currentFilter2;
                if (temp3 != "")
                    amount1 = currentFilter1;
                if (temp4 != "")
                    amount2 = currentFilter2;
                if(tempSearchString1 != "")
                    SearchString1 = currentFilter;
                if (tempSearchString2 != "")
                    SearchString2 = currentFilter;
            }

            var income = from inc in db.Income select inc;

            switch (Sorting_Order)
            {
                case "Amount":
                    income = income.OrderBy(x => x.Amount).Where(x => x.PersonID == name.PersonID); 
                    break;
                case "Date":
                    income = income.OrderBy(x => x.Date).Where(x => x.PersonID == name.PersonID); 
                    break;
                case "IncomeName":
                    income = (from i in db.Income
                              join it in db.IncomeType on i.IncomeType equals it.IncomeTypeID
                              where i.PersonID == name.PersonID
                              orderby it.IncomeName
                              select i);
                    break;
                case "Explanation":
                    income = income.OrderBy(e => e.Explanation).Where(x => x.Explanation != null && x.PersonID == name.PersonID); 
                    break;
                default:
                    income = income.OrderBy(x => x.IncomeID).Where(x => x.PersonID == name.PersonID); 
                    break;

            }
            
            if (!String.IsNullOrEmpty(SearchString1))
            {
                
                ViewBag.CurrentFilter = SearchString1;
                income = (from i in db.Income
                               join it in db.IncomeType on i.IncomeType equals it.IncomeTypeID
                               where it.IncomeName.Contains(SearchString1) && i.PersonID == name.PersonID
                               orderby i.IncomeID
                               select i);
            }
            else if (!String.IsNullOrEmpty(SearchString2))
            {
                ViewBag.CurrentFilter = SearchString2;
                income = income.Where(x => x.Explanation.Contains(SearchString2)
                && x.PersonID == name.PersonID).OrderBy(x => x.IncomeID);
            }
            else if (!String.IsNullOrEmpty(date1) && !String.IsNullOrEmpty(date2))
            {
                ViewBag.CurrentFilter1 = date1;
                ViewBag.CurrentFilter2 = date2;
                DateTime d1 = Convert.ToDateTime(date1);
                DateTime d2 = Convert.ToDateTime(date2);
                income = income.Where(x => x.Date >= d1 && x.Date <= d2
                && x.PersonID == name.PersonID).OrderBy(x => x.IncomeID);
            }
            else if (!String.IsNullOrEmpty(date1))
            {
                ViewBag.CurrentFilter1 = date1;
                DateTime d1 = Convert.ToDateTime(date1);
                income = income.Where(x => x.Date >= d1
                && x.PersonID == name.PersonID).OrderBy(x => x.IncomeID);
            }
            else if (!String.IsNullOrEmpty(date2))
            {
                ViewBag.CurrentFilter2 = date2;
                DateTime d2 = Convert.ToDateTime(date2);
                income = income.Where(x => x.Date <= d2
                && x.PersonID == name.PersonID).OrderBy(x => x.IncomeID);
            }

            else if (!String.IsNullOrEmpty(amount1) && !String.IsNullOrEmpty(amount2))
            {
                ViewBag.CurrentFilter1 = amount1;
                ViewBag.CurrentFilter2 = amount2;
                decimal a1 = Convert.ToDecimal(amount1);
                decimal a2 = Convert.ToDecimal(amount2);
                income = income.Where(x => x.Amount >= a1 && x.Amount <= a2
                && x.PersonID == name.PersonID).OrderBy(x => x.IncomeID);
            }
            else if (!String.IsNullOrEmpty(amount1))
            {
                ViewBag.CurrentFilter1 = amount1;
                decimal a1 = Convert.ToDecimal(amount1);
                income = income.Where(x => x.Amount >= a1
                && x.PersonID == name.PersonID).OrderBy(x => x.IncomeID);
            }
            else if (!String.IsNullOrEmpty(date2))
            {
                ViewBag.CurrentFilter2 = amount2;
                decimal a2 = Convert.ToDecimal(amount2);
                income = income.Where(x => x.Amount <= a2
                && x.PersonID == name.PersonID).OrderBy(x => x.IncomeID);
            }

            Total(income);
            ViewBag.HtmlStr = income.Count();
            return View(income.ToPagedList(pageNumber, pageSize));
        }

        // GET: Incomes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Income.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }
            return View(income);
        }

        // GET: Incomes/Create
        public ActionResult Create()
        {
            ViewBag.IncomeType = new SelectList(db.IncomeType, "IncomeTypeID", "IncomeName");
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name");
            return View();
        }

        // POST: Incomes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IncomeID,PersonID,IncomeType,Amount,Date,Explanation")] Income income)
        {
            if (ModelState.IsValid)
            {
                
                db.Income.Add(income);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IncomeType = new SelectList(db.IncomeType, "IncomeTypeID", "IncomeName", income.IncomeType);
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", income.PersonID);
            return View(income);
        }

        [HttpPost]
        public JsonResult IncomeTypeOther(int id)
        {
            var returnRsult = new ReturnResult();
            foreach (var item in db.IncomeType.Where(e => e.IncomeTypeID == id))
            {
                if (item.IncomeName == "DİĞER")
                {
                    returnRsult.Message = "AppearTextBox";
                }
                
                
            }
            return Json(returnRsult, JsonRequestBehavior.AllowGet);
        }

        // GET: Incomes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Income.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }
            ViewBag.IncomeType = new SelectList(db.IncomeType, "IncomeTypeID", "IncomeName", income.IncomeType);
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", income.PersonID);
            return View(income);
        }

        // POST: Incomes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IncomeID,PersonID,IncomeType,Amount,Date,Explanation")] Income income)
        {
            if (ModelState.IsValid)
            {
                db.Entry(income).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IncomeType = new SelectList(db.IncomeType, "IncomeTypeID", "IncomeName", income.IncomeType);
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", income.PersonID);
            return View(income);
        }

        // GET: Incomes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Income income = db.Income.Find(id);
            if (income == null)
            {
                return HttpNotFound();
            }
            return View(income);
        }

        // POST: Incomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Income income = db.Income.Find(id);
            db.Income.Remove(income);
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

        [HttpPost]
        public JsonResult SearchType(string value)
        {
            var returnRsult = new ReturnResult();
            if (value == "IncomeNameSearch")
            {
                returnRsult.Message = "AppearIncomeName";
            }
            else if (value == "AmountSearch")
            {
                returnRsult.Message = "AppearAmount";
            }
            else if (value == "DateSearch")
            {
                returnRsult.Message = "AppearDate";
            }
            else if (value == "OtherIncomeNameSearch")
            {
                returnRsult.Message = "AppearOtherIncomeName";
            }
            return Json(returnRsult, JsonRequestBehavior.AllowGet);
        }

        private class ReturnResult
        {
            public string Message { get; set; }
        }

        private void Total(IQueryable<Income> income)
        {
            decimal total = 0;
            var name = Session["UserName"] as Persons;
            if (name == null) { return; }
            foreach (var item in income.Where(x => x.PersonID == name.PersonID))
            {
                total += item.Amount;
            }

            ViewBag.TOTAL = total.ToString() + " " + "TL";
        }
    }
}
