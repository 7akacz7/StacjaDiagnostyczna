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
        public DodajPojazd()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource pojazdViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pojazdViewSource")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // pojazdViewSource.Źródło = [ogólne źródło danych]
            System.Windows.Data.CollectionViewSource pojazdViewSource1 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pojazdViewSource1")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // pojazdViewSource1.Źródło = [ogólne źródło danych]
            System.Windows.Data.CollectionViewSource pojazdViewSource2 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("pojazdViewSource2")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // pojazdViewSource2.Źródło = [ogólne źródło danych]
            System.Windows.Data.CollectionViewSource wlascicielViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("wlascicielViewSource")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // wlascicielViewSource.Źródło = [ogólne źródło danych]
            System.Windows.Data.CollectionViewSource wlascicielViewSource1 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("wlascicielViewSource1")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // wlascicielViewSource1.Źródło = [ogólne źródło danych]
            System.Windows.Data.CollectionViewSource wlascicielViewSource2 = ((System.Windows.Data.CollectionViewSource)(this.FindResource("wlascicielViewSource2")));
            // Załaduj dane poprzez ustawienie właściwości CollectionViewSource.Source:
            // wlascicielViewSource2.Źródło = [ogólne źródło danych]
        }
    }
}
