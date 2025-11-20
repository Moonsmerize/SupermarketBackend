using System;
using System.Collections.Generic;

namespace SupermercadoBackend.Models;

public partial class Categoria
{
    public long IdCategorias { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
