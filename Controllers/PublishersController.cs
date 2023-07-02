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



        public ActionResult CreatePublishersFromExcel()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePublishersFromExcel(IEnumerable<HttpPostedFileBase> excelFiles)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _services.CreatePublishersFromExcel(excelFiles);
                    TempData["SuccessMessage"] = "從 Excel 創建出版商成功";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(excelFiles);
                }
            }
            else
            {
                return View(excelFiles);
            }
        }






        public ActionResult CreateFromExcel()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFromExcel(HttpPostedFileBase excelFile)
        {
            if (excelFile != null && excelFile.ContentLength > 0)
            {
                try
                {
                    _services.CreatePublishersFromExcel(new[] { excelFile });

                    TempData["SuccessMessage"] = "從 Excel 創建出版商成功";
                    return RedirectToAction("Index", "Publisher");
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
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Phone,Email")] Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publisher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(publisher);
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