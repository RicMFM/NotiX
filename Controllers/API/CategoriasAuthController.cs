using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NotiX.Data;
using NotiX.Models;
using NotiX.ViewModels;

namespace NotiX.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CategoriasAuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriasAuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        /// <summary>
        /// devolve a lista com todas as Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categorias>>> GetCategorias()
        {
            var resultado = await _context.Categorias
                                    .Select(c => new CategoriaComNoticiasDTO {
                                        Id = c.Id,
                                        Categoria = c.Categoria,
                                        Noticias = c.ListaNoticias
                                                    .Select(n => new NoticiaDTO
                                                        {
                                                            Id = n.Id,
                                                            Titulo = n.Titulo,
                                                            Texto = n.Texto,
                                                            DataEscrita = n.DataEscrita
                                                         }).ToList()
                                    })
                                    .ToListAsync();

            return Ok(resultado);
        }

        /// <summary>
        /// GET: devolver uma Categorias, 
        /// quando a solicitação é feita através de HTTP GET
        /// </summary>
        /// <param name="id">identificador da categoria pretendida</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriasDTO>> GetCategoria(int id) {
            var categoria = await _context.Categorias
                                    .Where(c => c.Id == id)
                                    .Select(c=> new CategoriasDTO {
                                        Categoria = c.Categoria,
                                    }).FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        // PUT: api/Categorias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Edição de uma Categoria
        /// </summary>
        /// <param name="id">idemtificação da Categoria a editar</param>
        /// <param name="categoria">Novos dados da Categoria</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategorias(int id, Categorias categorias)
        {
            if (id != categorias.Id)
            {
                return BadRequest();
            }

            _context.Entry(categorias).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Adição de uma Categoria
        /// </summary>
        /// <param name="categoria">dados da Categoria a adicionar</param>
        /// <returns></returns>      
        [HttpPost]
        public async Task<ActionResult<Categorias>> PostCategorias(Categorias categorias)
        {
            _context.Categorias.Add(categorias);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategorias", new { id = categorias.Id }, categorias);
        }

        // DELETE: api/Categorias/5
        /// <summary>
        /// Apagar uma Categoria
        /// </summary>
        /// <param name="id">identificador da Categoria a apagar</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategorias(int id)
        {
            var categoria = await _context.Categorias
                              .Where(c => c.Id == id)
                              .FirstOrDefaultAsync();

            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Determina se a Categoria existe
        /// </summary>
        /// <param name="id">identificador da Categoria a procurar</param>
        /// <returns>'true', se a categoria existe, senão 'false'</returns>
        private bool CategoriasExists(int id)
        {
            return _context.Categorias.Any(e => e.Id == id);
        }
    }
}
