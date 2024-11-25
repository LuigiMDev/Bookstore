using Bookstore.Data;
using Bookstore.Models;
using Bookstore.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Services
{
    public class BookService
    {
        private readonly BookstoreContext _context;

        public BookService(BookstoreContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> FindAllAsync()
        {
            return await _context.Books.Include(x => x.Genres).ToListAsync();
        }

        public async Task Create(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            try
            {
                Book book = await _context.Books.FirstOrDefaultAsync(book => book.Id == id);
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new IntegrityException(ex.Message);
            }

        }

        public async Task Edit(Book bookEdited)
        {
            bool hasAny = await _context.Books.AnyAsync(x => x.Id == bookEdited.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrado");
            }

            try
            {
                _context.Books.Update(bookEdited);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DbConcorrencyException(ex.Message);
            }

        }

        public async Task<Book> FindById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<Book> Details(int id)
        {
            return await _context.Books.Include(gen => gen.Genres).FirstOrDefaultAsync(gen => gen.Id == id);
        }
    }
}
