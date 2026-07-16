namespace SteamowoClient.Models
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Login { get; set; } = string.Empty;
        public decimal SaldoPortfela { get; set; }
        public bool IsAdmin { get; set; }
    }

    public class GameDto
    {
        public int Id { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public string Producent { get; set; } = string.Empty;
        public string Gatunek { get; set; } = string.Empty;
        public decimal Cena { get; set; }
        public string? UrlOkladki { get; set; }
        public DateTime DataWydania { get; set; }
    }

    public class LibraryItemDto
    {
        public int GameId { get; set; }
        public string Tytul { get; set; } = string.Empty;
        public string Gatunek { get; set; } = string.Empty;
        public DateTime DataZakupu { get; set; }
        public double CzasGry { get; set; }
        public string? UrlOkladki { get; set; }
    }

    public class PurchaseResponse
    {
        public string Wiadomosc { get; set; } = string.Empty;
        public decimal NoweSaldo { get; set; }
    }

    public class TopUpResponse
    {
        public decimal NoweSaldo { get; set; }
    }

    public class TransactionDto
    {
        public int Id { get; set; }
        public string Typ { get; set; } = string.Empty;
        public decimal Kwota { get; set; }
        public string Opis { get; set; } = string.Empty;
        public DateTime Data { get; set; }
    }
}
