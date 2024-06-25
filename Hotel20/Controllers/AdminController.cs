using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel20.Models;

namespace Hotel20.Controllers
{
    public class AdminController : Controller
    {
        private HotelDB10Entities1 database = new HotelDB10Entities1();

        [HttpGet]
        public ActionResult Registeradmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registeradmin(AdminUser admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.NameUser))
                    ModelState.AddModelError(string.Empty, "Tên không được để trống");
                if (string.IsNullOrEmpty(admin.RoleUser))
                    ModelState.AddModelError(string.Empty, "Role không được để trống");
                if (string.IsNullOrEmpty(admin.PasswordUser))
                    ModelState.AddModelError(string.Empty, "Password không được để trống");

                // Kiểm tra xem có người nào đã đăng ký với tên đăng nhập này hay chưa
                var existingAdmin = database.AdminUsers.FirstOrDefault(k => k.NameUser == admin.NameUser);
                if (existingAdmin != null)
                {
                    ModelState.AddModelError(string.Empty, "Tên này đã được admin tạo");
                }

                if (ModelState.IsValid)
                {
                    // Không đặt giá trị cho trường khóa chính nếu nó là auto-increment
                    admin.ID = 0; // Hoặc bỏ dòng này nếu cột Id là auto-increment
                    database.AdminUsers.Add(admin);
                    database.SaveChanges();
                    return RedirectToAction("Loginadmin");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Loginadmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Loginadmin(AdminUser admin)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(admin.NameUser))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(admin.RoleUser))
                    ModelState.AddModelError(string.Empty, "Role không được để trống");
                if (string.IsNullOrEmpty(admin.PasswordUser))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");

                if (ModelState.IsValid)
                {
                    // Tìm admin với tên đăng nhập và password hợp lệ trong CSDL
                    var validAdmin = database.AdminUsers.FirstOrDefault(k => k.NameUser == admin.NameUser && k.RoleUser == admin.RoleUser && k.PasswordUser == admin.PasswordUser);
                    if (validAdmin != null)
                    {
                        // Lưu vào session
                        Session["TaiKhoan"] = validAdmin;
                        return RedirectToAction("Index", "Categories");
                    }
                    else
                    {
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                    }
                }
            }

            return View("Loginadmin");
        }
    }
}
