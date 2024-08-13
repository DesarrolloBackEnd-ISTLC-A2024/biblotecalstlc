using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bibloteca.Models;
using bibloteca.Context;
using BibliotecaISTLC.DTO;
using Humanizer;

namespace BibliotecaISTLC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorsController : ControllerBase
    {
        private readonly BibliotecaIstlcContext _context;

        public AutorsController(BibliotecaIstlcContext context)
        {
            _context = context;
        }

        // GET: api/Autors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> GetAutors()
        {
            var list = await _context.Autors.ToListAsync();

            return convierteaDTOAutors(list);
        }

        // GET: api/Autors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AutorDTO>> GetAutor(int id)
        {
            var autor = await _context.Autors.FindAsync(id);

            if (autor == null)
            {
                return NotFound();
            }

            var autorDTO = convierteaDTOAutor(autor);
            return autorDTO;
        }

        // PUT: api/Autors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutor(int id, AutorDTO autor)
        {
            Autor result = transformaDTOaAutor(autor);
            if (id != result.IdAutor)
            {
                return BadRequest();
            }

            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorExists(id))
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

        // POST: api/Autors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(AutorDTO autor)
        {
            Autor result = transformaDTOaAutor(autor);
            _context.Autors.Add(result);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AutorExists(autor.IdAutor))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAutor", new { id = autor.IdAutor }, autor);
        }

        // DELETE: api/Autors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id)
        {
            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            _context.Autors.Remove(autor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private ActionResult<IEnumerable<AutorDTO>> convierteaDTOAutors(List<Autor> list)
        {
            List<AutorDTO> result = new List<AutorDTO>();
            for (int i = 0; i < list.Count; i++)
            {
                AutorDTO obj = new AutorDTO();
                var item = list[i];
                obj.IdAutor = item.IdAutor;
                obj.NombreAutor = item.NombreAutor;
                result.Add(obj);
            }
            return result;
        }

        private AutorDTO convierteaDTOAutor(Autor autor)
        {
            AutorDTO obj = new AutorDTO
            {
                IdAutor = autor.IdAutor,
                NombreAutor = autor.NombreAutor
            };
            return obj;
        }

        private Autor transformaDTOaAutor(AutorDTO autor)
        {
            Autor obj = new Autor();
            obj.IdAutor = autor.IdAutor;
            obj.NombreAutor = autor.NombreAutor;
            obj.Estado = "A";

            return obj;
        }

        private bool AutorExists(int id)
        {
            return _context.Autors.Any(e => e.IdAutor == id);
        }
    }
}
