using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Labb2_BookStore.Data;
using Labb2_BookStore.Models;
using Microsoft.VisualBasic;

namespace AdminTools.Helper
{
    public static class BookStoreHelper
    {
        private const string Isbn13Pattern = @"^(978|979)-\d{1,5}-\d{1,7}-\d{1,6}-\d{1}$";
        public static async Task<Book> CreateBookHelper(DbService db)
        {
            Console.Clear();
            Console.WriteLine("======== Create New Book ========");

            string isbn13 = GetValidIsbn13();
            string title = GetValidTitle();
            string language = GetValidLanguage();
            decimal price = GetValidPrice();
            DateOnly publicationDate = GetValidPublicationDate();
            int authorId = await GetValidAuthorId(db);
            int publisherId =await GetValidPublisherId(db);

            Book newBook = await db.CreateBook(isbn13,title,language,price,publicationDate,authorId,publisherId);
          
            return newBook;
        }

        public static async Task ViewStoreInventory(DbService db)
        {
            Console.Clear();

            int storeId = await GetStoreList(db);

            if (storeId == -1)
            {
                Console.WriteLine("No stores were found. Press any key to return.");
                Console.ReadLine();
                return;
            }

            Console.Clear();
            Console.WriteLine("======== View Inventory ========");
            await DisplayInventory(storeId, db);
        }

        public static string GetValidIsbn13()
        {
            Console.WriteLine("Please enter the book ISBN13 e.g.978-1-52994-399-3:");
            string isbn13 = "";
            bool IsValid = false;

            do
            {
                isbn13 = Console.ReadLine();

                if (string.IsNullOrEmpty(isbn13))
                {
                    Console.WriteLine("ISBN13 cannot be empty. Please enter the number.");
                }
                else if (Regex.IsMatch(isbn13.Trim(), Isbn13Pattern))
                {
                    IsValid = true;
                }
                else
                {
                    Console.WriteLine("Invalid ISBN13 format.");
                    Console.WriteLine("Format e.g.978-1-52994-399-3");
                }
            } while (!IsValid);

            return isbn13;
        }

        public static string GetValidTitle()
        {
            string title = "";
            bool IsValid = false;

            do
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Please enter the book Title:");
                title = Console.ReadLine();

                if (string.IsNullOrEmpty(title))
                {
                    Console.WriteLine("Please enter the title.");
                }

                IsValid = true;

            } while (!IsValid);

            return title;
        }

        public static string GetValidLanguage()
        {
            string language = "";
            bool IsValid = false;

            do
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Please enter the book Language (e.g. English):");
                language = Console.ReadLine();

                if (string.IsNullOrEmpty(language))
                {
                    Console.WriteLine("Please enter the Language.");
                }

                IsValid = true;

            } while (!IsValid);

            return language;
        }

        public static decimal GetValidPrice()
        {
            decimal price = -1;
            bool IsValid = false;

            do
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("Please enter the book Price:");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter the Price.");
                }
                else if (decimal.TryParse(input.Trim(), out price) && price > 0)
                {
                    IsValid = true;
                }
                else
                {
                    Console.WriteLine("Invalid Price. Please enter a positive number");
                }

            } while (!IsValid);

            return price;
        }

        public static DateOnly GetValidPublicationDate()
        {
            DateOnly publicationDate = new DateOnly();
            const string dateFormat = "yyyy-MM-dd";
            bool isValid = false;

            do
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine($"Please enter the Publication Date (e.g: 2000-10-26):");
                
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("ERROR: Publication Date cannot be empty. Please enter a value.");
                }
              
                else if (DateOnly.TryParseExact(input.Trim(), dateFormat, null, System.Globalization.DateTimeStyles.None, out publicationDate))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine($"ERROR: Invalid Date Format. Please use the exact format yyyy-mm-dd.");
                }
            } while (!isValid);

            return publicationDate;
        }

        public static async Task<int> GetValidAuthorId(DbService db)
        {
            Console.WriteLine("--------------------------------------");

            List<Author> authors = await db.GetAllAuthor();

            if (authors == null || authors.Count == 0)
            {
                Console.WriteLine("No authors found in the database. Create author first.");
                return -1;
            }

            List<string> authorsOptions = authors.Select(a => $"{a.AuthorId}. {a.FirstName} {a.LastName}").ToList();

            MenuHelper authorMenu = new MenuHelper("Please choose an Author:", authorsOptions);

            int selectedIndex = authorMenu.ControlChoice();
            int matchedAuthorId = authors[selectedIndex].AuthorId;//safe, id(uuid) might not equal to index

            return matchedAuthorId;
        }

        public static async Task<int> GetValidPublisherId(DbService db)
        {
            Console.WriteLine("--------------------------------------");

            List<Publisher> publishers = await db.GetAllPublisher();

            if (publishers == null || publishers.Count == 0)
            {
                Console.WriteLine("No publishers found in the database. Create publisher first.");
                return -1;
            }

            List<string> publishersOptions = publishers.Select(p => $"{p.PublisherId}. {p.PublisherName}").ToList();

            MenuHelper publisherMenu = new MenuHelper("Please choose a publisher:", publishersOptions);

            int selectedIndex = publisherMenu.ControlChoice();
            int matchedAuthorId = publishers[selectedIndex].PublisherId;

            return matchedAuthorId;
        }

        public static async Task<int> GetStoreList(DbService db)
        {
            List<Store> stores = await db.GetAllStore();

            if ( stores == null ||  stores.Count == 0)
            {
                Console.WriteLine("No stores found in the database. Create store first.");
                return -1;
            }

            List<string> storeOptions = stores.Select(s => $"{s.StoreName}").ToList();

            MenuHelper storeMenu = new MenuHelper("======== Choose a store ========",storeOptions);

            int index = storeMenu.ControlChoice();
            int storeId = stores[index].StoreId;

            return storeId;
        }

        public static async Task DisplayInventory(int storeId, DbService db)
        {
            List<Inventory> inventories = await db.GetInventoryByStoreId(storeId);

            if(inventories == null || inventories.Count == 0)
            {
                Console.WriteLine("No inventory is found associated to this store. Create inventory first.");
            }

            List<string> bookQuantities = inventories.Select(i => $"{i.Quantity}").ToList();
            List<string> bookISBNs = inventories.Select(i => $"{i.Isbn13}").ToList();

            List<Book> books = new List<Book>();
            foreach(string isbn in bookISBNs)
            {
                books.Add(await db.GetBookByISBN(isbn));
            }

            if(books == null || books.Count == 0)
            {
                Console.WriteLine("No book is found.");
            }

            var combinedData = inventories.Zip(books, (inventory, book) => new 
            {
                Inventory = inventory,
                Book = book 
            }).Where(item=> item.Book != null).ToList();

            if (combinedData.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Inventory exists, but no matching books were found.");
                Console.ResetColor();
            }

            List<string> inventoryBookTitleOption = combinedData.Select(info => $"ISBN: {info.Inventory.Isbn13} - Quantity: {info.Inventory.Quantity} - Title: {info.Book.Title}").ToList();

            for(int i = 0; i < inventoryBookTitleOption.Count; i++)
            {
                Console.WriteLine($"{i + 1}.{inventoryBookTitleOption[i]}");
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Press any key to return.");
            Console.ReadKey();
        }
    }
}
