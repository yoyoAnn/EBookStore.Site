using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office2010.Excel;
using EBookStore.Site.Models.BooksViewsModel;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra.DapperRepository;

namespace EBookStore.Site.Controllers
{
    public class BooksDapperVMsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private readonly BookDapperRepository _repository;

        public BooksDapperVMsController()
        {
            _repository = new BookDapperRepository();
        }
        // GET: BooksDapperVMs
        public ActionResult Index()
        {
            var books = _repository.GetBookItems();
            return View(books);
        }

        // GET: BooksDapperVMs/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var books = _repository.GetBookById(id);
            return View(books);
        }



        public ActionResult CreateBook()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBook(BooksDapperVM vm)
        {
            try
            {
                _repository.CreateBookWithAuthor(vm);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                return View("Error", ex.Message);
            }
        }

        public ActionResult Edit(int id)
        {
            // 根據 ID 從資料庫中獲取書籍資訊
            BooksDapperVM book = _repository.GetBookById(id);

            if (book == null)
            {
                return HttpNotFound();
            }

            return View(book);
        }

        // POST: Books/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BooksDapperVM vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 更新書籍資訊
                    _repository.UpdateBook(vm);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the book: " + ex.Message);
                }
            }

            return View(vm);
        }


        //// GET: BooksDapperVMs/Edit/5
        //public ActionResult Edit()
        //{

        //    return View();
        //}

        //// POST: BooksDapperVMs/Edit/5
        //// 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        //// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,Name,PublisherName,Author,CategoryName,PublishDate,Summary,ISBN,EISBN,Stock,Status,Price,Discount")] BooksDapperVM booksDapperVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(booksDapperVM).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(booksDapperVM);
        //}


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
