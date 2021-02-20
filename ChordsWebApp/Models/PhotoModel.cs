using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChordsWebApp.Models
{
    public class Photo
    {
        [Key]
        public Guid Id { get; set; }

        //[Display(Name = "Izvajalec")]
        [Required(AllowEmptyStrings = false)]
        public string Artist { get; set; }

        public byte[] ArtistPhoto { get; set; }

        public byte[] ArtistThumb { get; set; }

    }
}
