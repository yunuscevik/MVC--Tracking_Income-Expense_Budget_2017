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
    public class ExpensesController : BaseController
    {
        private TIEB_BudgetDBEntities db = new TIEB_BudgetDBEntities();
        public static int InstallmentID; // Taksit bilgisinin tutulacağı değişken


        //Expenses/Index Gelirlerin listelendiği sayfadır Search ve Sort işlemlerini de bu Method yapar.
        public ActionResult Index(string SearchString1, string SearchString2, string SearchString3, string SearchString4,
            string date1, string date2, string amount1, string amount2, string Sorting_Order, string sortOrder, string SearchString5,
            string currentFilter, string currentFilter1, string currentFilter2, int? page,string value)
        {
            ViewBag.SortingPaymentType = String.IsNullOrEmpty(Sorting_Order) ? "PaymentType" : "";
            ViewBag.SortingAmount = String.IsNullOrEmpty(Sorting_Order) ? "Amount" : "";
            ViewBag.SortingDate = String.IsNullOrEmpty(Sorting_Order) ? "Date" : "";
            ViewBag.SortingBank = String.IsNullOrEmpty(Sorting_Order) ? "Bank" : "";
            ViewBag.SortingCredit = String.IsNullOrEmpty(Sorting_Order) ? "Credit" : "";
            ViewBag.SortingExpenseName = String.IsNullOrEmpty(Sorting_Order) ? "ExpenseName" : "";
            ViewBag.SortingPaymentInfo = String.IsNullOrEmpty(Sorting_Order) ? "PaymentInfo" : "";
            ViewBag.SortingExplanation = String.IsNullOrEmpty(Sorting_Order) ? "Explanation" : "";
            ViewBag.SortingPaid = String.IsNullOrEmpty(Sorting_Order) ? "Paid" : "";
            ViewBag.SortingNotPaid = String.IsNullOrEmpty(Sorting_Order) ? "NotPaid" : "";

            var name = Session["UserName"] as Persons;
            if (name == null) { return View(); }

            if (Sorting_Order == null)
                Sorting_Order = sortOrder;
            ViewBag.CurrentSort = Sorting_Order;
            

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            if (SearchString1 != null || SearchString2 != null || SearchString3 != null || SearchString4 != null ||
                date1 != null || date2 != null || amount1 != null || amount2 != null)
            {
                // Search değerleri null değilseler page değişkenine 1 atanır ve geri kalan search değerleri TempData da tutulur
                page = 1;
                TempData["temp1"] = SearchString1;
                TempData.Keep();
                TempData["temp2"] = SearchString2;
                TempData.Keep();
                TempData["temp3"] = SearchString3;
                TempData.Keep();
                TempData["temp4"] = SearchString4;
                TempData.Keep();
                TempData["temp5"] = date1;
                TempData.Keep();
                TempData["temp6"] = date2;
                TempData.Keep();
                TempData["temp7"] = amount1;
                TempData.Keep();
                TempData["temp8"] = amount2;
                TempData.Keep();
                TempData["temp9"] = SearchString5;
            }
            else
            {
                // Search değerleri null ise TempDataların içinde bulunan değerler geçici templere aktarılır
                string temp1 = "", temp2 = "", temp3 = "", temp4 = "";
                string temp5 = "", temp6 = "", temp7 = "", temp8 = "", temp9 = "";
                
                
                temp1 = Convert.ToString( TempData["temp1"]);
                TempData.Keep();
                temp2 = Convert.ToString(TempData["temp2"]);
                TempData.Keep();
                temp3 = Convert.ToString(TempData["temp3"]);
                TempData.Keep();
                temp4 = Convert.ToString(TempData["temp4"]);
                TempData.Keep();
                temp5 = Convert.ToString(TempData["temp5"]);
                TempData.Keep();
                temp6 = Convert.ToString(TempData["temp6"]);
                TempData.Keep();
                temp7 = Convert.ToString(TempData["temp7"]);
                TempData.Keep();
                temp8 = Convert.ToString(TempData["temp8"]);
                TempData.Keep();
                temp9 = Convert.ToString(TempData["temp9"]);
                TempData.Keep();

                // Templerin içi kontrol edilir ve boş olmayan değer varsa currenFilter e 
                // kaydedilen değer Search değerlerinden birine aktarılır
                if (temp1 != "")
                    SearchString1 = currentFilter;
                else if (temp2 != "")
                    SearchString2 = currentFilter;
                else if (temp3 != "")
                    SearchString3 = currentFilter;
                else if (temp4 != "")
                    SearchString4 = currentFilter;
                else if (temp9 != "")
                    SearchString5 = currentFilter;

                if (temp5 != "")
                    date1 = currentFilter1;
                if (temp6 != "")
                    date2 = currentFilter2;
                if (temp7 != "")
                    amount1 = currentFilter1;
                if (temp8 != "")
                    amount2 = currentFilter2;
                

            }


            // Expense Tablosundaki değerler expense değişkenine aktarılır
            var expense = from exp in db.Expense select exp;

            // Sorting_Order değişkenlerine göre sıralama işlemleri switch case default yapısıyla gerçekleşir
            switch (Sorting_Order)
            {
                case "PaymentType":
                    expense = expense.OrderBy(x => x.PaymentType).Where(x => x.PersonID == name.PersonID);
                    break;
                case "Amount":
                    expense = expense.OrderBy(x => x.Amount).Where(x => x.PersonID == name.PersonID);
                    break;
                case "Date":
                    expense = expense.OrderBy(x => x.Date).Where(x=> x.PersonID == name.PersonID);
                    break;
                case "PaymentInfo":
                    expense = expense.OrderBy(x => x.PaymentInfo).Where(x=>x.PaymentInfo != null && x.PersonID == name.PersonID);
                    break;
                case "Bank":
                    expense = (from e in db.Expense   // İlişkili tablolar olduğundan join kullanılır. BankID ve BankCartID değerine göre
                               join et in db.BankCartType on e.BankID equals et.BankCartID
                               where e.PersonID == name.PersonID
                               orderby et.BankCartName
                               select e);
                    break;
                case "Credit":
                    expense = (from e in db.Expense  // İlişkili tablolar olduğundan join kullanılır. CreditID ve CreditCartID değerine göre
                               join et in db.CreditCartType on e.CreditID equals et.CreditCartID
                               where e.PersonID == name.PersonID
                               orderby et.CreditCartName
                               select e);
                    break;
                case "ExpenseName":
                    expense = (from e in db.Expense // İlişkili tablolar olduğundan join kullanılır. ExpenseType ve ExpenseTypeID değerine göre
                               join et in db.ExpenseType on e.ExpenseType equals et.ExpenseTypeID
                               where e.PersonID == name.PersonID
                               orderby et.ExpenseName
                               select e);
                    break;
                case "Explanation":
                    expense = expense.OrderBy(e => e.Explanation).Where(x=>x.Explanation != null && x.PersonID == name.PersonID);
                    break;
                case "Paid":
                    expense = expense.OrderBy(e => e.ExpenseID).Where(x => x.PaymentInfo.Contains("Ödendi") && x.PersonID == name.PersonID);
                    break;
                case "NotPaid":
                    expense = expense.OrderBy(e => e.ExpenseID).Where(x => x.PaymentInfo.Contains("Ödenmedi") && x.PersonID == name.PersonID);
                    break;
                default:
                    expense = expense.OrderBy(x => x.ExpenseID).Where(x=>x.PersonID==name.PersonID);
                    break;

            }

            // String tipindeki Search değerlerinin boş olup olmadığı kontrol edilir ve Search değerleri içindeki 
            // değerler belirli tablolar içindek filtreleme işlemi yapar
            if (!String.IsNullOrEmpty(SearchString1))
            {
                ViewBag.CurrentFilter = SearchString1;
                expense = (from e in db.Expense
                           join et in db.ExpenseType on e.ExpenseType equals et.ExpenseTypeID
                                where et.ExpenseName.Contains(SearchString1) && e.PersonID == name.PersonID
                           orderby e.ExpenseID
                                select e);
            }
            else if (!String.IsNullOrEmpty(SearchString2))
            {
                ViewBag.CurrentFilter = SearchString2;
                expense = expense.Where(x => x.PaymentType.Contains(SearchString2) 
                && x.PersonID == name.PersonID).OrderBy(x => x.ExpenseID);
            }
            else if (!String.IsNullOrEmpty(SearchString3))
            {
                ViewBag.CurrentFilter = SearchString3;
                expense = (from e in db.Expense
                                join b in db.BankCartType on e.BankID equals b.BankCartID
                                where b.BankCartName.Contains(SearchString3) && e.PersonID == name.PersonID
                           orderby e.ExpenseID
                                select e);
            }
            else if (!String.IsNullOrEmpty(SearchString4))
            {
                ViewBag.CurrentFilter = SearchString4;
                expense = (from e in db.Expense
                                join c in db.CreditCartType on e.CreditID equals c.CreditCartID
                                where c.CreditCartName.Contains(SearchString4) && e.PersonID == name.PersonID
                           orderby e.ExpenseID
                                select e);
            }
            if (!String.IsNullOrEmpty(SearchString5))
            {
                ViewBag.CurrentFilter = SearchString2;
                expense = expense.Where(x => x.Explanation.Contains(SearchString5)
                && x.PersonID == name.PersonID).OrderBy(x => x.ExpenseID);
            }
            else if (!String.IsNullOrEmpty(date1) && !String.IsNullOrEmpty(date2))
            {
                ViewBag.CurrentFilter1 = date1;
                ViewBag.CurrentFilter2 = date2;
                DateTime d1 = Convert.ToDateTime(date1);
                DateTime d2 = Convert.ToDateTime(date2);
                expense = expense.Where(x => x.Date >= d1 && x.Date <= d2
                && x.PersonID == name.PersonID).OrderBy(x => x.ExpenseID);
            }
            else if (!String.IsNullOrEmpty(date1))
            {
                ViewBag.CurrentFilter1 = date1;
                DateTime d1 = Convert.ToDateTime(date1);
                expense = expense.Where(x => x.Date >= d1
                && x.PersonID == name.PersonID).OrderBy(x => x.ExpenseID);
            }
            else if (!String.IsNullOrEmpty(date2))
            {
                ViewBag.CurrentFilter2 = date2;
                DateTime d2 = Convert.ToDateTime(date2);
                expense = expense.Where(x => x.Date <= d2
                && x.PersonID == name.PersonID).OrderBy(x => x.ExpenseID);
            }

            else if (!String.IsNullOrEmpty(amount1) && !String.IsNullOrEmpty(amount2))
            {
                ViewBag.CurrentFilter1 = amount1;
                ViewBag.CurrentFilter2 = amount2;
                decimal a1 = Convert.ToDecimal(amount1);
                decimal a2 = Convert.ToDecimal(amount2);
                expense = expense.Where(x => x.Amount >= a1 && x.Amount <= a2
                && x.PersonID == name.PersonID).OrderBy(x => x.ExpenseID);
            }
            else if (!String.IsNullOrEmpty(amount1))
            {
                ViewBag.CurrentFilter1 = amount1;
                decimal a1 = Convert.ToDecimal(amount1);
                expense = expense.Where(x => x.Amount >= a1
                && x.PersonID == name.PersonID).OrderBy(x => x.ExpenseID);
            }
            else if (!String.IsNullOrEmpty(date2))
            {
                ViewBag.CurrentFilter2 = amount2;
                decimal a2 = Convert.ToDecimal(amount2);
                expense = expense.Where(x => x.Amount <= a2
                && x.PersonID == name.PersonID).OrderBy(x => x.ExpenseID);
            }

            Total(expense);
            ViewBag.HtmlStr = expense.Count();
            return View(expense.ToPagedList(pageNumber, pageSize));
        }

        // GET: Expenses/Details/5
        // Belirtilen id değerine uygun kişinin Harcama(Gider) bilgilerini getirir
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expense.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // GET: Expenses/Create
        public ActionResult Create()
        {
            var name = Session["UserName"] as Persons;
            if (name == null) { return View(); }
            InstallmentType(); // Taksit seçenekleri için InstallmentType Methodu çağrılır
            ViewBag.BankID = new SelectList(db.BankCartType.Where(e=>e.PersonID==name.PersonID), "BankCartID", "BankCartName");
            ViewBag.CreditID = new SelectList(db.CreditCartType.Where(e => e.PersonID == name.PersonID), "CreditCartID", "CreditCartName");
            ViewBag.ExpenseType = new SelectList(db.ExpenseType, "ExpenseTypeID", "ExpenseName");
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name");
            return View();
        }

        // POST: Expenses/Create
        // View/Expenses/Create üzerinden Post edilen verileri alır ve Databaseye kaydeder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpenseID,PersonID,ExpenseType,PaymentType,Amount,Date,BankID,CreditID,InstallmentInfo,Explanation")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                // Taksit bilgisinin tutulduğu değişken eğer 1 den büyükse Viewden dönen miktar taksit değerine bölünür
                // böylece taksitli miktar bulunur
                if (InstallmentID > 1) 
                {
                    
                    DateTime InstallmentDate = expense.Date;
                    Decimal money = expense.Amount / InstallmentID;

                    // Döngü taksit değeri kadar döner ve databaseye kaydedilir
                    for (int i = 0; i < InstallmentID; i++)
                    {
                       
                        DateTime future = InstallmentDate.AddMonths(i);
                        expense.Date = future;
                        expense.Amount = money;
                        expense.InstallmentInfo = (i + 1).ToString() + "/" + InstallmentID.ToString();
                        expense.PaymentInfo = "Ödenmedi";
                        db.Expense.Add(expense);
                        db.SaveChanges();
                    }
                }
                else{
                    // Taksit değeri 1 ya seçilmediyse nakit işlem gibi çalışır
                    expense.PaymentInfo = "Ödenmedi";
                    db.Expense.Add(expense);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            ViewBag.BankID = new SelectList(db.BankCartType, "BankCartID", "BankCartName", expense.BankID);
            ViewBag.CreditID = new SelectList(db.CreditCartType, "CreditCartID", "CreditCartName", expense.CreditID);
            ViewBag.ExpenseType = new SelectList(db.ExpenseType, "ExpenseTypeID", "ExpenseName", expense.ExpenseType);
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", expense.PersonID);
            return View(expense);
        }
        // installment DropdownList te bulunan taksit değerlerinden biri seçildiği taktirde Json verisi olarak gelir.
        // JsonResult methodunun parametresinde yakalanan değer InstallmentID ye aktarılır ve değer tekrardan dönderilir.
        // Amaç burda seçilen değerin yakalanmasıdır.
        [HttpPost]
        public JsonResult CreateInstallment(int installment)
        {
            InstallmentID = installment;
            return Json(InstallmentID, JsonRequestBehavior.AllowGet);
        }
        // Taksit bilgilerini kaçtan kaça kadar olduğunu belirten yardımcı methoddur.
        private void InstallmentType()
        {
            var value = new List<string>();
            for (int i = 1; i <= 48; i++)
            {
                value.Add(i.ToString());
            }
            ViewBag.installment = new SelectList(value);
        }

        // GET: Expenses/Edit/5
        // Belirtilen id ile kişinin Database deki Banka bilgilerini Edit ekranındaki nesnelere doldurur
        public ActionResult Edit(int? id)
        {
            var name = Session["UserName"] as Persons;
            if (name == null) { return View(); }
            InstallmentType();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Expense expense = db.Expense.Find(id);
            
            if (expense == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankID = new SelectList(db.BankCartType.Where(e => e.PersonID == name.PersonID), "BankCartID", "BankCartName", expense.BankID);
            ViewBag.CreditID = new SelectList(db.CreditCartType.Where(e => e.PersonID == name.PersonID), "CreditCartID", "CreditCartName", expense.CreditID);
            ViewBag.ExpenseType = new SelectList(db.ExpenseType, "ExpenseTypeID", "ExpenseName", expense.ExpenseType);
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", expense.PersonID);
            return View(expense);
        }

        // POST: Expenses/Edit/5
        // View/Expenses/Edit üzerinden Post edilen bilgileri Database üzerine tekrardan kaydeder yani değiştirir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExpenseID,PersonID,ExpenseType,PaymentType,Amount,Date,BankID,CreditID,PaymentInfo,InstallmentInfo,Explanation")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankID = new SelectList(db.BankCartType, "BankCartID", "BankCartName", expense.BankID);
            ViewBag.CreditID = new SelectList(db.CreditCartType, "CreditCartID", "CreditCartName", expense.CreditID);
            ViewBag.ExpenseType = new SelectList(db.ExpenseType, "ExpenseTypeID", "ExpenseName", expense.ExpenseType);
            ViewBag.PersonID = new SelectList(db.Persons, "PersonID", "Name", expense.PersonID);
            return View(expense);
        }



        // GET: Expenses/Delete/5
        // id si belli olan veriyi View/Expenses/Delete ekranına gösterir ve silip silmeyeceğini sor
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expense.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: Expenses/Delete/5
        // View/Expenses/Delete ekranında sil butonuna basılıp Post edilen bilgi id si ile Database den silinir
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expense expense = db.Expense.Find(id);
            db.Expense.Remove(expense);
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


        // Yardımcı methoddur. ExpenseType DropDownListinden seçilen verinin id sini yakalar ve ExpenseType tablosundaki ExpenseTypeID e 
        // eşit olan verileri getirir. Eğer bu veri "DİĞER" e eşit ise json olarak "AppearTextBox" geri dönderilir
        // Amaç Harcama türü DropDownListinde DİĞER seçildiğinde Hidden olan bir TextBox ın gösterilmesi
        [HttpPost]
        public JsonResult ExpenseTypeOther(int id)
        {
            var returnRsult = new ReturnResult();
            foreach (var item in db.ExpenseType.Where(e=>e.ExpenseTypeID==id))
            {
                if (item.ExpenseName == "DİĞER")
                {
                    returnRsult.Message = "AppearTextBox";
                }
            }
            return Json(returnRsult, JsonRequestBehavior.AllowGet);
        }

        // Yardımcı methoddur. PaymentType DropDownListinden seçilen verinin value sini yakalar.
        // Yakalanan değerlere göre json olarak değerler geri dönderilir.
        // Amaç Ödeme türü DropDownListinde seçilen değerlerin value sine göre json tipinde veri döndürmektir.
        [HttpPost]
        public JsonResult PaymentType(string value)
        {
            var returnRsult = new ReturnResult();
            if (value == "Nakit")
            {
                returnRsult.Message = "AppearCash";
            }
            else if (value == "Kart")
            {
                returnRsult.Message = "AppearCard";
            }
            else if (value == "Banka")
            {
                returnRsult.Message = "AppearBank";
            }
            return Json(returnRsult, JsonRequestBehavior.AllowGet);
        }

        // Yardımcı methoddur. SearchType DropDownListinden seçilen verinin value sini yakalar.
        // Yakalanan değerlere göre json olarak değerler geri dönderilir.
        // Amaç Arama kriteri DropDownListinde seçilen değerlerin value sine göre json tipinde veri döndürmektir.
        [HttpPost]
        public JsonResult SearchType(string value)
        {
            var returnRsult = new ReturnResult();
            if (value == "PaymentSearch")
            {
                returnRsult.Message = "AppearPayment";
            }
            else if (value == "AmountSearch")
            {
                returnRsult.Message = "AppearAmount";
            }
            else if (value == "DateSearch")
            {
                returnRsult.Message = "AppearDate";
            }
            else if (value == "ExpenseTypeSearch")
            {
                returnRsult.Message = "AppearExpenseT";
            }
            else if (value == "BankSearch")
            {
                returnRsult.Message = "AppearBank";
            }
            else if (value == "CreditSearch")
            {
                returnRsult.Message = "AppearCredit";
            }
            else if (value == "OtherExpenseTypeSearch")
            {
                returnRsult.Message = "AppearOtherExpenseT";
            }
            return Json(returnRsult, JsonRequestBehavior.AllowGet);
        }

        private class ReturnResult
        {
            public string Message { get; set; }
        }

        
        // Expense Tablosundaki Miktarların toplanıp View da gösterilmesi için oluşturulmuş yardımcı methoddur.
        private void Total(IQueryable<Expense> expense)
        {
            decimal total = 0;
            var name = Session["UserName"] as Persons;
            if (name == null) { return; }
            foreach (var item in expense.Where(x=>x.PersonID==name.PersonID))
            {
                total += item.Amount;
            }

            ViewBag.TOTAL = total.ToString() + " " + "TL";
        }

    }
}
