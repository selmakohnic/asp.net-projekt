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
        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }

        //Referenser till stuga
        public int CottageId { get; set; }

        public Cottage Cottage { get; set; }

        //Datum för ankomst med en kontroll av val av datum
        [Required(ErrorMessage = "Välj ett datum för din ankomst")]
        [Display(Name = "Ankomst")]
        [DataType(DataType.Date)]
        [DateCheck(ErrorMessage = "Du kan inte välja ett datum som redan har varit")]
        public DateTime DateArrival { get; set; }

        //Datum för hemkomst med en kontroll av val av datum
        [Required(ErrorMessage = "Välj ett datum för din hemfärd")]
        [Display(Name = "Hemfärd")]
        [DataType(DataType.Date)]
        [DateCheck(ErrorMessage = "Du kan inte välja ett datum som redan har varit")]
        public DateTime DateLeaving { get; set; }

        [Display(Name = "Totalt pris")]
        public double TotalPrice { get; set; }
    }

    //Kontroll av datum som väljs så att användaren inte väljer ett datum som är tidigare än dagens datum
    public class DateCheckAttribute : ValidationAttribute
    {
        //Returnerar true om datumet som valts är dagens datum eller senare, returnerar false om det är innan dagens datum
        public override bool IsValid(object value)
        {
            DateTime date = Convert.ToDateTime(value);
            return date >= DateTime.Now;
        }
    }
}
