using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.Servives;

namespace EBookStore.Site.Controllers
{
    public class AuthorsController : Controller
    {
        private AuthorService _service;
        private readonly AppDbContext db = new AppDbContext();

        public AuthorsController()
        {
            _service = new AuthorService(db);
        }

        // GET: Authors
        public ActionResult Index()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            }
            return View(db.Authors.ToList());
        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Photo,Profile")] AuthorDto dto)
        {
            if (ModelState.IsValid)
            {
                var author = dto.ToEntity();
                db.Authors.Add(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dto);
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
                    int initialCount = _service.GetAuthorsCount(); // 紀錄創建前的作者數量

                    _service.CreateAuthorsFromExcel(new[] { excelFiles }, categoryName);

                    int newCount = _service.GetAuthorsCount(); //取得新增的數量


                    if (newCount > initialCount)
                    {
                        TempData["SuccessMessage"] = "從 Excel 創建作者成功";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Excel 中的所有作者都已存在";
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




        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Photo,Profile")] AuthorDto dto)
        {
            if (ModelState.IsValid)
            {
                var author = dto.ToEntity();

                db.Entry(author).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dto);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.Authors.Find(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Author author = db.Authors.Find(id);
            db.Authors.Remove(author);
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
