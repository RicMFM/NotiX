using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotiX.Data;
using NotiX.Models;
using NuGet.Packaging.Signing;

namespace NotiX.Controllers
{
    [Authorize]
    public class FotosController : Controller
    {
		/// <summary>
		/// referencia a base de dados
		/// </summary>
		private readonly ApplicationDbContext _context;

        /// <summary>
        /// objeto que contém os dados referentes ao ambiente 
        /// do Servidor
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Construtor da classe do Fotoscontroller
        public FotosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Fotos
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Fotos.ToListAsync());
        }

        // GET: Fotos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotos = await _context.Fotos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (fotos == null)
            {
                return NotFound();
            }

            return View(fotos);
        }

        // GET: Fotos/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fotos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome")] Fotos fotos, IFormFile Foto)
        {// [Bind] - anotação para indicar que dados, vindos da View,
		 //          devem ser 'aproveitados'

			// vars. auxiliares
			// Variáveis auxiliares
			string nomeImagem = "";
			bool haImagem = false;

			// Verifica se foi fornecido um ficheiro e caso não tenha sido, adiciona uma mensagem de erro
			if (Foto == null) {
				ModelState.AddModelError("", "O fornecimento de uma imagem é obrigatório.");
				return View(fotos);
			}
			else {
				// Verifica se o ficheiro é do tipo PNG ou JPG e caso não seja , adiciona uma mensagem de erro
				if (!(Foto.ContentType == "image/png" || Foto.ContentType == "image/jpeg")) {
					ModelState.AddModelError("", "A imagem tem de ser do tipo PNG ou JPG.");
					return View(fotos);
				}
				else {
					if (fotos != null) {
						// Marca que há uma imagem válida
						haImagem = true;

                        // Obtem a extensão da imagem
                        string extensao = Path.GetExtension(Foto.FileName);

						// Gera nome único usando o nome original + timestamp
						string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

						// Gera um nome único com base no timestamp de quando a imagem foi carregada
						nomeImagem = $"{fotos.Nome}_{timestamp}_{extensao}";

						// Atribui o nome ao modelo
						fotos.Nome = nomeImagem;
					}
				}
			}

			// Verifica se o modelo é válido
			if (ModelState.IsValid) {
				// Adiciona à base de dados
				_context.Add(fotos);
				await _context.SaveChangesAsync();

				// Se houver imagem válida, guardar imagem no servidor na pasta "Imagens" do arquivo wwwroot
				if (haImagem) {
					string caminhoBase = _webHostEnvironment.WebRootPath;
					string caminhoImagens = Path.Combine(caminhoBase, "Imagens");

					// Verifica se a pasta "Imagens" existe, caso não exista, cria-a
					if (!Directory.Exists(caminhoImagens)) {
						Directory.CreateDirectory(caminhoImagens);
					}

					// Caminho completo da imagem a ser guardada
					string caminhoCompletoImagem = Path.Combine(caminhoImagens, nomeImagem);

					// Guarda a imagem no servidor
					using (var stream = new FileStream(caminhoCompletoImagem, FileMode.Create)) {
						await Foto.CopyToAsync(stream);
					}
				}

				return RedirectToAction(nameof(Index));
			}

			// Se algo falhar, devolve à view original
			return View(fotos);
		}

		// GET: Fotos/Edit/5
		// Mostrar a view para editar uma foto
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fotos = await _context.Fotos.FindAsync(id);
            if (fotos == null)
            {
                return NotFound();
            }
            return View(fotos);
        }

        // POST: Fotos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome")] Fotos fotos)
        {
			// Verifica se o ID da foto corresponde ao ID fornecido
			if (id != fotos.Id)
            {
                return NotFound();
            }
			// Verifica se o modelo é válido
			if (ModelState.IsValid)
            {
                try
                {
					// Atualiza a foto na base de dados
					_context.Update(fotos);
                    await _context.SaveChangesAsync();
                }
				// Se ocorrer uma exceção de concorrência, verifica se a foto ainda existe
				catch (DbUpdateConcurrencyException)
                {
					// Verifica se a foto ainda existe na base de dados
					if (!FotosExists(fotos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(fotos);
        }

        // GET: Fotos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
			// Verifica se o ID é nulo
			if (id == null)
            {
                return NotFound();
            }
			// Busca a foto na base de dados
			var fotos = await _context.Fotos.FirstOrDefaultAsync(m => m.Id == id);
            if (fotos == null)
            {
                return NotFound();
            }

            return View(fotos);
        }

        // POST: Fotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			// Verifica se a foto existe na base de dados
			var fotos = await _context.Fotos.FindAsync(id);
            if (fotos != null)
            {
                // Caminho da imagem no servidor
                string caminhoImagem = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens", fotos.Nome);

                // Verificar se o arquivo existe
                if (System.IO.File.Exists(caminhoImagem))
                {
                    // Apagar o arquivo
                    System.IO.File.Delete(caminhoImagem);
                }

                // Remover a imagem da base de dados
                await _context.SaveChangesAsync();

                _context.Fotos.Remove(fotos);
            }

			// Salvar as alterações na base de dados
			await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		// verifica se a foto existe na base de dados
		private bool FotosExists(int id)
        {
            return _context.Fotos.Any(e => e.Id == id);
        }
    }
}
