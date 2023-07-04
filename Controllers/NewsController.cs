using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EBookStore.Site.Controllers
{
    public class NewsController : Controller
    {
        
        public ActionResult Index()
        {
            IEnumerable<NewsIndexVm> news = GetNews();
            return View(news);
        }

		public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewsCreateVm vm, HttpPostedFileBase file1)
        {
         
            if(ModelState.IsValid==false) return View(vm);

            string path = Server.MapPath("~/Uploads/NewsImage");
			string fileName = SaveUploadedFile(path, file1);

            vm.Image = fileName;

			CreateNews(vm);

            return RedirectToAction("Index"); 

        }


		public ActionResult Edit(int? id)
		{
            NewsEditVm model = GetNewsDetail(id);
            return View(model);
		}

        [HttpPost]
        public ActionResult Edit(NewsEditVm vm, HttpPostedFileBase file1)
        {
			if (ModelState.IsValid == false) return View(vm);

			string path = Server.MapPath("~/Uploads/NewsImage");
			string fileName = SaveUploadedFile(path, file1);

			vm.Image = fileName;

            EditNews(vm);

			return RedirectToAction("Index");

		}

		public ActionResult Delete(int? id)
		{
			var db = new AppDbContext();
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			News news = db.News.Find(id);
			if (news == null)
			{
				return HttpNotFound();
			}
			return View(news);
		}

		// POST: News1/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			var db = new AppDbContext();
			News news = db.News.Find(id);
			db.News.Remove(news);
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


		private void EditNews(NewsEditVm vm)
		{
            var db =  new AppDbContext();
            var newsInDb =db.News.Find(vm.Id);
            
            newsInDb.Title = vm.Title;
            newsInDb.Content = vm.Content;
            newsInDb.Status = vm.Status;    
            newsInDb.Image = vm.Image;
            newsInDb.CreatedTime = vm.CreatedTime;
            db.SaveChanges();
            
		}

		private NewsEditVm GetNewsDetail(int? id)
		{
            News newsInDb = new AppDbContext().News.FirstOrDefault(x => x.Id == id);
            return newsInDb == null ? null : new NewsEditVm()
            {
                Id=newsInDb.Id,
                Title = newsInDb.Title,
                Content = newsInDb.Content,
                Status = newsInDb.Status,
                Image   = newsInDb.Image,
                CreatedTime = newsInDb.CreatedTime

            };
		}


		private IEnumerable<NewsIndexVm> GetNews()
		{
            var db = new AppDbContext();

            return db.News.ToList().Select(x =>new NewsIndexVm
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,    
                PageViews = x.PageViews,
                Status = x.Status,
                Image = x.Image,
                CreatedTime = x.CreatedTime,

            });
		}


		private string SaveUploadedFile(string path, HttpPostedFileBase file1)
		{
			
			if (file1 == null || file1.ContentLength == 0) return string.Empty;

			
			string ext = System.IO.Path.GetExtension(file1.FileName);

			
			string[] allowedExts = new string[] { ".jpg", ".jpeg", ".png", ".tif" };
			if (allowedExts.Contains(ext.ToLower()) == false) return string.Empty;

		
			string newFileName = Guid.NewGuid().ToString("N") + ext;
			
			string fullName = System.IO.Path.Combine(path, newFileName);

		
			file1.SaveAs(fullName);

			return newFileName;
		}

		private void CreateNews(NewsCreateVm vm)
		{
            var db = new AppDbContext();

            var news = new News()
            {
                Title = vm.Title,
                Content = vm.Content,
                Status = vm.Status,
                Image = vm.Image,
                CreatedTime = vm.CreatedTime
            };

            db.News.Add(news);
            db.SaveChanges();
		}
	}
}