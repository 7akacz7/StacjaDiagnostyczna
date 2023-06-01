using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StacjaDiagnostyczna
{
    /// <summary>
    /// Logika interakcji dla klasy Start.xaml
    /// </summary>
    public partial class Start : Page
    {
        StacjaEntities imiona = new StacjaEntities();
        public Start()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Wczytaj listę diagnostów do ListBox
            using (StacjaEntities context = new StacjaEntities())
            {
                peopleListBox.ItemsSource = context.Diagnosta.Select(d => d.Imie).ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Sprawdź, czy wybrano diagnostę
            if (peopleListBox.SelectedItem != null)
            {
                string selectedImie = (string)peopleListBox.SelectedItem;

                // Przejdź do strony PrzegladStrona, przekazując wybrane imię diagnosty
                NavigationService.Navigate(new PrzegladStrona(selectedImie));
            }
            else
            {
                MessageBox.Show("Wybierz diagnostę przed rozpoczęciem przeglądu.");
            }
        }

    }
}
