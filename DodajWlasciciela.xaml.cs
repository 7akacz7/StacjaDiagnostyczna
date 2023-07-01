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
    /// Okno dodawania nowego właściciela.
    /// </summary>
    public partial class DodajWlasciciela : Window
    {
        /// <summary>
        /// PESEL właściciela.
        /// </summary>
        public decimal pesel;

        /// <summary>
        /// Imię właściciela.
        /// </summary>
        public string Imie { get; private set; }

        /// <summary>
        /// Nazwisko właściciela.
        /// </summary>
        public string Nazwisko { get; private set; }

        /// <summary>
        /// Inicjalizuje nową instancję klasy DodajWlasciciela.
        /// </summary>
        /// <param name="pesel">PESEL właściciela.</param>
        public DodajWlasciciela(decimal pesel)
        {
            InitializeComponent();
            this.pesel = pesel;
        }

        /// <summary>
        /// Obsługa zdarzenia Loaded okna.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Implementacja zdarzenia Loaded
        }

        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Pobieranie danych z TextBoxów
            string imie = imieTextBox.Text;
            string nazwisko = nazwiskoTextBox.Text;
            string telefon = telefonTextBox.Text;

            // Dodanie nowego właściciela do tabeli Wlasciciel
            using (var context = new StacjaEntities())
            {
                int idWlasciciela = context.Wlasciciel.Max(w => w.Id_Wlasciciela) + 1;

                Wlasciciel wlasciciel = new Wlasciciel()
                {
                    Id_Wlasciciela = idWlasciciela,
                    Imie = imie,
                    Nazwisko = nazwisko,
                    TELEFON = telefon,
                    PESEL = pesel
                };

                context.Wlasciciel.Add(wlasciciel);
                context.SaveChanges();

                MessageBox.Show("Właściciel został dodany do bazy danych.");
                Imie = imieTextBox.Text;
                Nazwisko = nazwiskoTextBox.Text;

                this.Close();
            }
        }

        /// <summary>
        /// Obsługa zdarzenia zmiany tekstu w TextBoxie telefonu.
        /// </summary>
        private void telefonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            telefonTextBox.MaxLength = 9;
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
    }
}
