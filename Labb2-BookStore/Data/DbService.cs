using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Labb2_BookStore.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore; 

namespace Labb2_BookStore.Data
{
    public class DbService
    {
        private readonly AppDBContext _context;
        public DbService()
        {
            _context = new AppDBContext();
        }

        //Create
        public async Task<Book> CreateBook(string isbn13, string title, string language, decimal price, DateOnly publicationDate, int authorId, int publisherId)
        {
            if(await _context.Books.AnyAsync(b=>b.Isbn13 == isbn13))
            {
                throw new InvalidOperationException($"A book with ISBN:{isbn13} already exists.");
            }

            if(!await _context.Authors.AnyAsync(a=>a.AuthorId == authorId))
            {
                throw new KeyNotFoundException($"Author with ID:{authorId} is not found.");
            }

            if(!await _context.Publishers.AnyAsync(p=>p.PublisherId == publisherId))
            {
                throw new KeyNotFoundException($"Publisher with ID:{publisherId} is not found.");
            }

            var newBook = new Book
            {
                Isbn13 = isbn13,
                Title = title,
                Language = language,
                Price = price,
                PublicationDate = publicationDate,
                AuthorId = authorId,
                PublisherId = publisherId
            };

            _context.Books.Add(newBook);
            await _context.SaveChangesAsync();
            return newBook;
        }

        //Read
        public async Task<List<Author>> GetAllAuthor()
        {
            try
            {
                return await _context.Authors.ToListAsync();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Database error: Could not fetch authors. Detail: {ex.Message}");
                Console.ResetColor();

                return new List<Author>();
            }
            
        }

        public async Task<List<Publisher>> GetAllPublisher()
        {
            try
            {
                return await _context.Publishers.ToListAsync();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Database error: Could not fetch publishers. Detail: {ex.Message}");
                Console.ResetColor();

                return new List<Publisher>();
            }
            
        }

        public async Task<List<Store>> GetAllStore()
        {
            try
            {
                return await _context.Stores.ToListAsync();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Database error: Could not fetch stores. Detail: {ex.Message}");
                Console.ResetColor();

                return new List<Store>();
            }
            
        }

        public async Task<List<Inventory>> GetInventoryByStoreId(int storeId)
        {
            try
            {
                return await _context.Inventories.Where(i => i.StoreId == storeId).ToListAsync();
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Database error: Could not fetch inventory. Detail: {ex.Message}");
                Console.ResetColor();

                return new List<Inventory>();
            }
            
        }

        public async Task<Book?> GetBookByISBN(string isbn)
        {
            string trimmedIsbn = isbn.Trim();
            Book? book = null;

            try
            {
                book = await _context.Books.Where(b => b.Isbn13.Trim() == trimmedIsbn).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Database error: Could not fetch book. Detail: {ex.Message}");
                Console.ResetColor();

                return null;
            }

            if(book == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"No book with ISBN:{isbn} is found. Please try again");
                Console.ResetColor();
            }

            return book;
        }

        //Update
        public async Task<string> UpdateInventory(string isbn, int quantity, int storeId)
        {
            var inventory = await _context.Inventories.Where(i => i.StoreId == storeId).FirstOrDefaultAsync();
            inventory.Quantity = quantity;

            await _context.SaveChangesAsync();

            return $"Book({isbn})'s quantiry updated!";
        }
        //Delete

    }
}
