using myMusic.Helper;
using myMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using static myMusic.Helper.HashHelper;
namespace myMusic.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private Music_DB db = new Music_DB();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tài khoản và mật khẩu.";
                return View();
            }

            string hashedPassword = HashHelper.ToSHA256(password);

            var account = db.Accounts.FirstOrDefault(a =>
                a.Username == username &&
                a.PasswordHash == hashedPassword
            );

            if (account != null)
            {
                Session["UserID"] = account.AccountID;
                Session["username"] = account.Username;
                Session["accountId"] = account.AccountID;
                Session["role"] = account.Role;

                if (account.Role == true)
                {
                    return RedirectToAction("Dashboard", "Dashboard", new { area = "Admin" });
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng.";
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu không khớp.";
                return View();
            }

            var existingUser = db.Accounts.FirstOrDefault(a => a.Username == username);
            if (existingUser != null)
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại.";
                return View();
            }

            var hashedPassword = HashHelper.ToSHA256(password); // đảm bảo bạn có class HashHelper
            var newAccount = new Accounts
            {
                Username = username,
                PasswordHash = hashedPassword,
                Role = false // hoặc true nếu mặc định admin
            };

            db.Accounts.Add(newAccount);
            db.SaveChanges();

            ViewBag.Success = "Đăng ký thành công. Bạn có thể đăng nhập ngay.";
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


    }
}