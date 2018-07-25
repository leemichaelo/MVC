using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookGalleyModel
{
    class Program
    {
        static void Main(string[] args)
        {
            //Context uses the IDispose, so we need to wrap it in using to dispose
            using (var context = new Context())
            {
                context.ComicBooks.Add(new Models.ComicBook()
                {
                    SeriesTitle = "The Amazing Spider-Man",
                    IssueNumber = 1,
                    PublishedOn = DateTime.Today
                });

                context.SaveChanges();

                var comicBooks = context.ComicBooks.ToList();

                foreach(var comicBook in comicBooks)
                {
                    Console.WriteLine(comicBook.SeriesTitle);
                }

                Console.ReadLine();
            }
   
        }
    }
}
