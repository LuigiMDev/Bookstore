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

        public BooksController(BookService service)
        {
            _service = service;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await _service.Create(book);

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

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book, int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            book.Id = id;
            await _service.Edit(book);
            return RedirectToAction(nameof(Index));
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
