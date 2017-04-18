// ***********************************************************************
// Assembly         : ChatModule
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 04-03-2017
// ***********************************************************************
// <copyright file="UsersController.cs" company="Berenberg">
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

namespace ChatModule.Controllers
{
    /// <summary>
    /// Class UsersController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class UsersController : Controller
    {
        /// <summary>
        /// The database
        /// </summary>
        private ChatContext db = new ChatContext();

        // GET: Users
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View(db.User.ToList());
        }

        // GET: Users/Details/5
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
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Creates the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>ActionResult.</returns>
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
        /// <summary>
        /// Edits the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>ActionResult.</returns>
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
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        /// <summary>
        /// Deletes the confirmed.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
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
        /// Gets the supports by subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>JSonResult of the Supports assigned to this subject</returns>
        public JsonResult GetSupportsBySubject(string subject)
        {
            return Json((from a in db.SupportGroup
                         join b in db.User2SupportGroup on a.supportgroup_id equals b.supportgroup_id
                         join c in db.User on b.user_id equals c.user_id
                         where a.subject == subject
                         select new
                         {
                             supporter_id = b.user_id,
                             user_name = c.first_name + " " + c.last_name,
                             status = c.status,
                             avatarlink = c.avatarlink
                         }), JsonRequestBehavior.AllowGet
            );
        }
        /// <summary>
        /// Gets the contact list for admin.
        /// </summary>
        /// <param name="currentUserId">The Current User</param>
        /// <returns>JSonResult with the Contact list for this User</returns>
        public JsonResult GetContactListForAdmin(int currentUserId)
        {
            var result = db.Database.SqlQuery<User>("SELECT * FROM Users u LEFT JOIN User2SupportGroup u2u ON u2u.user_id = u.user_id WHERE u2u.user_id IS NULL AND u.user_id in (Select c.sender_id from Chatlogs c where c.empfaenger_id=" + currentUserId + ")").ToList();
            //var result2 = db.Database.SqlQuery<int>(", (SELECT chatlog_id from Chatlog where sender_id = "++" and empfaenger_id = " + empfaenger_id + ") as NewMessages").Count();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Checks the user is admin.
        /// </summary>
        /// <param name="user_id">The user identifier.</param>
        /// <returns><c>true</c> if user is assigned to an subject, <c>false</c> otherwise.</returns>
        public bool CheckUserIsAdmin(int user_id)
        {
            var result = db.Database.SqlQuery<int>("Select user_id from User2SupportGroup where user_id=" + user_id).Count();
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

        public void Method()
        {
            throw new System.NotImplementedException();
        }
    }
}
