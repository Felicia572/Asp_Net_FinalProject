using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Asp_Net_FinalProject.Models;

namespace Asp_Net_FinalProject.Controllers
{
    public class UsersController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: Users
        public ActionResult AdminIndex()
        {
            var user = db.User.Include(u => u.User_Role);
            return View(user.ToList());
        }

        //public ActionResult UserIndex()
        //{
        //    // 一般用户角色的处理逻辑
        //    return View();
        //}

        // GET: Users/Details/5
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
            // 获取除了管理员角色之外的所有角色列表
            var roles = db.User_Role.Where(r => r.Role_Name != "Admin").ToList();
            ViewBag.Role_id = new SelectList(roles, "Id", "Role_Name");
            return View();
        }

        // POST: Users/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,Password,Email,Role_id")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Registration_date = DateTime.Now; // 设置注册日期为当前日期和时间
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }

            return View(user);
        }

        // GET: Users/Edit/5
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,Password,Email,Registration_date,Role_id")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            ViewBag.Role_id = new SelectList(db.User_Role, "Id", "Role_Name", user.Role_id);
            return View(user);
        }

        // GET: Users/Delete/5
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
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("AdminIndex");
        }

        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                // 查询数据库，验证用户凭据
                User user = db.User.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    // 登录成功，设置身份验证 Cookie
                    FormsAuthentication.SetAuthCookie(model.Email, false);

                    // 获取用户的角色信息
                    int userRole = user.Role_id; // 假设角色字段 "Role"

                    if (userRole == 1)
                    {
                        // 管理员角色的处理逻辑
                        return RedirectToAction("AdminIndex");
                    }
                    else
                    {
                        // 一般用户角色的处理逻辑
                        return RedirectToAction("Create","Posts");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            return View(model);
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
