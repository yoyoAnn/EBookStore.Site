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
using EBookStore.Site.Models.ViewModels;
using Microsoft.Ajax.Utilities;

namespace EBookStore.Site.Controllers
{
    public class ArticlesController : Controller
    {
		//private AppDbContext db = new AppDbContext();


		public PartialViewResult BookIndex(string bookName)
		{
			AppDbContext db = new AppDbContext();

			IQueryable<Book> query = db.Books;

			if (string.IsNullOrEmpty(bookName) == false)
			{
				//如果Name有值
				query = query.Where(p => p.Name.Contains(bookName));
			}

			List<BooksIndexForArticleVm> book = query.Select(x => new BooksIndexForArticleVm
			{
				Id = x.Id,
				Name = x.Name,
				PublisherId = x.PublisherId,
				PublisherName = x.Publisher.Name,
				CategoryId = x.CategoryId,
				CategoryName = x.Category.Name
				
			}).ToList();

			return PartialView(book);
		}




		public PartialViewResult WriterIndex(string writerName)
		{
			//IEnumerable<WriterIndexVm> vm = GetWriterList();
			//return PartialView(vm);

			AppDbContext db = new AppDbContext();

			IQueryable<Writer> query = db.Writers;

			if (string.IsNullOrEmpty(writerName) == false)
			{
				//如果Name有值
				query = query.Where(p => p.Name.Contains(writerName));
			}

			List<WriterIndexVm> writer = query.Select(x => new WriterIndexVm
			{
				Id = x.Id,
				Name = x.Name,
				Photo = x.Photo,
				Profile = x.Profile,
				Email = x.Email,
			}).ToList();

			PrepareBookDataSource(null);
			//ViewBag.WriterList = writer;
			return PartialView(writer);
		}


		private IEnumerable<WriterIndexVm> GetWriterList()
		{
			var db = new AppDbContext();
			return db.Writers.ToList().Select(x => new WriterIndexVm
			{
				Id = x.Id,
				Name = x.Name,
				Photo = x.Photo,
				Profile = x.Profile,
				Email = x.Email,

			});
		}


		public ActionResult Index()
        {
			IEnumerable<ArticleIndexVm> articles = GetArticleList();

			return View(articles);
		}

		public ActionResult Create()
		{

            PrepareBookDataSource(null);
            PrepareWriterDataSource(null);



			//ViewBag.WriterList = new List<WriterIndexVm>();

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ArticleCreateVm vm)
		{
			if (ModelState.IsValid==false)
			{
				PrepareBookDataSource(vm.BookId);
				PrepareWriterDataSource(vm.WriterId);
				return View(vm);
			}

            CreateArticle(vm);
			return RedirectToAction("Index");
		}

		
		public ActionResult Edit(int id)
		{
			if (id == 0)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

			ArticleEditVm article = GetArticleInfo(id);		
	
			if (article == null)
			{
				return HttpNotFound();
			}

			PrepareBookDataSource(article.BookId);
			PrepareWriterDataSource(article.WriterId);
			return View(article);
		}


		

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit( ArticleEditVm vm)
		{
			if (ModelState.IsValid == false)
			{
				PrepareBookDataSource(vm.BookId);
				PrepareWriterDataSource(vm.WriterId);
				return View(vm);
			}
			var result=UpdateArticleProfile(vm);
			if (result.IsFail)
			{
				ModelState.AddModelError(string.Empty, result.ErrorMessage);
				
				return View(vm);
			}
			return RedirectToAction("Index");
		}





		private Result UpdateArticleProfile(ArticleEditVm vm)
		{
			//if(vm.WriterId==0||vm.BookId ==0) return Result.Fail("找不到此筆專欄");

			var db = new AppDbContext();
			var articleInDb = db.Articles.FirstOrDefault(x => x.Id == vm.Id);

			if (articleInDb == null) return Result.Fail("找不到此筆專欄");
			//articleInDb.BookId = vm.BookId;
			//articleInDb.WriterId = vm.WriterId;
			articleInDb.Title = vm.Title;
			articleInDb.Content	= vm.Content;
			articleInDb.Status = vm.Status;
			articleInDb.CreatedTime = vm.CreatedTime;

			db.SaveChanges();
			return Result.Success();
		}

		private ArticleEditVm GetArticleInfo(int id)
		{
			var db = new AppDbContext();
			var articleInDb = db.Articles.Find(id);

			if (articleInDb == null) return null;

			return new ArticleEditVm
			{
				Id = articleInDb.Id,
				BookId = articleInDb.BookId,
				BookName = articleInDb.Book.Name,
				WriterId = articleInDb.WriterId,
				WriterName = articleInDb.Writer.Name,
				Title = articleInDb.Title,
				Content = articleInDb.Content,
				PageViews = articleInDb.PageViews,
				Status = articleInDb.Status,
				CreatedTime = articleInDb.CreatedTime
			};


		}


		private void CreateArticle(ArticleCreateVm vm)
		{
			Article entity = new Article()
			{
				BookId = vm.BookId,
				WriterId = vm.WriterId,
				Title = vm.Title,
				Content = vm.Content,
				Status = vm.Status,
				CreatedTime = vm.CreatedTime
			};
            var db = new AppDbContext();
			db.Articles.Add(entity);
			db.SaveChanges();
		
		}

		private void PrepareBookDataSource(int? id)
		{
			var books = new AppDbContext().Books.ToList().Prepend(new Book());
			ViewBag.BookId = new SelectList(books, "Id", "Name", id);
		}

        private void PrepareWriterDataSource(int? id)
        {

			var writers = new AppDbContext().Writers.ToList().Prepend(new Writer());
			ViewBag.WriterId = new SelectList(writers, "Id", "Name", id);
		}
	

		private IEnumerable<ArticleIndexVm> GetArticleList()
		{
			var db = new AppDbContext();
			return db.Articles.Include(x => x.Book).Include(x => x.Writer).OrderBy(x => x.CreatedTime).Select(x => new ArticleIndexVm
			{
				Id = x.Id,
				BookName = x.Book.Name,
				WriterName = x.Writer.Name,
				Title = x.Title,
				Content = x.Content,
				PageViews = x.PageViews,
				Status = x.Status,
				CreatedTime = x.CreatedTime

			});





		}

   
		       

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {

			var db = new AppDbContext();
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Article article = db.Articles.Find(id);
			if (article == null)
			{
				return HttpNotFound();
			}
			return View(article);
		}

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
			var db = new AppDbContext();
			Article article = db.Articles.Find(id);
            db.Articles.Remove(article);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

   //     protected override void Dispose(bool disposing)
   //     {
			//var db = new AppDbContext();
			//if (disposing)
   //         {

   //             db.Dispose();
   //         }
   //         base.Dispose(disposing);
   //     }
    }
}
