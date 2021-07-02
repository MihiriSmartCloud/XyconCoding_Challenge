using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace XyiconLK.CodingChallenge.Models
{
    public partial class Code_challengeDBContext : DbContext
    {
        public Code_challengeDBContext()
        {
        }

        public Code_challengeDBContext(DbContextOptions<Code_challengeDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblDeveloper> TblDevelopers { get; set; }
        public virtual DbSet<TblForeignTour> TblForeignTours { get; set; }
        public virtual DbSet<TblLog> TblLogs { get; set; }
        public virtual DbSet<TblPersonDetail> TblPersonDetails { get; set; }
        public virtual DbSet<TblUser> TblUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-GCGDOS7H\\SQL2019;Initial Catalog=Code_challengeDB; user id=sa; password=sa123#");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblDeveloper>(entity =>
            {
                entity.HasKey(e => e.DevId);

                entity.ToTable("tblDeveloper");

                entity.Property(e => e.DevId).HasColumnName("dev_Id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");
            });

            modelBuilder.Entity<TblForeignTour>(entity =>
            {
                entity.HasKey(e => e.VisitId);

                entity.ToTable("tblForeignTour");

                entity.Property(e => e.VisitId).HasColumnName("visit_id");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblForeignTours)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblForeignTour_tblUser");
            });

            modelBuilder.Entity<TblLog>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.ToTable("tblLog");

                entity.Property(e => e.LogId).HasColumnName("log_id");

                entity.Property(e => e.EventDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("event_date_time");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("event_type");

                entity.Property(e => e.RecordId).HasColumnName("record_id");

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("table_name");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblLogs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblLog_tblUser");
            });

            modelBuilder.Entity<TblPersonDetail>(entity =>
            {
                entity.ToTable("tblPersonDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Birthdate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("tblUser");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("created_on");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("mobile_no");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("updated_by");

                entity.Property(e => e.UpdatedOn)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_on");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
