using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookGalleyModel.Models
{
    public class Series
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        public string Description { get; set; }

        //Each series can havea list of comic books that must be part of the constructor
        public ICollection<ComicBook> ComicBooks { get; set; }

        public Series()
        {
            ComicBooks = new List<ComicBook>();
        }
    }
}
