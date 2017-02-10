using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ChatModule.DBShema;
using ChatModule.DBShema.Models;

namespace ChatModule.Controllers
{
    public class UsersController : Controller
    {
        private ChatContext db = new ChatContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

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
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "USER_ID,FIRST_NAME,LAST_NAME,AVATARLINK")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "USER_ID,FIRST_NAME,LAST_NAME,AVATARLINK")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
        public JsonResult GetCategoryByUser()
        {
            return Json((from a in db.Category2User
                         join b in db.Category on a.category_id equals b.category_id
                         join c in db.User on a.user_id equals c.user_id
                         select new
                         {
                             category_name = b.bezeichnung,
                             user_name = c.first_name + " " + c.last_name
                         }), JsonRequestBehavior.AllowGet
            );
        }
        public JsonResult GetAllUser()
        {
            return Json((from a in db.User
                         select new
                         {
                             user_id = a.user_id,
                             user_name = a.first_name + " " + a.last_name
                         }), JsonRequestBehavior.AllowGet
            );
        }
        //Zeigt die "Chatlist" für den Admin. Nur Chats, wo schon Kommunikation stattfand werden angezeigt
        public JsonResult GetUserWithoutSupport(int currentUserId)
        {
            var result = db.Database.SqlQuery<User>("SELECT * FROM Users u LEFT JOIN Category2User u2u ON u2u.user_id = u.user_id WHERE u2u.user_id IS NULL AND u.user_id in (Select c.sender_id from Chatlogs c where c.empfaenger_id=" + currentUserId + ")").ToList();
            //var result2 = db.Database.SqlQuery<int>(", (SELECT chatlog_id from Chatlog where sender_id = "++" and empfaenger_id = " + empfaenger_id + ") as NewMessages").Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool CheckUserIsAdmin(int user_id)
        {
            var result = db.Database.SqlQuery<int>("Select user_id from Category2User where user_id=" + user_id).Count();
            if (result > 0)
            {
                return true;
            }

            else if (result == 0)
            {
                return false;
            }
            return false;
        }
    }
}
