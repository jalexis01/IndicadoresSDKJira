using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MQTT.Infrastructure.Models
{
    public partial class EYSIntegrationContext : DbContext
    {
        private readonly string _connectionString;
        public EYSIntegrationContext(string connectionString) : base()
        {
            _connectionString = connectionString;
        }
        public EYSIntegrationContext()
        {
        }

        public EYSIntegrationContext(DbContextOptions<EYSIntegrationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<TbCommands> TbCommands { get; set; }
        public virtual DbSet<TbElementTypes> TbElementTypes { get; set; }
        public virtual DbSet<TbElements> TbElements { get; set; }
        public virtual DbSet<TbEndPoints> TbEndPoints { get; set; }
        public virtual DbSet<TbEquivalenceTypes> TbEquivalenceTypes { get; set; }
        public virtual DbSet<TbEquivalences> TbEquivalences { get; set; }
        public virtual DbSet<TbHeaderFields> TbHeaderFields { get; set; }
        public virtual DbSet<TbHeaderMessage> TbHeaderMessage { get; set; }
        public virtual DbSet<TbLogElements> TbLogElements { get; set; }
        public virtual DbSet<TbLogExecutionProcessor> TbLogExecutionProcessor { get; set; }
        public virtual DbSet<TbLogExecutions> TbLogExecutions { get; set; }
        public virtual DbSet<TbLogHistoryMessageIn> TbLogHistoryMessageIn { get; set; }
        public virtual DbSet<TbLogMessageIn> TbLogMessageIn { get; set; }
        public virtual DbSet<TbLogMessageInSummaryDay> TbLogMessageInSummaryDay { get; set; }
        public virtual DbSet<TbLogProcessedTypes> TbLogProcessedTypes { get; set; }
        public virtual DbSet<TbLogRequestsIn> TbLogRequestsIn { get; set; }
        public virtual DbSet<TbMessageTypeFields> TbMessageTypeFields { get; set; }
        public virtual DbSet<TbMessageTypes> TbMessageTypes { get; set; }
        public virtual DbSet<TbMessages> TbMessages { get; set; }
        public virtual DbSet<TbParameters> TbParameters { get; set; }
        public virtual DbSet<TbSettings> TbSettings { get; set; }
        public virtual DbSet<TbValidFields> TbValidFields { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
            else
            {
            if (!optionsBuilder.IsConfigured)
            {
                    optionsBuilder.UseSqlServer("Server=DESKTOP-LG97MDG\\SQLEXPRESS;Database=EYSIntegration;Trusted_Connection=True;");
            }
        }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<TbCommands>(entity =>
            {
                entity.ToTable("tbCommands", "Operation");

                entity.Property(e => e.CodigoMensaje).HasColumnName("codigoMensaje");

                entity.Property(e => e.CodigoPuerta)
                    .HasColumnName("codigoPuerta")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaHoraEnvioDato)
                    .HasColumnName("fechaHoraEnvioDato")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaHoraLecturaDato)
                    .HasColumnName("fechaHoraLecturaDato")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdEstacion)
                    .HasColumnName("idEstacion")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdOperador)
                    .HasColumnName("idOperador")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdPuerta)
                    .HasColumnName("idPuerta")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdRegistro)
                    .HasColumnName("idRegistro")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdVagon)
                    .HasColumnName("idVagon")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mensaje)
                    .HasColumnName("mensaje")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TipoTrama).HasColumnName("tipoTrama");

                entity.Property(e => e.VersionTrama)
                    .HasColumnName("versionTrama")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdHeaderMessageNavigation)
                    .WithMany(p => p.TbCommands)
                    .HasForeignKey(d => d.IdHeaderMessage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbCommands_tbHeaderMessage");
            });

            modelBuilder.Entity<TbElementTypes>(entity =>
            {
                entity.ToTable("tbElementTypes", "Management");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TbElements>(entity =>
            {
                entity.ToTable("tbElements", "Management");

                entity.HasIndex(e => e.IdElementFather)
                    .HasName("IX_tbElementParents");

                entity.HasIndex(e => e.IdElementType)
                    .HasName("IX_tbElementType");

                entity.HasIndex(e => e.Name)
                    .HasName("IX_tbElementName");

                entity.Property(e => e.Code).HasMaxLength(250);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Enable)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value).HasMaxLength(250);

                entity.HasOne(d => d.IdElementFatherNavigation)
                    .WithMany(p => p.InverseIdElementFatherNavigation)
                    .HasForeignKey(d => d.IdElementFather)
                    .HasConstraintName("FK_tbElements_tbElementsParents");
            });

            modelBuilder.Entity<TbEndPoints>(entity =>
            {
                entity.ToTable("tbEndPoints", "Configuration");

                entity.Property(e => e.AzureLocation).HasMaxLength(500);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(8);
            });

            modelBuilder.Entity<TbEquivalenceTypes>(entity =>
            {
                entity.ToTable("tbEquivalenceTypes", "Configuration");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TbEquivalences>(entity =>
            {
                entity.ToTable("tbEquivalences", "General");

                entity.Property(e => e.CrerationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastUpdate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.IdEquivalenceTypeNavigation)
                    .WithMany(p => p.TbEquivalences)
                    .HasForeignKey(d => d.IdEquivalenceType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbEquivalences_tbEquivalenceTypes");
            });

            modelBuilder.Entity<TbHeaderFields>(entity =>
            {
                entity.ToTable("tbHeaderFields", "Configuration");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.CustomName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.DataType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Enabled)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TbHeaderMessage>(entity =>
            {
                entity.HasKey(e => e.IdHeaderMessage)
                    .HasName("PK_tbMessagesIn");

                entity.ToTable("tbHeaderMessage", "Operation");

                entity.HasIndex(e => e.Idmanatee)
                    .HasName("IX_tbHeaderMessage_Unique")
                    .IsUnique();

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EstadoEnvio).HasColumnName("estadoEnvio");

                entity.Property(e => e.EstadoEnvioManatee).HasColumnName("estadoEnvioManatee");

                entity.Property(e => e.FechaHoraEnvio)
                    .HasColumnName("fechaHoraEnvio")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaPrimerIntento)
                    .HasColumnName("fechaPrimerIntento")
                    .HasColumnType("datetime");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Idmanatee)
                    .HasColumnName("IDManatee")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Trama).HasColumnName("trama");
            });

            modelBuilder.Entity<TbLogElements>(entity =>
            {
                entity.ToTable("tbLogElements", "Log");

                entity.HasIndex(e => e.IdElement)
                    .HasName("IX_tbLogIdElement");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.NewName).HasMaxLength(250);

                entity.Property(e => e.NewValue)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OldName).HasMaxLength(250);

                entity.Property(e => e.OldValue)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdElementNavigation)
                    .WithMany(p => p.TbLogElements)
                    .HasForeignKey(d => d.IdElement)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbLogElements_tbElements");
            });

            modelBuilder.Entity<TbLogExecutionProcessor>(entity =>
            {
                entity.ToTable("tbLogExecutionProcessor", "Log");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.InitDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TbLogExecutions>(entity =>
            {
                entity.ToTable("tbLogExecutions", "Log");

                entity.Property(e => e.EndDateTime).HasColumnType("datetime");

                entity.Property(e => e.InitDateTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TbLogHistoryMessageIn>(entity =>
            {
                entity.ToTable("tbLogHistoryMessageIn", "Log");

                entity.HasIndex(e => e.CreationDate);

                entity.HasIndex(e => e.IdHeaderMessage);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateProcessed).HasColumnType("datetime");

                entity.Property(e => e.IdProcessed).HasDefaultValueSql("((1))");

                entity.Property(e => e.Message).IsRequired();

                entity.HasOne(d => d.IdHeaderMessageNavigation)
                    .WithMany(p => p.TbLogHistoryMessageIn)
                    .HasForeignKey(d => d.IdHeaderMessage)
                    .HasConstraintName("FK_tbLogHistoryMessageIn_tbHeaderMessage");
            });

            modelBuilder.Entity<TbLogMessageIn>(entity =>
            {
                entity.ToTable("tbLogMessageIn", "Log");

                entity.HasIndex(e => e.CreationDate)
                    .HasName("IX_tbLogMessageIn_New_CreationDate");

                entity.HasIndex(e => e.IdHeaderMessage)
                    .HasName("IX_tbLogMessageIn_New_IdHeaderMessage");

                entity.HasIndex(e => e.Processed)
                    .HasName("IX_tbLogMessageIn_New_Processed");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DateProcessed).HasColumnType("datetime");

                entity.Property(e => e.IdProcessed).HasDefaultValueSql("((1))");

                entity.Property(e => e.Message).IsRequired();
            });

            modelBuilder.Entity<TbLogMessageInSummaryDay>(entity =>
            {
                entity.ToTable("tbLogMessageInSummaryDay", "Log");

                entity.HasIndex(e => e.DateDay)
                    .HasName("IX_tbLogMessageInSummaryDayDateDay")
                    .IsUnique();

                entity.Property(e => e.DateDay).HasColumnType("date");
            });

            modelBuilder.Entity<TbLogProcessedTypes>(entity =>
            {
                entity.ToTable("tbLogProcessedTypes", "Log");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<TbLogRequestsIn>(entity =>
            {
                entity.ToTable("tbLogRequestsIn", "Log");

                entity.HasIndex(e => e.CreationDate)
                    .HasName("IX_tbLogRequestsIn_CreationDate_Desc");

                entity.HasIndex(e => e.IdEndPoint);

                entity.HasIndex(e => e.Processed);

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Processed)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdEndPointNavigation)
                    .WithMany(p => p.TbLogRequestsIn)
                    .HasForeignKey(d => d.IdEndPoint)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbLogRequestsIn_tbEndPoints");
            });

            modelBuilder.Entity<TbMessageTypeFields>(entity =>
            {
                entity.ToTable("tbMessageTypeFields", "Configuration");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.CustomName).HasMaxLength(500);

                entity.Property(e => e.Enable)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");

                entity.HasOne(d => d.IdMessageTypeNavigation)
                    .WithMany(p => p.TbMessageTypeFields)
                    .HasForeignKey(d => d.IdMessageType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbMessageFields_tbMessageFields");

                entity.HasOne(d => d.IdValidFieldNavigation)
                    .WithMany(p => p.TbMessageTypeFields)
                    .HasForeignKey(d => d.IdValidField)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbMessageFields_tbValidFields");
            });

            modelBuilder.Entity<TbMessageTypes>(entity =>
            {
                entity.ToTable("tbMessageTypes", "Configuration");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Enable)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FieldCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FieldIdentifierMessage)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FieldWeft)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(117)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<TbMessages>(entity =>
            {
                entity.ToTable("tbMessages", "Operation");

                entity.HasIndex(e => e.CodigoEvento)
                    .HasName("idx_codigoEvento");

                entity.HasIndex(e => e.FechaHoraEnvioDato)
                    .HasName("IX_Operation_tbMessages_fechaHoraEnvioDato");

                entity.HasIndex(e => e.FechaHoraLecturaDato)
                    .HasName("IX_Operation_tbMessages_fechaHoraLecturaDato");

                entity.HasIndex(e => e.IdEstacion)
                    .HasName("idx_estaciones");

                entity.Property(e => e.CiclosApertura).HasColumnName("ciclosApertura");

                entity.Property(e => e.CodigoAlarma)
                    .HasColumnName("codigoAlarma")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoEvento)
                    .HasColumnName("codigoEvento")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoNivelAlarma)
                    .HasColumnName("codigoNivelAlarma")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CodigoPuerta)
                    .HasColumnName("codigoPuerta")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoAperturaCierrePuertas)
                    .HasColumnName("estadoAperturaCierrePuertas")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EstadoBotonManual).HasColumnName("estadoBotonManual");

                entity.Property(e => e.EstadoErrorCritico).HasColumnName("estadoErrorCritico");

                entity.Property(e => e.FechaHoraEnvioDato)
                    .HasColumnName("fechaHoraEnvioDato")
                    .HasColumnType("datetime");

                entity.Property(e => e.FechaHoraLecturaDato)
                    .HasColumnName("fechaHoraLecturaDato")
                    .HasColumnType("datetime");

                entity.Property(e => e.FuerzaMotor).HasColumnName("fuerzaMotor");

                entity.Property(e => e.HorasServicio).HasColumnName("horasServicio");

                entity.Property(e => e.IdEstacion)
                    .HasColumnName("idEstacion")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdOperador)
                    .HasColumnName("idOperador")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdPuerta)
                    .HasColumnName("idPuerta")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdRegistro)
                    .HasColumnName("idRegistro")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdVagon)
                    .HasColumnName("idVagon")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdVehiculo)
                    .HasColumnName("idVehiculo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModoOperacion).HasColumnName("modoOperacion");

                entity.Property(e => e.NombreEstacion)
                    .HasColumnName("nombreEstacion")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NombreVagon)
                    .HasColumnName("nombreVagon")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.NumeroEventoBusEstacion).HasColumnName("numeroEventoBusEstacion");

                entity.Property(e => e.NumeroParada)
                    .HasColumnName("numeroParada")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlacaVehiculo)
                    .HasColumnName("placaVehiculo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PorcentajeCargaBaterias).HasColumnName("porcentajeCargaBaterias");

                entity.Property(e => e.TiempoApertura).HasColumnName("tiempoApertura");

                entity.Property(e => e.TipoEnergizacion).HasColumnName("tipoEnergizacion");

                entity.Property(e => e.TipoTrama).HasColumnName("tipoTrama");

                entity.Property(e => e.TipoTramaBusEstacion)
                    .HasColumnName("tipoTramaBusEstacion")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.TipologiaVehiculo)
                    .HasColumnName("tipologiaVehiculo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TramaRetransmitida)
                    .HasColumnName("tramaRetransmitida")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UsoBotonManual).HasColumnName("usoBotonManual");

                entity.Property(e => e.VelocidaMotor).HasColumnName("velocidaMotor");

                entity.Property(e => e.VersionTrama)
                    .HasColumnName("versionTrama")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdHeaderMessageNavigation)
                    .WithMany(p => p.TbMessages)
                    .HasForeignKey(d => d.IdHeaderMessage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbMessages_tbHeaderMessage");
            });

            modelBuilder.Entity<TbParameters>(entity =>
            {
                entity.ToTable("tbParameters", "Operation");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("value")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TbSettings>(entity =>
            {
                entity.ToTable("tbSettings", "General");

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdParentNavigation)
                    .WithMany(p => p.InverseIdParentNavigation)
                    .HasForeignKey(d => d.IdParent)
                    .HasConstraintName("FK_tbParameters_tbParameters");
            });

            modelBuilder.Entity<TbValidFields>(entity =>
            {
                entity.ToTable("tbValidFields", "Configuration");

                entity.Property(e => e.CreationDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DataType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description).IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
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
}
