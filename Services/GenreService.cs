using Bookstore.Data;
using Bookstore.Models;
using Bookstore.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using NuGet.Packaging.Signing;

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
            try
            {
				Genre genre = await _context.Genres.FirstOrDefaultAsync(gen => gen.Id == id);
				_context.Genres.Remove(genre);
				await _context.SaveChangesAsync();
			} catch (DbUpdateException ex)
            {
                throw new IntegrityException(ex.Message);
            }
            
        }

        public async Task Edit(Genre genreEdited)
        {
            bool hasAny = await _context.Genres.AnyAsync(x => x.Id == genreEdited.Id);

            if (!hasAny)
            {
                throw new NotFoundException("Id não encontrado");
            }

            try
            {
				_context.Genres.Update(genreEdited);
				await _context.SaveChangesAsync();
			} catch(DbUpdateException ex)
            {
                throw new DbConcorrencyException(ex.Message);
            }
            
        }

        public async Task<Genre> FindById(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<Genre> Details (int id)
        {
            return await _context.Genres.Include(gen => gen.Books).FirstOrDefaultAsync(gen => gen.Id == id);
        }
    }
}
