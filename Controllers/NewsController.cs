using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EBookStore.Site.Controllers
{
    public class NewsController : Controller
    {
        


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

            return View("Index"); 

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