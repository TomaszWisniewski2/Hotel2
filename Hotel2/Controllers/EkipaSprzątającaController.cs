using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel2.Models;
using Hotel2.VievModel;
using System.IO;

namespace Hotel2.Controllers
{
    public class EkipaSprzątającaController : Controller
    {
       
        private HotelDBEntities objHotelDBEntities; //obiekt typu hoteldb
        public EkipaSprzątającaController()
        {
            objHotelDBEntities = new HotelDBEntities();//do obiektu przypisanie wartości z model db
        }
        public ActionResult Index()
        {
            
            
            return View();
        }
        
        
        public PartialViewResult GetRoomBrudny()
        {
            IEnumerable<RoomDetailsViewModel> ListOfRoomDetailsViewModels =
                (from objRoom in objHotelDBEntities.Rooms
                 join objBooking in objHotelDBEntities.BookingStatus on
                 objRoom.BookingStatusid equals objBooking.BookingStatusid

                 join objRoomType in objHotelDBEntities.RoomTypes on
                 objRoom.RoomTypeid equals objRoomType.RoomTypeid

                 select new RoomDetailsViewModel()
                 {
                     RoomNumber = objRoom.RoomNumber,


                     BookingStatus = objBooking.BookingStatus,
                     RoomType = objRoomType.RoomTypeName,
                     RoomImage = objRoom.RoomImage,
                     Roomid = objRoom.Roomid

                 }).ToList();

            return PartialView("_RoomBrudny", ListOfRoomDetailsViewModels);
        }
        [HttpGet]
        public JsonResult SprzątajPokój(int roomid)
        {
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == roomid);
            objRoom.BookingStatusid = 1;
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Pokój nr" + objRoom.RoomNumber + " posprzątany", success = true }, JsonRequestBehavior.AllowGet);

        }
    }
}