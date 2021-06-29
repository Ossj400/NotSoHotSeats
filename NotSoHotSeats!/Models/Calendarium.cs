using System;
using System.Collections.Generic;

#nullable disable

namespace NotSoHotSeats_.Models
{
    public partial class Calendarium
    {
        public int IdReservation { get; set; }
        public string SeatSymbol { get; set; }
        public DateTime DateOfReservation { get; set ; }
        public int IdUser { get; set; }


    }
}
