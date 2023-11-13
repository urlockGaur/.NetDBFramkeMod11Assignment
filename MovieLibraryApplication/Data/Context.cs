using ContextExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContextExample.Data
{
    public class Context : IContext
    {
        public List<Movie> Movies { get; set; }


        public Context()
        {
            Movies = new List<Movie>()
            {
                new Movie { Id = 1, Title = "Toy Story" },
                new Movie { Id = 2, Title = "Star Wars" },
                new Movie { Id = 3, Title = "Oceans 11" },
                new Movie { Id = 4, Title = "Oceans 12" },
                new Movie { Id = 5, Title = "Predator" },
                new Movie { Id = 6, Title = "Aliens" },
                new Movie { Id = 7, Title = "Commando" },
                new Movie { Id = 8, Title = "Bloodsport" },
                new Movie { Id = 9, Title = "Terminator 2: Judgement Day" },
                new Movie { Id = 10, Title = "Total Recall" },
                new Movie { Id = 11, Title = "Night of the Living Dead" },
                new Movie { Id = 12, Title = "Day of the Dead" }
            };
        }

        public Movie GetById(int id)
        {
            return Movies.FirstOrDefault(x => x.Id == id);
        }

        public Movie GetByTitle(string title)
        {
            return Movies.FirstOrDefault(x => x.Title == title);
        }

        public List<Movie> FindMovie(string title)
        {
            // find by title - could return more than one item
            return Movies.Where(movie => movie.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
