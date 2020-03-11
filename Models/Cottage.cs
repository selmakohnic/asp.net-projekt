using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smalandscamping.Models
{
    public class Cottage
    {
        //Id för stuga
        public int CottageId { get; set; }
        
        //Namn
        [Required]
        [Display(Name = "Namn")]
        public string Name { get; set; }

        //Pris
        [Required]
        [Display(Name = "Pris")]
        public int Price { get; set; }

        //Antal gäster
        [Required]
        [Display(Name = "Antal gäster")]
        public int NumberOfGuest { get; set; }

        //Om djur är tillåtna
        [Required]
        [Display(Name = "Djur tillåtna")]
        public bool AnimalsAllowed { get; set; }

        //Beskrivning
        [Required]
        [DisplayFormat(DataFormatString = "{0,20}")]
        [UIHint("ShortDescription")]
        [Display(Name = "Beskrivning")]
        public string Description { get; set; }
    }
}
