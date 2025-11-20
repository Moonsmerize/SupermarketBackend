using System;
using System.Collections.Generic;

namespace SupermercadoBackend.Models;

public partial class Producto
{
    public long IdProductos { get; set; }

    public long? IdCategorias { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public string? ImagenUrl { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Categoria? IdCategoriasNavigation { get; set; }
}
