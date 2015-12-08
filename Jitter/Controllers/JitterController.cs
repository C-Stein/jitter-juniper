using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jitter.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Jitter.Controllers
{
    public class JitterController : Controller
    {
        public JitterRepository Repo { get; set; }

        public JitterController() : base()
        {
            Repo = new JitterRepository();
        }
        // GET: Jitter
        // Maybe the Public feed here?
        public ActionResult Index()
        {
            List<Jot> my_jots = new List<Jot>();
            my_jots.Add(new Jot { Content = "Yay!", Date = DateTime.Now });
            my_jots.Add(new Jot { Content = "Yay!", Date = DateTime.Now });
            my_jots.Add(new Jot { Content = "Yay!", Date = DateTime.Now });
            my_jots.Add(new Jot { Content = "Yay!", Date = DateTime.Now });
            return View(my_jots);
        }

        [Authorize]
        public ActionResult TopFavs()
        {
            return View();
        }

        [Authorize]
        public ActionResult UserFeed()

        {
            ApplicationUserManager _userManager =  HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            string user_id = User.Identity.GetUserId();
            /*
            string user_id = User.Identity
            
            */
            JitterUser me = Repo.Context.Users.FirstOrDefault(u => u.Id == user_id);

            /*string userId = User.Identity.GetUserId();
            ApplicationUser app_user = _userManager.FindById(userId);
            JitterUser me = Repo.GetAllUsers().Where(u => u.RealUser.Id == userId).Single();
            */
            List<Jot> list_of_jots = Repo.GetUserJots(me);
            return View(list_of_jots);
        }

        // GET: Jitter/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Jitter/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Jitter/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jitter/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Jitter/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Jitter/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Jitter/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
