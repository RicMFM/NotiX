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
		/// <summary>
		/// Objeto que contém os dados referentes às mensagens de log do Servidor
		/// </summary>
		private readonly ILogger<HomeController> _logger;
		
		/// <summary>
		/// referencia a base de dados
		/// </summary>
		private readonly ApplicationDbContext _context;

		// Construtor da classe do HomeController
		public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) {
			_logger = logger;
			_context = context;
		}

		public async Task<IActionResult> Index(string search, int? categoriaId,int pagina = 1) {
			// Definir o tamanho da página
			const int pageSize = 6;

			// Criar a consulta para buscar as notícias
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

			// Contar o total de notícias que correspondem aos filtros aplicados
			var totalNoticias = await query.CountAsync();


			var noticias = await query 
				.OrderByDescending(n => n.DataEscrita) // Ordenar as notícias pela data de Criação, da mais recente para a mais antiga
				.Skip((pagina - 1) * pageSize) // Saltar as notícias das páginas anteriores (exemplo: página 2 com 6 resultados por página é igual a ter skip(6) que é igual a saltar as primeiras 6 notícias )
				.Take(pageSize) // Retorna as notícias da página atual (exemplo: na página 2 retorna as notícias 7,8,9,10,11,12)
				.ToListAsync();

			// Mapear as notícias para o ViewModel
			var viewModel = noticias.Select(n => new NoticiasFotosViewMo {
				Noticias = n,
				Nome = n.ListaFotos.FirstOrDefault()?.Nome
			}).ToList();

			// Criar a ViewBag para passar os dados para a View
			ViewBag.Categorias = new SelectList(_context.Categorias, "Id", "Categoria"); // Lista de categorias para o dropdown
			ViewBag.CategoriaId = categoriaId; // Categoria selecionada
			ViewBag.Search = search; // Termo de pesquisa
			ViewBag.PaginaAtual = pagina; //Página Atual
			ViewBag.TotalPaginas = (int)Math.Ceiling(totalNoticias / (double)pageSize); // Calcular o total de páginas (exemplo: 10 notícias, 6 por página = Math.Ceiling(1,6) = 2 páginas)

			return View(viewModel);
		}

		// retorna a View Sobre Nós
		public IActionResult SobreNos() {
			return View();
		}

		// retorna a View Sobre Nós
		public IActionResult Privacy() {
			return View();
		}
		// 

		// evitar guardar erros em cache
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

		// retorna a View de Erro
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
