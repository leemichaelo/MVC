using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookGalleyModel.Models
{
    public class ComicBook
    {
        //ID, ComicBookId, ComicBookID
        public int Id { get; set; }
        public int SeriesId { get; set; }       
        public int IssueNumber { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public decimal? AverageRating { get; set; }

        //Each comic book can only belong to one series
        public Series Series { get; set; }
        //Each comic book can have one or any artists and needs to be part of the constructor
        public virtual ICollection<ComicBookArtist> Artists { get; set; }

        public ComicBook()
        {
            Artists = new List<ComicBookArtist>();
        }

        public string DisplayText
        {
            get
            {
                return $"{Series?.Title} #{IssueNumber}";
            }
        }

        public void AddArtist(Artist artist, Role role)
        {
            Artists.Add(new ComicBookArtist()
            {
                Artist = artist,
                Role = role
            });
        }
    }
}
