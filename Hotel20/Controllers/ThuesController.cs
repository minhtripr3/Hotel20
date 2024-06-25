using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Hotel20.Models;

namespace Hotel20.Controllers
{
    public class ThueController : Controller
    {
        private HotelDB10Entities1 db = new HotelDB10Entities1();

        // GET: Thue
        public ActionResult Index()
        {
            var thues = db.Thues.ToList();
            return View(thues);
        }

        // GET: Thue/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thue thue = db.Thues.Find(id);
            if (thue == null)
            {
                return HttpNotFound();
            }
            return View(thue);
        }

        // GET: Thue/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Thue/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ThueID,TenThue,PhanTramThue")] Thue thue)
        {
            if (ModelState.IsValid)
            {
                db.Thues.Add(thue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(thue);
        }

        // GET: Thue/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thue thue = db.Thues.Find(id);
            if (thue == null)
            {
                return HttpNotFound();
            }
            return View(thue);
        }

        // POST: Thue/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ThueID,TenThue,PhanTramThue")] Thue thue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(thue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(thue);
        }

        // GET: Thue/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thue thue = db.Thues.Find(id);
            if (thue == null)
            {
                return HttpNotFound();
            }
            return View(thue);
        }

        // POST: Thue/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Thue thue = db.Thues.Find(id);
            db.Thues.Remove(thue);
            db.SaveChanges();
            return RedirectToAction("Index");
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
