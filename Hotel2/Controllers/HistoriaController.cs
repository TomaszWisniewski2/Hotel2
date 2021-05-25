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
    public class HistoriaController : Controller
    {


        private HotelDBEntities objHotelDBEntities; //obiekt typu hoteldb

        public HistoriaController()
        {
            objHotelDBEntities = new HotelDBEntities();//do obiektu przypisanie wartości z model db
        }
        //---------------------------------------------------------------------------------------------------
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetRoomZapłacony()
        {
            List<RoomBookingViewModel> listOfBookingViewModel = new List<RoomBookingViewModel>();
            listOfBookingViewModel = (from objHotelBooking in objHotelDBEntities.RoomBookings
                                      join objRoom in objHotelDBEntities.Rooms on
                                      objHotelBooking.AssignRoomid equals objRoom.Roomid

                                      join objPayment in objHotelDBEntities.Payments on
                                      objHotelBooking.Paymentid equals objPayment.Paymentid
                                      select new RoomBookingViewModel()
                                      {
                                          
                                          BookingFrom = objHotelBooking.BookingFrom,
                                          BookingTo = objHotelBooking.BookingTo,
                                          CustomerName = objHotelBooking.CustomerName,
                                          CustomerAddres = objHotelBooking.CustomerAddres,
                                          Zapłacone = objPayment.Zapłacone,
                                          TotalAmount = objHotelBooking.TotalAmount,
                                          RoomNumber = objRoom.RoomNumber,
                                          Bookingid = objHotelBooking.Bookingid

                                      }).ToList();
            return PartialView("_GuestInfo", listOfBookingViewModel);
        }
        [HttpGet]
        public ActionResult Zapłać(int bookingid)//można zmienić na roomid = 0 null
        {
            RoomBooking objRoomBooking = objHotelDBEntities.RoomBookings.Single(model => model.Bookingid == bookingid);
            objRoomBooking.Paymentid = 2;
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Zapłacone", success = true }, JsonRequestBehavior.AllowGet);

        }
    }
}