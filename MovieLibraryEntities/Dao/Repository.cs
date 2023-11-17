using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using System.Diagnostics.Metrics;
using System;

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

        public Movie GetById(long id)
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
                                               
        

        public void DeleteMovie(long movieIdDelete)
        {            
                var movieDelete = GetById(movieIdDelete);

                if (movieDelete != null)
                {
                    MovieDetails(movieDelete);
                    ConsoleColor textColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                                       
                    Console.WriteLine($"Are you sure you want to delete {movieDelete.Title}: y / n ");
                    Console.ForegroundColor = textColor;
                    var deleteConfirmationInput = Console.ReadLine();
                    if (deleteConfirmationInput.ToLower() == "y")
                    {
                        try
                        {
                            var movieToDelete = _context.Movies.Find(movieIdDelete);

                            if (movieToDelete != null)
                            {
                                _context.Movies.Remove(movieToDelete);
                                _context.SaveChanges();
                                Console.WriteLine("Movie deleted successfully.");
                            }
                            else
                            {
                                Console.WriteLine("Movie not found");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"An error occured while attempting to delete the movie: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Delete canceled");
                    }
                }
                else
                {
                    Console.WriteLine("Movie not found. Try again");
                }
            }
            
        
                        

        private void MovieDetails(Movie movie)
        {
            ConsoleColor textColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"Movie Details: ");
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Release Date: {movie.ReleaseDate.ToString("MM/dd/yyy")}");
            Console.WriteLine();

            Console.ForegroundColor = textColor;
        }

        public void ListMovieLibrary()
        {
            ConsoleColor textColor = Console.ForegroundColor;

            try {
                Console.WriteLine("Movie Library List: ");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine();

                var movieList = GetAll();
                if (movieList.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Green;

                    int moviesPerPage = 10;

                    for (int i = 0; i < movieList.Count(); i += moviesPerPage)
                    {
                        var moviesGroup = movieList.Skip(i).Take(moviesPerPage);

                        foreach (var movie in moviesGroup)
                        {
                            string genre = string.Join(", ", movie.MovieGenres.Select(x => x.Genre.Name));
                            Console.WriteLine($"Id: {movie.Id} | Title: {movie.Title} | Release Date: {movie.ReleaseDate.ToString("MM/dd/yyy")} | Genres: {genre} ");

                        }
                        Console.WriteLine();
                        Console.WriteLine("Press enter to view more movies or type 'exit' to stop...");
                        var userInput = Console.ReadLine();

                        if (userInput.ToLower() == "exit")
                        {
                            Console.Clear();
                            break;
                        }
                        Console.Clear();
                    }
                    Console.ForegroundColor = textColor;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

            
    }
}
