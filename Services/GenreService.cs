using Bookstore.Data;
using Bookstore.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Services
{
    public class GenreService
    {
        private readonly BookstoreContext _context;

        public GenreService(BookstoreContext context)
        {
            _context = context;
        }

        public List<Genre> FindAll()
        {
            return _context.Genres.ToList();
        }

        public async Task<List<Genre>> Create(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
             
            return _context.Genres.ToList();
        }

        public async Task<List<Genre>> Delete(int id)
        {
            Genre genero = await _context.Genres.FirstOrDefaultAsync(gen => gen.Id == id);
            _context.Genres.Remove(genero);
            await _context.SaveChangesAsync();

            return _context.Genres.ToList();
        }
    }
}
