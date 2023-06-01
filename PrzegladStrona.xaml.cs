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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Drawing;
using System.Web;
using System.Windows.Markup;

namespace StacjaDiagnostyczna
{
    public partial class PrzegladStrona : Page
    {
        StacjaEntities context = new StacjaEntities();
        private System.Windows.Data.CollectionViewSource przegladViewSource;

        public PrzegladStrona()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            przegladViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("przegladViewSource")));
        }

        string selectedImie;

        public PrzegladStrona(string imie)
        {
            InitializeComponent();
            selectedImie = imie;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Pobierz dane z formularza
            DateTime data = dataDatePicker.SelectedDate.GetValueOrDefault();
            bool wynik = wynikCheckBox.IsChecked.GetValueOrDefault();
            string numerRejestracyjny = numer_RejestracyjnyTextBox.Text;

            // Pobierz ID_Diagnosty na podstawie wybranego elementu z peopleComboBox
            int idDiagnosty = context.Diagnosta.FirstOrDefault(d => d.Imie == selectedImie)?.Id_Diagnosty ?? 0;

            // Sprawdź, czy Numer Rejestracyjny istnieje w tabeli Pojazd
            bool numerRejestracyjnyExists = context.Pojazd.Any(p => p.Numer_Rejestracyjny == numerRejestracyjny);

            if (numerRejestracyjnyExists)
            {
                // Pobierz obiekt Pojazd na podstawie Numeru Rejestracyjnego
                Pojazd pojazd = context.Pojazd.FirstOrDefault(p => p.Numer_Rejestracyjny == numerRejestracyjny);

                if (pojazd != null)
                {
                    // Dodaj nowy rekord do bazy danych
                    Przeglad newPrzeglad = new Przeglad
                    {
                        Data = data,
                        ID_Diagnosty = idDiagnosty,
                        Pojazd = pojazd, // Ustaw obiekt Pojazd jako część relacji
                        Wynik = wynik
                    };

                    context.Przeglad.Add(newPrzeglad);
                    

                    // Opcjonalnie, wyświetl komunikat o sukcesie lub wykonaj inne czynności po dodaniu rekordu
                    MessageBox.Show("Nowy wiersz został dodany do bazy danych.");
                }
            }
            else
            {
                // Otwórz okno DodajPojazd.xaml
                DodajPojazd dodajPojazdWindow = new DodajPojazd();
                dodajPojazdWindow.ShowDialog();
            }
        }
    }
}

