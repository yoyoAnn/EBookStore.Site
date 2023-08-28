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

namespace EBookStore.Site.Controllers
{
	public class UsersController : Controller
	{
		private AppDbContext db = new AppDbContext();

		// GET: Users
		[Authorize(Roles = "執行長,專欄作家")]
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
        public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = db.Users.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// GET: Users/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Users/Create
		// 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
		// 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Account,Password,Email,Name,Phone,Address,Gender,Photo,CreatedTime,IsConfirmed,ConfirmCode")] User user)
		{
			if (ModelState.IsValid)
			{
				db.Users.Add(user);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(user);
		}



		// GET: Users/Edit/5
		public ActionResult Edit(int id)
		{
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserEditVM user = GetUserInfo(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            //PrepareEmployeeDataSource(employee.Id);

            return View(user);
        }


        private UserEditVM GetUserInfo(int id)
        {
            var db = new AppDbContext();
            var userInDb = db.Users.Find(id);
            if (userInDb == null) return null;

            return new UserEditVM
            {
                Id = userInDb.Id,
                Email = userInDb.Email,
                Name = userInDb.Name,
                Phone = userInDb.Phone,
                Account = userInDb.Account,
				Address = userInDb.Address,
                //Password = userInDb.Password,
                Gender = (bool)userInDb.Gender
                //CreatedTime = userInDb.CreatedTime
            };
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditVM vm)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(employee).State = EntityState.Modified;
                var db = new AppDbContext();
                var en = new User
                {
                    Id = vm.Id,
                    Account = vm.Account,
                    Email = vm.Email,
                    Name = vm.Name,
                    Phone = vm.Phone,
                    Password = "123",
                    Gender = vm.Gender,
					Address = vm.Address,
					Photo = "defaultUser.jpg",
                    CreatedTime = DateTime.Now,
                };
                db.Entry(en).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", employee.RoleId);
            return View();
        }


        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			User user = db.Users.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		// POST: Users/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			User user = db.Users.Find(id);
			db.Users.Remove(user);
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