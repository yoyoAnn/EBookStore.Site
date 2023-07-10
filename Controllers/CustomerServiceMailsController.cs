using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Wordprocessing;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.ViewModels;

namespace EBookStore.Site.Controllers
{
    public class CustomerServiceMailsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: CustomerServiceMails
        public ActionResult Index(CSMailCriteria criteria)
        {
            ViewBag.Criteria = criteria;

			var problemTypes = db.ProblemTypes.ToList().Prepend(new ProblemType { Name="問題種類:" });
			ViewBag.ProblemTypeId = new SelectList(problemTypes, "Id", "Name", criteria.ProblemTypeId);

            ViewBag.MailStatus = criteria.MailStatus;

			var query = db.CustomerServiceMails.Include(c => c.Order).Include(c => c.ProblemType);

            #region where
            switch (criteria.MailStatus)
            {
                case "All":
                    break;
				case "NotRead":
					query = query.Where(p => p.IsRead == false);
					break;
				case "NotReplied":
					query = query.Where(p => p.IsReplied == false);
					break;
				case "Read":
					query = query.Where(p => p.IsRead == true);
					break;
				case "Replied":
					query = query.Where(p => p.IsReplied == true);
					break;
			}

            if (criteria.ProblemTypeId != null && criteria.ProblemTypeId.Value > 0)
            {
                query = query.Where(p => p.ProblemTypeId == criteria.ProblemTypeId.Value);
            }
            if (string.IsNullOrEmpty(criteria.Account) == false)
            {
                query = query.Where(p => p.UserAccount.Contains(criteria.Account));
            }
            if (string.IsNullOrEmpty(criteria.ProblemStatement) == false)
            {
                query = query.Where(p => p.ProblemStatement.Contains(criteria.ProblemStatement));
            }
            if (criteria.CreatedTime != null)
            {
                var dateStart = criteria.CreatedTime.Value;
                var dateEnd = dateStart.AddDays(1);
                query = query.Where(p => p.CreatedTime >= dateStart && p.CreatedTime <= dateEnd);
            }
            #endregion

            return View(query.OrderByDescending(p => p.CreatedTime).ToList());
        }

        // GET: CustomerServiceMails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            if (customerServiceMail == null)
            {
                return HttpNotFound();
            }

			customerServiceMail.IsRead = true;
			db.SaveChanges();

			return View(customerServiceMail);
		}
		// GET: CustomerServiceMails/Create
		public ActionResult Create(int? id)
        {
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            if (customerServiceMail == null)
			{
				return HttpNotFound();
			}

            ReplyMailCreateVM mail = new ReplyMailCreateVM
			{
                CSId = customerServiceMail.Id,
                Account = customerServiceMail.UserAccount,
                ProblemTypeId = customerServiceMail.ProblemTypeId
		    };
            ViewBag.Email = customerServiceMail.Email;
            ViewBag.MailTitle = $"回覆問題:[{customerServiceMail.ProblemType.Name}]";
            ViewBag.MailContent = $"親愛的{customerServiceMail.UserAccount}用戶您好，針對您提出的提問，客服人員在此回覆您：";

			return View(mail);
		}

        // POST: CustomerServiceMails/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReplyMailCreateVM vm)
        {
			if (ModelState.IsValid == false) return View(vm);

            Result result = SaveRepliedMail(vm);

			if (result.IsSuccess)
			{
				// 若成功，轉到 SuccessReplied 頁
				return View("SuccessReplied");
			}
			else
			{
				ModelState.AddModelError(string.Empty, result.ErrorMessage);
				return View(vm);
			}

			//return RedirectToAction("Index");
        }

		private Result SaveRepliedMail(ReplyMailCreateVM vm)
		{
			var db = new AppDbContext();
			var repliedMail = new RepliedMail
            {
                CSId = vm.CSId,
                Email = vm.Email,
                Title = vm.Title,
                Content = vm.Content,
                CreatedTime = DateTime.Now
            };
            db.RepliedMails.Add(repliedMail);
            db.SaveChanges();

            // to do 寄發 email
			SendRepliedMail(vm);

			// 發完email後更新客服信件之是否回復狀態
			var csInDb = db.CustomerServiceMails.FirstOrDefault(m => m.Id == vm.CSId);
			csInDb.IsReplied = true;
			db.SaveChanges();

			return Result.Success();
        }

		private Result SendRepliedMail(ReplyMailCreateVM vm)
		{
			// 發email
            new EmailHelper().SendFromGmail(null, vm.Email, vm.Title, vm.Content);
            return Result.Success();
		}

		// GET: CustomerServiceMails/Edit/5
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            if (customerServiceMail == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "ReceiverName", customerServiceMail.OrderId);
            ViewBag.ProblemTypeId = new SelectList(db.ProblemTypes, "Id", "Name", customerServiceMail.ProblemTypeId);

            return View(customerServiceMail);
        }

        // POST: CustomerServiceMails/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserAccount,Email,ProblemTypeId,ProblemStatement,OrderId,IsRead,IsReplied,CreatedTime")] CustomerServiceMail customerServiceMail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerServiceMail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "Id", "ReceiverName", customerServiceMail.OrderId);
            ViewBag.ProblemTypeId = new SelectList(db.ProblemTypes, "Id", "Name", customerServiceMail.ProblemTypeId);
            return View(customerServiceMail);
        }

        // GET: CustomerServiceMails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            if (customerServiceMail == null)
            {
                return HttpNotFound();
            }
            return View(customerServiceMail);
        }

        // POST: CustomerServiceMails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerServiceMail customerServiceMail = db.CustomerServiceMails.Find(id);
            db.CustomerServiceMails.Remove(customerServiceMail);
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
