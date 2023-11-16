using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;

        public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEnumerable<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public IEnumerable<Movie> Search(string searchString)
        {
            var allMovies = _context.Movies;
            var listOfMovies = allMovies.ToList();
            var temp = listOfMovies.Where(x => x.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase));

            return temp;
        }

        public Movie GetById(int id)
        {
            return _context.Movies.FirstOrDefault(x => x.Id == id);
        }

        //above search method is preferred over the FindMovie() - use this if results needed immediately or in small list

        //public List<Movie> FindMovie(string title)
        //{
            // find by title - could return more than one item
          //  return _context.Movies.Where(movie => movie.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        //}

        public Movie AddMovie(string title, DateTime releaseDate)  
        {
            using (var db = new MovieContext())
            {
                Movie newMovie = new Movie()
                {
                    Title = title,
                    ReleaseDate = releaseDate
                };

                try
                {
                    _context.Movies.Add(newMovie);
                    _context.SaveChanges();
                    return newMovie;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving movie: {ex.Message}");
                    return null;
                }
            }
        }
                                               
        

        public void DeleteMovie(long movieId)
        {
            using (var db = new MovieContext()) {
                var movieDelete = _context.Movies.Find(movieId);

                if (movieDelete != null)
                {
                    _context.Movies.Remove(movieDelete);
                    _context.SaveChanges();
                }
            }
            
        }
    }
}
