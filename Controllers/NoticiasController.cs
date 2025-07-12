using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NotiX.Data;
using NotiX.Models;
using NotiX.ViewModels;

namespace NotiX.Controllers
{
    [Authorize]
    // qq tarefa desta classe só pode ser efetuada
    // por pessoas autorizadas (ie. autenticadas)
    // exceto se se criar uma exceção
    public class NoticiasController : Controller
    {
        /// <summary>
        /// referência à BD do projeto
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// objeto que contém os dados referentes ao ambiente 
        /// do Servidor
        /// </summary>
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NoticiasController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: Noticias
        /// <summary>
        /// mostra todos os cursos existentes na BD
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]// esta anotação isenta da obrigação
                        // do utilizador estar autenticado
        public async Task<IActionResult> Index()
        {
			return RedirectToAction("Index", "Home");
		}

        // GET: Noticias/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var noticias = await _context.Noticias
                                    .Include(f => f.ListaFotos)
                                    .Include(c => c.Categoria)
                                    .FirstOrDefaultAsync(m => m.Id == id);
            if (noticias == null)
            {
                return NotFound();
            }

            return View(noticias);
        }

        // GET: Noticias/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["CategoriaFK"] = await _context.Categorias.ToListAsync();
            NoticiasFotosViewMo noti = new();
            return View(noti);
        }

        // POST: Noticias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NoticiasFotosViewMo noticia, IFormFile ListaFotos)
        {


			if (ModelState.IsValid) {
				// Variáveis auxiliares
				string nomeImagem = "";
				bool haImagem = false;
				// Verifica se foi fornecido um ficheiro (obrigatório para uma única foto)
				if (ListaFotos == null) {
					ModelState.AddModelError("", "O fornecimento de uma imagem é obrigatório.");
					ViewData["CategoriaFK"] = await _context.Categorias.ToListAsync();
					return View(noticia);
				}
				else {
					// Verifica se o ficheiro é do tipo PNG ou JPG
					if (!(ListaFotos.ContentType == "image/png" || ListaFotos.ContentType == "image/jpeg")) {
						ModelState.AddModelError("", "A imagem tem de ser do tipo PNG ou JPG.");
						ViewData["CategoriaFK"] = await _context.Categorias.ToListAsync();
						return View(noticia);
					}
					else {
						// Marca que há uma imagem válida
						haImagem = true;
						// Obtém a extensão da imagem
						string extensao = Path.GetExtension(ListaFotos.FileName);
						// Gera nome único usando o Nome do ViewModel + timestamp
						string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

						// Usa o Nome fornecido no ViewModel
						nomeImagem = $"{noticia.Nome}_{timestamp}{extensao}";
					}
				}
				// Adiciona a nova notícia à base de dados
				Noticias n = noticia.Noticias;
				_context.Add(n);
				await _context.SaveChangesAsync(); // Salva para obter o Id da notícia
												   // Cria e adiciona a foto associada à notícia
				if (haImagem) {
					// Cria o objeto Fotos
					Fotos novaFoto = new Fotos {
						Nome = nomeImagem
					};
					// Adiciona a notícia à lista de notícias da foto (relação many-to-many)
					novaFoto.ListaNoticias.Add(n);
					// Adiciona a foto à base de dados
					_context.Add(novaFoto);
					await _context.SaveChangesAsync();
					// Guarda a imagem no disco rígido do servidor
					string localImagem = _webHostEnvironment.WebRootPath;
					localImagem = Path.Combine(localImagem, "Imagens");
					// Verifica se a pasta 'Imagens' existe
					if (!Directory.Exists(localImagem)) {
						Directory.CreateDirectory(localImagem);
					}
					// Caminho completo da imagem
					string caminhoCompleto = Path.Combine(localImagem, nomeImagem);

					// Cria o ficheiro no disco
					using var stream = new FileStream(caminhoCompleto, FileMode.Create);
					await ListaFotos.CopyToAsync(stream);
				}
				// Redireciona o utilizador para a página Index
				return RedirectToAction(nameof(Index));
			}
			// Se há erros de validação, volta à View com os dados fornecidos
			ViewData["CategoriaFK"] = await _context.Categorias.ToListAsync();
			return View(noticia);
		}

		// GET: Noticias/Edit/5
		[HttpGet]
        public async Task<IActionResult> Edit(int? id, List<string> ListaNotFotos)
        {

            ViewData["CategoriaFK"] = await _context.Categorias.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            //lista de noticias
            var noticias = await _context.Noticias
                                            .Include(f => f.ListaFotos)
                                            .Include(f => f.Categoria)
                                            .Where(f => f.Id == id)
                                            .FirstAsync();
            if (noticias == null)
            {
                return NotFound();
            }
            var fotos = await _context.Fotos.Where(f => !noticias.ListaFotos.Select(r => r.Id).Contains(f.Id)).ToListAsync(); // Seleciona fotos que não estão associadas à notícia atual
			TempData["fotos"] = fotos;  
			ViewData["CategoriaFK"] = await _context.Categorias.ToListAsync();
			NoticiasFotosViewMo noti = new() {
				Noticias = noticias // Atribui a notícia carregada ao ViewModel
			};

			return View(noti);
        }

        // POST: Noticias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NoticiasFotosViewMo noticia, IFormFile ListaFotos, List<string> ListaNotFotos)
        {
            ViewData["CategoriaFK"] = await _context.Categorias.ToListAsync();
            // [Bind] - anotação para indicar que dados, vindos da View,
            //          devem ser 'aproveitados'

            //Recebe ficheiro do utilizador
            var Fotos = HttpContext.Request.Form.Files;
            string msgErro = "";
            //vars. auxiliares
            string nomeImagem = "";
            bool haImagem = false;
            Dictionary<Fotos, IFormFile> mapFotos = [];
            //verifica se existe ficheiro
            if (Fotos != null)
            {
                int fotoIndex = 0;

                foreach (var foto in Fotos)
                {
                    if (!(foto.ContentType == "image/png" || foto.ContentType == "image/jpeg"))
                    {
                        msgErro = "A imagem tem de ser do tipo png ou jpeg!";
                        ModelState.AddModelError("Foto", msgErro);

                    }
                    else
                    {
						// Gera nome único usando o nome original + timestamp
						string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
						// Cria o nome da imagem
						nomeImagem = $"{noticia.Nome}_{timestamp}";
                        // obter a extensão do nome do ficheiro
                        string extensao = Path.GetExtension(foto.FileName);
                        nomeImagem += extensao;
                        
                        Fotos f = new(nomeImagem);
                        noticia.Noticias.ListaFotos.Add(f);
                        mapFotos.Add(f, foto);
                        haImagem = true;
                    }
                }
            }

            if (ModelState.IsValid)
            {
                ICollection<Fotos> f = await _context.Fotos.Where(f => ListaNotFotos.Contains(f.Nome)).ToListAsync();
                Noticias n = noticia.Noticias;
                foreach (var g in f)
                {
                    n.ListaFotos.Add(g);
                }
                _context.Update(n);
                //mantém a data de criação original da noticia
                _context.Entry(n).Property(n => n.DataEscrita).IsModified = false;
                // Atribui a data e hora atual ao atributo DataEdicao
                n.DataEdicao = DateTime.Now;
                await _context.SaveChangesAsync();
                // se há ficheiro de imagem,
                // vamos guardar no disco rígido do servidor
                if (haImagem)
                {
                    // determinar onde se vai guardar a imagem
                    string localImagem = _webHostEnvironment.WebRootPath;
                    // já sei o caminho até à pasta wwwroot
                    // especifico onde vou guardar a imagem
                    localImagem = Path.Combine(localImagem, "Imagens");
                    // e, existe a pasta 'Imagens'?
                    if (!Directory.Exists(localImagem))
                    {
                        Directory.CreateDirectory(localImagem);
                    }
                    foreach (KeyValuePair<Fotos, IFormFile> i in mapFotos)
                    {
                        localImagem = Path.Combine(localImagem, i.Key.Nome); // caminho completo da imagem
						using var stream = new FileStream(localImagem, FileMode.Create); // cria o ficheiro no disco
						await i.Value.CopyToAsync(stream); // copia o conteúdo do ficheiro para o disco
						localImagem = _webHostEnvironment.WebRootPath; // volta ao caminho raiz do servidor
						localImagem = Path.Combine(localImagem, "Imagens");
					}
                }
                TempData["atualizado"] = "Noticia " + n.Titulo + " atualizada com sucesso!";
                // redireciona o utilizador para a página Index
                return RedirectToAction(nameof(Edit));
            }
            // se cheguei aqui é pq alguma coisa correu mal
            // volta à View com os dados fornecidos pela Vie
            return View(noticia);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFoto(int fotoId, int noticiaId)
        {
            var foto = await _context.Fotos.FindAsync(fotoId);
            if (foto != null)
            {
                // Caminho da imagem no servidor
                string caminhoImagem = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens", foto.Nome);

                // Remover a imagem da lista de fotos da notícia
                var noticia = await _context.Noticias
                                                .Include(n => n.ListaFotos)
                                                .FirstOrDefaultAsync(n => n.Id == noticiaId);
				if (noticia != null)
                {
                    noticia.ListaFotos.Remove(foto);
                }
                await _context.SaveChangesAsync();
            }

            // Redirecionar para a página de edição da notícia
            return RedirectToAction("Edit", new { id = noticiaId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFoto(int fotoId, int noticiaId)
        {
            var foto = await _context.Fotos.FindAsync(fotoId);
            if (foto != null)
            {
                // Caminho da imagem no servidor
                string caminhoImagem = Path.Combine(_webHostEnvironment.WebRootPath, "Imagens", foto.Nome);

                // Verificar se o arquivo existe
                if (System.IO.File.Exists(caminhoImagem))
                {
                    // Apagar o arquivo
                    System.IO.File.Delete(caminhoImagem);
                }

                // Remover a imagem da base de dados
                _context.Fotos.Remove(foto);
                await _context.SaveChangesAsync();
            }

            // Redirecionar para a página de edição da notícia
            return RedirectToAction("Edit", new { id = noticiaId });
        }


        // GET: Noticias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noticias = await _context.Noticias
                .FirstOrDefaultAsync(m => m.Id == id);
            if (noticias == null)
            {
                return NotFound();
            }

            return View(noticias);
        }

        // POST: Noticias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var noticias = await _context.Noticias.FindAsync(id);
            if (noticias != null)
            {
                _context.Noticias.Remove(noticias);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticiasExists(int id)
        {
            return _context.Noticias.Any(e => e.Id == id);
        }
    }
}