//HomeController

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotiX.Data;
using NotiX.Models;
using NotiX.ViewModels;
using System.Diagnostics;

namespace NotiX.Controllers {
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;
		private readonly ApplicationDbContext _context;

		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) {
			_logger = logger;
			_context = context;
		}

		public async Task<IActionResult> Index() {
			var noticias = await _context.Noticias
				.Include(n => n.Categoria)
				.Include(n => n.ListaFotos)
				.OrderByDescending(n => n.DataEscrita)
				.Take(6) // ou o número que quiseres mostrar
				.ToListAsync();

			var viewModel = noticias.Select(n => new NoticiasFotosViewMo {
				Noticias = n,
				Nome = n.ListaFotos.FirstOrDefault()?.Nome
			}).ToList();


			return View(viewModel);
		}

		public IActionResult Privacy() {
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
