// ***********************************************************************
// Assembly         : ChatModule
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-14-2017
// ***********************************************************************
// <copyright file="ChatlogsController.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
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
using System.Data.Entity.Validation;

namespace ChatModule.Controllers
{
    /// <summary>
    /// Class ChatlogsController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class ChatlogsController : Controller
    {
        /// <summary>
        /// The database
        /// </summary>
        private ChatContext db = new ChatContext();

        // GET: Chatlogs
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            var request = Request;
            return View(db.Chatlog.ToList());
        }

        // GET: Chatlogs/Details/5
        /// <summary>
        /// Detailses the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
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
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chatlogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates the specified chatlog.
        /// </summary>
        /// <param name="chatlog">The chatlog.</param>
        /// <returns>ActionResult.</returns>
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
        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
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
        /// <summary>
        /// Edits the specified chatlog.
        /// </summary>
        /// <param name="chatlog">The chatlog.</param>
        /// <returns>ActionResult.</returns>
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
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
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
        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chatlog chatlog = db.Chatlog.Find(id);
            db.Chatlog.Remove(chatlog);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Releases unmanaged resources and optionally releases managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="sender_id">The sender identifier.</param>
        /// <param name="empfaenger_id">The empfaenger identifier.</param>
        /// <param name="message">The message.</param>
        /// <returns>String...The Message parameter</returns>
        public String SendMessage(int sender_id, int empfaenger_id, string message)
        {
            var decoded = HttpUtility.HtmlDecode(message);
            Console.WriteLine(message);
            var chatlog = new Chatlog
            {
                sender_id = sender_id,
                empfaenger_id = empfaenger_id,
                message = decoded,
                timestamp = DateTime.Now
            };
            try
            {
                db.Chatlog.Add(chatlog);
                db.SaveChanges();
                return "Message send success!";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return "Message send failed!";
                //throw;
            }
        }
        //Gibt die Chat Messages in der Datenbank als JsonObject zurück
        /// <summary>
        /// Gets the chat user2 user.
        /// </summary>
        /// <param name="sender_id">The sender identifier.</param>
        /// <param name="empfaenger_id">The empfaenger identifier.</param>
        /// <returns>JsonResult...The Chat history of the 2 Users</returns>
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
        /// <summary>
        /// Gets all subjects.
        /// </summary>
        /// <returns>JsonResult...Get All Subjects in Database</returns>
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
        /// <summary>
        /// Sets the user status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="currentUserId">The current user identifier.</param>
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
        /// <summary>
        /// Gets the messages since last login.
        /// </summary>
        /// <param name="currentUserId">The current user identifier.</param>
        /// <returns>JsonResult.</returns>
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
        /// <summary>
        /// Clears the chat.
        /// </summary>
        /// <param name="sender_id">The sender identifier.</param>
        /// <param name="empfaenger_id">The empfaenger identifier.</param>
        public void ClearChat(int sender_id, int empfaenger_id)
        {
            var rows = from o in db.Chatlog
                       where (o.sender_id == sender_id && o.empfaenger_id == empfaenger_id) || (o.sender_id == empfaenger_id && o.empfaenger_id == sender_id)
                       select o;
            foreach (var row in rows)
            {
                db.Chatlog.Remove(row);
            }
            db.SaveChanges();
        }
        /// <summary>
        /// Gets the user information by identifier.
        /// </summary>
        /// <param name="currentUserId">The current user identifier.</param>
        /// <returns>JsonResult.</returns>
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
