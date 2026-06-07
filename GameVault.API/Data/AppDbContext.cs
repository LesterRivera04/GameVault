using GameVault.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GameVault.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }      
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<GamePlatform> GamePlatforms { get; set; }
        public DbSet<GameGenre> GameGenres { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ========================
            // TABLA USUARIO
            // ========================
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.UserName).IsRequired().HasMaxLength(50);
                entity.Property(o => o.Email).IsRequired().HasMaxLength(100);
            });

            // ========================
            // TABLA PLATFORM
            // ========================
            modelBuilder.Entity<Platform>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Name).IsRequired().HasMaxLength(100);
            });

            // ========================
            // TABLA GENRE
            // ========================
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Name).IsRequired().HasMaxLength(100);
            });

            // ========================
            // TABLA GAME
            // ========================
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Name).IsRequired().HasMaxLength(100);
                //entity.Property(o => o.Genre).IsRequired().HasMaxLength(50);
                entity.Property(o => o.Rating).HasDefaultValue(0);
                entity.Property(o => o.ImageUrl).HasMaxLength(500);
                entity.Property(o => o.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(o => o.TimesAdded).HasDefaultValue(0);
            });

            // ========================
            // TABLA USERGAME
            // ========================
            modelBuilder.Entity<UserGame>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.AddedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(o => o.IsFavorite).HasDefaultValue(false);
                entity.HasOne(ug => ug.Usuario)
                      .WithMany(u => u.UserGames)
                      .HasForeignKey(ug => ug.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ug => ug.Game)
                      .WithMany(g => g.UserGames)
                      .HasForeignKey(ug => ug.GameId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ========================
            // TABLA GAMEPLATFORM
            // ========================
            modelBuilder.Entity<GamePlatform>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.GameId).IsRequired();
                entity.Property(o => o.PlatformId).IsRequired();
                entity.HasOne(gp => gp.Game)
                      .WithMany(g => g.GamePlatforms)
                      .HasForeignKey(gp => gp.GameId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(gp => gp.Platform)
                      .WithMany(p => p.GamePlatforms)
                      .HasForeignKey(gp => gp.PlatformId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ========================
            // TABLA GAMEGENRE
            // ========================
            modelBuilder.Entity<GameGenre>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.GameId).IsRequired();
                entity.Property(o => o.GenreId).IsRequired();
                entity.HasOne(o => o.Game)
                      .WithMany(g => g.GameGenres)
                      .HasForeignKey(o => o.GameId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.Genre)
                      .WithMany(g => g.GameGenres)
                      .HasForeignKey(o => o.GenreId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
