using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermercadoBackend.Data;
using SupermercadoBackend.Models;
using Microsoft.AspNetCore.Authorization;

namespace SupermercadoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly SupermarketContext _context;

        public ProductosController(SupermarketContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            return await _context.Productos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(long id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Producto>> PostProducto(Producto producto)
        {
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.IdCategorias == producto.IdCategorias);
            if (!categoriaExiste)
            {
                return BadRequest("El id_categorias especificado no existe. Crea primero una categoría.");
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.IdProductos }, producto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProducto(long id, Producto producto)
        {
            if (id != producto.IdProductos)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo del mensaje");
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProducto(long id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(long id)
        {
            return _context.Productos.Any(e => e.IdProductos == id);
        }
    }
}