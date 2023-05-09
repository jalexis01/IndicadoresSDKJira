using System;
using System.Collections.Generic;
using DB.Data.ModelDB;
using Microsoft.EntityFrameworkCore;

namespace DB.Data;

public partial class PuertasTransmilenioDbassaabloyContext : DbContext
{
    string contex;
    public PuertasTransmilenioDbassaabloyContext(string contex)
    {
        this.contex = contex;
    }

    public PuertasTransmilenioDbassaabloyContext(DbContextOptions<PuertasTransmilenioDbassaabloyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<TbCommand> TbCommands { get; set; }

    public virtual DbSet<TbElement> TbElements { get; set; }

    public virtual DbSet<TbElementType> TbElementTypes { get; set; }

    public virtual DbSet<TbEndPoint> TbEndPoints { get; set; }

    public virtual DbSet<TbEquivalence> TbEquivalences { get; set; }

    public virtual DbSet<TbEquivalenceType> TbEquivalenceTypes { get; set; }

    public virtual DbSet<TbHeaderField> TbHeaderFields { get; set; }

    public virtual DbSet<TbHeaderMessage> TbHeaderMessages { get; set; }

    public virtual DbSet<TbLogElement> TbLogElements { get; set; }

    public virtual DbSet<TbLogExecution> TbLogExecutions { get; set; }

    public virtual DbSet<TbLogExecutionProcessor> TbLogExecutionProcessors { get; set; }

    public virtual DbSet<TbLogHistoryMessageIn> TbLogHistoryMessageIns { get; set; }

    public virtual DbSet<TbLogMessageIn> TbLogMessageIns { get; set; }

    public virtual DbSet<TbLogMessageInSummaryDay> TbLogMessageInSummaryDays { get; set; }

    public virtual DbSet<TbLogProcessedType> TbLogProcessedTypes { get; set; }

    public virtual DbSet<TbLogRequestsIn> TbLogRequestsIns { get; set; }

    public virtual DbSet<TbMessage> TbMessages { get; set; }

    public virtual DbSet<TbMessageType> TbMessageTypes { get; set; }

    public virtual DbSet<TbMessageTypeField> TbMessageTypeFields { get; set; }

    public virtual DbSet<TbParameter> TbParameters { get; set; }

    public virtual DbSet<TbSetting> TbSettings { get; set; }

    public virtual DbSet<TbValidField> TbValidFields { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(contex);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<TbCommand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Commands");

            entity.ToTable("tbCommands", "Operation");

            entity.Property(e => e.CodigoMensaje).HasColumnName("codigoMensaje");
            entity.Property(e => e.CodigoPuerta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigoPuerta");
            entity.Property(e => e.FechaHoraEnvioDato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fechaHoraEnvioDato");
            entity.Property(e => e.FechaHoraLecturaDato)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("fechaHoraLecturaDato");
            entity.Property(e => e.IdEstacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idEstacion");
            entity.Property(e => e.IdOperador)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idOperador");
            entity.Property(e => e.IdPuerta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idPuerta");
            entity.Property(e => e.IdRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idRegistro");
            entity.Property(e => e.IdVagon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idVagon");
            entity.Property(e => e.Mensaje)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("mensaje");
            entity.Property(e => e.TipoTrama).HasColumnName("tipoTrama");
            entity.Property(e => e.VersionTrama)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("versionTrama");

            entity.HasOne(d => d.IdHeaderMessageNavigation).WithMany(p => p.TbCommands)
                .HasForeignKey(d => d.IdHeaderMessage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbCommands_tbHeaderMessage");
        });

        modelBuilder.Entity<TbElement>(entity =>
        {
            entity.ToTable("tbElements", "Management");

            entity.HasIndex(e => e.Name, "IX_tbElementName");

            entity.HasIndex(e => e.IdElementFather, "IX_tbElementParents");

            entity.HasIndex(e => e.IdElementType, "IX_tbElementType");

            entity.Property(e => e.Code).HasMaxLength(250);
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Enable)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(250);

            entity.HasOne(d => d.IdElementFatherNavigation).WithMany(p => p.InverseIdElementFatherNavigation)
                .HasForeignKey(d => d.IdElementFather)
                .HasConstraintName("FK_tbElements_tbElementsParents");
        });

        modelBuilder.Entity<TbElementType>(entity =>
        {
            entity.ToTable("tbElementTypes", "Management");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<TbEndPoint>(entity =>
        {
            entity.ToTable("tbEndPoints", "Configuration");

            entity.Property(e => e.AzureLocation).HasMaxLength(500);
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Version).HasMaxLength(8);
        });

        modelBuilder.Entity<TbEquivalence>(entity =>
        {
            entity.ToTable("tbEquivalences", "General");

            entity.Property(e => e.CrerationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LastUpdate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Value).HasMaxLength(250);

            entity.HasOne(d => d.IdEquivalenceTypeNavigation).WithMany(p => p.TbEquivalences)
                .HasForeignKey(d => d.IdEquivalenceType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbEquivalences_tbEquivalenceTypes");
        });

        modelBuilder.Entity<TbEquivalenceType>(entity =>
        {
            entity.ToTable("tbEquivalenceTypes", "Configuration");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<TbHeaderField>(entity =>
        {
            entity.ToTable("tbHeaderFields", "Configuration");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.CustomName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Enabled)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbHeaderMessage>(entity =>
        {
            entity.HasKey(e => e.IdHeaderMessage).HasName("PK_tbMessagesIn");

            entity.ToTable("tbHeaderMessage", "Operation");

            entity.HasIndex(e => e.Idmanatee, "IX_tbHeaderMessage_Unique").IsUnique();

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EstadoEnvio).HasColumnName("estadoEnvio");
            entity.Property(e => e.EstadoEnvioManatee).HasColumnName("estadoEnvioManatee");
            entity.Property(e => e.FechaHoraEnvio)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraEnvio");
            entity.Property(e => e.FechaPrimerIntento)
                .HasColumnType("datetime")
                .HasColumnName("fechaPrimerIntento");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Idmanatee)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDManatee");
            entity.Property(e => e.Trama).HasColumnName("trama");
        });

        modelBuilder.Entity<TbLogElement>(entity =>
        {
            entity.ToTable("tbLogElements", "Log");

            entity.HasIndex(e => e.IdElement, "IX_tbLogIdElement");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.NewName).HasMaxLength(250);
            entity.Property(e => e.NewValue)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.OldName).HasMaxLength(250);
            entity.Property(e => e.OldValue)
                .HasMaxLength(250)
                .IsUnicode(false);

            entity.HasOne(d => d.IdElementNavigation).WithMany(p => p.TbLogElements)
                .HasForeignKey(d => d.IdElement)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbLogElements_tbElements");
        });

        modelBuilder.Entity<TbLogExecution>(entity =>
        {
            entity.ToTable("tbLogExecutions", "Log");

            entity.Property(e => e.EndDateTime).HasColumnType("datetime");
            entity.Property(e => e.InitDateTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbLogExecutionProcessor>(entity =>
        {
            entity.ToTable("tbLogExecutionProcessor", "Log");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.InitDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbLogHistoryMessageIn>(entity =>
        {
            entity.ToTable("tbLogHistoryMessageIn", "Log");

            entity.HasIndex(e => e.CreationDate, "IX_tbLogHistoryMessageIn_CreationDate").IsDescending();

            entity.HasIndex(e => e.IdHeaderMessage, "IX_tbLogHistoryMessageIn_IdHeaderMessage");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DateProcessed).HasColumnType("datetime");
            entity.Property(e => e.IdProcessed).HasDefaultValueSql("((1))");

            entity.HasOne(d => d.IdHeaderMessageNavigation).WithMany(p => p.TbLogHistoryMessageIns)
                .HasForeignKey(d => d.IdHeaderMessage)
                .HasConstraintName("FK_tbLogHistoryMessageIn_tbHeaderMessage");
        });

        modelBuilder.Entity<TbLogMessageIn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbLogMessageIn_New");

            entity.ToTable("tbLogMessageIn", "Log");

            entity.HasIndex(e => e.CreationDate, "IX_tbLogMessageIn_New_CreationDate").IsDescending();

            entity.HasIndex(e => e.IdHeaderMessage, "IX_tbLogMessageIn_New_IdHeaderMessage");

            entity.HasIndex(e => e.Processed, "IX_tbLogMessageIn_New_Processed");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DateProcessed).HasColumnType("datetime");
            entity.Property(e => e.IdProcessed).HasDefaultValueSql("((1))");
        });

        modelBuilder.Entity<TbLogMessageInSummaryDay>(entity =>
        {
            entity.ToTable("tbLogMessageInSummaryDay", "Log");

            entity.HasIndex(e => e.DateDay, "IX_tbLogMessageInSummaryDayDateDay")
                .IsUnique()
                .IsDescending();

            entity.Property(e => e.DateDay).HasColumnType("date");
        });

        modelBuilder.Entity<TbLogProcessedType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbParameters_1");

            entity.ToTable("tbLogProcessedTypes", "Log");

            entity.Property(e => e.Name).HasMaxLength(250);
        });

        modelBuilder.Entity<TbLogRequestsIn>(entity =>
        {
            entity.ToTable("tbLogRequestsIn", "Log");

            entity.HasIndex(e => e.CreationDate, "IX_tbLogRequestsIn_CreationDate_Desc").IsDescending();

            entity.HasIndex(e => e.IdEndPoint, "IX_tbLogRequestsIn_IdEndPoint");

            entity.HasIndex(e => e.Processed, "IX_tbLogRequestsIn_Processed");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Processed)
                .IsRequired()
                .HasDefaultValueSql("((1))");

            entity.HasOne(d => d.IdEndPointNavigation).WithMany(p => p.TbLogRequestsIns)
                .HasForeignKey(d => d.IdEndPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbLogRequestsIn_tbEndPoints");
        });

        modelBuilder.Entity<TbMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Messages");

            entity.ToTable("tbMessages", "Operation");

            entity.HasIndex(e => e.FechaHoraEnvioDato, "IX_Operation_tbMessages_fechaHoraEnvioDato");

            entity.HasIndex(e => e.FechaHoraLecturaDato, "IX_Operation_tbMessages_fechaHoraLecturaDato");

            entity.Property(e => e.CiclosApertura).HasColumnName("ciclosApertura");
            entity.Property(e => e.CodigoAlarma)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigoAlarma");
            entity.Property(e => e.CodigoEvento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigoEvento");
            entity.Property(e => e.CodigoNivelAlarma)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigoNivelAlarma");
            entity.Property(e => e.CodigoPuerta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("codigoPuerta");
            entity.Property(e => e.EstadoAperturaCierrePuertas)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("estadoAperturaCierrePuertas");
            entity.Property(e => e.EstadoBotonManual).HasColumnName("estadoBotonManual");
            entity.Property(e => e.EstadoErrorCritico).HasColumnName("estadoErrorCritico");
            entity.Property(e => e.FechaHoraEnvioDato)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraEnvioDato");
            entity.Property(e => e.FechaHoraLecturaDato)
                .HasColumnType("datetime")
                .HasColumnName("fechaHoraLecturaDato");
            entity.Property(e => e.FuerzaMotor).HasColumnName("fuerzaMotor");
            entity.Property(e => e.HorasServicio).HasColumnName("horasServicio");
            entity.Property(e => e.IdEstacion)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idEstacion");
            entity.Property(e => e.IdOperador)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idOperador");
            entity.Property(e => e.IdPuerta)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idPuerta");
            entity.Property(e => e.IdRegistro)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idRegistro");
            entity.Property(e => e.IdVagon)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idVagon");
            entity.Property(e => e.IdVehiculo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("idVehiculo");
            entity.Property(e => e.ModoOperacion).HasColumnName("modoOperacion");
            entity.Property(e => e.NombreEstacion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombreEstacion");
            entity.Property(e => e.NombreVagon)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("nombreVagon");
            entity.Property(e => e.NumeroEventoBusEstacion).HasColumnName("numeroEventoBusEstacion");
            entity.Property(e => e.NumeroParada)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("numeroParada");
            entity.Property(e => e.PlacaVehiculo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("placaVehiculo");
            entity.Property(e => e.PorcentajeCargaBaterias).HasColumnName("porcentajeCargaBaterias");
            entity.Property(e => e.TiempoApertura).HasColumnName("tiempoApertura");
            entity.Property(e => e.TipoEnergizacion).HasColumnName("tipoEnergizacion");
            entity.Property(e => e.TipoTrama).HasColumnName("tipoTrama");
            entity.Property(e => e.TipoTramaBusEstacion)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("tipoTramaBusEstacion");
            entity.Property(e => e.TipologiaVehiculo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipologiaVehiculo");
            entity.Property(e => e.TramaRetransmitida)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tramaRetransmitida");
            entity.Property(e => e.UsoBotonManual).HasColumnName("usoBotonManual");
            entity.Property(e => e.VelocidaMotor).HasColumnName("velocidaMotor");
            entity.Property(e => e.VersionTrama)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("versionTrama");

            entity.HasOne(d => d.IdHeaderMessageNavigation).WithMany(p => p.TbMessages)
                .HasForeignKey(d => d.IdHeaderMessage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbMessages_tbHeaderMessage");
        });

        modelBuilder.Entity<TbMessageType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbMessages");

            entity.ToTable("tbMessageTypes", "Configuration");

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Enable)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.FieldCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FieldIdentifierMessage)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.FieldWeft)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(117)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<TbMessageTypeField>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbMessageFields");

            entity.ToTable("tbMessageTypeFields", "Configuration");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.CustomName).HasMaxLength(500);
            entity.Property(e => e.Enable)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.IdMessageTypeNavigation).WithMany(p => p.TbMessageTypeFields)
                .HasForeignKey(d => d.IdMessageType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbMessageFields_tbMessageFields");

            entity.HasOne(d => d.IdValidFieldNavigation).WithMany(p => p.TbMessageTypeFields)
                .HasForeignKey(d => d.IdValidField)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbMessageFields_tbValidFields");
        });

        modelBuilder.Entity<TbParameter>(entity =>
        {
            entity.ToTable("tbParameters", "Operation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Value)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("value");
        });

        modelBuilder.Entity<TbSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_tbParameters_2");

            entity.ToTable("tbSettings", "General");

            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Value)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.IdParentNavigation).WithMany(p => p.InverseIdParentNavigation)
                .HasForeignKey(d => d.IdParent)
                .HasConstraintName("FK_tbParameters_tbParameters");
        });

        modelBuilder.Entity<TbValidField>(entity =>
        {
            entity.ToTable("tbValidFields", "Configuration");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DataType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.PrimaryData)
                .IsRequired()
                .HasDefaultValueSql("((1))");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
