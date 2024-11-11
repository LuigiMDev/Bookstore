using Bookstore.Data;
using Bookstore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace Bookstore.Services
{
    public class GenreService
    {
        private readonly BookstoreContext _context;

        public GenreService(BookstoreContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> FindAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task Create(Genre genre)
        {
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            Genre genre = await _context.Genres.FirstOrDefaultAsync(gen => gen.Id == id);
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
        }

        public async Task Edit(Genre genreEdited)
        {
            _context.Genres.Update(genreEdited);
            await _context.SaveChangesAsync();
        }

        public async Task<Genre> Details (int id)
        {
            return await _context.Genres.FirstOrDefaultAsync (gen => gen.Id == id);
        }
    }
}
