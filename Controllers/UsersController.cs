using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asp_Net_FinalProject.Attributes;
using Asp_Net_FinalProject.Models;

namespace Asp_Net_FinalProject.Controllers
{
    public class UsersController : Controller
    {
        private dbEntities db = new dbEntities();

        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // 清除對話紀錄
            return RedirectToAction("Login");
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(Models.User model)
        {
            if (ModelState.IsValid)
            {
                // 查詢資料庫，驗證用戶
                User user = db.User.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    // 登入成功，設置身分驗證 Cookie
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Create", "Posts");
                }
                else
                {
                    ViewBag.Error="Invalid email or password!";
                }
            }

            return View(model);
        }

        // GET: Users
        [CustomAuthorize(Users = "admin@example.com")]
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.User_Role);
            return View(user.ToList());
        }

        // GET: Users/Details/5
        [CustomAuthorize(Users = "admin@example.com")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.Role_id = 2;
            return View();
        }

        // POST: Users/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,Email,Role_id")] User user)
        {
            if (user.UserName.ToLower() == "admin") //不能使用admin命名
            {
                ModelState.AddModelError("UserName", "Don't use admin.");
            }

            if (db.User.Any(u => u.Email == user.Email)) //如果已經有人使用過該Email就不能再使用
            {
                ModelState.AddModelError("Email", "This email address is already taken.");
            }

            if (ModelState.IsValid)
            {
                user.Registration_date = DateTime.Now; // 註冊日期和時間
                user.Role_id = 2; // 固定的角色ID，2表示"User"角色
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }
        
        // GET: Users/Edit/5
        [CustomAuthorize(Users = "admin@example.com")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Role_id = new SelectList(db.User_Role, "Id", "Role_Name", user.Role_id);
            return View(user);
        }

        // POST: Users/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [CustomAuthorize(Users = "admin@example.com")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,Email,Registration_date,Role_id")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Role_id = new SelectList(db.User_Role, "Id", "Role_Name", user.Role_id);
            return View(user);
        }

        // GET: Users/Delete/5
        [CustomAuthorize(Users = "admin@example.com")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [CustomAuthorize(Users = "admin@example.com")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
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
