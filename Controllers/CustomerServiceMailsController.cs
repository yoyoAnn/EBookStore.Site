using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EBookStore.Site.Models.EFModels;

namespace EBookStore.Site.Controllers
{
    public class CustomerServiceMailsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: CustomerServiceMails
        public ActionResult Index()
        {
            var customerServiceMails = db.CustomerServiceMails.Include(c => c.Order).Include(c => c.ProblemType);
            return View(customerServiceMails.ToList());
        }

        // GET: CustomerServiceMails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            if (customerServiceMail == null)
            {
                return HttpNotFound();
            }
            return View(customerServiceMail);
        }

        // GET: CustomerServiceMails/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "ReceiverName");
            ViewBag.ProblemTypeId = new SelectList(db.ProblemTypes, "Id", "Name");
            return View();
        }

        // POST: CustomerServiceMails/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserAccount,Email,ProblemTypeId,ProblemStatement,OrderId,IsRead,IsReplied,CreatedTime")] CustomerServiceMail customerServiceMail)
        {
            if (ModelState.IsValid)
            {
                db.CustomerServiceMails.Add(customerServiceMail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "Id", "ReceiverName", customerServiceMail.OrderId);
            ViewBag.ProblemTypeId = new SelectList(db.ProblemTypes, "Id", "Name", customerServiceMail.ProblemTypeId);
            return View(customerServiceMail);
        }

        // GET: CustomerServiceMails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            if (customerServiceMail == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "ReceiverName", customerServiceMail.OrderId);
            ViewBag.ProblemTypeId = new SelectList(db.ProblemTypes, "Id", "Name", customerServiceMail.ProblemTypeId);
            return View(customerServiceMail);
        }

        // POST: CustomerServiceMails/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserAccount,Email,ProblemTypeId,ProblemStatement,OrderId,IsRead,IsReplied,CreatedTime")] CustomerServiceMail customerServiceMail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerServiceMail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "ReceiverName", customerServiceMail.OrderId);
            ViewBag.ProblemTypeId = new SelectList(db.ProblemTypes, "Id", "Name", customerServiceMail.ProblemTypeId);
            return View(customerServiceMail);
        }

        // GET: CustomerServiceMails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            if (customerServiceMail == null)
            {
                return HttpNotFound();
            }
            return View(customerServiceMail);
        }

        // POST: CustomerServiceMails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            db.CustomerServiceMails.Remove(customerServiceMail);
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
    }
}
