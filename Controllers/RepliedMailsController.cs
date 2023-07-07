﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EBookStore.Site.Models.EFModels;

namespace EBookStore.Site.Controllers
{
    public class RepliedMailsController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: RepliedMails
        public ActionResult Index()
        {
            var repliedMails = db.RepliedMails.Include(r => r.CustomerServiceMail);
            return View(repliedMails.ToList());
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
            RepliedMail repliedMail = db.RepliedMails.Find(id);
            if (repliedMail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CSId = new SelectList(db.CustomerServiceMails, "Id", "UserAccount", repliedMail.CSId);
            return View(repliedMail);
        }

        // POST: RepliedMails/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CSId,Email,Title,Content,CreatedTime")] RepliedMail repliedMail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repliedMail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CSId = new SelectList(db.CustomerServiceMails, "Id", "UserAccount", repliedMail.CSId);
            return View(repliedMail);
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
        [ValidateAntiForgeryToken]
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
