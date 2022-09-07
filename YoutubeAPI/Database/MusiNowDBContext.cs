using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using YoutubeAPI.Models;

namespace YoutubeAPI.Databse
{
    public partial class MusiNowDBContext : DbContext
    {
        public MusiNowDBContext()
        {
        }

        public MusiNowDBContext(DbContextOptions<MusiNowDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SearchHistory> SearchHistories { get; set; }
        public virtual DbSet<UserDatum> UserData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SearchHistory>(entity =>
            {
                entity.ToTable("SearchHistory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.KeyWord)
                    .HasMaxLength(255)
                    .HasColumnName("keyWord");

                entity.Property(e => e.Repeat).HasColumnName("repeat");
            });

            modelBuilder.Entity<UserDatum>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DataBackUp).HasColumnName("dataBackUp");

                entity.Property(e => e.DeviceBackUp)
                    .HasMaxLength(100)
                    .HasColumnName("deviceBackUp");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.LastBackUp)
                    .HasMaxLength(255)
                    .HasColumnName("lastBackUp");

                entity.Property(e => e.UserName)
                    .HasMaxLength(255)
                    .HasColumnName("userName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
