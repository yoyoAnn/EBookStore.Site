using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.Infra.DapperRepository;
using EBookStore.Site.Models.ViewModels;

namespace EBookStore.Site.Controllers
{
    public class RepliedMailsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: RepliedMails
        public ActionResult Index(RepliedMailCriteria criteria)
        {
			ViewBag.Criteria = criteria;

			var problemTypes = db.ProblemTypes.ToList().Prepend(new ProblemType { Name = "問題種類:" });
			ViewBag.ProblemTypeId = new SelectList(problemTypes, "Id", "Name", criteria.ProblemTypeId);


			var query = new RepliedMailDapperRepository().GetRepliedMails();

			#region where

			if (criteria.ProblemTypeId != null && criteria.ProblemTypeId.Value > 0)
			{
                var problemList = db.ProblemTypes.ToList();
                var pId = int.Parse(criteria.ProblemTypeId.ToString());
				criteria.ProblemTypeName = problemList[pId-1].Name;

				query = (IEnumerable<RepliedMailVM>)query.Where(p => p.Title.Contains(criteria.ProblemTypeName));
			}
            if (criteria.CreatedTime != null)
            {
                var dateStart = criteria.CreatedTime.Value;
                var dateEnd = dateStart.AddDays(1);
                query = query.Where(p => p.CreatedTime >= dateStart && p.CreatedTime <= dateEnd);
            }
            #endregion


            return View(query.ToList());
        }

        // GET: RepliedMails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepliedMail repliedMail = db.RepliedMails.Find(id);
            if (repliedMail == null)
            {
                return HttpNotFound();
            }
            return View(repliedMail);
        }

        // GET: RepliedMails/Create
        public ActionResult Create()
        {
            ViewBag.CSId = new SelectList(db.CustomerServiceMails, "Id", "UserAccount");
            return View();
        }

        // POST: RepliedMails/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CSId,Email,Title,Content,CreatedTime")] RepliedMail repliedMail)
        {
            if (ModelState.IsValid)
            {
                db.RepliedMails.Add(repliedMail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CSId = new SelectList(db.CustomerServiceMails, "Id", "UserAccount", repliedMail.CSId);
            return View(repliedMail);
        }

        // GET: RepliedMails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			var query = new RepliedMailDapperRepository().GetRepliedMailById(id);
			if (query == null)
            {
                return HttpNotFound();
            }

            return View(query);
        }

        // POST: RepliedMails/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RepliedMailEditVM vm)
        {
            if (ModelState.IsValid!=true)
            {
				return View();

			}

            Result editResult = new RepliedMailDapperRepository().UpdateRepliedMails(vm);
            if (editResult.IsSuccess)
            {
				//new EmailHelper().SendFromGmail(null, vm.Email, vm.Title, vm.Content);

				return RedirectToAction("Index");
			}
            return View();
        }

        // GET: RepliedMails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RepliedMail repliedMail = db.RepliedMails.Find(id);
            if (repliedMail == null)
            {
                return HttpNotFound();
            }
            return View(repliedMail);
        }

        // POST: RepliedMails/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RepliedMail repliedMail = db.RepliedMails.Find(id);
            db.RepliedMails.Remove(repliedMail);
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
