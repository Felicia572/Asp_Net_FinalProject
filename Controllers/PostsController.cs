using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Asp_Net_FinalProject.Models;

namespace Asp_Net_FinalProject.Controllers
{
    public class PostsController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: Posts
        [Authorize]
        public ActionResult Index()
        {
            var post = db.Post.Include(p => p.User);
            return View(post.ToList());
        }

        // GET: Posts/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [Authorize] 
        public ActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] 
        public ActionResult Create([Bind(Include = "Id,Title,Content,User_id")] Post post)
        {
            if (ModelState.IsValid)
            {
                // 當前登入用戶的用戶 ID
                string userEmail = User.Identity.Name;
                User user = db.User.FirstOrDefault(u => u.Email == userEmail);
                if (user != null)
                {
                    post.User_id = user.Id; // 正確的用戶 ID
                    post.Post_date = DateTime.Now;
                    db.Post.Add(post);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(post);
        }


        // GET: Posts/Edit/5
        [Authorize] 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_id = new SelectList(db.User, "Id", "UserName", post.User_id);
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] 
        public ActionResult Edit([Bind(Include = "Id,Title,Content,User_id,Post_date")] Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_id = new SelectList(db.User, "Id", "UserName", post.User_id);
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize] 
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Post.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize] 
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Post.Find(id);
            db.Post.Remove(post);
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
