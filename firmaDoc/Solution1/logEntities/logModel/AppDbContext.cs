﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace logEntities.logModel
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<errorLog> errorLog { get; set; } = null!;
        public virtual DbSet<operation> operation { get; set; } = null!;
        public virtual DbSet<operationLog> operationLog { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DELL-PABLO\\sqlexpress;Database=logDb;persist security info=True;Trusted_Connection=True;user id=sa;password=ade@D47@3a53;multipleactiveresultsets=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<errorLog>(entity =>
            {
                entity.Property(e => e._params).HasColumnName("params");

                entity.Property(e => e.errorDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<operationLog>(entity =>
            {
                entity.Property(e => e.operationDate).HasColumnType("datetime");

                entity.HasOne(d => d.operation)
                    .WithMany(p => p.operationLog)
                    .HasForeignKey(d => d.operationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__operation__opera__286302EC");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
