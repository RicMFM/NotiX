//HomeController

using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

		public async Task<IActionResult> Index(string search, int? categoriaId,int pagina = 1) {

			const int pageSize = 6;

			var query = _context.Noticias
				.Include(n => n.Categoria)
				.Include(n => n.ListaFotos)
				.AsQueryable(); // IQueryable permite construir consultas dinâmicas

			// Filtrar por título
			if (!string.IsNullOrEmpty(search)) {
				query = query.Where(n => n.Titulo.Contains(search));
			}

			// Filtrar por categoria
			if (categoriaId.HasValue && categoriaId.Value != 0) {
				query = query.Where(n => n.CategoriaFK == categoriaId.Value);
			}

			var totalNoticias = await query.CountAsync();

			var noticias = await query 
				.OrderByDescending(n => n.DataEscrita) // Ordenar as notícias pela data de Criação, da mais recente para a mais antiga
				.Skip((pagina - 1) * pageSize) // Saltar as notícias das páginas anteriores (exemplo: página 2 com 6 resultados por página é igual a ter skip(6) que é igual a saltar as primeiras 6 notícias )
				.Take(pageSize) // 
				.ToListAsync();

			var viewModel = noticias.Select(n => new NoticiasFotosViewMo {
				Noticias = n,
				Nome = n.ListaFotos.FirstOrDefault()?.Nome
			}).ToList();

			ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Categoria"); // Lista de categorias para o dropdown
			ViewBag.CategoriaId = categoriaId; // Categoria selecionada
			ViewBag.Search = search; // Termo de pesquisa
			ViewBag.PaginaAtual = pagina; //Página Atual
			ViewBag.TotalPaginas = (int)Math.Ceiling(totalNoticias / (double)pageSize); // Calcular o total de páginas (exemplo: 10 notícias, 6 por página = Math.Ceiling(1,6) = 2 páginas)

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
