namespace SteamowoAPI.Models
{
    // Gra dostepna w sklepie
    public class Game
    {
        public int Id { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public string Producent { get; set; } = string.Empty;
        public string Gatunek { get; set; } = string.Empty;
        public decimal Cena { get; set; }
        public string? UrlOkladki { get; set; }
        public DateTime DataWydania { get; set; }

        public List<UserGame> Wlasciciele { get; set; } = new();
    }
}
