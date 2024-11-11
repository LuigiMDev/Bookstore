﻿using Bookstore.Data;
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
    }
}
