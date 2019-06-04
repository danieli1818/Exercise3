using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// The Ex3.Controllers Namespace Of All The Controllers.
/// </summary>
namespace Ex3.Controllers
{
    /// <summary>
    /// The HomeController Class Which Extends The Controller Class is the controller of the home page.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The Function Index Which Handles The Request For The Home Page. It Returns The View For The Request.
        /// </summary>
        /// <returns>The View Of The Home Page.</returns>
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}