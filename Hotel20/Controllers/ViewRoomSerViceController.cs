
using Hotel20.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hotel11.Controllers
{
    public class ViewRoomSerViceController : Controller
    {
        private HotelDB10Entities1 db = new HotelDB10Entities1();

        public ActionResult Index()
        {
            var rooms = db.Rooms.ToList(); // Lấy danh sách Rooms từ database
            var services = db.SerVices.ToList(); // Lấy danh sách Services từ database
            var customers = db.Customers.ToList();

            var viewModel = new RoomServiceViewModel
            {
                Rooms = rooms,
                Services = services,
                Customers = customers

            };

            return View(viewModel);
        }

        public ActionResult Detail(decimal? minPrice, decimal? maxPrice)
        {
            var rooms = db.Rooms.AsQueryable();

            if (minPrice.HasValue)
            {
                rooms = rooms.Where(x => x.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                rooms = rooms.Where(x => x.Price <= maxPrice.Value);
            }

            var services = db.SerVices.ToList(); // Lấy danh sách Services từ database

            var viewModel = new RoomServiceViewModel
            {
                Rooms = rooms.ToList(),
                Services = services
            };

            return View(viewModel);
        }
        public ActionResult Chitiet()
        {
            var rooms = db.Rooms.ToList(); // Lấy danh sách Rooms từ database
            var services = db.SerVices.ToList(); // Lấy danh sách Services từ database

            var viewModel = new RoomServiceViewModel
            {
                Rooms = rooms,
                Services = services
            };

            return View(viewModel);
        }


    }


}


