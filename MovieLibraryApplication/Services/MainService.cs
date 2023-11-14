﻿
using ContextExample.Models;
using MovieLibraryEntities.Dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ContextExample.Services;

/// <summary>
///     You would need to inject your interfaces here to execute the methods in Invoke()
///     See the commented out code as an example
/// </summary>
public class MainService : IMainService
{
    private readonly IRepository _repository;

    public MainService(IRepository repository)
    {
        _repository = repository;
    }

    public void Invoke()
    {
        string movieLibraryMenu;
        // provide an option to the user to 
        // 1. select by id
        // 2. select by title 
        // 3. find movie by title


        //Menu
        ConsoleColor textColor = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Welcome!");
        Console.ForegroundColor = textColor;

        Console.Write("You've entered the ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("Framke Movie Library");

        Console.WriteLine(); //line break 
        Console.WriteLine("----------------------------------------------");
        Console.ForegroundColor = textColor;
        Console.WriteLine(); //line break 

        Console.WriteLine("Please choose from the following options: ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("1. Search for a Movie");
        Console.WriteLine("2. Add a Movie");
        Console.WriteLine("3. List all Movies ");
        Console.WriteLine("4. Delete a Movie ");
        Console.WriteLine();
        Console.WriteLine("----------------------------------------------");
        Console.ForegroundColor = textColor;
        movieLibraryMenu = Console.ReadLine();

        if (movieLibraryMenu == "1")
        {
            Console.WriteLine("Search by Movie Title");
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Please enter your search terms: ");
            Console.ForegroundColor = textColor;
            var searchTerm = Console.ReadLine();

            var movies = _repository.Search(searchTerm);

            if (movies.Any())
            {
                Console.WriteLine("Search Results: ");

                foreach (var movie in movies)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Title: {movie.Title} Release Date: {movie.ReleaseDate}");
                    Console.ForegroundColor = textColor;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Movie not found. Please check your input and try again.");
                Console.ForegroundColor = textColor;
            }
        }
        
        else if (movieLibraryMenu == "2")
        {
            Console.WriteLine("Please enter the Title of the movie you want to add: ");
            var movieTitle = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Please enter the Release Date of the movie (YYYY-MM-DD): ");
            Console.WriteLine();

            if (DateTime.TryParse(Console.ReadLine(), out DateTime releaseDate))
            {
                var movie = _repository.AddMovie(movieTitle, releaseDate);

                if (movie != null)
                {
                    Console.Write($"Movie added: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Title: {movie.Title} Release Date: {movie.ReleaseDate}");
                    Console.ForegroundColor = textColor;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Could not add the movie. Please review the input and try again.");
                    Console.ForegroundColor = textColor;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid date formate. Please enter the release date in the correct format (YYYY-MM-DD).");
                Console.ForegroundColor = textColor;
            }
        }
        else if (movieLibraryMenu == "3")

        {
                Console.WriteLine("Movie Library List: ");
                Console.WriteLine("----------------------------------------------");
                Console.WriteLine();

                var movieList = _repository.GetAll();
                if (movieList.Any())
                {
                Console.ForegroundColor = ConsoleColor.Green;

                int moviesPerPage = 10;

                for (int i = 0; i < movieList.Count(); i += moviesPerPage)
                {
                    var moviesGroup = movieList.Skip(i).Take(moviesPerPage);

                    foreach( var movie in moviesGroup)
                    {
                        string genre = string.Join(", ", movie.MovieGenres.Select(x => x.Genre.Name));
                        Console.WriteLine($"Title: {movie.Title}, Release Date: {movie.ReleaseDate.ToString("MM/dd/yyy")}, Genres: {genre} ");                        
                                              
                    }
                    Console.WriteLine();
                    Console.WriteLine("Press enter to view more movies or type 'exit' to stop...");
                    var userInput = Console.ReadLine();

                    if(userInput.ToLower() == "exit")
                    {
                        Console.Clear();
                        break;
                    }
                    Console.Clear();
                }
                Console.ForegroundColor = textColor;
                }
                else
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("No movies found...");
                    Console.ForegroundColor = textColor;                   
                }
            }
        else if (movieLibraryMenu == "4")
        {
            Console.WriteLine("Enter the Id of the movie you want to delete: ");
            var deleteInput = Console.ReadLine();
            Console.WriteLine("....");

            if (int.TryParse(deleteInput, out int movieIdDelete)) 
            {
                
                var movieDelete = _repository.GetById(Convert.ToInt32(deleteInput));
                
                if (movieDelete != null)
                {
                    Console.WriteLine($"Movie Details: ");
                    Console.WriteLine($"Title: {movieDelete.Title}");
                    Console.WriteLine($"Release Date: {movieDelete.ReleaseDate.ToString("MM/dd/yyy")}");

                    Console.WriteLine($"Are you sure you want to delete {movieDelete}: y / n ");
                    var deleteConfirmationInput = Console.ReadLine();
                    if (deleteConfirmationInput.ToLower() == "y") 
                    {
                        _repository.DeleteMovie(movieIdDelete);
                        Console.WriteLine("Movie deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Delete canceled.");
                    }
                }
                else 
                { 
                    Console.WriteLine("Movie not found. Try again"); 
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid Movie Id.");
            }
        }
    }
}

