using PizzaMargarittaUI.Models;
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

namespace PizzaMargarittaUI
{
    /// <summary>
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        List<Pizza> forbasket = new List<Pizza>();
        public Basket(List<Pizza> l)
        {
            InitializeComponent();
            forbasket = l;
            ListViewForPizza.ItemsSource = forbasket;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }
    }
}
