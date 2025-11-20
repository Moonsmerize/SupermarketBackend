using System;
using System.Collections.Generic;

namespace SupermercadoBackend.Models;

public partial class Usuario
{
    public long IdUsuarios { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public long? IdRoles { get; set; }

    public bool? Activo { get; set; }

    public virtual Role? IdRolesNavigation { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
