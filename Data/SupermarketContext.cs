using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SupermercadoBackend.Models;

namespace SupermercadoBackend.Data;

public partial class SupermarketContext : DbContext
{
    public SupermarketContext()
    {
    }

    public SupermarketContext(DbContextOptions<SupermarketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategorias).HasName("categorias_pkey");

            entity.ToTable("categorias");

            entity.Property(e => e.IdCategorias).HasColumnName("id_categorias");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedidos).HasName("detalle_pedidos_pkey");

            entity.ToTable("detalle_pedidos");

            entity.Property(e => e.IdDetallePedidos).HasColumnName("id_detalle_pedidos");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdPedidos).HasColumnName("id_pedidos");
            entity.Property(e => e.IdProductos).HasColumnName("id_productos");
            entity.Property(e => e.PrecioUnitario)
                .HasPrecision(10, 2)
                .HasColumnName("precio_unitario");

            entity.HasOne(d => d.IdPedidosNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedidos)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("detalle_pedidos_id_pedidos_fkey");

            entity.HasOne(d => d.IdProductosNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProductos)
                .HasConstraintName("detalle_pedidos_id_productos_fkey");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedidos).HasName("pedidos_pkey");

            entity.ToTable("pedidos");

            entity.Property(e => e.IdPedidos).HasColumnName("id_pedidos");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasDefaultValueSql("'pendiente'::character varying")
                .HasColumnName("estado");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha");
            entity.Property(e => e.IdUsuarios).HasColumnName("id_usuarios");
            entity.Property(e => e.Total)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("total");

            entity.HasOne(d => d.IdUsuariosNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdUsuarios)
                .HasConstraintName("pedidos_id_usuarios_fkey");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermisos).HasName("permisos_pkey");

            entity.ToTable("permisos");

            entity.HasIndex(e => e.Nombre, "permisos_nombre_key").IsUnique();

            entity.Property(e => e.IdPermisos).HasColumnName("id_permisos");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProductos).HasName("productos_pkey");

            entity.ToTable("productos");

            entity.Property(e => e.IdProductos).HasColumnName("id_productos");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_creacion");
            entity.Property(e => e.IdCategorias).HasColumnName("id_categorias");
            entity.Property(e => e.ImagenUrl)
                .HasMaxLength(255)
                .HasColumnName("imagen_url");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasPrecision(10, 2)
                .HasColumnName("precio");
            entity.Property(e => e.Stock)
                .HasDefaultValue(0)
                .HasColumnName("stock");

            entity.HasOne(d => d.IdCategoriasNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.IdCategorias)
                .HasConstraintName("productos_id_categorias_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRoles).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Nombre, "roles_nombre_key").IsUnique();

            entity.Property(e => e.IdRoles).HasColumnName("id_roles");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");

            entity.HasMany(d => d.IdPermisos).WithMany(p => p.IdRoles)
                .UsingEntity<Dictionary<string, object>>(
                    "RolesPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("IdPermisos")
                        .HasConstraintName("roles_permisos_id_permisos_fkey"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("IdRoles")
                        .HasConstraintName("roles_permisos_id_roles_fkey"),
                    j =>
                    {
                        j.HasKey("IdRoles", "IdPermisos").HasName("roles_permisos_pkey");
                        j.ToTable("roles_permisos");
                        j.IndexerProperty<long>("IdRoles").HasColumnName("id_roles");
                        j.IndexerProperty<long>("IdPermisos").HasColumnName("id_permisos");
                    });
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.HasIndex(e => e.Email, "usuarios_email_key").IsUnique();

            entity.Property(e => e.IdUsuarios).HasColumnName("id_usuarios");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Direccion).HasColumnName("direccion");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.IdRoles).HasColumnName("id_roles");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdRolesNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRoles)
                .HasConstraintName("usuarios_id_roles_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
