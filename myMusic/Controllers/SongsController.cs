using myMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace myMusic.Controllers
{
    public class SongsController : Controller
    {
        // GET: Songs
        private Music_DB db = new Music_DB();
        
        public ActionResult Details(int id)
        {
            var song = db.Songs
                .Include(s => s.Accounts)
                .FirstOrDefault(s => s.SongID == id);

            if (song == null)
            {
                return HttpNotFound();
            }

            return View(song);
        }
        public ActionResult Index(int? genreId, string sort = "date", int page = 1)
        {
            int pageSize = 6;

            // Dropdown list of genres
            var genres = db.Genres.ToList();
            ViewBag.Genres = new SelectList(genres, "GenreID", "GenreName", genreId);

            var songs = db.Songs.Include("Genres")
                                .Include(s => s.Accounts)
                                .AsQueryable();

            if (genreId.HasValue)
            {
                songs = songs.Where(s => s.GenreID == genreId.Value);
            }

            // Sorting
            switch (sort)
            {
                case "views":
                    songs = songs.OrderByDescending(s => s.Views);
                    break;
                case "date":
                default:
                    songs = songs.OrderByDescending(s => s.UploadDate);
                    break;
            }

            // Total count after filtering
            int totalSongs = songs.Count();
            int totalPages = (int)Math.Ceiling((double)totalSongs / pageSize);

            // Apply pagination
            var pagedSongs = songs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // ViewBag data for view
            ViewBag.CurrentSort = sort;
            ViewBag.CurrentGenre = genreId;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(pagedSongs);
        }

        public ActionResult UserSongs(int userId)
        {
            var songs = db.Songs
                          .Where(s => s.AccountID == userId)
                          .Include(s => s.Genres)  // nếu cần lấy tên thể loại
                          .ToList();

            return PartialView("_UserSongs", songs); // tên partial view của bạn
        }
        public ActionResult SongsByUser(int userId, string sort = "date")
        {
            var songs = db.Songs.Include(s => s.Genres)
                                .Include(s => s.Accounts)
                                .Where(s => s.AccountID == userId)
                                .AsQueryable();

            switch (sort)
            {
                case "views":
                    songs = songs.OrderByDescending(s => s.Views);
                    break;
                case "date":
                default:
                    songs = songs.OrderByDescending(s => s.UploadDate);
                    break;
            }

            ViewBag.CurrentSort = sort;
            ViewBag.CurrentUserId = userId;

            return PartialView("_SongsListPartial", songs.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToPlaylist(int songId, int? collectionId, string newCollectionName)
        {
            int accountId = 1; // Thay bằng User.Identity.GetUserId() hoặc tương tự

            // Kiểm tra bài hát tồn tại
            var song = db.Songs.FirstOrDefault(s => s.SongID == songId);
            if (song == null)
            {
                TempData["Error"] = "Bài hát không tồn tại.";
                return RedirectToAction("Index");
            }

            // Trường hợp: Tạo playlist mới
            if (!string.IsNullOrEmpty(newCollectionName))
            {
                var playlist = new Collections
                {
                    Name = newCollectionName,
                    AccountID = accountId,
                    TypeID = 1, // Mặc định TypeID, bạn có thể thêm dropdown để chọn
                    CreatedAt = DateTime.Now,
                    IsLocked = false
                };

                db.Collections.Add(playlist);
                db.SaveChanges();

                db.CollectionLibrary.Add(new CollectionLibrary
                {
                    AccountID = accountId,
                    CollectionID = playlist.CollectionID,
                    SavedAt = DateTime.Now
                });

                db.CollectionSongs.Add(new CollectionSongs
                {
                    CollectionID = playlist.CollectionID,
                    SongID = songId,
                    SongOrder = 1
                });

                db.SaveChanges();
                TempData["Success"] = "Đã tạo playlist mới và thêm bài hát!";
                return RedirectToAction("Index");
            }

            // Trường hợp: Thêm vào playlist hiện có
            if (!collectionId.HasValue)
            {
                TempData["Error"] = "Vui lòng chọn playlist hoặc nhập tên playlist mới.";
                return RedirectToAction("Index");
            }

            var playlist = db.Collections
                .FirstOrDefault(c => c.CollectionID == collectionId && c.AccountID == db.CollectionLibrary
                    .Where(cl => cl.CollectionID == collectionId).Select(cl => cl.AccountID).FirstOrDefault());

            if (playlist == null)
            {
                TempData["Error"] = "Playlist không tồn tại hoặc không thuộc tài khoản.";
                return RedirectToAction("Index");
            }

            if (db.CollectionSongs.Any(cs => cs.CollectionID == collectionId && cs.SongID == songId))
            {
                TempData["Error"] = "Bài hát đã có trong playlist.";
                return RedirectToAction("Index");
            }

            db.CollectionSongs.Add(new CollectionSongs
            {
                CollectionID = collectionId.Value,
                SongID = songId,
                SongOrder = db.CollectionSongs.Where(cs => cs.CollectionID == collectionId).Count() + 1
            });

            db.SaveChanges();
            TempData["Success"] = "Đã thêm bài hát vào playlist!";
            return RedirectToAction("Index");
        }



    }
}