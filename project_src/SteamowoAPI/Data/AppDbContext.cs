using Microsoft.EntityFrameworkCore;
using SteamowoAPI.Models;

namespace SteamowoAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<UserGame> UserGames => Set<UserGame>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // precyzja dla wartosci pieniezych (18 cyfr, 2 po przecinku) - usuwa warning EF Core
            modelBuilder.Entity<Game>().Property(g => g.Cena).HasPrecision(18, 2);
            modelBuilder.Entity<User>().Property(u => u.SaldoPortfela).HasPrecision(18, 2);
            modelBuilder.Entity<UserGame>().Property(ug => ug.CenaZakupu).HasPrecision(18, 2);
            modelBuilder.Entity<Transaction>().Property(t => t.Kwota).HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Game)
                .WithMany()
                .HasForeignKey(t => t.GameId)
                .OnDelete(DeleteBehavior.SetNull);

            // login i email maja byc unikalne
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // jeden uzytkownik nie moze kupic tej samej gry dwa razy
            modelBuilder.Entity<UserGame>()
                .HasIndex(ug => new { ug.UserId, ug.GameId })
                .IsUnique();

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.User)
                .WithMany(u => u.PosiadaneGry)
                .HasForeignKey(ug => ug.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserGame>()
                .HasOne(ug => ug.Game)
                .WithMany(g => g.Wlasciciele)
                .HasForeignKey(ug => ug.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // troche danych startowych, zeby sklep nie byl pusty
            modelBuilder.Entity<Game>().HasData(
                new Game { Id = 1, Tytul = "Wiedzmin 3: Dziki Gon", Opis = "Klasyk RPG od CD Projekt Red.", Producent = "CD Projekt Red", Gatunek = "RPG", Cena = 49.99m, DataWydania = new DateTime(2015, 5, 19), UrlOkladki = "https://picsum.photos/seed/steamowo1/300/400" },
                new Game { Id = 2, Tytul = "Counter Strike 2", Opis = "Kultowa strzelanka druzynowa.", Producent = "Valve", Gatunek = "FPS", Cena = 0m, DataWydania = new DateTime(2023, 9, 27), UrlOkladki = "https://picsum.photos/seed/steamowo2/300/400" },
                new Game { Id = 3, Tytul = "Stardew Valley", Opis = "Relaksujaca gra farmerska.", Producent = "ConcernedApe", Gatunek = "Symulator", Cena = 29.99m, DataWydania = new DateTime(2016, 2, 26), UrlOkladki = "https://picsum.photos/seed/steamowo3/300/400" },
                new Game { Id = 4, Tytul = "Elden Ring", Opis = "Trudne souls-like w otwartym swiecie.", Producent = "FromSoftware", Gatunek = "RPG/Akcja", Cena = 199.99m, DataWydania = new DateTime(2022, 2, 25), UrlOkladki = "https://picsum.photos/seed/steamowo4/300/400" },
                new Game { Id = 5, Tytul = "Cyberpunk 2077", Opis = "Futurystyczne RPG akcji w Night City.", Producent = "CD Projekt Red", Gatunek = "RPG/Akcja", Cena = 149.99m, DataWydania = new DateTime(2020, 12, 10), UrlOkladki = "https://picsum.photos/seed/steamowo5/300/400" },
                new Game { Id = 6, Tytul = "Grand Theft Auto V", Opis = "Otwarty swiat przestepczosci w Los Santos.", Producent = "Rockstar Games", Gatunek = "Akcja", Cena = 99.99m, DataWydania = new DateTime(2013, 9, 17), UrlOkladki = "https://picsum.photos/seed/steamowo6/300/400" },
                new Game { Id = 7, Tytul = "Minecraft", Opis = "Buduj i eksploruj w swiecie z klockow.", Producent = "Mojang", Gatunek = "Sandbox", Cena = 89.99m, DataWydania = new DateTime(2011, 11, 18), UrlOkladki = "https://picsum.photos/seed/steamowo7/300/400" },
                new Game { Id = 8, Tytul = "Dota 2", Opis = "Darmowa gra MOBA od Valve.", Producent = "Valve", Gatunek = "MOBA", Cena = 0m, DataWydania = new DateTime(2013, 7, 9), UrlOkladki = "https://picsum.photos/seed/steamowo8/300/400" },
                new Game { Id = 9, Tytul = "Hollow Knight", Opis = "Klimatyczna metroidvania w podziemnym swiecie.", Producent = "Team Cherry", Gatunek = "Platformowa", Cena = 39.99m, DataWydania = new DateTime(2017, 2, 24), UrlOkladki = "https://picsum.photos/seed/steamowo9/300/400" },
                new Game { Id = 10, Tytul = "FIFA 24", Opis = "Symulator pilki noznej.", Producent = "EA Sports", Gatunek = "Sportowa", Cena = 249.99m, DataWydania = new DateTime(2023, 9, 29), UrlOkladki = "https://picsum.photos/seed/steamowo10/300/400" },
                new Game { Id = 11, Tytul = "Baldur's Gate 3", Opis = "Rozbudowane RPG oparte na D&D.", Producent = "Larian Studios", Gatunek = "RPG", Cena = 219.99m, DataWydania = new DateTime(2023, 8, 3), UrlOkladki = "https://picsum.photos/seed/steamowo11/300/400" },
                new Game { Id = 12, Tytul = "Rocket League", Opis = "Pilka nozna samochodami.", Producent = "Psyonix", Gatunek = "Sportowa/Akcja", Cena = 0m, DataWydania = new DateTime(2015, 7, 7), UrlOkladki = "https://picsum.photos/seed/steamowo12/300/400" }
            );
        }
    }
}
