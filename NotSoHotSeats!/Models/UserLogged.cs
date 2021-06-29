using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace NotSoHotSeats_.Models
{
    public static class UserLogged 
    {
        public static int IdUser { get; set; }
        public static string FirstName { get; set; }
        public static string LastName { get; set; }

    }
}
