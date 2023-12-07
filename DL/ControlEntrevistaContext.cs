using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DL;

public partial class ControlEntrevistaContext : DbContext
{
    //No quitar
    private readonly IConfiguration _configuration;

    public ControlEntrevistaContext()
    {
    }

    public ControlEntrevistaContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public ControlEntrevistaContext(DbContextOptions<ControlEntrevistaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidato> Candidatos { get; set; }

    public virtual DbSet<CitaAsistencium> CitaAsistencia { get; set; }

    public virtual DbSet<CitaEmpresaCnf> CitaEmpresaCnfs { get; set; }

    public virtual DbSet<Citum> Cita { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Reclutador> Reclutadors { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Vacante> Vacantes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(_configuration["ConnectionStrings:Dev"]);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidato>(entity =>
        {
            entity.HasKey(e => e.IdCandidato).HasName("PK__Candidat__D5598905F504ED42");

            entity.ToTable("Candidato");

            entity.HasIndex(e => e.Correo, "UQ__Candidat__60695A19AAE42B92").IsUnique();

            entity.Property(e => e.IdCandidato)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Celular)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdVacanteNavigation).WithMany(p => p.Candidatos)
                .HasForeignKey(d => d.IdVacante)
                .HasConstraintName("FK__Candidato__IdVac__182C9B23");
        });

        modelBuilder.Entity<CitaAsistencium>(entity =>
        {
            entity.HasKey(e => e.IdCitaAsistencia).HasName("PK__CitaAsis__69FBFA61FD06199E");

            entity.Property(e => e.IdCitaAsistencia).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.Observaciones).IsUnicode(false);

            entity.HasOne(d => d.IdCitaNavigation).WithMany(p => p.CitaAsistencia)
                .HasForeignKey(d => d.IdCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CitaAsist__IdCit__21B6055D");
        });

        modelBuilder.Entity<CitaEmpresaCnf>(entity =>
        {
            entity.HasKey(e => e.IdCitaEmpresaCnf).HasName("PK__CitaEmpr__FDF7F117D2F5E63A");

            entity.ToTable("CitaEmpresaCNF");

            entity.Property(e => e.IdCitaEmpresaCnf)
                .ValueGeneratedNever()
                .HasColumnName("IdCitaEmpresaCNF");
        });

        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Cita__394B0202AB1B9DA3");

            entity.Property(e => e.IdCita).ValueGeneratedNever();
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdCandidato)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.NombreCandidato)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreReclutador)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdCandidatoNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdCandidato)
                .HasConstraintName("FK__Cita__IdCandidat__1CF15040");

            entity.HasOne(d => d.IdReclutadorNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdReclutador)
                .HasConstraintName("FK__Cita__IdReclutad__1DE57479");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("FK__Cita__IdStatus__1ED998B2");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.IdEmpresa).HasName("PK__Empresa__5EF4033E54B31DA9");

            entity.ToTable("Empresa");

            entity.Property(e => e.IdEmpresa).ValueGeneratedNever();
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Reclutador>(entity =>
        {
            entity.HasKey(e => e.IdReclutador).HasName("PK__Reclutad__D05E21DE7A85A33B");

            entity.ToTable("Reclutador");

            entity.Property(e => e.IdReclutador).ValueGeneratedNever();
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PK__Status__B450643AA729FC7D");

            entity.ToTable("Status");

            entity.Property(e => e.IdStatus).ValueGeneratedNever();
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Vacante>(entity =>
        {
            entity.HasKey(e => e.IdVacante).HasName("PK__Vacante__A58A8FA88E7DF563");

            entity.ToTable("Vacante");

            entity.Property(e => e.IdVacante).ValueGeneratedNever();
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Vacantes)
                .HasForeignKey(d => d.IdEmpresa)
                .HasConstraintName("FK__Vacante__IdEmpre__1273C1CD");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
