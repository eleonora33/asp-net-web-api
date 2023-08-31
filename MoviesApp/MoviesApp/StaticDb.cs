using MoviesApp.Models;
using MoviesApp.Models.Enum;

namespace MoviesApp
{
    public static class StaticDb
    {
        public static List<Movie> Movies = new List<Movie>()
        {
            new Movie
            {
                Id = 1,
                Title = "Foo",
                Description = "Bar",
                Year = 2016,
                Genre = GenreEnum.Comedy
            },
            new Movie
            {
                Id = 2,
                Title = "007",
                Description = "James Bond",
                Year = 1971,
                Genre = GenreEnum.Action
            },
            new Movie
            {
                Id = 3,
                Title = "Sleeping Beauty",
                Description = "Very good",
                Year = 2022,
                Genre = GenreEnum.Romance
            }
        };
    }
}
