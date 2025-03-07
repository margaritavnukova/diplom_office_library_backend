using office_library_backend.Models;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

//protected override void OnModelCreating(DbModelBuilder modelBuilder)
//{
//    modelBuilder.Entity<Book>()
//        .HasRequired<Genre_Dictionary>(b => b.Genre_Dictionary)
//        .WithMany(g => g.Book)
//        .HasForeignKey(b => b.Genre);

//    modelBuilder.Entity<Book>()
//        .HasRequired<Author>(b => b.Author1)
//        .WithMany(a => a.Book)
//        .HasForeignKey(b => b.Author);

//    modelBuilder.Entity<UserBookHistory>()
//    .HasKey(c => new { c.BookId, c.UserId }); // Композитный ключ

//    modelBuilder.Entity<UserBookHistory>()
//        .HasRequired<Book>(c => c.Book)
//        .WithMany(b => b.UserBookHistory)
//        .HasForeignKey(c => c.BookId)
//        .WillCascadeOnDelete(false);

//    modelBuilder.Entity<UserBookHistory>()
//        .HasRequired<AspNetUsers>(c => c.AspNetUsers)
//        .WithMany(r => r.UserBookHistory)
//        .HasForeignKey(c => c.UserId)
//        .WillCascadeOnDelete(false);

//    modelBuilder.Entity<AspNetUsers>()
//        .HasMany<AspNetRoles>(u => u.AspNetRoles)
//        .WithMany(r => r.AspNetUsers);

//    modelBuilder.Entity<AspNetRoles>()
//        .HasMany<AspNetUsers>(r => r.AspNetUsers)
//        .WithMany(u => u.AspNetRoles);

//    throw new UnintentionalCodeFirstException();
//}