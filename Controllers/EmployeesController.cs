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

        public ActionResult Edit(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //Employee employee = db.Employees.Find(id);
            //if (employee == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", employee.RoleId);
            //return View(employee);

            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EmployeeEditVM employee = GetEmployeeInfo(id);

            if (employee == null)
            {
                return HttpNotFound();
            }

            PrepareEmployeeDataSource(employee.RoleId);
   
            return View(employee);
        }

        private void PrepareEmployeeDataSource(int? id)
        {
            var roles = new AppDbContext().Roles.ToList().Prepend(new Role());
            ViewBag.RoleId = new SelectList(roles, "Id", "Name", id);
        }



        private EmployeeEditVM GetEmployeeInfo(int id)
        {
            var db = new AppDbContext();
            var employeeInDb = db.Employees.Find(id);
            if (employeeInDb == null) return null;

            return new EmployeeEditVM
            {
                Id = employeeInDb.Id,
                Email = employeeInDb.Email,
                Name = employeeInDb.Name,
                Phone = employeeInDb.Phone,
                RoleId = employeeInDb.RoleId,
                Account = employeeInDb.Account,
                //Password = employeeInDb.Password,
                Gender = employeeInDb.Gender
                //CreatedTime = employeeInDb.CreatedTime
            };
        }


        // 新增的方法，處理自訂按鈕的觸發事件
        [HttpPost]
        public ActionResult EditRoleId(int Id, int RoleId)
        {
            new EmployeeEditDapper().EmployeeDapper(RoleId, Id);

            return RedirectToAction("Index"); // 重新導向到訂單列表頁面或其他適當的頁面

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeEditVM vm)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(employee).State = EntityState.Modified;
                var db = new AppDbContext();
                var en = new Employee
                {
                    Id = vm.Id,
                    Account = vm.Account,
                    Email = vm.Email,
                    Name = vm.Name,
                    Phone = vm.Phone,
                    RoleId = vm.RoleId,
                    Password = "123",
                    Gender = vm.Gender,
                    CreatedTime= DateTime.Now,
                };
                db.Entry(en).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.RoleId = new SelectList(db.Roles, "Id", "Name", employee.RoleId);
            return View();
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
                : Result.Fail("帳號或密碼有誤");
                //: ViewBag.ErrorMessage = "帳號或密碼有誤";
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


        [Authorize]
        public ActionResult PersonalIndex()
        {
           return View();
        }


        [Authorize]
        public ActionResult EditProfile()
        {

            var currentUserAccount = User.Identity.Name;

            var model = GetEmployeeProfile(currentUserAccount);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(EditProfileVM vm)
        {
            var currentUserAccount = User.Identity.Name;

            if (ModelState.IsValid == false) return View(vm);

            Result updateResult = UpdateProfile(vm);
            if (updateResult.IsSuccess) return RedirectToAction("PersonalIndex");

            ModelState.AddModelError(string.Empty, updateResult.ErrorMessage);
            return View(vm);
        }

        private EditProfileVM GetEmployeeProfile(string account)
        {
            var employeeInDb = new AppDbContext().Employees.FirstOrDefault(e => e.Account == account);
            return employeeInDb == null
                ? null
                : new EditProfileVM
                {
                    Id = employeeInDb.Id,
                    Email = employeeInDb.Email,
                    Name = employeeInDb.Name,
                    Phone = employeeInDb.Phone
                };
        }

        private Result UpdateProfile(EditProfileVM vm)
        {
            // 取得在db裡的原始記錄
            var db = new AppDbContext();

            var currentUserAccount = User.Identity.Name;
            var memberInDb = db.Employees.FirstOrDefault(m => m.Account == currentUserAccount);
            if (memberInDb == null) return Result.Fail("找不到要修改的會員記錄");

            // 更新記錄
            memberInDb.Name = vm.Name;
            memberInDb.Email = vm.Email;
            memberInDb.Phone = vm.Phone;

            db.SaveChanges();

            return Result.Success();
        }


        [Authorize]
        public ActionResult EditPassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditPassword(EditPasswordVM vm)
        {
            if (ModelState.IsValid == false) return View(vm);

            var currentUserAccount = User.Identity.Name;

            Result result = ChangePassword(currentUserAccount, vm);

            if (result.IsSuccess == false)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(vm);
            }

            return RedirectToAction("PersonalIndex");
        }

        private Result ChangePassword(string account, EditPasswordVM vm)
        {
            var salt = HashUtility.GetSalt();
            var hashOrigPassword = HashUtility.ToSHA256(vm.OriginalPassword, salt);

            var db = new AppDbContext();

            var memberInDb = db.Employees.FirstOrDefault(m => m.Account == account && m.Password == hashOrigPassword);
            if (memberInDb == null) return Result.Fail("帳號或密碼錯誤");

            var hashPassword = HashUtility.ToSHA256(vm.Password, salt);

            // 更新密碼
            memberInDb.Password = hashPassword;
            db.SaveChanges();

            return Result.Success();
        }

        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(ForgetPasswordVM vm)
        {
            if (ModelState.IsValid == false) return View(vm);

            // 生成email裡的連結
            var urlTemplate = Request.Url.Scheme + "://" +  // 生成 http:.// 或 https://
                             Request.Url.Authority + "/" + // 生成網域名稱或 ip
                             "Members/ResetPassword?memberid={0}&confirmCode={1}"; // 生成網頁 url

            Result result = ProcessResetPassword(vm.Account, vm.Email, urlTemplate);

            if (result.IsFail)
            {
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
                return View(vm);
            }

            return View("ConfirmForgetPassword");
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        //public ActionResult ResetPassword(ResetPasswordVM vm, int memberId, string confirmCode)
        //{
        //    if (ModelState.IsValid == false) return View(vm);
        //    Result result = ProcessChangePassword(memberId, confirmCode, vm.Password);

        //    //if (result.IsSuccess == false) { }
        //    //if (!result.IsSuccess) { }
        //    if (result.IsFail)
        //    {
        //        ModelState.AddModelError(string.Empty, result.ErrorMessage);
        //        return View(vm);
        //    }

        //    return View("ConfirmResetPassword");
        //}

        //private Result ProcessChangePassword(int memberId, string confirmCode, string newPassword)
        //{
        //    var db = new AppDbContext();

        //    // 驗證 memberId, confirmCode是否正確
        //    var memberInDb = db.Employees.FirstOrDefault(m => m.Id == memberId && m.ConfirmCode == confirmCode);
        //    if (memberInDb == null) return Result.Fail("找不到對應的會員記錄");

        //    // 更新密碼,並將 confirmCode清空
        //    var salt = HashUtility.GetSalt();
        //    var encryptedPassword = HashUtility.ToSHA256(newPassword, salt);

        //    memberInDb.EncryptedPassword = encryptedPassword;
        //    memberInDb.ConfirmCode = null;

        //    db.SaveChanges();

        //    return Result.Success();
        //}

        private Result ProcessResetPassword(string account, string email, string urlTemplate)
        {
            var db = new AppDbContext();
            // 檢查account,email正確性
            var memberInDb = db.Employees.FirstOrDefault(m => m.Account == account);

            if (memberInDb == null) return Result.Fail("帳號或 Email 錯誤"); // 故意不告知確切錯誤原因

            if (string.Compare(email, memberInDb.Email, StringComparison.CurrentCultureIgnoreCase) != 0) return Result.Fail("帳號或 Email 錯誤");

            //// 檢查 IsConfirmed必需是true, 因為只有已啟用的帳號才能重設密碼
            //if (memberInDb.IsConfirmed == false) return Result.Fail("您還沒有啟用本帳號, 請先完成才能重設密碼");

            //// 更新記錄, 填入 confirmCode
            //var confirmCode = Guid.NewGuid().ToString("N");
            //memberInDb.ConfirmCode = confirmCode;
            //db.SaveChanges();

            // 發email
            //var url = string.Format(urlTemplate, memberInDb.Id, confirmCode);
            //new EmailHelper().SendForgetPasswordEmail(url, memberInDb.Name, email);

            return Result.Success();
        }

    }
}
