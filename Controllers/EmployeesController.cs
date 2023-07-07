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
using EBookStore.Site.Models.Infra;
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

        [HttpGet]//返回一个页面：这个页面是用来展示登录的页面
        public ActionResult Logintest()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM vm)
        {
            if (ModelState.IsValid == false) return View(vm);

           
            //驗證帳密的正確性
            Result result = ValidLogin(vm);

            if (result.IsSuccess != true) // 若驗證失敗...
            {
                ModelState.AddModelError("", result.ErrorMessage);
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
            var db = new AppDbContext();
            var employee = db.Employees.FirstOrDefault(e => e.Account == vm.Account);

            if (employee == null) return Result.Fail("帳密有誤");

            //if (employee.IsConfirmed.HasValue == false || employee.IsConfirmed.Value == false) return Result.Fail("會員資格尚未確認");

            var salt = HashUtility.GetSalt();
            var hashPassword = HashUtility.ToSHA256(vm.Password, salt);

            return string.Compare(employee.Password, hashPassword) == 0
                ? Result.Success()
                : Result.Fail("帳密有誤");
        }
        private (string returnUrl, HttpCookie cookie) ProcessLogin(string account, bool rememberMe)
        {
            var roles = string.Empty; // 在本範例, 沒有用到角色權限,所以存入空白

            // 建立一張認證票
            var ticket =
                new FormsAuthenticationTicket(
                    1,          // 版本別, 沒特別用處
                    account,
                    DateTime.Now,   // 發行日
                    DateTime.Now.AddDays(2), // 到期日
                    rememberMe,     // 是否續存
                    roles,          // userdata
                    "/" // cookie位置
                );

            // 將它加密
            var value = FormsAuthentication.Encrypt(ticket);

            // 存入cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);

            // 取得return url
            var url = FormsAuthentication.GetRedirectUrl(account, true); //第二個引數沒有用處

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
