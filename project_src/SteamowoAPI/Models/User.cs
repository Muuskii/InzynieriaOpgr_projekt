namespace SteamowoAPI.Models
{
    // Uzytkownik systemu (konto)
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime DataRejestracji { get; set; } = DateTime.Now;
        public decimal SaldoPortfela { get; set; } = 0;
        public bool IsAdmin { get; set; } = false;

        // relacje
        public List<UserGame> PosiadaneGry { get; set; } = new();
    }
}
