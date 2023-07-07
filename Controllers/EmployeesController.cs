using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.ViewModels;

namespace EBookStore.Site.Controllers
{
    public class EmployeesController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Employees
        public ActionResult Index(EmployeeCriteria criteria)
        {
            PrepareCategoryDataSource(criteria.RoleId);
            ViewBag.Criteria = criteria;


            // 查詢記錄, 由於第一次進到這網頁時,criteria是沒有值的
            var query = db.Employees.Include(e => e.Role);

           
            if (string.IsNullOrEmpty(criteria.Name) == false)
            {
                query = query.Where(e => e.Name.Contains(criteria.Name));
            }
            if (criteria.RoleId != null && criteria.RoleId.Value > 0)
            {
                query = query.Where(e => e.RoleId == criteria.RoleId.Value);
            }

            var employees = query.ToList()
                .Select(e => e.ToIndexVM());
            return View(employees);

            //var employees = db.Employees.Include(e => e.Role)
            //    .ToList()
            //    .Select(e => e.ToIndexVM());
            //return View(employees);
        }

        private void PrepareCategoryDataSource(int? roleId)
        {
            var roles = db.Roles.ToList().Prepend(new Role());
            ViewBag.RoleId = new SelectList(roles, "Id", "Name", roleId);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }
        
        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RoleId,Account,Password,Email,Name,Gender,Phone")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreatedTime = DateTime.Now;

                db.Employees.Add(employee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", employee.RoleId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", employee.RoleId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RoleId,Account,Password,Email,Name,Gender,Phone,CreatedTime")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                // employee.CreatedTime = DateTime.Now;
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", employee.RoleId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
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
