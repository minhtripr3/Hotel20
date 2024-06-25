using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using System.IO;
using Hotel20.Models;

namespace Hotel20.Controllers
{
    public class ProductsController : Controller
    {
        private HotelDB10Entities1 db = new HotelDB10Entities1();

        public ActionResult Index(decimal? minPrice, decimal? maxPrice)
        {
            var products = db.Rooms.Include(r => r.Category1).ToList();

            // Load trạng thái BookingStatus1 từ cơ sở dữ liệu
            foreach (var room in products)
            {
                // Kiểm tra xem phòng đã được đặt trong OrderRooms trong ngày hôm nay hay chưa
                var isBooked = db.OrderRooms.Any(or => or.ProductID == room.ProductID && or.DateOrder == DateTime.Today);

                // Kiểm tra xem có chi tiết đơn hàng (Quanly) nào liên kết với phòng này và được đặt trong ngày hôm nay hay không
                var hasQuanlyEntry = db.Quanlies.Any(q => q.IDProduct == room.ProductID && q.OrderRoom.DateOrder == DateTime.Today);

                // Cập nhật trạng thái BookingStatus1 của phòng
                room.BookingStatus1 = isBooked || hasQuanlyEntry;
            }


            // Lọc theo khoảng giá nếu có chỉ định
            if (minPrice.HasValue)
            {
                products = products.Where(x => x.Price >= minPrice.Value).ToList();
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(x => x.Price <= maxPrice.Value).ToList();
            }
            if (TempData["ErrorMessage"] != null)
            {
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            }

            return View(products);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            if (db.Rooms.Count() >= 10)
            {
                TempData["ErrorMessage"] = "Số lượng phòng đã đạt giới hạn tối đa là 10. Không thể tạo thêm phòng mới.";
                return RedirectToAction("Index");
            }

            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,NamePro,DecriptionPro,Category,Price,ImagePro,ImagePro1,ImagePro2,BookingStatus,SucChua,Ngayphongtrong,Giuong,DiaChi,BookingStatus1,Quantity")] Room room,
    HttpPostedFileBase ImagePro, HttpPostedFileBase ImagePro1, HttpPostedFileBase ImagePro2)
        {
            if (db.Rooms.Count() >= 10)
            {
                TempData["ErrorMessage"] = "Số lượng phòng đã đạt giới hạn tối đa là 10. Không thể tạo thêm phòng mới.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                if (ImagePro != null)
                {
                    var fileName = Path.GetFileName(ImagePro.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    room.ImagePro = fileName;
                    ImagePro.SaveAs(path);
                }
                if (ImagePro1 != null)
                {
                    var fileName = Path.GetFileName(ImagePro1.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    room.ImagePro1 = fileName;
                    ImagePro1.SaveAs(path);
                }
                if (ImagePro2 != null)
                {
                    var fileName = Path.GetFileName(ImagePro2.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                    room.ImagePro2 = fileName;
                    ImagePro2.SaveAs(path);
                }
                // Đặt BookingStatus1 mặc định là chưa đặt (false)
                room.BookingStatus1 = false;
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", room.Category);
            return View(room);
        }
        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            // Tạo danh sách các giá trị cho BookingStatus1
            ViewBag.BookingStatusList = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Đã đặt" },
                new SelectListItem { Value = "false", Text = "Chưa đặt" }
            };

            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", room.Category);
            return View(room);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,NamePro,DecriptionPro,Category,Price,ImagePro,ImagePro1,ImagePro2,BookingStatus,SucChua,Ngayphongtrong,Giuong,DiaChi,BookingStatus1,Quantity")] Room room,
     HttpPostedFileBase ImagePro, HttpPostedFileBase ImagePro1, HttpPostedFileBase ImagePro2)
        {
            if (ModelState.IsValid)
            {
                var roomDB = db.Rooms.FirstOrDefault(r => r.ProductID == room.ProductID);
                if (roomDB != null)
                {
                    roomDB.NamePro = room.NamePro;
                    roomDB.DecriptionPro = room.DecriptionPro;
                    roomDB.Price = room.Price;
                    if (ImagePro != null)
                    {
                        var fileName = Path.GetFileName(ImagePro.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        roomDB.ImagePro = fileName;
                        ImagePro.SaveAs(path);
                    }
                    if (ImagePro1 != null)
                    {
                        var fileName = Path.GetFileName(ImagePro1.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        roomDB.ImagePro1 = fileName;
                        ImagePro1.SaveAs(path);
                    }
                    if (ImagePro2 != null)
                    {
                        var fileName = Path.GetFileName(ImagePro2.FileName);
                        var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                        roomDB.ImagePro2 = fileName;
                        ImagePro2.SaveAs(path);
                    }

                    roomDB.BookingStatus = room.BookingStatus;
                    roomDB.SucChua = room.SucChua;
                    roomDB.Ngayphongtrong = room.Ngayphongtrong;
                    roomDB.Giuong = room.Giuong;
                    roomDB.DiaChi = room.DiaChi;
                    roomDB.Category = room.Category;
                    roomDB.BookingStatus1 = room.BookingStatus1;
                    roomDB.Quantity = room.Quantity; // Cập nhật số lượng phòng
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BookingStatusList = new List<SelectListItem>
    {
        new SelectListItem { Value = "true", Text = "Đã đặt" },
        new SelectListItem { Value = "false", Text = "Chưa đặt" }
    };

            ViewBag.Category = new SelectList(db.Categories, "IDCate", "NameCate", room.Category);
            return View(room);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ChiTiet(int id)
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
