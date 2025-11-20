using System;
using System.Collections.Generic;

namespace SupermercadoBackend.Models;

public partial class DetallePedido
{
    public long IdDetallePedidos { get; set; }

    public long? IdPedidos { get; set; }

    public long? IdProductos { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public virtual Pedido? IdPedidosNavigation { get; set; }

    public virtual Producto? IdProductosNavigation { get; set; }
}
