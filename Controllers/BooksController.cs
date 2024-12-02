using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bookstore.Data;
using Bookstore.Models;
using Bookstore.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Bookstore.Models.ViewModels;
using System.Diagnostics;
using Bookstore.Services.Exceptions;

namespace Bookstore.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookService _service;
        private readonly GenreService _genreService;

        public BooksController(BookService service, GenreService genreService)
        {
            _service = service;
            _genreService = genreService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.FindAllAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return RedirectToAction(nameof(Error), new { message = "id não fornecido" });
            }
            Book book = await _service.Details(id.Value);
            if (book is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não foi encontrado" });
            }

            return View(book);
        }

        public async Task<IActionResult> Create()
        {
            List<Genre> genres = await _genreService.FindAllAsync();

            BookFormViewModel viewModel = new BookFormViewModel { Genres = genres };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = await _genreService.FindAllAsync();
                return View(viewModel);
            }

            viewModel.Book.Genres = new List<Genre>();

            foreach (int genreId in viewModel.SelectedGenresIds)
            {
                Genre genre = await _genreService.FindById(genreId);
                if (genre is not null)
                {
                    viewModel.Book.Genres.Add(genre);
                }
            }

            await _service.Create(viewModel.Book);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id is null)
            {
                return RedirectToAction(nameof(Error), new { message = "id não fornecido" });
            }
            Book book = await _service.FindById(id.Value);
            if (book is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não foi encontrado" });
            }

            List<Genre> genres = await _genreService.FindAllAsync();
            BookFormViewModel viewModel = new BookFormViewModel { Book = book, Genres = genres };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookFormViewModel viewModel, int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (id != viewModel.Book.Id)
            {
                return RedirectToAction(nameof(Error), new { message = "Id's não condizentes" });
            }

            try
            {
                await _service.Edit(viewModel);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return RedirectToAction(nameof(Error), new { message = "id não fornecido" });
            }
            Book book = await _service.FindById(id.Value);
            if (book is null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id não foi encontrado" });
            }

            try
            {
                await _service.Delete(id.Value);

                return RedirectToAction(nameof(Index));

            }
            catch (IntegrityException ex)
            {
                return RedirectToAction(nameof(Error), new { message = ex.Message });
            }

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
