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
    /// <summary>
    /// Logika interakcji dla klasy DodajWlasciciela.xaml
    /// </summary>
    public partial class DodajWlasciciela : Window
    {
        public decimal pesel;
        public string Imie { get; private set; }
        public string Nazwisko { get; private set; }
        public DodajWlasciciela(decimal pesel)
        {
            InitializeComponent();
            this.pesel = pesel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

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
