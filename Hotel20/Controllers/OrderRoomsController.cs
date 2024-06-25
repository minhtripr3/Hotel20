using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

using Hotel20.Models;

namespace Hotel20.Controllers
{
    public class OrderRoomsController : Controller
    {
        private HotelDB10Entities1 db = new HotelDB10Entities1();

        // GET: OrderRooms
        public ActionResult Index()
        {
            var orderRooms = db.OrderRooms.Include(o => o.Room);


            return View(orderRooms.ToList());
        }

        // GET: OrderRooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderRoom orderRoom = db.OrderRooms.Find(id);
            if (orderRoom == null)
            {
                return HttpNotFound();
            }
            return View(orderRoom);
        }

        // GET: OrderRooms/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Truy vấn phòng từ cơ sở dữ liệu dựa trên id
            Room room = db.Rooms.Find(id);

            if (room == null)
            {
                return HttpNotFound();
            }

            // Tạo một đối tượng OrderRoom và thiết lập các giá trị mặc định hoặc trống tùy theo yêu cầu
            OrderRoom orderRoom = new OrderRoom
            {
                ProductID = id.Value, // Lấy id của phòng từ tham số truyền vào
                DateOrder = DateTime.Now,
                RoomName = room.NamePro,
                Total = room.Price ?? 0,
                StatusOrder = room.BookingStatus
            };

            // Load danh sách thuế từ cơ sở dữ liệu và thêm một mục mặc định vào đầu danh sách
            var thueList = db.Thues.ToList();
            thueList.Insert(0, new Thue { ThueID = 1, TenThue = "-- VAT --", });

            // Đưa danh sách thuế vào ViewBag
            ViewBag.ThueList = new SelectList(thueList, "ThueID", "TenThue");

            // Trả về view Create với đối tượng OrderRoom mới đã được thiết lập
            return View(orderRoom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateOrder,NameCus,PhoneCus,DressCus,Total,StatusOrder,ProductID,CusID,RoomName,CheckInDate,CheckOutDate,DiscountCode,ThueID")] OrderRoom orderRoom)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin phòng từ cơ sở dữ liệu
                var room = db.Rooms.Find(orderRoom.ProductID);
                if (room != null)
                {
                    // Tính toán tổng số tiền dựa trên giá phòng và thuế áp dụng
                    decimal totalWithoutTax = room.Price ?? 0;
                    decimal taxPercent = 0;

                    // Lấy thông tin thuế từ cơ sở dữ liệu
                    var tax = db.Thues.Find(orderRoom.ThueID);
                    if (tax != null)
                    {
                        taxPercent = tax.PhanTramThue ?? 0;
                    }

                    decimal taxAmount = totalWithoutTax * (taxPercent / 100);

                    // Kiểm tra nếu có mã giảm giá được áp dụng
                    if (!string.IsNullOrEmpty(orderRoom.DiscountCode))
                    {
                        // Tìm mã giảm giá từ cơ sở dữ liệu
                        var discount = db.Discounts.SingleOrDefault(d => d.Code == orderRoom.DiscountCode);

                        if (discount != null)
                        {
                            // Tính toán giảm giá dựa trên phần trăm giảm
                            decimal discountAmount = discount.Amount;
                            decimal discountedTotal = totalWithoutTax * (1 - discountAmount / 100);
                            orderRoom.Total = discountedTotal + (discountedTotal * (taxPercent / 100));
                        }
                        else
                        {
                            // Nếu mã giảm giá không hợp lệ, sử dụng giá trị gốc
                            orderRoom.Total = totalWithoutTax + taxAmount;
                        }
                    }
                    else
                    {
                        // Nếu không có mã giảm giá, sử dụng giá trị gốc
                        orderRoom.Total = totalWithoutTax + taxAmount;
                    }

                    // Lưu đơn đặt phòng vào cơ sở dữ liệu
                    db.OrderRooms.Add(orderRoom);
                    db.SaveChanges();

                    // Giảm số lượng phòng khi đặt
                    if (room.Quantity > 0)
                    {
                        room.Quantity -= 1;
                        db.SaveChanges();

                        // Cập nhật trạng thái BookingStatus1 của phòng đã đặt
                        room.BookingStatus1 = true; // Đã đặt
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Không còn phòng trống.";
                        Console.WriteLine("TempData ErrorMessage set.");
                        return RedirectToAction("Create", "OrderRooms");
                    }

                    return RedirectToAction("Index");
                }
            }

            // Load danh sách thuế vào ViewBag để hiển thị trong DropdownList
            var thueList = db.Thues.ToList();
            ViewBag.ThueList = new SelectList(db.Thues, "ThueID", "TenThue");

            ViewBag.ProductID = new SelectList(db.Rooms, "ProductID", "NamePro", orderRoom.ProductID);
            return View(orderRoom);
        }

        // GET: OrderRooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderRoom orderRoom = db.OrderRooms.Find(id);
            if (orderRoom == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Rooms, "ProductID", "NamePro", orderRoom.ProductID);
            return View(orderRoom);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DateOrder,NameCus,PhoneCus,DressCus,Total,StatusOrder,ProductID,CusID,RoomName,CheckInDate,CheckOutDate,DiscountCode")] OrderRoom orderRoom)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu có mã giảm giá được áp dụng
                if (!string.IsNullOrEmpty(orderRoom.DiscountCode))
                {
                    // Tìm mã giảm giá từ cơ sở dữ liệu
                    var discount = db.Discounts.SingleOrDefault(d => d.Code == orderRoom.DiscountCode);

                    if (discount != null)
                    {
                        // Tính toán giá mới dựa trên giảm giá và cập nhật vào đối tượng OrderRoom
                        orderRoom.Total *= (1 - discount.Amount);
                    }
                }

                db.Entry(orderRoom).State = EntityState.Modified;
                db.SaveChanges();

                // Cập nhật trạng thái BookingStatus1 của phòng
                var room = db.Rooms.Find(orderRoom.ProductID);
                if (room != null)
                {
                    // Ví dụ: Cập nhật lại trạng thái thành chưa đặt nếu khách hàng không còn ở phòng
                    if (orderRoom.CheckOutDate.HasValue && orderRoom.CheckOutDate <= DateTime.Now)
                    {
                        room.BookingStatus1 = false;
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Rooms, "ProductID", "NamePro", orderRoom.ProductID);
            return View(orderRoom);
        }

        // GET: OrderRooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderRoom orderRoom = db.OrderRooms.Find(id);
            if (orderRoom == null)
            {
                return HttpNotFound();
            }
            return View(orderRoom);
        }

        // POST: OrderRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderRoom orderRoom = db.OrderRooms.Find(id);
            db.OrderRooms.Remove(orderRoom);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult CheckDiscount(string code)
        {
            // Truy vấn mã giảm giá từ cơ sở dữ liệu
            var discount = db.Discounts.FirstOrDefault(d => d.Code == code);

            if (discount != null)
            {
                // Mã giảm giá hợp lệ, trả về giảm giá
                return Json(new { valid = true, amount = discount.Amount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Mã giảm giá không hợp lệ
                return Json(new { valid = false }, JsonRequestBehavior.AllowGet);
            }
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
