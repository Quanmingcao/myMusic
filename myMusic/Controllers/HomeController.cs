using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myMusic.Models;
using System.Data.Entity;
using myMusic.ViewModel;

namespace myMusic.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private Music_DB db = new Music_DB();
        public ActionResult Index(int page = 1)
        {
            int pageSize = 6;
            int totalSongs = db.Songs.Count();
            int totalPages = (int)Math.Ceiling((double)totalSongs / pageSize);

            var songs = db.Songs
                .Include(s => s.Accounts)
                .OrderByDescending(s => s.UploadDate)
               
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new SongViewModel
                {
                    SongID = s.SongID,
                    Title = s.Title,
                    UploadDate = s.UploadDate,
                    Views = s.Views,
                    Username = s.Accounts.Username,
                    CoverImage = s.CoverImage,
                    FilePath = s.FilePath
                })
                .ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(songs);
        }
        public PartialViewResult TopSongs()
        {
            var topSongs = db.Songs
                .OrderByDescending(s => s.Views)
                .Take(5)
                .Select(s => new SongViewModel
                {
                    SongID = s.SongID,
                    Title = s.Title,
                    Views = s.Views
                })
                .ToList();

            return PartialView("_TopSongs", topSongs);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        

    }
}