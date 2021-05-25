using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotel2.Models;
using Hotel2.VievModel;
using Newtonsoft.Json;

namespace Hotel2.Controllers
{
    public class BookingController : Controller
    {
        //-------------------------------------------------------------------
        HotelDBEntities objHotelDBEntities;
        public BookingController()//konstruktor
        {
            objHotelDBEntities = new HotelDBEntities();
        }
        //-------------------------------------------------------------------
        public ActionResult Index()
        {
            BookingViewModel objBookingViewModel = new BookingViewModel();
            objBookingViewModel.ListOfRooms = (from objRoom in objHotelDBEntities.Rooms
                                               where objRoom.BookingStatusid == 1
                                               where objRoom.RoomNumber != "0"
                                               select new SelectListItem()
                                               {
                                                   Text = objRoom.RoomNumber,
                                                   Value= objRoom.Roomid.ToString()
                                               }).ToList();

            objBookingViewModel.BookingFrom = DateTime.Now;
            objBookingViewModel.BookingTo = DateTime.Now.AddDays(1);
            return View(objBookingViewModel);
        }
        //-------------------------------------------------------------------
        [HttpPost]
        public ActionResult Index(BookingViewModel objBookingViewModel)
        {
            // string dt = objBookingViewModel.BookingTo.ToString("MM/dd/yyyy");
            int numberOfDays =Convert.ToInt32( (objBookingViewModel.BookingTo - objBookingViewModel.BookingFrom).TotalDays);
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == objBookingViewModel.AssignRoomid);
            decimal RoomPrice = objRoom.RoomPrice;
            decimal TotalPrice = RoomPrice * numberOfDays;

            if ((objRoom.RoomCapacity >= objBookingViewModel.NoOfMembers) && (objBookingViewModel.NoOfMembers > 0))
            {
                RoomBooking roomBooking = new RoomBooking()
                {
                    BookingFrom = objBookingViewModel.BookingFrom,
                    BookingTo = objBookingViewModel.BookingTo,
                    AssignRoomid = objBookingViewModel.AssignRoomid,
                    CustomerAddres = objBookingViewModel.CustomerAddres,
                    CustomerName = objBookingViewModel.CustomerName,
                    NoOfMembers = objBookingViewModel.NoOfMembers,
                    TotalAmount = TotalPrice,
                    Paymentid = 1


                };
                objHotelDBEntities.RoomBookings.Add(roomBooking);
                objHotelDBEntities.SaveChanges();

                objRoom.BookingStatusid = 2;

                objHotelDBEntities.SaveChanges();

                return Json(new { message = "Booking Added", success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "No zgaduj co zrobiłeś nie tak", success = true });
            }

        }
        
        
        
        public PartialViewResult GetAllRooms2()//widok zarezerwowanych pokoi
        {
            IEnumerable<RoomRezerwacjeViewModel> ListOfRoomDetailsViewModels =
                (from objHotelBooking in objHotelDBEntities.RoomBookings
                 join objRoom in objHotelDBEntities.Rooms on
                 objHotelBooking.AssignRoomid equals objRoom.Roomid

                 join objBookingStatus in objHotelDBEntities.BookingStatus on
                 objRoom.BookingStatusid equals objBookingStatus.BookingStatusid


                 select new RoomRezerwacjeViewModel()
                 {
                     RoomNumber = objRoom.RoomNumber,
                     CustomerName = objHotelBooking.CustomerName,
                     CustomerAddres = objHotelBooking.CustomerAddres,
                     NoOfMembers = objHotelBooking.NoOfMembers,
                     BookingFrom = objHotelBooking.BookingFrom,
                     BookingTo = objHotelBooking.BookingTo,
                     RoomCapacity = objRoom.RoomCapacity,

                     BookingStatus = objBookingStatus.BookingStatus,

                     Bookingid = objHotelBooking.Bookingid,
                     Roomid = objRoom.Roomid

                 }).ToList();

            return PartialView("_RoomZarezerwowane", ListOfRoomDetailsViewModels);
        }
        public PartialViewResult GetAllRoomsWolny()
        {
            IEnumerable<RoomDetailsViewModel> ListOfRoomDetailsViewModels =
                (from objRoom in objHotelDBEntities.Rooms
                 join objBooking in objHotelDBEntities.BookingStatus on
                 objRoom.BookingStatusid equals objBooking.BookingStatusid

                 join objRoomType in objHotelDBEntities.RoomTypes on
                 objRoom.RoomTypeid equals objRoomType.RoomTypeid

                 where objRoom.IsActive == true
                 select new RoomDetailsViewModel()
                 {
                     RoomNumber = objRoom.RoomNumber,
                     RoomDescription = objRoom.RoomDescription,
                     RoomCapacity = objRoom.RoomCapacity,
                     RoomPrice = objRoom.RoomPrice,
                     BookingStatus = objBooking.BookingStatus,
                     RoomType = objRoomType.RoomTypeName,
                     RoomImage = objRoom.RoomImage,
                     Roomid = objRoom.Roomid

                 }).ToList();

            return PartialView("_RoomWolny", ListOfRoomDetailsViewModels);
        }
        public PartialViewResult GetRoomZajęty()//widok zarezerwowanych pokoi
        {
            IEnumerable<RoomRezerwacjeViewModel> ListOfRoomDetailsViewModels =
                (from objHotelBooking in objHotelDBEntities.RoomBookings
                 join objRoom in objHotelDBEntities.Rooms on
                 objHotelBooking.AssignRoomid equals objRoom.Roomid

                 join objBookingStatus in objHotelDBEntities.BookingStatus on
                 objRoom.BookingStatusid equals objBookingStatus.BookingStatusid

                 join objPayment in objHotelDBEntities.Payments on
                 objHotelBooking.Paymentid equals objPayment.Paymentid
                 select new RoomRezerwacjeViewModel()
                 {
                     RoomNumber = objRoom.RoomNumber,
                     CustomerName = objHotelBooking.CustomerName,
                     CustomerAddres = objHotelBooking.CustomerAddres,

                     Zapłacone = objPayment.Zapłacone,

                     BookingFrom = objHotelBooking.BookingFrom,
                     BookingTo = objHotelBooking.BookingTo,
                     RoomCapacity = objRoom.RoomCapacity,
                     TotalAmount = objHotelBooking.TotalAmount,
                     BookingStatus = objBookingStatus.BookingStatus,
                     Paymentid = objPayment.Paymentid,
                     Bookingid = objHotelBooking.Bookingid,
                     Roomid = objRoom.Roomid

                 }).ToList();

            return PartialView("_RoomZajęty", ListOfRoomDetailsViewModels);
        }

