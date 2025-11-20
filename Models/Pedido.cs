using System;
using System.Collections.Generic;

namespace SupermercadoBackend.Models;

public partial class Pedido
{
    public long IdPedidos { get; set; }

    public long? IdUsuarios { get; set; }

    public DateTime? Fecha { get; set; }

    public decimal Total { get; set; }

    public string? Estado { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Usuario? IdUsuariosNavigation { get; set; }
}
