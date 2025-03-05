using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PPMarch.Entites;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Materialmovement> Materialmovements { get; set; }

    public virtual DbSet<Materialquality> Materialqualities { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Purchasedetail> Purchasedetails { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<Warehousestock> Warehousestocks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("host=localhost;port=5432;database=postgres;username=postgres;password=36951");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("account_type_enum", new[] { "Админ", "Библиотекарь" })
            .HasPostgresExtension("pgcrypto");

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Materialid).HasName("materials_pkey");

            entity.ToTable("materials");

            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Minstock)
                .HasPrecision(10, 2)
                .HasColumnName("minstock");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasColumnName("unit");
        });

        modelBuilder.Entity<Materialmovement>(entity =>
        {
            entity.HasKey(e => e.Movementid).HasName("materialmovement_pkey");

            entity.ToTable("materialmovement");

            entity.Property(e => e.Movementid).HasColumnName("movementid");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Movementdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("movementdate");
            entity.Property(e => e.Movementtype)
                .HasMaxLength(20)
                .HasColumnName("movementtype");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.Warehouseid).HasColumnName("warehouseid");

            entity.HasOne(d => d.Material).WithMany(p => p.Materialmovements)
                .HasForeignKey(d => d.Materialid)
                .HasConstraintName("materialmovement_materialid_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Materialmovements)
                .HasForeignKey(d => d.Warehouseid)
                .HasConstraintName("materialmovement_warehouseid_fkey");
        });

        modelBuilder.Entity<Materialquality>(entity =>
        {
            entity.HasKey(e => e.Qualitycheckid).HasName("materialquality_pkey");

            entity.ToTable("materialquality");

            entity.Property(e => e.Qualitycheckid).HasColumnName("qualitycheckid");
            entity.Property(e => e.Checkdate).HasColumnName("checkdate");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Result)
                .HasMaxLength(20)
                .HasColumnName("result");

            entity.HasOne(d => d.Material).WithMany(p => p.Materialqualities)
                .HasForeignKey(d => d.Materialid)
                .HasConstraintName("materialquality_materialid_fkey");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Purchaseid).HasName("purchases_pkey");

            entity.ToTable("purchases");

            entity.Property(e => e.Purchaseid).HasColumnName("purchaseid");
            entity.Property(e => e.Purchasedate).HasColumnName("purchasedate");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasColumnName("status");
            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Totalamount)
                .HasPrecision(12, 2)
                .HasColumnName("totalamount");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.Supplierid)
                .HasConstraintName("purchases_supplierid_fkey");
        });

        modelBuilder.Entity<Purchasedetail>(entity =>
        {
            entity.HasKey(e => e.Detailid).HasName("purchasedetails_pkey");

            entity.ToTable("purchasedetails");

            entity.Property(e => e.Detailid).HasColumnName("detailid");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Purchaseid).HasColumnName("purchaseid");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.Unitprice)
                .HasPrecision(10, 2)
                .HasColumnName("unitprice");

            entity.HasOne(d => d.Material).WithMany(p => p.Purchasedetails)
                .HasForeignKey(d => d.Materialid)
                .HasConstraintName("purchasedetails_materialid_fkey");

            entity.HasOne(d => d.Purchase).WithMany(p => p.Purchasedetails)
                .HasForeignKey(d => d.Purchaseid)
                .HasConstraintName("purchasedetails_purchaseid_fkey");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Supplierid).HasName("suppliers_pkey");

            entity.ToTable("suppliers");

            entity.Property(e => e.Supplierid).HasColumnName("supplierid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Contactperson)
                .HasMaxLength(100)
                .HasColumnName("contactperson");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Warehouseid).HasName("warehouse_pkey");

            entity.ToTable("warehouse");

            entity.Property(e => e.Warehouseid).HasColumnName("warehouseid");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Capacity)
                .HasPrecision(10, 2)
                .HasColumnName("capacity");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Warehousestock>(entity =>
        {
            entity.HasKey(e => e.Stockid).HasName("warehousestock_pkey");

            entity.ToTable("warehousestock");

            entity.Property(e => e.Stockid).HasColumnName("stockid");
            entity.Property(e => e.Lastupdated)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdated");
            entity.Property(e => e.Materialid).HasColumnName("materialid");
            entity.Property(e => e.Quantity)
                .HasPrecision(10, 2)
                .HasColumnName("quantity");
            entity.Property(e => e.Warehouseid).HasColumnName("warehouseid");

            entity.HasOne(d => d.Material).WithMany(p => p.Warehousestocks)
                .HasForeignKey(d => d.Materialid)
                .HasConstraintName("warehousestock_materialid_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Warehousestocks)
                .HasForeignKey(d => d.Warehouseid)
                .HasConstraintName("warehousestock_warehouseid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
