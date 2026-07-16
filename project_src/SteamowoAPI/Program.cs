using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SteamowoAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// EF Core + SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS - zeby klient WPF (i ewentualnie przegladarka) mogl sie odpytywac o API lokalnie
builder.Services.AddCors(options =>
{
    options.AddPolicy("PozwolNaWszystko", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// automatyczne tworzenie/aktualizacja bazy przy starcie (wygodne na potrzeby projektu studenckiego)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // tworzymy konto admina, jesli jeszcze nie istnieje (login: admin / haslo: admin123)
    if (!db.Users.Any(u => u.Login == "admin"))
    {
        db.Users.Add(new SteamowoAPI.Models.User
        {
            Login = "admin",
            Email = "admin@steamowo.local",
            PasswordHash = HashujHaslo("admin123"),
            IsAdmin = true,
            SaldoPortfela = 0
        });
        db.SaveChanges();
    }
}

// ta sama logika hashowania co w AuthController - SHA-256, ciag hex
static string HashujHaslo(string haslo)
{
    byte[] bajtyHasla = Encoding.UTF8.GetBytes(haslo);
    byte[] hash = SHA256.HashData(bajtyHasla);

    var sb = new StringBuilder();
    foreach (byte b in hash)
        sb.Append(b.ToString("x2"));

    return sb.ToString();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // wejscie na sama glowna strone (http://localhost:5205/) przekierowuje do Swaggera
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.UseCors("PozwolNaWszystko");
app.UseAuthorization();
app.MapControllers();

app.Run();
