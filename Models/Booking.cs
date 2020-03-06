using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace smalandscamping.Models
{
    public class Booking
    {
        //Id för bokning
        public int BookingId { get; set; }

        //Referenser till användare
        //[ForeignKey("User")]
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        //Referenser till stuga
        public int CottageId { get; set; }
        public Cottage Cottage { get; set; }

        //Datum för ankomst
        [Required]
        [Display(Name = "Datum för ankomst")]
        [DataType(DataType.Date)]
        public DateTime DateArrival { get; set; }

        //Datum för hemkost
        [Required]
        [Display(Name = "Datum för hemkost")]
        [DataType(DataType.Date)]
        public DateTime DateLeaving { get; set; }

        //Totalt pris baserat på antal dagar
        public double TotalPrice { get; set; }

        public Booking()
        {
            TotalPrice = 0;
        }

        //Uträkning av total kostnad baserat på antal dagar som väljs
        public void CalcCost()
        {
            int days = (DateLeaving.Date - DateArrival.Date).Days;

            if (days > 2 && days <= 4)
            {
                TotalPrice = Cottage.Price + 1000;
            }
            else if (days > 4)
            {
                TotalPrice = Cottage.Price + 2000;
            }
        }
    }
}
