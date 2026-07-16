using System.Text.RegularExpressions;
using System.Windows;
using SteamowoClient.Services;
using SteamowoClient; // dla MainWindow

namespace SteamowoClient.Views
{
    public partial class LoginWindow : Window
    {
        private readonly ApiService _api = new();
        private static readonly Regex WzorzecEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void BtnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            TxtKomunikat.Text = "";

            if (string.IsNullOrWhiteSpace(TxtLogin.Text) || string.IsNullOrWhiteSpace(TxtHaslo.Password))
            {
                TxtKomunikat.Text = "Podaj login i haslo.";
                return;
            }

            try
            {
                var wynik = await _api.Login(TxtLogin.Text.Trim(), TxtHaslo.Password);
                if (wynik == null) return;

                OtworzGlowneOkno(wynik);
            }
            catch (System.Exception ex)
            {
                TxtKomunikat.Text = ex.Message;
            }
        }

        private async void BtnZarejestruj_Click(object sender, RoutedEventArgs e)
        {
            TxtKomunikat.Text = "";

            var blad = WalidujFormularzRejestracji();
            if (blad != null)
            {
                TxtKomunikat.Text = blad;
                return;
            }

            try
            {
                var wynik = await _api.Register(TxtLogin.Text.Trim(), TxtEmail.Text.Trim(), TxtHaslo.Password);
                if (wynik == null) return;

                OtworzGlowneOkno(wynik);
            }
            catch (System.Exception ex)
            {
                TxtKomunikat.Text = ex.Message;
            }
        }

        // sprawdza formularz lokalnie, przed wyslaniem czegokolwiek do API
        // (API i tak waliduje to samo po swojej stronie - to tylko szybszy feedback dla uzytkownika)
        private string? WalidujFormularzRejestracji()
        {
            if (string.IsNullOrWhiteSpace(TxtLogin.Text) || TxtLogin.Text.Trim().Length < 3)
                return "Login musi miec co najmniej 3 znaki.";

            if (string.IsNullOrWhiteSpace(TxtEmail.Text) || !WzorzecEmail.IsMatch(TxtEmail.Text.Trim()))
                return "Podaj poprawny adres email.";

            if (string.IsNullOrWhiteSpace(TxtHaslo.Password) || TxtHaslo.Password.Length < 6)
                return "Haslo musi miec co najmniej 6 znakow.";

            if (!TxtHaslo.Password.Any(char.IsDigit))
                return "Haslo musi zawierac co najmniej jedna cyfre.";

            if (TxtHaslo.Password != TxtHasloPowtorz.Password)
                return "Podane hasla nie sa takie same.";

            return null; // brak bledow
        }

        private void OtworzGlowneOkno(Models.LoginResponse zalogowanyUzytkownik)
        {
            var glowneOkno = new MainWindow(zalogowanyUzytkownik);
            glowneOkno.Show();
            Close();
        }
    }
}
