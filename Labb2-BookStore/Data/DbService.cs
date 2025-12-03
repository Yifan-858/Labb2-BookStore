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
            return await _context.Authors.ToListAsync();
        }

        public async Task<List<Publisher>> GetAllPublisher()
        {
            return await _context.Publishers.ToListAsync();
        }
    }
}
