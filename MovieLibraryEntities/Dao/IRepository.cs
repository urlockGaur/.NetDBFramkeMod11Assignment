using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        IEnumerable<Movie> GetAll();
        IEnumerable<Movie> Search(string searchString);

        Movie GetById(int id);

        //List<Movie> FindMovie(string title);

        Movie AddMovie(string title, DateTime releaseDate);
    }
}
