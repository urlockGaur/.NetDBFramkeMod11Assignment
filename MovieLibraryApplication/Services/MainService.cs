using ContextExample.Data;
using ContextExample.Models;
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
    private readonly IContext _context;

    public MainService(IContext context)
    {
        _context = context;
    }

    public void Invoke()
    {
        string movieLibraryMenu;
        // provide an option to the user to 
        // 1. select by id
        // 2. select by title 
        // 3. find movie by title


        //NOTES FOR PROFESSOR
        //Found out I can change console text color this weekend.
        //Wanted to experiment with it. I remembered seeing a library (not sure if this is the accurate term) in class regarding
        //the movie library assignment
        //that changed text color but couldn't find it. I probably wouldn't use the below methodolgy again.
        //Extra code that doesn't add much imo

        //This is version 2.0. First attempt program would crash after entering a searchTerm in option 3. Only way i could get program to run
        //was adding a 2nd Console.ReadLine() after the first. The search term would read and print to console, but the program would crash again after
        //I copied/paste my code into a new repo and Visual Studio window - code works and program doesn't crash.
        //Spent a few hours trying to debug and nothing would come up that would indicate an error somewhere.

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
        Console.WriteLine("1. Get movie by ID");
        Console.WriteLine("2. Get movie by Title");
        Console.WriteLine("3. Find list of movies matching title ");
        Console.WriteLine("----------------------------------------------");
        Console.ForegroundColor = textColor;
        movieLibraryMenu = Console.ReadLine();

        if (movieLibraryMenu == "1")
        {
            Console.WriteLine("Please enter the ID of the movie you would like to view: ");
            var movieID = Console.ReadLine();

            if (int.TryParse(movieID, out int id))
            {
                var movie = _context.GetById(id);

                if (movie != null)
                {

                    Console.Write("Your movie is ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{movie.Title}");
                    Console.ForegroundColor = textColor;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Movie not found. Please check ID and try again.");
                    Console.ForegroundColor = textColor;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid input. Only numbers can be entered.");
                Console.ForegroundColor = textColor;
            }
        }
        else if (movieLibraryMenu == "2")
        {
            Console.WriteLine("Please enter the Title of the movie you would to view: ");
            var movieTitle = Console.ReadLine();
            Console.WriteLine();

            var movie = _context.GetByTitle(movieTitle);

            if (movie != null)
            {
                Console.Write($"Movie found: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(movie.Title);
                Console.ForegroundColor = textColor;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Movie not found. Please check Title and try again.");
                Console.ForegroundColor = textColor;
            }
        }
        else if (movieLibraryMenu == "3")

        {
            bool searching = true;

            while (searching)
            {
                Console.WriteLine("Please enter a search term: ");
                var searchTerm = Console.ReadLine();

                var searchResults = _context.FindMovie(searchTerm);
                if (searchResults.Any())
                {
                    Console.WriteLine("Search Results:");

                    Console.ForegroundColor = ConsoleColor.Green;
                    searchResults.ForEach(movie => Console.WriteLine(movie.Title));
                    Console.ForegroundColor = textColor;

                    searching = false;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No movies found. Hit the 'Enter' key to search again or type 'X' to close the program.");
                    Console.ForegroundColor = textColor;
                    string searchAgain = Console.ReadLine();

                    if (searchAgain.Equals("X", StringComparison.OrdinalIgnoreCase))
                    {
                        searching = false;
                    }
                }

            }
        }
    }
}
