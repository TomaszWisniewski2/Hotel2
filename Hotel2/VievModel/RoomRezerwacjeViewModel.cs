using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hotel2.VievModel
{
    public class RoomRezerwacjeViewModel
    {
        //-------------------------------------------------------------
        public int Bookingid { get; set; }
        //-------------------------------------------------------------
        public string CustomerName { get; set; }
        //-------------------------------------------------------------

        public string CustomerAddres { get; set; }
        //-------------------------------------------------------------

        public DateTime BookingFrom { get; set; }
        //-------------------------------------------------------------

        public DateTime BookingTo { get; set; }
        //-------------------------------------------------------------

        public string RoomNumber { get; set; }
        //-------------------------------------------------------------

        public int NoOfMembers { get; set; }
        //-------------------------------------------------------------
        public Nullable<decimal> TotalAmount { get; set; }
        //-------------------------------------------------------------
        public int RoomCapacity { get; set; }

        public string RoomImage { get; set; }

        public decimal RoomPrice { get; set; }

        public string BookingStatus { get; set; }
        public int Roomid { get; set; }
        public int BookingStatusid { get; set; }
        public string Zapłacone { get; set; }
        public int Paymentid { get; set; }

    }
}