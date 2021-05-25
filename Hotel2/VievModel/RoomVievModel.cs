using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Hotel2.VievModel
{
    public class RoomVievModel
    {
        public int Roomid { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Nr. Pokoju")]
        [Required(ErrorMessage ="Podaj Numer ")]
        public string RoomNumber { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Zdjęcie Pokoju")]
        public string RoomImage { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Cena Pokoju")]
        [Required(ErrorMessage = "Podaj cene ")]
        [Range(500, 10000, ErrorMessage ="przedział cen 500 - 10 000")]
        public decimal RoomPrice { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Status")]
        [Required(ErrorMessage = "Podaj status ")]
        public int BookingStatusid { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Typ Pokoju")]
        [Required(ErrorMessage = "Podaj typ ")]
        public int RoomTypeid { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Ilość łóżek")]
        [Required(ErrorMessage = "Podaj wielkość ")]
        [Range(1,8, ErrorMessage ="Przedział 1-8")]
        public int RoomCapacity { get; set; }
        //-------------------------------------------------------------
        public HttpPostedFileBase Image { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Opis")]
        [Required(ErrorMessage = "Podaj opis ")]
        public string RoomDescription { get; set; }
        //-------------------------------------------------------------
        private int Bookingid { get; set; }
        public List<SelectListItem> ListOfBookingStatus { get; set; }
        public List<SelectListItem> ListOfRoomType { get; set; }
        
    }
}