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
    public class CommentsController : Controller
    {
        private dbEntities db = new dbEntities();

        // GET: Comments
        public ActionResult Index()
        {
            var comment = db.Comment.Include(c => c.Post).Include(c => c.User);
            return View(comment.ToList());
        }

        // GET: Comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // GET: Comments/Create
        public ActionResult Create()
        {
            ViewBag.Post_id = new SelectList(db.Post, "Id", "Title");
            ViewBag.User_id = new SelectList(db.User, "Id", "UserName");
            return View();
        }

        // POST: Comments/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Content,User_id,Post_id")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Comment_date = DateTime.Now;
                db.Comment.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Post_id = new SelectList(db.Post, "Id", "Title", comment.Post_id);
            ViewBag.User_id = new SelectList(db.User, "Id", "UserName", comment.User_id);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Post_id = new SelectList(db.Post, "Id", "Title", comment.Post_id);
            ViewBag.User_id = new SelectList(db.User, "Id", "UserName", comment.User_id);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Content,User_id,Post_id,Comment_date")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Post_id = new SelectList(db.Post, "Id", "Title", comment.Post_id);
            ViewBag.User_id = new SelectList(db.User, "Id", "UserName", comment.User_id);
            return View(comment);
        }

        // GET: Comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comment.Find(id);
            db.Comment.Remove(comment);
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
