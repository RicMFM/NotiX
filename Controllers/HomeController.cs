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
				.AsQueryable(); // IQueryable permite construir consultas din�micas

			// Filtrar por t�tulo
			if (!string.IsNullOrEmpty(search)) {
				query = query.Where(n => n.Titulo.Contains(search));
			}

			// Filtrar por categoria
			if (categoriaId.HasValue && categoriaId.Value != 0) {
				query = query.Where(n => n.CategoriaFK == categoriaId.Value);
			}

			var totalNoticias = await query.CountAsync();

			var noticias = await query 
				.OrderByDescending(n => n.DataEscrita) // Ordenar as not�cias pela data de Cria��o, da mais recente para a mais antiga
				.Skip((pagina - 1) * pageSize) // Saltar as not�cias das p�ginas anteriores (exemplo: p�gina 2 com 6 resultados por p�gina � igual a ter skip(6) que � igual a saltar as primeiras 6 not�cias )
				.Take(pageSize) // Retorna as not�cias da p�gina atual (exemplo: na p�gina 2 retorna as not�cias 7,8,9,10,11,12)
				.ToListAsync();

			var viewModel = noticias.Select(n => new NoticiasFotosViewMo {
				Noticias = n,
				Nome = n.ListaFotos.FirstOrDefault()?.Nome
			}).ToList();

			ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Categoria"); // Lista de categorias para o dropdown
			ViewBag.CategoriaId = categoriaId; // Categoria selecionada
			ViewBag.Search = search; // Termo de pesquisa
			ViewBag.PaginaAtual = pagina; //P�gina Atual
			ViewBag.TotalPaginas = (int)Math.Ceiling(totalNoticias / (double)pageSize); // Calcular o total de p�ginas (exemplo: 10 not�cias, 6 por p�gina = Math.Ceiling(1,6) = 2 p�ginas)

			return View(viewModel);
		}

		public IActionResult SobreNos() {
			return View();
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
