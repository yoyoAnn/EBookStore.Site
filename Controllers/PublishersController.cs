using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Servives;
using EBookStore.Site.Models.ViewsModel;
using System.IO;
using EBookStore.Site.Models.Infra;

namespace EBookStore.Site.Controllers
{
    public class PublishersController : Controller
    {
        private PublishersServices _services;
        private readonly AppDbContext db = new AppDbContext();


        public PublishersController()
        {
            _services = new PublishersServices(db);
        }

        // GET: Publishers
        public ActionResult Index()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            }
            return View(db.Publishers.ToList());
        }


        // GET: Publishers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // GET: Publishers/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Publishers/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Phone,Email")] PublishersVM vm)
        {
            if (ModelState.IsValid)
            {
                var pbServices = new PublishersServices(db);

                if (pbServices.IsPublisherNameExists(vm.Name))
                {
                    ModelState.AddModelError("", "已存在相同名稱的出版商");
                    return View(vm);
                }
                _services.CreatePublisher(vm.ToDto());


                return RedirectToAction("Index");
            }

            return View(vm);
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
                    int initialCount = _services.GetPublishersCount(); // 紀錄創建前的出版商數量

                    _services.CreatePublishersFromExcel(new[] { excelFiles }, categoryName);

                    int newCount = _services.GetPublishersCount(); //取得新增的數量


                    if (newCount > initialCount)
                    {
                        TempData["SuccessMessage"] = "從 Excel 創建出版商成功";
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Excel 中的所有出版商都已存在";
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



        // GET: Publishers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Phone,Email")] PublishersVM vm)
        {
            if (ModelState.IsValid)
            {
                var publisher = vm.ToDto().ToEntity();

                db.Entry(publisher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        // GET: Publishers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Publisher publisher = db.Publishers.Find(id);
            if (publisher == null)
            {
                return HttpNotFound();
            }
            return View(publisher);
        }

        // POST: Publishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Publisher publisher = db.Publishers.Find(id);
            db.Publishers.Remove(publisher);
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