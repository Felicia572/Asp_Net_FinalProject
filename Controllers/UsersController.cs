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
        public ActionResult Login()
        {
            return View();
        }

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
                    ViewBag.Error="帳號(Email)或密碼錯誤！";
                }
            }

            return View(model);
        }

        // GET: Users/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,Email,Role_id")] User user)
        {
            if (user.UserName.ToLower() == "admin")
            {
                ModelState.AddModelError("UserName", "请勿使用 'admin' 作為名字！");
            }

            if (db.User.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "此 Email 已被使用！");
            }

            if (ModelState.IsValid)
            {
                user.Registration_date = DateTime.Now;
                user.Role_id = 2;
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Create", "Posts");
            }

            return View(user);
        }


        // GET: Users
        [AdminAuthorize]
        public ActionResult Index()
        {
            var user = db.User.Include(u => u.User_Role);
            return View(user.ToList());
        }

        // GET: Users/Details/5
        [AdminAuthorize]
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

        // GET: Users/Edit/5
        [AdminAuthorize]
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
        [AdminAuthorize]
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
        [AdminAuthorize]
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
        [AdminAuthorize]
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
