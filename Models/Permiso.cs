using System;
using System.Collections.Generic;

namespace SupermercadoBackend.Models;

public partial class Permiso
{
    public long IdPermisos { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Role> IdRoles { get; set; } = new List<Role>();
}
