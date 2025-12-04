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
            {Console.ForegroundColor = ConsoleColor.Gray;
                Console.Clear();
                Console.WriteLine("=== Company Database Admin Tool ===");
                Console.WriteLine("1. Add new book title with ISBN");
                Console.WriteLine("2. Add a new author");
                Console.WriteLine("3. Send book to a store");
                Console.WriteLine("4. Edit store inventory");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var newBook = await BookStoreHelper.CreateBook(db);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("");
                        Console.WriteLine($"A new book is successfully added into database!");
                        Console.WriteLine($"ISBN13: {newBook.Isbn13}");
                        Console.WriteLine($"Title: {newBook.Title}");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                    case "2":
                        break;
                    case "3":
                        await BookStoreHelper.AddBookToStore(db);
                        break;
                    case "4":
                        await BookStoreHelper.UpdateInventory(db);
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }
    }
}
