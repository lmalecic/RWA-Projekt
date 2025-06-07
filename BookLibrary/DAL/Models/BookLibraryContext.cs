using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class BookLibraryContext : DbContext
{
    public BookLibraryContext()
    {
    }

    public BookLibraryContext(DbContextOptions<BookLibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookLocation> BookLocations { get; set; }

    public virtual DbSet<BookLog> BookLogs { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserReservation> UserReservations { get; set; }

    public virtual DbSet<UserReview> UserReviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:BookLibrary");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Book__3214EC078AD23859");

            entity.ToTable("Book");

            entity.HasIndex(e => e.Isbn, "UQ__Book__447D36EAB34A11D5").IsUnique();

            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Book__GenreId__3A81B327");
        });

        modelBuilder.Entity<BookLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookLoca__3214EC075BF980B7");

            entity.ToTable("BookLocation");

            entity.HasOne(d => d.Book).WithMany(p => p.BookLocations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookLocat__BookI__4222D4EF");

            entity.HasOne(d => d.Location).WithMany(p => p.BookLocations)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookLocat__Locat__4316F928");
        });

        modelBuilder.Entity<BookLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookLog__3214EC0737FAB446");

            entity.ToTable("BookLog");

            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Genre__3214EC0794461B40");

            entity.ToTable("Genre");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC07EFAB2D7C");

            entity.ToTable("Location");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC07CE289E7F");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Phone).HasMaxLength(50);
            entity.Property(e => e.PwdHash).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasDefaultValue("User");
            entity.Property(e => e.Username).HasMaxLength(31);
        });

        modelBuilder.Entity<UserReservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRese__3214EC07BAA9E62B");

            entity.ToTable("UserReservation");

            entity.Property(e => e.Date)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Book).WithMany(p => p.UserReservations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserReser__BookI__48CFD27E");

            entity.HasOne(d => d.User).WithMany(p => p.UserReservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserReser__UserI__47DBAE45");
        });

        modelBuilder.Entity<UserReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRevi__3214EC073A12CBC3");

            entity.ToTable("UserReview");

            entity.HasOne(d => d.Book).WithMany(p => p.UserReviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRevie__BookI__4CA06362");

            entity.HasOne(d => d.User).WithMany(p => p.UserReviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRevie__UserI__4D94879B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
