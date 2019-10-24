using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using PizzaMargaritta.Models;
using PizzaMargarittaUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
      
        List<Pizza> To_basket = new List<Pizza>();
        List<Pizza> listOFPizzas = new List<Pizza>();
        public UserWindow(UserModel um)
        {
            InitializeComponent();
            TextUserName.Text = um.FirstName + " " + "\"" + um.Login + "\"" + " " + um.LastName;
            CurrentUser = um;

            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/pizzas");
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            WebResponse web = httpWebRequest.GetResponse();
            string response = "";
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }

            listOFPizzas = JsonConvert.DeserializeObject<List<Pizza>>(response);

            

         


            //Pizza pizza = new Pizza() { Name = "Mozzarella", Description = "Жоста з сирком і памідорками", Image = @"D:\Mozarella-pizza_cr.jpg", Price = 200 };
            //Pizza pizza2 = new Pizza() { Name = "Paperoni", Description = "Тупа с острими калбасками", Image = @"D:\Mozarella - pizza_cr.jpg", Price = 300 };

            //listOFPizzas.Add(pizza); listOFPizzas.Add(pizza2);
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

        //private void ListViewForPizza_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    if (ListViewForPizza.SelectedItems.Count>0)
        //    {
        //        ForDesc.Text = (ListViewForPizza.SelectedItems[0] as Pizza).Description;

        //        Description_Dialog.IsOpen = true;
        //        ButtonAcc.Opacity = 100;
        //    }

        //}

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    //ButtonAcc.Opacity = 0;
        //    //Description_Dialog.IsOpen = false;
        //}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Basket basket = new Basket(To_basket);
            basket.Owner = this;
            this.Hide();
            basket.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var item = ((StackPanel)(((Button)sender).Parent)).DataContext as Pizza;
            ListViewForPizza.SelectedItem = item;
            if (ListViewForPizza.SelectedItems.Count > 0)
            {
                To_basket.Add(ListViewForPizza.SelectedItems[0] as Pizza);
                Badge.Badge = To_basket.Count;
                ListViewForPizza.SelectedItems.Clear();
            }
        }

        private void ListViewForPizza_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BoxOFPizza.Header = (ListViewForPizza.SelectedItem as Pizza).Name;
            string path = Environment.CurrentDirectory + "/../../";
            OurPizza.Source = new BitmapImage(new Uri(path+(ListViewForPizza.SelectedItem as Pizza).Image));

        }
    }
}
