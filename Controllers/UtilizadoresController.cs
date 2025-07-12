using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NotiX.Data;
using NotiX.Models;

namespace NotiX.Controllers
{
    [Authorize]
    public class UtilizadoresController : Controller
    {
		/// <summary>
		/// Referencia à base de dados
		/// </summary>
		private readonly ApplicationDbContext _context;

		// Construtor da classe UtilizadoresController
		public UtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Utilizadores
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
			// Obtém a lista de utilizadores da base de dados
			var g = await _context.Utilizadores.ToListAsync();
            return View(g);
        }

        // GET: Utilizadores/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
			// Verifica se o id do utilizador é nulo, se for o caso, retorna NotFound
			if (id == null)
            {
                return NotFound();
            }

			// Obtém os detalhes do utilizador com o id fornecido
			var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);

			// Verifica se o utilizador foi encontrado, se não, retorna NotFound
			if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        // GET: Utilizadores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilizadores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome,ImagemPerf,Email,Contacto,Idade,Tipo,DataInicio")] Utilizadores utilizadores)
        {
            if (ModelState.IsValid)
            {
                //Atribui a data atual do sistema ao atributo DataInicio
                utilizadores.DataInicio = DateTime.Now;
                _context.Add(utilizadores);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilizadores);
        }

        // GET: Utilizadores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
			// Tenta encontrar o utilizador com o id fornecido
			var utilizadores = await _context.Utilizadores.FindAsync(id);

			// Verifica se o utilizador foi encontrado, se não, retorna NotFound
			if (utilizadores == null)
            {
                return NotFound();
            }
            return View(utilizadores);
        }

        // POST: Utilizadores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,ImagemPerf,Email,Contacto,Idade,Tipo,DataInicio")] Utilizadores utilizadores)
        {
            if (id != utilizadores.Id)
            {
                return NotFound();
            }

			// Verifica se o modelo é válido
			if (ModelState.IsValid)
            {
                try
                {
					// Atualiza o utilizador na base de dados
					_context.Update(utilizadores);
                    await _context.SaveChangesAsync();
                }
				// Se ocorrer uma exceção de concorrência, verifica se o utilizador ainda existe
				catch (DbUpdateConcurrencyException)
                {
                    if (!UtilizadoresExists(utilizadores.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
				// Redireciona para a lista de utilizadores após a edição
				return RedirectToAction(nameof(Index));
            }
            return View(utilizadores);
        }

        // GET: Utilizadores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
			// Tenta encontrar o utilizador com o id fornecido na base de dados
			var utilizadores = await _context.Utilizadores
                .FirstOrDefaultAsync(m => m.Id == id);

			// Verifica se o utilizador foi encontrado, se não, retorna NotFound
			if (utilizadores == null)
            {
                return NotFound();
            }

            return View(utilizadores);
        }

        // POST: Utilizadores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
			// Verifica se o utilizador com o id fornecido existe na base de dados
			var utilizadores = await _context.Utilizadores.FindAsync(id);
            if (utilizadores != null)
            {
				// Remove o utilizador da base de dados
				_context.Utilizadores.Remove(utilizadores);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }
    }
}
