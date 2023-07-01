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
 /// <summary>
 /// Strona przeglądu pojazdu.
 /// </summary>
    public partial class PrzegladStrona : Page
    {
        StacjaEntities context = new StacjaEntities();
        private System.Windows.Data.CollectionViewSource przegladViewSource;

        /// <summary>
        /// Inicjalizuje nową instancję klasy PrzegladStrona.
        /// </summary>
        public PrzegladStrona()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Obsługa zdarzenia Loaded strony.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            przegladViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("przegladViewSource")));
        }

        string selectedImie;

        /// <summary>
        /// Inicjalizuje nową instancję klasy PrzegladStrona z wybranym imieniem diagnosty.
        /// </summary>
        /// <param name="imie">Imię diagnosty.</param>
        public PrzegladStrona(string imie)
        {
            InitializeComponent();
            selectedImie = imie;
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Zapisz".
        /// </summary>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

            // Pobieranie danych z formularza
            DateTime data = dataDatePicker.SelectedDate.GetValueOrDefault();
            bool wynik = wynikCheckBox.IsChecked.GetValueOrDefault();
            string numerRejestracyjny = numer_RejestracyjnyTextBox.Text;
            string uwagi = uwagiTextBox.Text;
           
            // Pobieranie ID_Diagnosty na podstawie wybranego elementu z peopleComboBox
            int idDiagnosty = context.Diagnosta.FirstOrDefault(d => d.Imie == selectedImie)?.Id_Diagnosty ?? 0;

            
            int lastPrzegladId = context.Przeglad.OrderByDescending(p => p.Id_Przegladu).Select(p => p.Id_Przegladu).FirstOrDefault();

            

            
                // Pobieranie obiektu Pojazd na podstawie Numeru Rejestracyjnego
                Pojazd pojazd = context.Pojazd.FirstOrDefault(p => p.Numer_Rejestracyjny == numerRejestracyjny);

                if (pojazd != null && idDiagnosty != 0)
                {

                if (int.TryParse(przebiegTextBox.Text, out int nowyPrzebieg) && nowyPrzebieg >= pojazd.Przebieg)
                {

                    int wynikValue = wynik ? 1 : 0;
                    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Stacja;Integrated Security=True;";
                    string query = $"INSERT INTO Przeglad VALUES({lastPrzegladId + 1}, '{data.ToString("yyyy-MM-dd")}', {pojazd.Id_Pojazdu}, {idDiagnosty}, {wynikValue}, '{uwagi}')";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                    }
                    pojazd.Przebieg = nowyPrzebieg;
                    context.SaveChanges();
                    
                    Raport raport = new Raport();
                    raport.ShowDialog();
                    
                }
                else
                {
                    MessageBox.Show("Wprowadź poprawną wartość dla pola Przebieg.");
                }
            }
            

        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku "Sprawdź".
        /// </summary>
        private void sprawdz_Click(object sender, RoutedEventArgs e)
        {
            string numerRejestracyjny = numer_RejestracyjnyTextBox.Text;

            // Sprawdzenie, czy Numer Rejestracyjny istnieje w tabeli Pojazd
            bool numerRejestracyjnyExists = context.Pojazd.Any(p => p.Numer_Rejestracyjny == numerRejestracyjny);

            if (numerRejestracyjnyExists)
            {
                // Ustawianie przycisk "Zapisz" jako widoczny
                saveButton.Visibility = Visibility.Visible;
            }
            else
            {
                // Otwieranie okno DodajPojazd.xaml
                DodajPojazd dodajPojazdWindow = new DodajPojazd(numerRejestracyjny);
                dodajPojazdWindow.ShowDialog();
            }
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany tekstu w TextBox przebiegTextBox.
        /// </summary>
        private void przebiegTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                string newText = "";
                foreach (char c in textBox.Text)
                {
                    if (char.IsDigit(c))
                    {
                        newText += c;
                    }
                }
                textBox.Text = newText;
                textBox.CaretIndex = newText.Length; // Ustawianie kursora na końcu tekstu
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Zamknięcie aplikacji
            Application.Current.Shutdown();
        }
    }
}

