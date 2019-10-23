using MaterialDesignThemes.Wpf;
using PizzaMargaritta.Models;
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
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        UserModel CurrentUser;
        public UserWindow(UserModel um)
        {
            InitializeComponent();
            TextUserName.Text = um.FirstName + " " + "\"" + um.Login + "\"" + " " + um.LastName;
            CurrentUser = um;
            
            Pizza pizza = new Pizza() { Name = "Mozzarella", Description = "Жоста з сирком і памідорками", Image = @"D:\Mozarella-pizza_cr.jpg", Price = 200 };
            Pizza pizza2 = new Pizza() { Name = "Paperoni", Description = "Тупа с острими калбасками", Image = @"D:\Mozarella - pizza_cr.jpg", Price = 300 };
            List<Pizza> listOFPizzas = new List<Pizza>();
            listOFPizzas.Add(pizza); listOFPizzas.Add(pizza2);
            ListViewForPizza.ItemsSource = listOFPizzas;

        }  

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitIcon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void ListViewForPizza_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ForDesc.Text = (ListViewForPizza.SelectedItems[0] as Pizza).Description;
           
            Description_Dialog.IsOpen = true;
            ButtonAcc.Opacity = 100;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ButtonAcc.Opacity = 0;
            Description_Dialog.IsOpen = false;
        }
    }
}
