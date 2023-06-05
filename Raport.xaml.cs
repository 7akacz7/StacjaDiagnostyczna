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
using System.Data.SqlClient;
using System.Data;

namespace StacjaDiagnostyczna
{
    /// <summary>
    /// Logika interakcji dla klasy Raport.xaml
    /// </summary>
    public partial class Raport : Window
    {
        public Raport()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Tworzenie połączenia z bazą danych
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Stacja;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Zapytanie SQL do pobrania ostatniego wiersza z tabeli Przeglad
                string query = "SELECT TOP 1 * FROM Przeglad ORDER BY Id_Przegladu DESC";
                SqlCommand command = new SqlCommand(query, connection);

                // Wykonanie zapytania i odczytanie danych
                SqlDataReader reader = command.ExecuteReader();
                
                if (reader.Read())
                {
                    // Pobranie danych z wiersza
                    DateTime dataPrzegladu = (DateTime)reader["Data"];
                    int idDiagnosty = (int)reader["Id_Diagnosty"];
                    int idPojazdu = (int)reader["Id_Pojazdu"];
                    int idWlasciciela = 0; // Zmienna przechowująca Id_Wlasciciela
                    bool wynikPrzegladu = (bool)reader["wynik"];
                    object uwagiObject = reader["Uwagi"];
                    string uwagiText = uwagiObject != DBNull.Value ? (string)uwagiObject : string.Empty;

                    reader.Close(); // Zamknięcie SqlDataReader

                    // Pobranie danych Pojazdu
                    string queryPojazd = $"SELECT Marka, Model, Rocznik, Numer_Rejestracyjny, Przebieg, Id_Wlasciciela FROM Pojazd WHERE Id_Pojazdu = {idPojazdu}";
                    SqlCommand commandPojazd = new SqlCommand(queryPojazd, connection);
                    SqlDataReader readerPojazd = commandPojazd.ExecuteReader();
                    string markaPojazdu = string.Empty;
                    string modelPojazdu = string.Empty;
                    int rocznikPojazdu = 0;
                    decimal przebieg = 0;
                    string numerRejestracyjnyPojazdu = string.Empty;
                    if (readerPojazd.Read())
                    {
                        markaPojazdu = (string)readerPojazd["Marka"];
                        modelPojazdu = (string)readerPojazd["Model"];
                        rocznikPojazdu = (int)readerPojazd["Rocznik"];
                        numerRejestracyjnyPojazdu = (string)readerPojazd["Numer_Rejestracyjny"];
                        przebieg = (decimal)readerPojazd["Przebieg"];
                        idWlasciciela = (int)readerPojazd["Id_Wlasciciela"];
                    }
                    readerPojazd.Close();

                    // Pobranie danych Diagnosty
                    string queryDiagnosta = $"SELECT Imie, Nazwisko FROM Diagnosta WHERE Id_Diagnosty = {idDiagnosty}";
                    SqlCommand commandDiagnosta = new SqlCommand(queryDiagnosta, connection);
                    SqlDataReader readerDiagnosta = commandDiagnosta.ExecuteReader();
                    string imieDiagnosty = string.Empty;
                    string nazwiskoDiagnosty = string.Empty;
                    if (readerDiagnosta.Read())
                    {
                        imieDiagnosty = (string)readerDiagnosta["Imie"];
                        nazwiskoDiagnosty = (string)readerDiagnosta["Nazwisko"];
                    }
                    readerDiagnosta.Close();

                    // Pobranie danych Wlasciciela
                    string queryWlasciciel = $"SELECT Imie, Nazwisko FROM Wlasciciel WHERE Id_Wlasciciela = {idWlasciciela}";
                    SqlCommand commandWlasciciel = new SqlCommand(queryWlasciciel, connection);
                    SqlDataReader readerWlasciciel = commandWlasciciel.ExecuteReader();
                    string imieWlasciciela = string.Empty;
                    string nazwiskoWlasciciela = string.Empty;
                    if (readerWlasciciel.Read())
                    {
                        imieWlasciciela = (string)readerWlasciciel["Imie"];
                        nazwiskoWlasciciela = (string)readerWlasciciel["Nazwisko"];
                    }
                    readerWlasciciel.Close();

                    // Wyświetlanie danych w kontrolkach w oknie Raport
                    dataLabel.Content = dataPrzegladu.ToString();
                    imieLabel.Content = imieDiagnosty;
                    nazwiskoLabel.Content = nazwiskoDiagnosty;
                    markaLabel.Content = markaPojazdu;
                    modelLabel.Content = modelPojazdu;
                    rocznikLabel.Content = rocznikPojazdu.ToString();
                    przebiegLabel.Content = przebieg.ToString();
                    numer_RejestracyjnyLabel.Content = numerRejestracyjnyPojazdu;
                    uwagiTextBox.Text = uwagiText;
                    imieLabel1.Content = imieWlasciciela;
                    nazwiskoLabel1.Content = nazwiskoWlasciciela;

                    if (wynikPrzegladu)
                    {
                        wynikImage.Source = new BitmapImage(new Uri("tak.png", UriKind.Relative));
                    }
                    else
                    {
                        wynikImage.Source = new BitmapImage(new Uri("nie.png", UriKind.Relative));
                    }
                }

                reader.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}