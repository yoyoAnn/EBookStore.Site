using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EBookStore.Site.Models;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra.DapperRepository;
using EBookStore.Site.Models.Servives;
using EBookStore.Site.Models.ViewsModel;
using Microsoft.Ajax.Utilities;

namespace EBookStore.Site.Controllers
{
    public class BooksController : Controller
    {

        private readonly BooksService _service;
        private AppDbContext db = new AppDbContext();

   
        public BooksController()
        {
            _service = new BooksService(db);
        }
        // GET: Books
        public ActionResult Index()
        {
            //var books = db.Books.Include(b => b.Category).Include(b => b.Publisher);

            var books = _service.GetBooks();

            return View(books);
        }


        public ActionResult IndexDapper()
        {
            var booksItems = new BookDapperRepository().GetBookItems();

            return View(booksItems);
        }



        public ActionResult GetBooksByPriceAscending()
        {
            var books = _service.GetBooksByPriceAscending();
            return View(books);
        }

        public ActionResult GetBooksByPriceDescending()
        {
            var books = _service.GetBooksByPriceDescending();
            return View(books);
        }



        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name");
            return View();
        }

        // POST: Books/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,CategoryId,PublisherId,PublishDate,Summary,ISBN,EISBN,Stock,Status,Price,Discount")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
            return View();
        }

        public ActionResult CreateFromVM()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name");
            ViewBag.Author = new SelectList(db.Authors, "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFromVM(BooksVM vm)
        {
            if (ModelState.IsValid)
            {
                var dto = vm.ToDto();
                _service.CreateBook(dto);
                return RedirectToAction("IndexDapper");
            }
            return View(vm);
        }



        // GET: Books/Edit/5
            public ActionResult Edit(int? id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Book book = db.Books.Find(id);
                if (book == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
                ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
                return View(book);
            }

            // POST: Books/Edit/5
            // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
            // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "Id,Name,CategoryId,PublisherId,PublishDate,Summary,ISBN,EISBN,Stock,Status,Price,Discount")] Book book)
            {
                if (ModelState.IsValid)
                {
                    db.Entry(book).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexDapper");
                }
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", book.CategoryId);
                ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name", book.PublisherId);
                return View(book);
            }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Books.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Books.Find(id);
            db.Books.Remove(book);
            db.SaveChanges();
            return RedirectToAction("IndexDapper");
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
