using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using myMusic.ViewModel;
using System.Data.Entity;
using myMusic.Models;
namespace myMusic.Controllers

{
    public class CollectionsController : Controller
    {
        // GET: Collections
        private Music_DB db = new Music_DB();

        // GET: Collections
        public ActionResult Index(int page = 1)
        {
            int pageSize = 6;
            int totalSongs = db.Songs.Count();
            int totalPages = (int)Math.Ceiling((double)totalSongs / pageSize);

            var collections = db.Collections
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
    })
    .ToList();



            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(collections);
        }
        public ActionResult Details(int id)
        {
            var collection = db.Collections
                .Include(c => c.CollectionSongs.Select(cs => cs.Songs))
                .Include(c => c.Accounts)
                .Include(c => c.CollectionTypes)
                .FirstOrDefault(c => c.CollectionID == id);

            if (collection == null)
            {
                return HttpNotFound();
            }

            var viewModel = new CollectionDetailsViewModel
            {
                CollectionID = collection.CollectionID,
                Name = collection.Name,
                Description = collection.Description,
                CoverImage = collection.CoverImage,
                Username = collection.Accounts.Username,
                TypeName = collection.CollectionTypes.TypeName,
                CreatedAt = collection.CreatedAt,
                Songs = collection.CollectionSongs
                    .OrderBy(cs => cs.SongOrder)
                    .Select(cs => cs.Songs)
                    .ToList()
            };

            return View(viewModel);
        }
        public ActionResult UserCollections(int userId)
        {
            var collections = db.Collections
                .Include(c => c.Accounts).Include(c => c.CollectionTypes) // giả sử bạn có quan hệ CollectionType
                .Where(c => c.AccountID == userId)
                .Select(c => new CollectionViewModel
                {
                    CollectionID = c.CollectionID,
                    Name = c.Name,
                    Username = c.Accounts.Username, // giả sử bạn có quan hệ Account
                    CoverImage = c.CoverImage,
                    TypeName = c.CollectionTypes.TypeName, // giả sử bạn có quan hệ CollectionType
                    CreatedAt = c.CreatedAt,
                    SongCount = c.CollectionSongs.Count() // hoặc Songs.Count nếu liên kết theo Songs
                })
                .ToList();

            return View(collections);
        }
        [HttpPost]
        public ActionResult AddToPlaylist(int songId, int? collectionId, string newCollectionName)
        {
            if (Session["accountId"] == null)
            {
                // Chưa đăng nhập
                return RedirectToAction("Login", "Account");
            }

            int userId = (int)Session["accountId"];

            if (!string.IsNullOrEmpty(newCollectionName))
            {
                var newCol = new Collections
                {
                    Name = newCollectionName,
                    AccountID = userId,
                    CreatedAt = DateTime.Now,
                    TypeID = 1 // giả định mặc định
                };
                db.Collections.Add(newCol);
                db.SaveChanges();

                collectionId = newCol.CollectionID;
            }

            if (collectionId.HasValue)
            {
                bool exists = db.CollectionSongs.Any(cs => cs.SongID == songId && cs.CollectionID == collectionId.Value);
                if (!exists)
                {
                    var newEntry = new CollectionSongs
                    {
                        SongID = songId,
                        CollectionID = collectionId.Value
                    };
                    db.CollectionSongs.Add(newEntry);
                    db.SaveChanges();

                    TempData["Message"] = "Đã thêm bài hát vào danh sách phát.";
                }
                else
                {
                    TempData["Message"] = "Bài hát đã tồn tại trong danh sách phát này.";
                }
            }
            else
            {
                TempData["Message"] = "Bạn chưa chọn hoặc tạo danh sách phát.";
            }

            return RedirectToAction("Index", "Songs");
        }









        // Chi tiết một danh sách phát


    }
}