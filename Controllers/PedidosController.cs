using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermercadoBackend.Data;
using SupermercadoBackend.DTOs;
using SupermercadoBackend.Models;
using System.Security.Claims;

namespace SupermercadoBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly SupermarketContext _context;

        public PedidosController(SupermarketContext context)
        {
            _context = context;
        }

        // 1. POST: api/Pedidos
        [HttpPost]
        public async Task<ActionResult> CrearPedido(CrearPedidoDto pedidoDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            long usuarioId = long.Parse(userIdClaim.Value);

            var nuevoPedido = new Pedido
            {
                IdUsuarios = usuarioId,
                Fecha = DateTime.Now,
                Estado = "Pendiente",
                Total = 0
            };

            decimal totalCalculado = 0;

            foreach (var item in pedidoDto.Detalles)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);

                if (producto == null)
                {
                    return BadRequest($"El producto con ID {item.ProductoId} no existe.");
                }

                if (producto.Stock < item.Cantidad)
                {
                    return BadRequest($"No hay suficiente stock para: {producto.Nombre}");
                }

                producto.Stock -= item.Cantidad;

                decimal subtotal = producto.Precio * item.Cantidad;
                totalCalculado += subtotal;

                var detalle = new DetallePedido
                {
                    IdProductos = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.Precio,
                    IdPedidosNavigation = nuevoPedido
                };

                _context.DetallePedidos.Add(detalle);
            }

            nuevoPedido.Total = totalCalculado;

            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync();

            return Ok(new { Mensaje = "Pedido creado exitosamente", IdPedido = nuevoPedido.IdPedidos });
        }

        [HttpGet("mis-pedidos")]
        public async Task<ActionResult> GetMisPedidos()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            long usuarioId = long.Parse(userIdClaim.Value);

            var pedidos = await _context.Pedidos
                .Where(p => p.IdUsuarios == usuarioId) 
                .Include(p => p.DetallePedidos) 
                .ThenInclude(d => d.IdProductosNavigation) 
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();

            return Ok(pedidos);
        }
    }
}