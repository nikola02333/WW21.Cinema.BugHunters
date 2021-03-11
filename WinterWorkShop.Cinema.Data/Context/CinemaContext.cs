using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Data
{
    public class CinemaContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Projection> Projections { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Auditorium> Auditoriums { get; set; }
        public DbSet<Seat> Seats { get; set; }


        public DbSet<Ticket> Tickets { get; set; }
        public CinemaContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// <summary>
            /// Seat -> Auditorium relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Seat>()
                .HasOne(x => x.Auditorium)
                .WithMany(x => x.Seats)
                .HasForeignKey(x => x.AuditoriumId)
                .IsRequired();

            /// <summary>
            /// Auditorium -> Seat relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Auditorium>()
                .HasMany(x => x.Seats)
                .WithOne(x => x.Auditorium)
                .IsRequired();


            /// <summary>
            /// Cinema -> Auditorium relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Cinema>()
                .HasMany(x => x.Auditoriums)
                .WithOne(x => x.Cinema)
                .IsRequired();
            
            /// <summary>
            /// Auditorium -> Cinema relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Auditorium>()
                .HasOne(x => x.Cinema)
                .WithMany(x => x.Auditoriums)
                .HasForeignKey(x => x.CinemaId)
                .IsRequired();


            /// <summary>
            /// Auditorium -> Projection relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Auditorium>()               
               .HasMany(x => x.Projections)
               .WithOne(x => x.Auditorium)
               .IsRequired();

            /// <summary>
            /// Projection -> Auditorium relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Projection>()
                .HasOne(x => x.Auditorium)
                .WithMany(x => x.Projections)
                .HasForeignKey(x => x.AuditoriumId)
                .IsRequired();


            /// <summary>
            /// Projection -> Movie relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Projection>()
                .HasOne(x => x.Movie)
                .WithMany(x => x.Projections)
                .HasForeignKey(x => x.MovieId)
                .IsRequired();

            /// <summary>
            /// Movie -> Projection relation
            /// </summary>
            /// <returns></returns>
            modelBuilder.Entity<Movie>()
                .HasMany(x => x.Projections)
                .WithOne(x => x.Movie)
                .IsRequired();


            /*---*/
          

            modelBuilder.Entity<Ticket>()
                .HasOne(a => a.Seat)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.SeatId)
                .IsRequired();

            modelBuilder.Entity<Ticket>()
                .HasOne(a => a.User)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.UserId)
                .IsRequired();


            modelBuilder.Entity<Ticket>()
              .HasOne(a => a.Projection)
              .WithMany(t => t.Tickets)
              .HasForeignKey(t => t.ProjectionId)
              .IsRequired();


            /*---*/

            modelBuilder.Entity<Seat>()
                 .HasMany(t => t.Tickets)
                 .WithOne(a => a.Seat);

            modelBuilder.Entity<Projection>()
               .HasMany(t => t.Tickets)
               .WithOne(a => a.Projection);

            modelBuilder.Entity<User>()
                .HasMany(t => t.Tickets)
                .WithOne(a => a.User);

          

            // Index
            modelBuilder.Entity<Movie>().HasIndex(i => new { i.Year, i.HasOscar });
            modelBuilder.Entity<Movie>().HasIndex(i => i.Rating);

            modelBuilder.Entity<User>().HasIndex(i => i.UserName);
        }
    }
}
