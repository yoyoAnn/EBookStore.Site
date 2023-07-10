﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.ViewModels;
using NPOI.SS.Formula.Functions;

namespace EBookStore.Site.Controllers
{
    public class OrdersController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Orders
        public ActionResult Index(ProductCriteria criteria)
        {
            ViewBag.Criteria = criteria;

            //ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name");
            //ViewBag.ShippingStatusId = new SelectList(db.ShippingStatuses, "Id", "Name");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Account");

            PrepareCategoryDataSource(criteria.OrderId, null, null);

            //var categories = db.Categories.ToList().Prepend(new Category());
            //ViewBag.Order = new SelectList(categories, "Id", "Name", categoryId);

            //查詢紀錄，由於第一次進到這網頁時，criteria是沒有值的
            //var quary = db.Orders.Include(o => o.OrderStatus).Include(o => o.ShippingStatus).Include(o => o.User);

            var customerAccount = User.Identity.Name;
            var query = new OrderDapperRepository().GetOrdersItemsByAccount(customerAccount);

            #region where
            if (string.IsNullOrEmpty(criteria.Name) == false)
            {
                query = (IEnumerable<OrdersItemDapperVM>)query.Where(p => p.Account.Contains(criteria.Name));
            }
            if (criteria.OrderId != null && criteria.OrderId.Value > 0)
            {
                query = (IEnumerable<OrdersItemDapperVM>)query.Where(p => p.OrderStatusId == criteria.OrderId.Value);
            }

            if ((criteria.Date_Start) != null)
            {
                query = (IEnumerable<OrdersItemDapperVM>)query.Where(p => p.OrderTime >= (criteria.Date_Start));
            }

            if ((criteria.Date_End) != null)
            {
                query = (IEnumerable<OrdersItemDapperVM>)query.Where(p => p.OrderTime <= (criteria.Date_End));
            }

            #endregion

            return View(query.ToList());

        }

        // GET: Orders/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult OrderItemDetails(long orderId)
        {

            var OrderItem = new OrderItemDapperRepository().GetOrdersItemsByOrderId(orderId);

            return View(OrderItem);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            //ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name");
            //ViewBag.ShippingStatusId = new SelectList(db.ShippingStatuses, "Id", "Name");
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Account");

            PrepareCategoryDataSource(null, null, null);
            return View();
        }

        //// POST: Orders/Create
        //// 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        //// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,UserId,ReceiverName,ReceiverAddress,ReceiverPhone,TaxIdNum,VehicleNum,Remark,OrderTime,OrderStatusId,TotalAmount,ShippingNumber,ShippingTime,ShippingFee,ShippingStatusId,TotalPayment")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Orders.Add(order);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    //ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.OrderStatusId);
        //    //ViewBag.ShippingStatusId = new SelectList(db.ShippingStatuses, "Id", "Name", order.ShippingStatusId);
        //    //ViewBag.UserId = new SelectList(db.Users, "Id", "Account", order.UserId);


        //    PrepareCategoryDataSource(order.OrderStatusId, order.ShippingStatusId, order.UserId);
        //    return View(order);
        //}

        // POST: Orders/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,ReceiverName,ReceiverAddress,ReceiverPhone,TaxIdNum,VehicleNum,Remark,OrderTime,OrderStatusId,TotalAmount,ShippingNumber,ShippingTime,ShippingFee,ShippingStatusId,TotalPayment")] OrderVm order, HttpPostedFileBase file1)
        {
            if (order.UserId == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // 儲存檔案
            string path = Server.MapPath("/Uploads/Execel");
            var savedFileName = SaveUploadedFile(path, file1);
            order.XlsFile = savedFileName;

            if (savedFileName == null) ModelState.AddModelError("ProductImage", "請選擇檔案");

            if (ModelState.IsValid)
            {
                // 將 view model轉型為 Product
                Order orders = order.ToEntity();

                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.OrderStatusId);
            //ViewBag.ShippingStatusId = new SelectList(db.ShippingStatuses, "Id", "Name", order.ShippingStatusId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Account", order.UserId);


            PrepareCategoryDataSource(order.OrderStatusId, order.ShippingStatusId, order.UserId);
            return View(order);
        }

        private string SaveUploadedFile(string path, HttpPostedFileBase file1)
        {
            // 如果沒有上傳檔案或檔案是空的,就不處理, 傳回 string.empty
            if (file1 == null || file1.ContentLength == 0) return string.Empty;

            // 取得上傳檔案的副檔名
            string ext = System.IO.Path.GetExtension(file1.FileName); // ".jpg" 而不是 "jpg"

            // 如果副檔名不在允許的範圍裡,表示上傳不合理的檔案類型, 就不處理, 傳回 string.empty
            string[] allowedExts = new string[] { ".xls", ".xlsx" };
            if (allowedExts.Contains(ext.ToLower()) == false) return string.Empty;

            string dateTime = DateTime.Now.ToString("yyyyMMdd");

            // 生成一個不會重複的檔名
            string newFileName = Guid.NewGuid().ToString("N") + dateTime + ext; // 生成 er534263454r45636t34534sfggtwer6563462343.jpg
            string fullName = System.IO.Path.Combine(path, newFileName);

            // 將上傳檔案存放到指定位置
            file1.SaveAs(fullName);

            new UploadExecelToDataBase().Upload(fullName);

            // 傳回存放的檔名
            return newFileName;
        }



        // GET: Orders/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            //ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.OrderStatusId);
            //ViewBag.ShippingStatusId = new SelectList(db.ShippingStatuses, "Id", "Name", order.ShippingStatusId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Account", order.UserId);

            PrepareCategoryDataSource(order.OrderStatusId, order.ShippingStatusId, order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,ReceiverName,ReceiverAddress,ReceiverPhone,TaxIdNum,VehicleNum,Remark,OrderTime,OrderStatusId,TotalAmount,ShippingNumber,ShippingTime,ShippingFee,ShippingStatusId,TotalPayment")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.OrderStatusId);
            //ViewBag.ShippingStatusId = new SelectList(db.ShippingStatuses, "Id", "Name", order.ShippingStatusId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Account", order.UserId);

            PrepareCategoryDataSource(order.OrderStatusId, order.ShippingStatusId, order.UserId);
            return View(order);
        }

        // 新增的方法，處理自訂按鈕的觸發事件
        [HttpPost]
        public ActionResult CustomAction(long orderId, int ShippingStatusId)
        {
            new OrderEditDapperRepository().PostOrdersShippingStatusIdByOrderId(orderId, ShippingStatusId);

            return RedirectToAction("Index"); // 重新導向到訂單列表頁面或其他適當的頁面

        }



        private void PrepareCategoryDataSource(int? OrderStatusId, int? ShippingStatusId, int? UserId)
        {
            ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", OrderStatusId);
            ViewBag.ShippingStatusId = new SelectList(db.ShippingStatuses, "Id", "Name", ShippingStatusId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Account", UserId);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete2(long id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
