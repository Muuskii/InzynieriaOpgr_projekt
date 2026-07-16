namespace SteamowoAPI.DTOs
{
    public record RegisterDto(string Login, string Email, string Password);
    public record LoginDto(string Login, string Password);
    public record LoginResponseDto(int UserId, string Login, decimal SaldoPortfela, bool IsAdmin);

    public record GameDto(int Id, string Tytul, string Opis, string Producent, string Gatunek, decimal Cena, string? UrlOkladki, DateTime DataWydania);
    public record UpsertGameDto(int RequestUserId, string Tytul, string Opis, string Producent, string Gatunek, decimal Cena, string? UrlOkladki, DateTime DataWydania);

    public record LibraryItemDto(int GameId, string Tytul, string Gatunek, DateTime DataZakupu, double CzasGry, string? UrlOkladki);

    public record PurchaseRequestDto(int UserId, int GameId);
    public record PurchaseResponseDto(string Wiadomosc, decimal NoweSaldo);
    public record TopUpDto(int UserId, decimal Kwota);
}
