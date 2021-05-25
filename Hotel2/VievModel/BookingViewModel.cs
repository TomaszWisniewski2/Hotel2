using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
namespace Hotel2.VievModel
{
    public class BookingViewModel
    {

        // public int Bookingid { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Imie")]
        [Required(ErrorMessage = "Podaj imie ")]
        public string CustomerName { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Nazwisko")]
        [Required(ErrorMessage = "Podaj adres ")]
        public string CustomerAddres { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Data od")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Podaj date od ")]
        [DisplayFormat(DataFormatString ="{0:dd-MMM-yyyy}", ApplyFormatInEditMode =true)]
        public DateTime BookingFrom { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Data do")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Podaj date do ")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BookingTo { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Nr pokoju")]
        [Required(ErrorMessage = "Podaj pokój ")]
        public int AssignRoomid { get; set; }
        //-------------------------------------------------------------
        [Display(Name = "Ilość osób")]
        [Required(ErrorMessage = "Podaj ilość ")]
        public int NoOfMembers { get; set; }
        //-------------------------------------------------------------
        // public decimal TotalAmount { get; set; }
        public int Paymentid { get; set; }
        public IEnumerable<SelectListItem> ListOfRooms {get; set;}


    }
}