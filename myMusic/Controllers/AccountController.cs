using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
    using myMusic.Models;
using myMusic.ViewModel;
namespace myMusic.Controllers
{
    public class AccountController : Controller
    {
        private Music_DB db = new Music_DB();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Profile(int? id)
        {
            var user = db.Accounts.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var totalViews = db.Songs
                .Where(s => s.AccountID == id)
                .Sum(s => (int?)s.Views) ?? 0;

            var viewModel = new UserProfileViewModel
            {
                UserID = user.AccountID,
                Username = user.Username,
                TotalViews = totalViews
            };

            return View(viewModel);
        }
        public ActionResult Library()
        {
            if (Session["UserID"] == null)
                return RedirectToAction("Login", "Login");

            int userId = (int)Session["UserID"];

            var userCollections = db.Collections
                .Where(c => c.AccountID == userId)
                .Include(c => c.Accounts)
                .Include(c => c.CollectionTypes)
                .Include(c => c.CollectionSongs)
                .ToList()
                .Select(c => new CollectionViewModel
                {
                    CollectionID = c.CollectionID,
                    Name = c.Name,
                    Username = c.Accounts.Username,
                    CoverImage = c.CoverImage,
                    TypeName = c.CollectionTypes.TypeName,
                    CreatedAt = c.CreatedAt,
                    SongCount = c.CollectionSongs.Count()
                }).ToList();


            return View(userCollections);
        }

    }
}