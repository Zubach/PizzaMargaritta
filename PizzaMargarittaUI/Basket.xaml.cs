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
        List<int> pizzas_count = new List<int>();
        List<BPizza> basket = new List<BPizza>();
        public Basket(List<Pizza> l,List<int> counts)
        {
            InitializeComponent();
            forbasket = l;
            pizzas_count = counts;
            for(int i = 0; i < forbasket.Count; i++)
            {
                BPizza bp = new BPizza();
                bp.Description = forbasket[i].Description;
                bp.Image = forbasket[i].Image;
                bp.Name = forbasket[i].Name;
                bp.Price = forbasket[i].Price;
                bp.ToBuy = pizzas_count[i];
                basket.Add(bp);
            }
            ListViewForPizza.ItemsSource = basket;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Owner.Show();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }
    }
}
