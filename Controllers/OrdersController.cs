using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Spreadsheet;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.ViewModels;
using NPOI.SS.Formula.Functions;
using System.Runtime.Serialization;
using DocumentFormat.OpenXml.Drawing;
using System.Net.Http.Headers;
using System.Web.WebPages;

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

        [HttpPost]
        public FileResult DownloadXlsx()
        {
            IEnumerable<DetailDapperVM> OrderItem = (IEnumerable<DetailDapperVM>)TempData["OrderItem"];

            long orderId = (long)TempData["orderId"];


            string sheetName = "訂單明細";

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add(sheetName);

                //# header
                var header = ws.FirstRow();
                header.Cell(1).Value = "品名";
                header.Cell(2).Value = "金額";
                header.Cell(3).Value = "個數";

                //# data
                // copy data row to excel
                int rowIdx = 2;
                ws.Column(1).Style.NumberFormat.Format = "@"; // 設定Column(1~13)為文字型態
                ws.Column(2).Style.NumberFormat.Format = "@";
                ws.Column(3).Style.NumberFormat.Format = "@";

                foreach (var c in OrderItem)
                {
                    var detail = ws.Row(rowIdx);
                    detail.Cell(1).Value = c.Name;
                    detail.Cell(2).Value = c.Price;
                    detail.Cell(3).Value = c.Qty;

                    rowIdx++;
                }

                // 自適應欄寬
                ws.Columns().AdjustToContents();

                // return
                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet; charset=utf-8", "訂單明細.xlsx");
                }

            }

            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add(sheetName);
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                int rowIdx = 1;
                var FirstRow = ws.Row(rowIdx);
                ws.Range(rowIdx, 1, rowIdx, 11).Merge();
                FirstRow.Style.Font.Bold = true;
                FirstRow.Style.Font.FontSize = 12;
                FirstRow.Cell(1).Value = "訂單明細";
                rowIdx++;

                var SecondRow = ws.Row(rowIdx);
                ws.Range(rowIdx, 1, rowIdx, 11).Merge();
                SecondRow.Style.Font.Bold = true;
                SecondRow.Style.Font.FontSize = 10;
                //string Reason_C = "";
                //foreach (var c in Reason_C_List)
                //{
                //    if (string.IsNullOrWhiteSpace(Reason_C))
                //    {
                //        Reason_C = c.Code + "." + c.Name;
                //    }
                //    else
                //    {
                //        Reason_C += "　" + c.Code + "." + c.Name;
                //    }
                //}
                SecondRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                SecondRow.Cell(1).Value = orderId;

                SecondRow.Cell(1).Style.NumberFormat.Format = "@";

                ws.Column(1).Style.NumberFormat.Format = "@";
                ws.Column(2).Style.NumberFormat.Format = "@";
                ws.Column(3).Style.NumberFormat.Format = "@";
                ws.Column(4).Style.NumberFormat.Format = "@";// 設定Column(1~13)為文字型態
                rowIdx++;

                //# header
                var header = ws.Row(rowIdx);
                header.Cell(1).Value = "品名";
                header.Cell(2).Value = "金額";
                header.Cell(3).Value = "個數";

                rowIdx++;

                //# data
                // copy data row to excel
                foreach (var c in OrderItem)
                {
                    var detail = ws.Row(rowIdx);
                    detail.Cell(1).Value = c.Name;
                    detail.Cell(2).Value = c.Price;
                    detail.Cell(3).Value = c.Qty;
                    //detail.Cell(2).SetValue($"{c.IdNo.Trim()}");
                    //detail.Cell(3).SetValue($"{c.Reason_C.Trim()}");
                    //detail.Cell(4).Value = c.Jrnl_Bal;
                    //detail.Cell(4).Style.NumberFormat.Format = "0";
                    //detail.Cell(5).SetValue($"{c.Payment_Rate}");
                    //detail.Cell(6).Value = c.consume.Trim();
                    //detail.Cell(7).Value = c.Note.Trim();
                    //detail.Cell(7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    //detail.Cell(8).Value = c.Empname.Trim();

                    //if (!string.IsNullOrWhiteSpace(c.Do_User))
                    //{
                    //    detail.Cell(9).Value = c.Do_User.Trim();
                    //}
                    //else
                    //{
                    //    detail.Cell(9).Value = "";
                    //}

                    //detail.Cell(10).Value = "";
                    //detail.Cell(11).Value = "";

                    // next row
                    rowIdx++;
                }

                // 自適應欄寬
                ws.Columns().AdjustToContents(1);

                // return
                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);

                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "filename.xlsx");
                }
            }
        }

        public ActionResult OrderItemDetails(long orderId)
        {
            ViewBag.OrderId = orderId;

            var OrderItem = new OrderItemDapperRepository().GetOrdersItemsByOrderId(orderId);

            ViewBag.OrderItem = OrderItem;

            TempData["OrderItem"] = OrderItem;
            TempData["orderId"] = orderId;

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

            if(file1 == null || file1.ContentLength == 0 || !file1.FileName.EndsWith(".xlsx"))
            {
                ModelState.AddModelError("file1", "資料類型錯誤或不能為空!");
                PrepareCategoryDataSource(order.OrderStatusId, order.ShippingStatusId, order.UserId);
                return View(order);
            }

            int seed = Guid.NewGuid().GetHashCode();
            var random = new Random(seed);

            string OrderId = "";

            for (int i = 0; i < 16; i++)
            {
                if (i == 0)
                {
                    // 確保第一碼不為 0
                    OrderId += random.Next(1, 9).ToString();
                }
                else
                {
                    OrderId += random.Next(0, 10).ToString();
                }
            }

            // 儲存檔案
            string path = Server.MapPath("/Uploads/Execel");
            var savedFileName = SaveUploadedFile(path, file1, OrderId);
            //order.file1 = savedFileName;

            //if (savedFileName == null) ModelState.AddModelError("Excel", "請選擇檔案");

            if (ModelState.IsValid)
            {
                // 將 view model轉型為 Product
                Order orders = order.ToEntity();

                orders.Id = (OrderId);
                //orders.ShippingStatusId = 2;
                //orders.OrderStatusId = 7;
                orders.OrderTime = DateTime.Now;
                ItemDetailDapperVM item = new OrderItemAllDapperRepository().GetAllItemByOrderId(Convert.ToInt64(OrderId));
                orders.TotalAmount = item.TotalPrice;
                orders.TotalPayment = item.TotalPrice + 80;
                orders.ShippingFee = 80;

                new OrderInsert().OrdersInsert(orders);

                //db.Orders.Add(orders);
                //db.SaveChanges();

                //new OrderUpdateFee().OrderUpdatePayment(Convert.ToInt64(OrderId), item.TotalPrice);

                return RedirectToAction("Index");
            }

            //ViewBag.OrderStatusId = new SelectList(db.OrderStatuses, "Id", "Name", order.OrderStatusId);
            //ViewBag.ShippingStatusId = new SelectList(db.ShippingStatuses, "Id", "Name", order.ShippingStatusId);
            //ViewBag.UserId = new SelectList(db.Users, "Id", "Account", order.UserId);

            PrepareCategoryDataSource(order.OrderStatusId, order.ShippingStatusId, order.UserId);
            return View(order);
        }

        private string SaveUploadedFile(string path, HttpPostedFileBase file1, string OrderId)
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

            new UploadExecelToDataBase().Upload(fullName, OrderId);



            // 傳回存放的檔名
            return newFileName;
        }



        // GET: Orders/Edit/5
        public ActionResult Edit(string id)
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
        public ActionResult CustomAction(string orderId, int ShippingStatusId, int OrderStatusId)
        {
            new OrderEditDapperRepository().PostOrdersShippingStatusIdByOrderId(orderId, ShippingStatusId, OrderStatusId);

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
        [ValidateAntiForgeryToken]
        public ActionResult Delete2(long id)
        {
            Order order = db.Orders.Find(id);
            long OrderId = id;

            new DeleteOrderItemDapperRepository().DeleteOrderItem(id);

            //OrderItem orderItem=db.OrderItems.Find(OrderId);
            //if(orderItem != null) 
            //{
            //    db.OrderItems.Remove(orderItem);
            //    db.SaveChanges();
            //}
            
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
