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
        
        public ActionResult Index(NewsCriteria criteria)
        {
		

			var selectListItems = new List<SelectListItem>
			{
				new SelectListItem { Text = string.Empty, Value =string.Empty},
				new SelectListItem { Text = "已發佈", Value ="true"},
				new SelectListItem { Text = "未發佈", Value ="false"}
			};

			ViewBag.statusList = new SelectList(selectListItems, "Value", "Text", criteria.Status.ToString());
			ViewBag.Criteria = criteria;



			//第一次進到這個網頁時，criteria是沒有值的，所以不會去執行下面會有這些if有沒有值的判斷。

			AppDbContext db = new AppDbContext();

			IQueryable<News> query =db.News;

			if (string.IsNullOrEmpty(criteria.Title) == false)
			{
				//如果Name有值
				query = query.Where(p => p.Title.Contains(criteria.Title));
			}
			if(criteria.Status!=null)
			{
				query = query.Where(p => p.Status == criteria.Status.Value);
			}

			if (criteria.StartDateTime.HasValue)
			{
				query = query.Where(p => p.CreatedTime >= criteria.StartDateTime);

			}

			if (criteria.EndDateTime.HasValue)
			{
				query = query.Where(p => p.CreatedTime <= criteria.EndDateTime);

			}

			var news = query.ToList().Select(x => new NewsIndexVm
			{
				Id = x.Id,
				Title = x.Title,
				Content = x.Content,
				PageViews = x.PageViews,
				Status = x.Status,
				Image = x.Image,
				CreatedTime = x.CreatedTime,

			});
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