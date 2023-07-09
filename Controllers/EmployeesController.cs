using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DocumentFormat.OpenXml.Spreadsheet;
using EBookStore.Site.Models.EFModels;
using EBookStore.Site.Models.Infra;
using EBookStore.Site.Models.ViewModels;
using static System.Web.Razor.Parser.SyntaxConstants;

namespace EBookStore.Site.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
    {
        private AppDbContext db = new AppDbContext();


        // GET: Employees
        [Authorize(Roles = "執行長")]
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

        [AllowAnonymous]
        public ActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginVM vm)
        {
            if (ModelState.IsValid == false)
            {
                return View(vm);
            }


            //驗證帳密的正確性
            Result result = ValidLogin(vm);

            if (result.IsSuccess != true) // 若驗證失敗...
            {
                //ModelState.AddModelError("", result.ErrorMessage);
                ViewBag.ErrorMessage = "帳號或密碼有誤";
                return View(vm);
            }

            const bool rememberMe = false; // 是否記住登入成功的會員

            //若登入帳密正確,就開始處理後續登入作業,將登入帳號編碼之後,加到 cookie裡

            (string returnUrl, HttpCookie cookie) processResult = ProcessLogin(vm.Account, rememberMe);

            Response.Cookies.Add(processResult.cookie);

            return Redirect(processResult.returnUrl);
        }

        private Result ValidLogin(LoginVM vm)
        {
            //var db = new AppDbContext();
            var employee = db.Employees.FirstOrDefault(e => e.Account == vm.Account);

            if (employee == null)
            {
                return Result.Fail("帳號或密碼有誤");
            }

            //if (employee.IsConfirmed.HasValue == false || employee.IsConfirmed.Value == false) return Result.Fail("會員資格尚未確認");

            var salt = HashUtility.GetSalt();
            var hashPassword = HashUtility.ToSHA256(vm.Password, salt);

            return string.Compare(employee.Password, hashPassword) == 0
                ? Result.Success()
                //: Result.Fail("帳號或密碼有誤");
                : ViewBag.ErrorMessage = "帳號或密碼有誤";
        }
        public bool IsValid(string account, string password)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Account == account);

            if (employee == null)
            {
                return false;
            }

            var salt = HashUtility.GetSalt();
            var hashPassword = HashUtility.ToSHA256(password, salt);

            return string.Compare(employee.Password, hashPassword) == 0;
        }

        private (string returnUrl, HttpCookie cookie) ProcessLogin(string account, bool rememberMe)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Account == account);

            if (employee == null)
            {
                throw new Exception("User not found");
            }

            //var roles = string.Empty;
            var roles = db.Roles.FirstOrDefault(r => r.Id == employee.RoleId)?.Name ?? string.Empty;

            var ticket = new FormsAuthenticationTicket(
                1,
                account,
                DateTime.Now,
                DateTime.Now.AddDays(2),
                rememberMe,
                roles,
                "/"
            );

            var value = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

            var url = FormsAuthentication.GetRedirectUrl(account, true);

            return (url, cookie);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return Redirect("/Employees/Login");
        }

    }
}
