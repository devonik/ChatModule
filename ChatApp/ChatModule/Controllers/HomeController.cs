// ***********************************************************************
// Assembly         : ChatModule
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-06-2017
// ***********************************************************************
// <copyright file="HomeController.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChatModule.Controllers
{
    /// <summary>
    /// Class HomeController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Abouts this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// Contacts this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        /// <summary>
        /// Gets the chat as a View.
        /// </summary>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        /// <param name="currentUserId">The current user identifier.</param>
        /// <returns>ActionResult...if Partial param is true: an full loaded View include the Layout, if Partial param is false: an Partial View without the Layout </returns>
        [AllowCrossSiteJson]
        public ActionResult GetChat(bool partial, int currentUserId)
        {
            var request = Request;
            ViewBag.Partial = partial;
            ViewBag.CurrentUserId = currentUserId;
            if (partial == true)
            {
                return PartialView("~/Views/Chatlogs/Index.cshtml");
            }
            else
            {
                return View("~/Views/Chatlogs/Index.cshtml");
            }
        }
    }
}