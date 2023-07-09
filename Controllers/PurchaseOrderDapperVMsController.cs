﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using EBookStore.Site.Models.BooksViewsModel;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra.DapperRepository;
using PagedList;

namespace EBookStore.Site.Controllers
{
    public class PurchaseOrderDapperVMsController : Controller
    {
        private AppDbContext db = new AppDbContext();
        private readonly PurchaseOrdersDapper _repository;

        public PurchaseOrderDapperVMsController()
        {
            _repository = new PurchaseOrdersDapper(db);
        }

        // GET: PurchaseOrderDapperVMs
        public ActionResult Index()
        {
         
            var purchaseOrders = _repository.GetAll();
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            }
            return View(purchaseOrders);
        }


        public ActionResult HistoryIndex()
        {
          
            var purchaseOrders = _repository.GetHistory();
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            }
            return View(purchaseOrders);
        }

        // GET: PurchaseOrderDapperVMs/Details/5
        public ActionResult Details(int id)
        {       
            PurchaseOrderDapperVM purchaseOrderDapperVM = _repository.GetById(id);
            if (purchaseOrderDapperVM == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrderDapperVM);
        }

        // GET: PurchaseOrderDapperVMs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PurchaseOrderDapperVMs/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BookId,BookName,PublisherId,PublisherName,CreateTime,Qty,Detail,PurchasePrice")] PurchaseOrderDapperVM vm)
        {
            if (ModelState.IsValid)
            {
                vm.BookId = _repository.GetOrCreateBookId(vm.BookName);

                if(vm.BookId == 0 && vm.PublisherId == 0)
                {
                    ModelState.AddModelError("", "找不到書與出版商，請輸入有效的名稱");
                    return View(vm);
                }
                if (vm.BookId == 0)
                {
                    ModelState.AddModelError("", "找不到書籍，請輸入有效的書籍名稱，或加入新的書籍");
                    return View(vm);
                }

                vm.PublisherId = _repository.GetOrCreatePublisherId(vm.PublisherName);
                if (vm.PublisherId == 0)
                {
                    ModelState.AddModelError("", "找不到出版商，請輸入正確的出版商名稱，或加入新的出版商");
                    return View(vm);
                }

                _repository.Create(vm);
                return RedirectToAction("Index");
            }

            return View(vm);
        }


        public ActionResult UpdateBookStock(int id)
        {
            var getdetail = _repository.GetById(id);
            return View(getdetail);
        }


        [HttpPost]
        public ActionResult UpdateBookStock(PurchaseOrderDapperVM vm)
        {
            _repository.ConfirmOrder(vm);
            _repository.CreateInHistory(vm);
            _repository.Delete(vm.Id);
            return RedirectToAction("Index");
        }



        // GET: PurchaseOrderDapperVMs/Edit/5
        public ActionResult Edit(int id)
        {
            var PurchaseOrderDetail = _repository.GetById(id);
            if (PurchaseOrderDetail == null)
            {
                return HttpNotFound();
            }

            return View(PurchaseOrderDetail);

        }

        // POST: PurchaseOrderDapperVMs/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BookId,BookName,PublisherId,PublisherName,CreateTime,Qty,Detail,PurchasePrice")] PurchaseOrderDapperVM purchaseOrderDapperVM)
        {
            if (ModelState.IsValid)
            {               
                db.Entry(purchaseOrderDapperVM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchaseOrderDapperVM);
        }

        // GET: PurchaseOrderDapperVMs/Delete/5
        public ActionResult Delete(int id)
        {
            var purchaseOrder = _repository.GetById(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

    
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _repository.Delete(id);
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