        [HttpGet]
        public JsonResult ZajmijPokój(int roomid)
        {
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == roomid);
            objRoom.BookingStatusid = 3;
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Pokój nr"+ objRoom.RoomNumber+" zajęty", success = true }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult AnulujRez(int roomid)
        {
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == roomid);

            objRoom.BookingStatusid = 1;

            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Pokój nr" + objRoom.RoomNumber + " rezerwacja anulowana", success = true }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult ZwróćPokój(int roomid)
        {
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == roomid);
            objRoom.BookingStatusid = 4;
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Pokój nr" + objRoom.RoomNumber + " zwrócony", success = true }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public  ActionResult Delete(int bookingid)//można zmienić na roomid = 0 null
        {
            RoomBooking objRoomBooking = objHotelDBEntities.RoomBookings.Single(model => model.Bookingid == bookingid);
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.RoomNumber == "0");//ustawienie pokoju zerowego
            objRoomBooking.AssignRoomid = objRoom.Roomid;
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Informacje o gościu w Histori", success = true }, JsonRequestBehavior.AllowGet);

        }
        

        [HttpGet]
        public ActionResult Zapłać(int bookingid)//można zmienić na roomid = 0 null
        {
            RoomBooking objRoomBooking = objHotelDBEntities.RoomBookings.Single(model => model.Bookingid == bookingid);
            objRoomBooking.Paymentid = 2;
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Zapłacone", success = true }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult U(HttpPostedFileBase filejson)
        {
            if (!filejson.FileName.EndsWith(".json"))
            {
                ViewBag.error = "TO ma BYĆ json!";
            }
            else
            {
                filejson.SaveAs(Server.MapPath("~/folderx/" + Path.GetFileName(filejson.FileName)));
                StreamReader reader = new StreamReader(Server.MapPath("~/folderx/" + Path.GetFileName(filejson.FileName)));
                string jsondata = reader.ReadToEnd();
                List < RoomBooking > emptlist = JsonConvert.DeserializeObject<List<RoomBooking>>(jsondata);
                emptlist.ForEach(p =>
                {
                    if (objHotelDBEntities.Rooms.Find(p.AssignRoomid) == null)
                    {
                        ViewBag.error = "TO ma BYĆ istniejący ID!";
                    }
                    else
                    {
                        if (objHotelDBEntities.Rooms.Find(p.AssignRoomid).BookingStatusid != 1)
                        {
                            ViewBag.error = "TO ma BYĆ istniejący ID!";
                        }
                        else
                        {
                            RoomBooking booking = new RoomBooking()
                            {
                                CustomerName = p.CustomerName,
                                CustomerAddres = p.CustomerAddres,
                                BookingFrom = p.BookingFrom,
                                BookingTo = p.BookingTo,
                                AssignRoomid = p.AssignRoomid,
                                NoOfMembers = p.NoOfMembers,
                                TotalAmount = p.TotalAmount,
                                Paymentid = p.Paymentid,

                            };

                            Room objRoom = objHotelDBEntities.Rooms.SingleOrDefault(model => model.Roomid == p.AssignRoomid);
                            objRoom.BookingStatusid = 2;

                            objHotelDBEntities.RoomBookings.Add(booking);
                            objHotelDBEntities.SaveChanges();
                        }
                    }
                });
            }
            return RedirectToAction("Index");
        }
    }
}