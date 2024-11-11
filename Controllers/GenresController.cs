﻿using Bookstore.Data;
using Bookstore.Models;
using Bookstore.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bookstore.Controllers
{
    public class GenresController : Controller
    {
        private readonly GenreService _service;

        public GenresController(GenreService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.FindAllAsync());
        }

        public IActionResult Create() 
        { 
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            Genre genre = await _service.Details(id);

            if (genre == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _service.Create(genre);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Genre genre, int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            genre.Id = id;
            await _service.Edit(genre);
            return RedirectToAction(nameof(Index));
        }
    }
}
