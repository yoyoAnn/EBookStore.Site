using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EBookStore.Site.Models.DTOs;
using EBookStore.Site.Models.EFModels;
<<<<<<<< HEAD:Controllers/CategoriesController.cs
using EBookStore.Site.Models.Servives;
using EBookStore.Site.Models.ViewsModel;

namespace EBookStore.Site.Controllers
{
    public class CategoriesController : Controller
========
using EBookStore.Site.Models.ViewModels;

namespace EBookStore.Site.Controllers
{
    public class UsersController : Controller
>>>>>>>> develop:Controllers/UsersController.cs
    {
        private readonly CategoriesServer _server;

        private AppDbContext db = new AppDbContext();

<<<<<<<< HEAD:Controllers/CategoriesController.cs

        public CategoriesController()
        {
            _server = new CategoriesServer(db);
        }

        // GET: Categories
        public ActionResult Index()
        {
            if (TempData.ContainsKey("SuccessMessage"))
            {
                ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            }
            var categories = _server.GetCategories();
            return View(categories);
        }

        // GET: Categories/Details/5
========
        // GET: Users
        public ActionResult Index(UserCriteria criteria)
        {
            ViewBag.Criteria = criteria;

            var query = db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                query = query.Where(u => u.Name.Contains(criteria.Name));
            }
            if (!string.IsNullOrEmpty(criteria.Address))
            {
                query = query.Where(u => u.Address.Contains(criteria.Address));
            }

            var users = query.ToList().Select(u => u.ToIndexVM());
            return View(users);
        }


        //public ActionResult Index(EmployeeCriteria criteria)
        //{
        //    ViewBag.Criteria = criteria;

        //    return View(db.Users.ToList());
        //    var users = db.Users.ToList().Select(u => u.ToIndexVM());
        //    return View(users);
        //}


        // GET: Users/Details/5
>>>>>>>> develop:Controllers/UsersController.cs
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
<<<<<<<< HEAD:Controllers/CategoriesController.cs
            Category category = db.Categories.Find(id);
            
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
========
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
>>>>>>>> develop:Controllers/UsersController.cs
        public ActionResult Create()
        {
            return View();
        }

<<<<<<<< HEAD:Controllers/CategoriesController.cs
        // POST: Categories/Create
========
        // POST: Users/Create
>>>>>>>> develop:Controllers/UsersController.cs
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<<< HEAD:Controllers/CategoriesController.cs
        public ActionResult Create([Bind(Include = "Id,Name,DisplayOrder")] CategoriesVM vm)
========
        public ActionResult Create([Bind(Include = "Id,Account,Password,Email,Name,Phone,Address,Gender,Photo,CreatedTime,IsConfirmed,ConfirmCode")] User user)
>>>>>>>> develop:Controllers/UsersController.cs
        {

            var dto = vm.ToDto();
 
            try
            {
<<<<<<<< HEAD:Controllers/CategoriesController.cs
                _server.CreateCategory(dto);
                TempData["SuccessMessage"] = "創建成功";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        // GET: Categories/Edit/5
========
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
>>>>>>>> develop:Controllers/UsersController.cs
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
<<<<<<<< HEAD:Controllers/CategoriesController.cs
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
========
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
>>>>>>>> develop:Controllers/UsersController.cs
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
<<<<<<<< HEAD:Controllers/CategoriesController.cs
        public ActionResult Edit([Bind(Include = "Id,Name,DisplayOrder")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
========
        public ActionResult Edit([Bind(Include = "Id,Account,Password,Email,Name,Phone,Address,Gender,Photo,CreatedTime,IsConfirmed,ConfirmCode")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }
        
        // GET: Users/Delete/5
>>>>>>>> develop:Controllers/UsersController.cs
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
<<<<<<<< HEAD:Controllers/CategoriesController.cs
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
========
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
>>>>>>>> develop:Controllers/UsersController.cs
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
<<<<<<<< HEAD:Controllers/CategoriesController.cs
            _server.DeleteCategory(id);
========
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
>>>>>>>> develop:Controllers/UsersController.cs
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
