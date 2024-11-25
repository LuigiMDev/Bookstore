using Bookstore.Data;
using Bookstore.Models;
using Bookstore.Models.ViewModels;
using Bookstore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Diagnostics;

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
		public async Task<IActionResult> Delete(int? id)
        {
            if(id is null)
            {
                return RedirectToAction(nameof(Error), new { message = "id não fornecido" });
            }
            Genre genre = await _service.FindById(id.Value);
            if (genre is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não foi encontrado" });
			}

            await _service.Delete(id.Value);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {

			if (id is null)
			{
				return RedirectToAction(nameof(Error), new { message = "id não fornecido" });
			}
			Genre genre = await _service.FindById(id.Value);
			if (genre is null)
			{
				return RedirectToAction(nameof(Error), new { message = "Id não foi encontrado" });
			}

			return View(genre);
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

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
				Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
