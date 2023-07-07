using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using DocumentFormat.OpenXml.Office2010.Excel;
using EBookStore.Site.Models.BooksViewsModel;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.Infra.DapperRepository;
using PagedList.Mvc;
using PagedList;

namespace EBookStore.Site.Controllers
{
    public class BooksDapperVMsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private readonly BookDapperRepository _repository;

        public BooksDapperVMsController()
        {
            _repository = new BookDapperRepository(db);
        }
        // GET: BooksDapperVMs
        public ActionResult Index()
        {

            var books = _repository.GetBookItems();
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            }
            return View(books);
        }

        // GET: BooksDapperVMs/Details/5
        public ActionResult Details(int id)
        {
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


        public ActionResult CreateFromExcel()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFromExcel(HttpPostedFileBase excelFiles)
        {
            var categories = BookHelper.GetCategories();
            ViewBag.CategoryId = new SelectList(categories, "Value", "Text");

            string categoryId = Request.Form["CategoryId"];
            // 從選項列表中查詢分類名稱
            string categoryName = categories.FirstOrDefault(c => c.Value == categoryId)?.Text;


            if (excelFiles != null && excelFiles.ContentLength > 0)
            {
                try
                {
                    int initialCount = _repository.GetBooksCount();

                    _repository.CreateFromExcel(new[] { excelFiles }, categoryName);

                    int newCount = _repository.GetBooksCount();
                    if (newCount > initialCount)
                    {
                        TempData["SuccessMessage"] = "從 Excel 創建書籍成功";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Excel 中的所有書籍都已存在";
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("", "Please select a valid Excel file.");
            }
            return View();

        }


        public ActionResult Edit(int id)
        {
            // 根據 ID 從資料庫中獲取書籍資訊
            BooksDapperVM book = _repository.GetBookById(id);

            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            ViewBag.PublisherId = new SelectList(db.Publishers, "Id", "Name");

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
                    _repository.UpdateBook(vm,vm.CategoryId,vm.PublisherId);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the book: " + ex.Message);
                }
            }

            return View(vm);
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
