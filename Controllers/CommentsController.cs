using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.ViewModels;

namespace EBookStore.Site.Controllers
{
	[Authorize(Roles = "執行長")]
	public class CommentsController : Controller
    {

        public ActionResult Index()
        {
            IEnumerable<CommentIndexVm> comments = GetComments();


			return View(comments);
		}

		private IEnumerable<CommentIndexVm> GetComments()
		{
			var db = new AppDbContext();


			return db.Comments.Include(c => c.Book)
				.Include(c => c.User)
				.ToList()
				.Select(x=> new CommentIndexVm
				{
					Id = x.Id,
					BookName = x.Book.Name,
					UserAccount = x.User.Account,
					CategoryName = x.Book.Category.Name,
					Scores = x.Scores,
					Content = x.Content
				});
			
		}


		//GET: Comments/Create
		public ActionResult Create()
		{
			var db = new AppDbContext();

			ViewBag.BookId = new SelectList(db.Books, "Id", "Name");
			ViewBag.UserId = new SelectList(db.Users, "Id", "Account");
			return View();
		}

		//POST: Comments/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,BookId,UserId,Scores,Content")] Comment comment)
		{

			var db = new AppDbContext();
			if (ModelState.IsValid)
			{
				db.Comments.Add(comment);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			ViewBag.BookId = new SelectList(db.Books, "Id", "Name", comment.BookId);
			ViewBag.UserId = new SelectList(db.Users, "Id", "Account", comment.UserId);
			return View(comment);
		}

		

		// GET: Comments/Delete/5
		public ActionResult Delete(int? id)
		{
			var db = new AppDbContext();
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Comment comment = db.Comments.Find(id);
			if (comment == null)
			{
				return HttpNotFound();
			}
			return View(comment);
		}

		// POST: Comments/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var db = new AppDbContext();
			Comment comment = db.Comments.Find(id);
			db.Comments.Remove(comment);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			var db = new AppDbContext();

			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}



	}
}
