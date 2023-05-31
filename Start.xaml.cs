using System;
using System.Collections.Generic;
using System.Linq;
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
        public Start()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // View Expense Report
            PrzegladStrona przegladstrona = new PrzegladStrona();
            this.NavigationService.Navigate(przegladstrona);
        }
    }
}
