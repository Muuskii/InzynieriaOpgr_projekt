namespace SteamowoAPI.Models
{
    // Wpis: uzytkownik kupil / posiada dana gre (biblioteka)
    public class UserGame
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int GameId { get; set; }
        public Game? Game { get; set; }

        public DateTime DataZakupu { get; set; } = DateTime.Now;
        public decimal CenaZakupu { get; set; }
        public double CzasGry { get; set; } = 0; // w godzinach
    }
}
