using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StacjaDiagnostyczna
{
    /// <summary>
    /// Logika interakcji dla klasy DodajPojazd.xaml
    /// </summary>
    public partial class DodajPojazd : Window
    {
        private string numerRejestracyjny;

        public DodajPojazd(string numerRejestracyjny)
        {
            InitializeComponent();
            this.numerRejestracyjny = numerRejestracyjny;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz numer PESEL z TextBox
            string peselText = pESELTextBox.Text;
            decimal pesel;

            if (decimal.TryParse(peselText, out pesel))
            {
                // Sprawdź istnienie właściciela o podanym numerze PESEL w bazie danych
                using (var context = new StacjaEntities())
                {
                    var wlasciciel = context.Wlasciciel.FirstOrDefault(w => w.PESEL == pesel);

                    if (wlasciciel != null)
                    {
                        // Jeśli właściciel został znaleziony, ustaw odpowiednie wartości w Label
                        imieLabel.Content = wlasciciel.Imie;
                        nazwiskoLabel.Content = wlasciciel.Nazwisko;
                    }
                    else
                    {
                        // Numer PESEL nie istnieje w bazie danych, otwórz okno DodajWlasciciela
                        DodajWlasciciela dodajWlascicielaWindow = new DodajWlasciciela();
                        dodajWlascicielaWindow.ShowDialog();

                        // Po zamknięciu okna DodajWlasciciela możesz wykonać dodatkowe czynności
                        // ...
                    }
                }
            }
            else
            {
                MessageBox.Show("Nieprawidłowy format numeru PESEL.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Pobierz wartości wpisane przez użytkownika
            string marka = markaTextBox.Text;
            string model = modelTextBox.Text;

            string peselText = pESELTextBox.Text;
            decimal.TryParse(peselText, out decimal pesel);
            string rocznik = rocznikTextBox.Text;

            // Sprawdź, czy właściciel o podanym numerze PESEL istnieje w bazie danych
            using (var context = new StacjaEntities())
            {
                // Pobierz ostatnią wartość Id_Pojazdu
                int lastPojazdId = context.Pojazd.Max(p => p.Id_Pojazdu);

                // Sprawdź, czy istnieje właściciel o podanym numerze PESEL
                var wlasciciel = context.Wlasciciel.FirstOrDefault(w => w.PESEL == pesel);

                if (wlasciciel != null)
                {
                    // Jeśli właściciel został znaleziony, dodaj nowy pojazd do bazy danych
                    var pojazd = new Pojazd
                    {
                        Id_Pojazdu = lastPojazdId + 1,
                        Marka = marka,
                        Model = model,
                        Numer_Rejestracyjny = numerRejestracyjny,
                        Id_Wlasciciela = wlasciciel.Id_Wlasciciela,
                        Przebieg = 0,
                        Rocznik = int.Parse(rocznik)
                    };

                    context.Pojazd.Add(pojazd);
                    context.SaveChanges();

                    this.Close();
                }
                else
                {
                    // Numer PESEL nie istnieje w bazie danych, otwórz okno DodajWlasciciela
                    DodajWlasciciela dodajWlascicielaWindow = new DodajWlasciciela();
                    dodajWlascicielaWindow.ShowDialog();

                    // Po zamknięciu okna DodajWlasciciela możesz wykonać dodatkowe czynności
                    // ...
                }
            }
        }
    }
}
