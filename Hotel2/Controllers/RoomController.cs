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
    public class RoomController : Controller
    {
        
        private HotelDBEntities objHotelDBEntities; //obiekt typu hoteldb
        public RoomController()
        {
            objHotelDBEntities = new HotelDBEntities();//do obiektu przypisanie wartości z model db
        }
        public ActionResult Index()
        {
            RoomVievModel objRoomVievModel = new RoomVievModel();
            objRoomVievModel.ListOfBookingStatus = (from obj in objHotelDBEntities.BookingStatus//łączenie tabel po id
                                                    select new SelectListItem()
                                                    {
                                                        Text = obj.BookingStatus,
                                                        Value = obj.BookingStatusid.ToString()

                                                    }).ToList();

            objRoomVievModel.ListOfRoomType = (from obj in objHotelDBEntities.RoomTypes //łączenie tabel po id
                                               select new SelectListItem()
                                               {
                                                   Text = obj.RoomTypeName,
                                                   Value = obj.RoomTypeid.ToString()

                                               }).ToList();
            return View(objRoomVievModel);
        }
        [HttpPost]
        public ActionResult Index(RoomVievModel objRoomVievModel)
        {
            string message = String.Empty;
            string ImageUniqueName = String.Empty;
            string ActualImageName = String.Empty;

            if (objRoomVievModel.Roomid == 0)
            {
                Room objR = objHotelDBEntities.Rooms.Single(model => model.RoomNumber == objRoomVievModel.RoomNumber);
                if (objR != null)
                {
                    return Json(data: new { message = "Taki nr już istnieje " + message, success = true });
                }
                else
                {
                    ImageUniqueName = Guid.NewGuid().ToString();
                    ActualImageName = ImageUniqueName + Path.GetExtension(objRoomVievModel.Image.FileName);

                    objRoomVievModel.Image.SaveAs(Server.MapPath("~/RoomImages/" + ActualImageName));

                    Room objRoom = new Room()
                    {
                        RoomNumber = objRoomVievModel.RoomNumber,
                        RoomDescription = objRoomVievModel.RoomDescription,
                        RoomPrice = objRoomVievModel.RoomPrice,
                        BookingStatusid = 1,
                        IsActive = true,
                        RoomImage = ActualImageName,
                        RoomCapacity = objRoomVievModel.RoomCapacity,
                        RoomTypeid = objRoomVievModel.RoomTypeid

                    };

                    objHotelDBEntities.Rooms.Add(objRoom);
                    message = "Added.";
                }
            }
            else
            {
                Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == objRoomVievModel.Roomid);
                if (objRoomVievModel.Image != null)
                {
                    ImageUniqueName = Guid.NewGuid().ToString();
                    ActualImageName = ImageUniqueName + Path.GetExtension(objRoomVievModel.Image.FileName);
                    objRoomVievModel.Image.SaveAs(Server.MapPath("~/RoomImages/" + ActualImageName));
                   objRoom.RoomImage = ActualImageName;
                }
                objRoom.RoomNumber = objRoomVievModel.RoomNumber;
                objRoom.RoomDescription = objRoomVievModel.RoomDescription;
                objRoom.RoomPrice = objRoomVievModel.RoomPrice;
                //objRoom.BookingStatusid = objRoomVievModel.BookingStatusid;
                objRoom.IsActive = true; 
                objRoom.RoomCapacity = objRoomVievModel.RoomCapacity;
                objRoom.RoomTypeid = objRoomVievModel.RoomTypeid;
                message = "Updated.";

            }

            
            objHotelDBEntities.SaveChanges();


            return Json(data:new { message = "Room "+message, success=true }, JsonRequestBehavior.AllowGet);
        }
       
        public PartialViewResult GetAllRooms()
        {
            IEnumerable<RoomDetailsViewModel> ListOfRoomDetailsViewModels =//łączenie tabel
                (from objRoom in objHotelDBEntities.Rooms
                 join objBooking in objHotelDBEntities.BookingStatus on
                 objRoom.BookingStatusid equals objBooking.BookingStatusid

                 join objRoomType in objHotelDBEntities.RoomTypes on
                 objRoom.RoomTypeid equals objRoomType.RoomTypeid

                 
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

            return PartialView("_RoomDetailsPartial", ListOfRoomDetailsViewModels);//dywołanie widoku z wartościami wyżej 
        }
        public PartialViewResult GetRoomWolny()
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
                     
                     RoomCapacity = objRoom.RoomCapacity,
                     
                     BookingStatus = objBooking.BookingStatus,
                     RoomType = objRoomType.RoomTypeName,
                     RoomImage = objRoom.RoomImage,
                     Roomid = objRoom.Roomid

                 }).ToList();

            return PartialView("_RoomWolnyDoRemontu", ListOfRoomDetailsViewModels);
        }
        public PartialViewResult GetRoomRemont()
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

                     RoomCapacity = objRoom.RoomCapacity,

                     BookingStatus = objBooking.BookingStatus,
                     RoomType = objRoomType.RoomTypeName,
                     RoomImage = objRoom.RoomImage,
                     Roomid = objRoom.Roomid

                 }).ToList();

            return PartialView("_RoomRemont", ListOfRoomDetailsViewModels);
        }
        
        //public PartialViewResult GetGuestInfo()
        //{
        //    List<RoomBookingViewModel> listOfBookingViewModel = new List<RoomBookingViewModel>();
        //    listOfBookingViewModel = (from objHotelBooking in objHotelDBEntities.RoomBookings
        //                              join objRoom in objHotelDBEntities.Rooms on objHotelBooking.AssignRoomid equals objRoom.Roomid
        //                              select new RoomBookingViewModel()
        //                              {
        //                                  BookingFrom = objHotelBooking.BookingFrom,
        //                                  BookingTo = objHotelBooking.BookingTo,
        //                                  CustomerName = objHotelBooking.CustomerName,
        //                                  CustomerAddres = objHotelBooking.CustomerAddres,
        //                                  NoOfMembers = objHotelBooking.NoOfMembers,
        //                                  TotalAmount = objHotelBooking.TotalAmount,
        //                                  RoomNumber = objRoom.RoomNumber,
        //                                  Bookingid = objHotelBooking.Bookingid,
        //                                  NumberOfDays = System.Data.Entity.DbFunctions.DiffDays(objHotelBooking.BookingFrom, objHotelBooking.BookingTo).Value,
        //                                  RoomPrice = objRoom.RoomPrice

        //                              }).ToList();
        //    return PartialView("_GuestInfo", listOfBookingViewModel);
        //}
        [HttpGet]
        public JsonResult EditRoomDetails(int roomid)
        {
            var result = objHotelDBEntities.Rooms.Single(model => model.Roomid == roomid);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteRoomDetails(int roomid)
        {
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == roomid);
            objHotelDBEntities.Rooms.Remove(objRoom);
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Pokój usunięty", success = true }, JsonRequestBehavior.AllowGet);
            
        }
        [HttpGet]
        public JsonResult Remont(int roomid)
        {
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == roomid);
            objRoom.BookingStatusid = 5;
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Pokój "+objRoom.RoomNumber+" wysłany do remontu", success = true }, JsonRequestBehavior.AllowGet);
            
        }
        [HttpGet]
        public JsonResult KoniecRemontu(int roomid)
        {
            Room objRoom = objHotelDBEntities.Rooms.Single(model => model.Roomid == roomid);
            objRoom.BookingStatusid = 1;
            objHotelDBEntities.SaveChanges();
            return Json(new { message = "Pokój " + objRoom.RoomNumber + " koniec remontu", success = true }, JsonRequestBehavior.AllowGet);

        }
    }
}