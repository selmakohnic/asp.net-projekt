using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smalandscamping.ViewModels
{
    public class CottageCreateViewModel
    {
        //Id för stuga
        public int CottageId { get; set; }

        //Namn på stuga som inte är längre än 30 tecken
        [Required(ErrorMessage = "Ange ett namn på stugan")]
        [Display(Name = "Namn")]
        [StringLength(30, ErrorMessage = "Namnet får inte vara längre än 30 tecken")]
        public string Name { get; set; }

        //Pris mellan 500 - 10 000 kr
        [Required(ErrorMessage = "Ange ett pris på stugan")]
        [Display(Name = "Pris")]
        [Range(500, 10000, ErrorMessage = "Priset måste vara mellan 500 - 10 000 kr")]
        public int Price { get; set; }

        //Max antal gäster i en stuga mellan 1 - 10 stycken
        [Required(ErrorMessage = "Ange max antal gäster i stugan")]
        [Display(Name = "Max antal gäster")]
        [Range(1, 10, ErrorMessage = "Antal gäster kan endast vara mellan 1 - 10 stycken")]
        public int NumberOfGuest { get; set; }

        //Om djur är tillåtna
        [Required]
        [Display(Name = "Djur tillåtna")]
        public bool AnimalsAllowed { get; set; }

        //Beskrivning av 
        [Required(ErrorMessage = "Ange en beskrivning av stugan")]
        [Display(Name = "Beskrivning")]
        [StringLength(300, MinimumLength = 20, ErrorMessage = "Beskrivningen ska vara mellan 20 - 300 tecken lång")]
        public string Description { get; set; }

        //Kortare version av beskrivning som visas i vissa vyer
        public string DescriptionTrimmed
        {
            get
            {
                //Om textlängden är mer än 50 tecken hämtas en trimmad version av beskrivningen
                if (Description.Length > 50)
                {
                    return Description.Substring(0, 50) + " [...]";
                }
                return Description;
            }
        }

        //Kontroll av om stugan är bokad
        [Display(Name = "Bokad")]
        public bool IsBooked { get; set; }

        //Bildfil
        [Display(Name = "Bild")]
        public IFormFile Photo { get; set; }

        public CottageCreateViewModel()
        {
            //Varje stuga är från början inte bokad och får då värdet false. Detta ändras till true vid en bokning
            IsBooked = false;
        }
    }
}
