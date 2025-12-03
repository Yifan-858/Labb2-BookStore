using Microsoft.EntityFrameworkCore.Internal;
using Labb2_BookStore.Data;
using AdminTools.Helper;

namespace AdminTools
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var db = new DbService();

            while (true)
            {
                Console.WriteLine("=== Company Database Admin Tool ===");
                Console.WriteLine("1. Add new book");
                Console.WriteLine("2. ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var newBook = await BookStoreHelper.CreateBookHelper(db);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"A new book is successfully added into database");
                        Console.WriteLine($"{newBook.Isbn13} {newBook.Title}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}
