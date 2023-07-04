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
    public class BookAuthorsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: BookAuthors
        public ActionResult Index()
        {
            var bookAuthors = db.BookAuthors.Include(b => b.Author).Include(b => b.Book);
            return View(bookAuthors.ToList());
        }

        // GET: BookAuthors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookAuthor bookAuthor = db.BookAuthors.Find(id);
            if (bookAuthor == null)
            {
                return HttpNotFound();
            }
            return View(bookAuthor);
        }

        // GET: BookAuthors/Create
        public ActionResult Create()
        {
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name");
            ViewBag.BookId = new SelectList(db.Books, "Id", "Name");
            return View();
        }

        // POST: BookAuthors/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookId,AuthorId")] BookAuthor bookAuthor)
        {
            if (ModelState.IsValid)
            {
                db.BookAuthors.Add(bookAuthor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name", bookAuthor.AuthorId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "Name", bookAuthor.BookId);
            return View(bookAuthor);
        }

        // GET: BookAuthors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookAuthor bookAuthor = db.BookAuthors.Find(id);
            if (bookAuthor == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name", bookAuthor.AuthorId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "Name", bookAuthor.BookId);
            return View(bookAuthor);
        }

        // POST: BookAuthors/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookId,AuthorId")] BookAuthor bookAuthor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookAuthor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Authors, "Id", "Name", bookAuthor.AuthorId);
            ViewBag.BookId = new SelectList(db.Books, "Id", "Name", bookAuthor.BookId);
            return View(bookAuthor);
        }

        // GET: BookAuthors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookAuthor bookAuthor = db.BookAuthors.Find(id);
            if (bookAuthor == null)
            {
                return HttpNotFound();
            }
            return View(bookAuthor);
        }

        // POST: BookAuthors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookAuthor bookAuthor = db.BookAuthors.Find(id);
            db.BookAuthors.Remove(bookAuthor);
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
