
using Hotel20.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Hotel11.Controllers
{
    public class DiscountCodesController : Controller
    {
        private HotelDB10Entities1 db = new HotelDB10Entities1();

        // GET: Discount1
        public ActionResult Index()
        {
            var discountCodes = db.Discounts.ToList();
            return View(discountCodes);
        }

        // GET: Discount1/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount1 = db.Discounts.Find(id);
            if (discount1 == null)
            {
                return HttpNotFound();
            }
            return View(discount1);
        }

        // GET: Discount1/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Amount,DisCountID")] Discount discount1)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem mã giảm giá đã tồn tại chưa
                if (db.Discounts.Any(d => d.Code == discount1.Code))
                {
                    ModelState.AddModelError("Code", "Mã giảm giá đã tồn tại.");
                    return View(discount1);
                }

                db.Discounts.Add(discount1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discount1);
        }


        // GET: Discount1/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount1 = db.Discounts.Find(id);
            if (discount1 == null)
            {
                return HttpNotFound();
            }
            return View(discount1);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Amount,DisCountID")] Discount discount1)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem mã giảm giá đã tồn tại chưa
                var existingDiscount = db.Discounts.FirstOrDefault(d => d.Code == discount1.Code && d.DisCountID != discount1.DisCountID);
                if (existingDiscount != null)
                {
                    ModelState.AddModelError("Code", "Mã giảm giá đã tồn tại.");
                    return View(discount1);
                }

                db.Entry(discount1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discount1);
        }
        // GET: Discount1/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount1 discount1 = db.Discounts.Find(id);
            if (discount1 == null)
            {
                return HttpNotFound();
            }
            return View(discount1);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            // Hiển thị xác nhận xóa từ người dùng
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Discount    discount1 = db.Discounts .Find(id);
            if (discount1 == null)
            {
                return HttpNotFound();
            }
            return View(discount1);
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