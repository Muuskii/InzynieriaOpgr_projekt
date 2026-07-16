namespace SteamowoAPI.Models
{
    public enum TypTransakcji
    {
        Zakup,
        Zwrot,
        Doladowanie
    }

    // Wpis historii - kazda zmiana salda zostawia tu slad
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public int? GameId { get; set; } // null przy doladowaniu
        public Game? Game { get; set; }

        public TypTransakcji Typ { get; set; }
        public decimal Kwota { get; set; } // dodatnia = wplyw, ujemna = wydatek
        public string Opis { get; set; } = string.Empty;
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
