using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using SteamowoClient.Models;
using SteamowoClient.Services;

namespace SteamowoClient
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _api = new();
        private readonly LoginResponse _uzytkownik;
        private int? _edytowanaGraId = null; // Id gry aktualnie wybranej do edycji w panelu admina

        public MainWindow(LoginResponse uzytkownik)
        {
            InitializeComponent();
            _uzytkownik = uzytkownik;

            TxtWitaj.Text = $"Zalogowano jako: {_uzytkownik.Login}" + (_uzytkownik.IsAdmin ? " (admin)" : "");
            OdswiezSaldo(_uzytkownik.SaldoPortfela);

            if (_uzytkownik.IsAdmin)
                ZakladkaAdmin.Visibility = Visibility.Visible;

            _ = ZaladujSklep();
        }

        private void OdswiezSaldo(decimal saldo)
        {
            _uzytkownik.SaldoPortfela = saldo;
            TxtSaldo.Text = $"Portfel: {saldo:0.00} zl";
        }

        private async System.Threading.Tasks.Task ZaladujSklep()
        {
            try
            {
                var gry = await _api.PobierzGry();
                ListaSklep.ItemsSource = gry;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Nie udalo sie pobrac listy gier: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task ZaladujBiblioteke()
        {
            try
            {
                var biblioteka = await _api.PobierzBiblioteke(_uzytkownik.UserId);
                ListaBiblioteka.ItemsSource = biblioteka;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Nie udalo sie pobrac biblioteki: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task ZaladujHistorie()
        {
            try
            {
                var historia = await _api.PobierzHistorie(_uzytkownik.UserId);
                ListaHistoria.ItemsSource = historia;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Nie udalo sie pobrac historii: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task ZaladujAdmina()
        {
            try
            {
                var gry = await _api.PobierzGry();
                ListaAdmin.ItemsSource = gry;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Nie udalo sie pobrac listy gier: {ex.Message}");
            }
        }

        private async void Zakladki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var wybrana = Zakladki.SelectedItem as TabItem;
            if (wybrana == null) return;

            if (wybrana.Header?.ToString() == "Moja biblioteka")
                await ZaladujBiblioteke();
            else if (wybrana.Header?.ToString() == "Historia transakcji")
                await ZaladujHistorie();
            else if (wybrana == ZakladkaAdmin)
                await ZaladujAdmina();
        }

        private async void BtnKup_Click(object sender, RoutedEventArgs e)
        {
            var przycisk = (Button)sender;
            int gameId = (int)przycisk.Tag;

            try
            {
                var noweSaldo = await _api.KupGre(_uzytkownik.UserId, gameId);
                OdswiezSaldo(noweSaldo);
                MessageBox.Show("Gra dodana do biblioteki!");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Nie udalo sie kupic gry");
            }
        }

        private async void BtnDoladuj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var noweSaldo = await _api.DoladujPortfel(_uzytkownik.UserId, 50m);
                OdswiezSaldo(noweSaldo);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Nie udalo sie doladowac");
            }
        }

        private async void BtnZwroc_Click(object sender, RoutedEventArgs e)
        {
            var przycisk = (Button)sender;
            int gameId = (int)przycisk.Tag;

            var potwierdzenie = MessageBox.Show(
                "Na pewno chcesz zwrocic te gre? Pieniadze wroca na Twoj portfel.",
                "Potwierdz zwrot", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (potwierdzenie != MessageBoxResult.Yes) return;

            try
            {
                var noweSaldo = await _api.ZwrocGre(_uzytkownik.UserId, gameId);
                OdswiezSaldo(noweSaldo);
                await ZaladujBiblioteke();
                MessageBox.Show("Gra zwrocona, srodki wrocily na portfel.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Nie udalo sie zwrocic gry");
            }
        }

        // ---------- Panel admina ----------

        private void ListaAdmin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListaAdmin.SelectedItem is not GameDto gra) return;

            _edytowanaGraId = gra.Id;
            TxtATytul.Text = gra.Tytul;
            TxtAProducent.Text = gra.Producent;
            TxtAGatunek.Text = gra.Gatunek;
            TxtACena.Text = gra.Cena.ToString(CultureInfo.InvariantCulture);
            TxtAOpis.Text = gra.Opis;
            TxtAOkladka.Text = gra.UrlOkladki;
            TxtAData.Text = gra.DataWydania.ToString("yyyy-MM-dd");
        }

        private void BtnAWyczysc_Click(object sender, RoutedEventArgs e)
        {
            _edytowanaGraId = null;
            TxtATytul.Text = "";
            TxtAProducent.Text = "";
            TxtAGatunek.Text = "";
            TxtACena.Text = "";
            TxtAOpis.Text = "";
            TxtAOkladka.Text = "";
            TxtAData.Text = "";
            ListaAdmin.SelectedItem = null;
        }

        private bool SprobujOdczytacFormularz(out GameDto gra)
        {
            gra = new GameDto();

            if (string.IsNullOrWhiteSpace(TxtATytul.Text))
            {
                MessageBox.Show("Podaj tytul gry.");
                return false;
            }

            if (!decimal.TryParse(TxtACena.Text, NumberStyles.Any, CultureInfo.InvariantCulture, out var cena))
            {
                MessageBox.Show("Cena musi byc liczba (np. 49.99).");
                return false;
            }

            if (!DateTime.TryParse(TxtAData.Text, out var data))
            {
                MessageBox.Show("Data wydania musi byc w formacie rrrr-mm-dd.");
                return false;
            }

            gra.Id = _edytowanaGraId ?? 0;
            gra.Tytul = TxtATytul.Text.Trim();
            gra.Producent = TxtAProducent.Text.Trim();
            gra.Gatunek = TxtAGatunek.Text.Trim();
            gra.Cena = cena;
            gra.Opis = TxtAOpis.Text.Trim();
            gra.UrlOkladki = string.IsNullOrWhiteSpace(TxtAOkladka.Text) ? null : TxtAOkladka.Text.Trim();
            gra.DataWydania = data;

            return true;
        }

        private async void BtnADodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!SprobujOdczytacFormularz(out var gra)) return;

            try
            {
                await _api.DodajGre(_uzytkownik.UserId, gra);
                await ZaladujAdmina();
                await ZaladujSklep();
                BtnAWyczysc_Click(sender, e);
                MessageBox.Show("Dodano nowa gre do sklepu.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Nie udalo sie dodac gry");
            }
        }

        private async void BtnAZapisz_Click(object sender, RoutedEventArgs e)
        {
            if (_edytowanaGraId == null)
            {
                MessageBox.Show("Zaznacz najpierw gre na liscie ponizej, ktora chcesz edytowac.");
                return;
            }

            if (!SprobujOdczytacFormularz(out var gra)) return;

            try
            {
                await _api.EdytujGre(_uzytkownik.UserId, gra);
                await ZaladujAdmina();
                await ZaladujSklep();
                MessageBox.Show("Zapisano zmiany.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Nie udalo sie zapisac zmian");
            }
        }

        private async void BtnAUsun_Click(object sender, RoutedEventArgs e)
        {
            if (_edytowanaGraId == null)
            {
                MessageBox.Show("Zaznacz najpierw gre na liscie ponizej, ktora chcesz usunac.");
                return;
            }

            var potwierdzenie = MessageBox.Show(
                "Na pewno usunac te gre ze sklepu? Ta operacja jest nieodwracalna.",
                "Potwierdz usuniecie", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (potwierdzenie != MessageBoxResult.Yes) return;

            try
            {
                await _api.UsunGre(_uzytkownik.UserId, _edytowanaGraId.Value);
                await ZaladujAdmina();
                await ZaladujSklep();
                BtnAWyczysc_Click(sender, e);
                MessageBox.Show("Gra usunieta ze sklepu.");
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Nie udalo sie usunac gry");
            }
        }
    }
}
