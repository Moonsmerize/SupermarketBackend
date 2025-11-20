using System;
using System.Collections.Generic;

namespace SupermercadoBackend.Models;

public partial class Role
{
    public long IdRoles { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<Permiso> IdPermisos { get; set; } = new List<Permiso>();
}
