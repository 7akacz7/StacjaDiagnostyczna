﻿using System;
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
    
    public partial class DodajPojazd : Window
    {
        private string numerRejestracyjny;


        /// <summary>
        /// Inicjalizuje nową instancję klasy DodajPojazd.
        /// </summary>
        public DodajPojazd(string numerRejestracyjny)
        {
            InitializeComponent();
            this.numerRejestracyjny = numerRejestracyjny;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {


        }
        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Pobieranie numer PESEL z TextBox
            string peselText = pESELTextBox.Text;
            decimal pesel;

            if (decimal.TryParse(peselText, out pesel))
            {
                // Sprawdzenie istnienia właściciela o podanym numerze PESEL w bazie danych
                using (var context = new StacjaEntities())
                {
                    var wlasciciel = context.Wlasciciel.FirstOrDefault(w => w.PESEL == pesel);

                    if (wlasciciel != null)
                    {
                        // Jeśli właściciel został znaleziony, ustawianie odpowiednich wartości w Label
                        imieLabel.Content = wlasciciel.Imie;
                        nazwiskoLabel.Content = wlasciciel.Nazwisko;
                    }
                    else
                    {
                        // Numer PESEL nie istnieje w bazie danych, otwieranie okno DodajWlasciciela
                        DodajWlasciciela dodajWlascicielaWindow = new DodajWlasciciela(pesel);
                        dodajWlascicielaWindow.ShowDialog();

                       
                    }
                }
            }
            else
            {
                MessageBox.Show("Nieprawidłowy format numeru PESEL.");
            }
        }
        /// <summary>
        /// Obsługa zdarzenia kliknięcia przycisku.
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Pobieranie wartości wpisanych przez użytkownika
            string marka = markaTextBox.Text;
            string model = modelTextBox.Text;

            string peselText = pESELTextBox.Text;
            decimal.TryParse(peselText, out decimal pesel);
            string rocznik = rocznikTextBox.Text;

            // Sprawdzenie, czy właściciel o podanym numerze PESEL istnieje w bazie danych
            using (var context = new StacjaEntities())
            {
                // Pobieranie ostatniej wartość Id_Pojazdu
                int lastPojazdId = context.Pojazd.Max(p => p.Id_Pojazdu);

                // Sprawdzenie, czy istnieje właściciel o podanym numerze PESEL
                var wlasciciel = context.Wlasciciel.FirstOrDefault(w => w.PESEL == pesel);

                if (wlasciciel != null)
                {
                    // Jeśli właściciel został znaleziony, dodawanie nowego pojazdu do bazy danych
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

                    // Znajdź instancję PrzegladStrona
                    var przegladStrona = Application.Current.Windows.OfType<PrzegladStrona>().FirstOrDefault();

                    if (przegladStrona != null)
                    {
                        // Zmień widoczność przycisku saveButton na widoczny
                        przegladStrona.saveButton.Visibility = Visibility.Visible;
                    }

                    this.Close();
                }
                else
                {
                    // Numer PESEL nie istnieje w bazie danych, otwieranie okno DodajWlasciciela
                    DodajWlasciciela dodajWlascicielaWindow = new DodajWlasciciela(pesel);
                    dodajWlascicielaWindow.ShowDialog();
                }
            }
        }
        /// <summary>
        /// Obsługa zdarzenia zmiany tekstu w TextBox pESELTextBox.
        /// </summary>
        private void pESELTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            pESELTextBox.MaxLength = 11;
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
        /// <summary>
        /// Obsługa zdarzenia zmiany tekstu w TextBox rocznikTextBox.
        /// </summary>
        private void rocznikTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            rocznikTextBox.MaxLength = 4;
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
