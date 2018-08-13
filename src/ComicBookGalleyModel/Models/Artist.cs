using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComicBookGalleyModel.Models
{
    public class Artist
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }

        //Each comic book can have one or any artists and needs to be part of the constructor
        public ICollection<ComicBookArtist> ComicBooks { get; set; }

        public Artist()
        {
            ComicBooks = new List<ComicBookArtist>();
        }
    }
}
