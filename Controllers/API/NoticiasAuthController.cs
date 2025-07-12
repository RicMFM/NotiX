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
    public class NoticiasAuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NoticiasAuthController(ApplicationDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        // GET: api/Noticias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoticiaComFotosDTO>>> GetNoticias()
        {
            var resultado = await _context.Noticias
                                               .Select(n => new NoticiaComFotosDTO
                                               {
                                                   Id = n.Id,
                                                   Titulo = n.Titulo,
                                                   Texto = n.Texto,
                                                   DataEscrita = n.DataEscrita,
                                                   DataEdicao = n.DataEdicao,
                                                   ListaFotos = n.ListaFotos
                                                                   .Select(f => new FotoDTO
                                                                   {
                                                                       Id = f.Id,
                                                                       Nome = f.Nome
                                                                   })
                                                                   .ToList()
                                               })
                                               .ToListAsync();

            return Ok(resultado);
        }

        // GET: api/Noticias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NoticiaComFotosDTO>> GetNoticias(int id)
        {
            var noticias = await _context.Noticias
                                                .Where(n => n.Id == id)
                                                .Select(n => new NoticiaComFotosDTO
                                                {
                                                    Id = n.Id,
                                                    Titulo = n.Titulo,
                                                    Texto = n.Texto,
                                                    DataEscrita = n.DataEscrita,
                                                    DataEdicao = n.DataEdicao,
                                                    ListaFotos = n.ListaFotos
                                                                    .Select(f => new FotoDTO
                                                                    {
                                                                        Id = f.Id,
                                                                        Nome = f.Nome
                                                                    })
                                                                    .ToList()
                                                })
                                                .FirstOrDefaultAsync();

            if (noticias == null)
            {
                return NotFound();
            }

            return noticias;
        }

        // PUT: api/Noticias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNoticias(int id, Noticias noticias)
        {
            if (id != noticias.Id)
            {
                return BadRequest();
            }

            _context.Entry(noticias).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoticiasExists(id))
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

        // POST: api/Noticias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Noticias>> PostNoticias(Noticias noticias)
        {
            _context.Noticias.Add(noticias);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNoticias", new { id = noticias.Id }, noticias);
        }

        // DELETE: api/Noticias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNoticias(int id)
        {
            var noticias = await _context.Noticias
                              .Where(n => n.Id == id)
                              .FirstOrDefaultAsync();
            if (noticias == null)
            {
                return NotFound();
            }

            _context.Noticias.Remove(noticias);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoticiasExists(int id)
        {
            return _context.Noticias.Any(e => e.Id == id);
        }
    }
}
