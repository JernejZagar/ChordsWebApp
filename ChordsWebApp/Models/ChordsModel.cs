using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWebApp.Models
{
    public class Chords
    {
        [Key]
        public Guid Id { get; set; }

        //[Display(Name = "Izvajalec")]
        [Required(AllowEmptyStrings = false)]
        public string Artist { get; set; }

        [Display(Name = "Title")]
        [Required(AllowEmptyStrings = false)]
        public string Song { get; set; }

        [Display(Name = "Capo")]
        [Required(AllowEmptyStrings = false)]
        public string Capo { get; set; }

        //[Display(Name = "Tonaliteta")]
        [Required(AllowEmptyStrings = false)]
        public string Key { get; set; }

        [Display(Name = "Lyrics & Chords")]
        [Required(AllowEmptyStrings = false)]
        public string Content { get; set; }

        //[Display(Name = "Datum dodajanja")]
        public DateTime DateAdded { get; set; }

        //[Display(Name = "Datum spreminjanja")]
        public DateTime DateEdited { get; set; }
        
        public string Uploader { get; set; }
    }
}
