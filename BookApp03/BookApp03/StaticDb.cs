using BookApp03.Models;

namespace BookApp03
{
    public static class StaticDb
    {
        public static List<Book> Books = new List<Book>
        {
            new Book()
            {
                Id = 1,
                Author = "Video Podgorec",
                Title = "Beloto Cigance",
            },
            new Book()
            {
                Id = 2,
                Author = "Vanco Nikolovski",
                Title = "Volsebnoto Samarce",

            },
            new Book()
            {
                Id = 3,
                Author = "Unknown",
                Title = "Drive"
            }
        };
    }
}
