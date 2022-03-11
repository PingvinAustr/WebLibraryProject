using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CoreLibraryProj
{
    public partial class CoreLibraryContext : DbContext
    {
        public CoreLibraryContext()
        {
        }

        public CoreLibraryContext(DbContextOptions<CoreLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<DocumentFullText> DocumentFullTexts { get; set; } = null!;
        public virtual DbSet<Language> Languages { get; set; } = null!;
        public virtual DbSet<Rubric> Rubrics { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-77MT8LVK; Database=CoreLibrary; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(e => e.AuthorBirthDate).HasColumnType("datetime");

                entity.Property(e => e.AuthorName).HasMaxLength(60);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.BookDescription).HasMaxLength(1000);

                entity.Property(e => e.BookName).HasMaxLength(60);

                entity.HasOne(d => d.BookAuthor)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.BookAuthorId)
                    .HasConstraintName("FK__Books__BookAutho__31EC6D26");

                entity.HasOne(d => d.BookRubric)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.BookRubricId)
                    .HasConstraintName("FK__Books__BookRubri__300424B4");
            });

            modelBuilder.Entity<DocumentFullText>(entity =>
            {
                entity.Property(e => e.FullDocumentText).HasColumnType("text");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.DocumentFullTexts)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK__DocumentF__Docum__6B24EA82");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.DocumentFullTexts)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("FK__DocumentF__Langu__6A30C649");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.Property(e => e.LanguageName).HasMaxLength(60);
            });

            modelBuilder.Entity<Rubric>(entity =>
            {
                entity.Property(e => e.RubricName).HasMaxLength(60);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
