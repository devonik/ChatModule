﻿using System;
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
    public class ChatlogsController : Controller
    {
        private ChatContext db = new ChatContext();

        // GET: Chatlogs
        public ActionResult Index()
        {
            var request = Request;
            return View(db.Chatlog.ToList());
        }

        // GET: Chatlogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chatlog chatlog = db.Chatlog.Find(id);
            if (chatlog == null)
            {
                return HttpNotFound();
            }
            return View(chatlog);
        }

        // GET: Chatlogs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chatlogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "chatlog_id,sender_name,category_id,message,timestamp")] Chatlog chatlog)
        {
            if (ModelState.IsValid)
            {
                db.Chatlog.Add(chatlog);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chatlog);
        }

        // GET: Chatlogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chatlog chatlog = db.Chatlog.Find(id);
            if (chatlog == null)
            {
                return HttpNotFound();
            }
            return View(chatlog);
        }

        // POST: Chatlogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "chatlog_id,sender_name,category_id,message,timestamp")] Chatlog chatlog)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chatlog).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chatlog);
        }

        // GET: Chatlogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chatlog chatlog = db.Chatlog.Find(id);
            if (chatlog == null)
            {
                return HttpNotFound();
            }
            return View(chatlog);
        }

        // POST: Chatlogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chatlog chatlog = db.Chatlog.Find(id);
            db.Chatlog.Remove(chatlog);
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
        public void SendMessage(int sender_id, int empfaenger_id, string message)
        {
            var chatlog = new Chatlog
            {
                sender_id = sender_id,
                empfaenger_id = empfaenger_id,
                message = message,
                timestamp = DateTime.Now
            };
            db.Chatlog.Add(chatlog);
            db.SaveChanges();
        }
        //Gibt die Chat Messages in der Datenbank als JsonObject zurück
        public JsonResult GetChatUser2User(int sender_id, int empfaenger_id)
        {
            //var rows = db.Database.SqlQuery<Chatlog>("SELECT * FROM Chatlog a where (a.sender_id = " + sender_id + " AND a.empfaenger_id = " + empfaenger_id + ") OR (a.sender_id = " + empfaenger_id + " AND a.empfaenger_id = " + sender_id + ")").ToList();
            //rows.ForEach(a =>
            //                {
            //                    a.new_message = "n";
            //                });
            return Json((from a in db.Chatlog
                         where (a.sender_id == sender_id && a.empfaenger_id == empfaenger_id) || (a.sender_id == empfaenger_id && a.empfaenger_id == sender_id)

                         select new
                         {
                             sender_id = a.sender_id,
                             empfaenger_id = a.empfaenger_id,
                             message = a.message,
                             timestamp = a.timestamp
                         }), JsonRequestBehavior.AllowGet
            );
        }
        public JsonResult GetAllSubjects()
        {
            return Json((from a in db.SupportGroup
                         select new
                         {
                             supportgroup_id = a.supportgroup_id,
                             subject = a.subject
                         }), JsonRequestBehavior.AllowGet
            );
        }
        public void SetUserStatus(string status, int currentUserId)
        {
            using (var db = new ChatContext())
            {
                var result = db.User.SingleOrDefault(a => a.user_id == currentUserId);
                if (result != null)
                {
                    if (status == "inactive")
                    {
                        result.last_logout = DateTime.Now;

                    }
                    result.status = status;
                    db.SaveChanges();
                }
            }
        }
        public JsonResult GetMessagesSinceLastLogin(int currentUserId)
        {
            var result = from a in db.Chatlog
                         join b in db.User on a.empfaenger_id equals b.user_id
                         where a.empfaenger_id == currentUserId
                         where a.timestamp > b.last_logout
                         select new
                         {
                             sender_name = (db.User.Where(u => u.user_id == a.sender_id).Select(u => u.first_name + " " + u.last_name)),
                             sender_id = a.sender_id,
                             supportgroup_id = (db.User2SupportGroup.Where(u => u.user_id == a.sender_id).Select(u => u.supportgroup_id)),
                             empfaenger_id = a.empfaenger_id,
                             message = a.message,
                             timestamp = a.timestamp,
                             last_logout_empfaenger = b.last_logout
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void ClearChat()
        {
            var rows = from o in db.Chatlog
                       select o;
            foreach (var row in rows)
            {
                db.Chatlog.Remove(row);
            }
            db.SaveChanges();
        }
        public JsonResult GetUserInfoById(int currentUserId)
        {
            //var result = db.User.Where(a => a.user_id == currentUserId).FirstOrDefault();
            var result = from a in db.User
                         where a.user_id == currentUserId
                         select new
                         {
                             user_id = a.user_id,
                             full_name = a.first_name + " " + a.last_name,
                             avatarlink = a.avatarlink,
                             last_logout = a.last_logout
                         };

            return Json(result.FirstOrDefault(), JsonRequestBehavior.AllowGet);
        }
    }
}
