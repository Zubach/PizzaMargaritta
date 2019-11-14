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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PizzaMargarittaUI.Pages
{
    /// <summary>
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Page
    {
      
 
        List<BPizza> basket = new List<BPizza>();
        UserModel User;
        int user_id;
        public Basket(UserModel model, int user_id)
        {
            InitializeComponent();

            User = model;
            this.user_id = user_id;
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/basketpizzas/get/{user_id}");
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            WebResponse web = httpWebRequest.GetResponse();
            string response = "";
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if (response == "BAN")
                ListViewForPizza.ItemsSource = null;
            else
            {
                var list = JsonConvert.DeserializeObject<List<BPizza>>(response);
                decimal totalPrice = 0;
                foreach (var item in list)
                {
                    item.Image = "https://localhost:44361/api/content/PizzaImages/" + item.Image;
                    totalPrice += item.Price * item.Count_in;
                }

                basket = list;
                PriceTB.Text = $"Total Price > {totalPrice}";
                ListViewForPizza.ItemsSource = basket;
            }

        }
        void refresh()
        {
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/basketpizzas/get/{user_id}");
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            WebResponse web = httpWebRequest.GetResponse();
            string response = "";
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if (response == "BAN")
            {
                ListViewForPizza.ItemsSource = null;
                PriceTB.Text = "";
            }
            else
            {
                var list = JsonConvert.DeserializeObject<List<BPizza>>(response);
                decimal totalPrice = 0;
                foreach (var item in list)
                {
                    item.Image = "https://localhost:44361/api/content/PizzaImages/" + item.Image;
                    totalPrice += item.Price * item.Count_in;
                }
                basket = list;
                PriceTB.Text = $"Total Price > {totalPrice}";
                ListViewForPizza.ItemsSource = basket;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        
          
        }
      
        private void ExitIcon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {


            this.NavigationService.Navigate(new MainPage(User, user_id));

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            var item = ((StackPanel)(((TextBox)sender).Parent)).DataContext as BPizza;
                ListViewForPizza.SelectedItem = item;
                if ((sender as TextBox).Text.Length > 0)
                {
                    HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/basketpizzas/edit/{user_id}");
                    httpWebRequest.Method = "PUT";
                    httpWebRequest.ContentType = "application/json";
                    BPizza pizzaModel = basket[ListViewForPizza.SelectedIndex];
                    pizzaModel.Count_in = Convert.ToInt32((sender as TextBox).Text);

                    using (Stream stream = httpWebRequest.GetRequestStream())
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(JsonConvert.SerializeObject(pizzaModel));
                        }
                    }
                    string response = "";
                    WebResponse web = httpWebRequest.GetResponse();
                    using (Stream stream = web.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        response = reader.ReadToEnd();
                    }
                    refresh();
                }
         
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewForPizza.SelectedItems.Count > 0)
            {
                HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/basketpizzas/DEL/{user_id}");
                httpWebRequest.Method = "DELETE";
                httpWebRequest.ContentType = "application/json";
                BPizza pizzaModel = basket[ListViewForPizza.SelectedIndex];

                using (Stream stream = httpWebRequest.GetRequestStream())
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(JsonConvert.SerializeObject(pizzaModel));
                    }
                }
                string response = "";
                WebResponse web = httpWebRequest.GetResponse();
                using (Stream stream = web.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    response = reader.ReadToEnd();
                }
                refresh();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/basketpizzas/clear/{user_id}");
            httpWebRequest.Method = "DELETE";
            httpWebRequest.ContentType = "application/json";
            WebResponse web = httpWebRequest.GetResponse();
            string response = "";
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            
            refresh();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Address.Text != "")
            {
                for (int i = 0; i < basket.Count; i++)
                {
                    HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/orders/add/{User.Login}:{User.Password}");
                    httpWebRequest.Method = "POST";
                    httpWebRequest.ContentType = "application/json";
                    OrderModel Om = new OrderModel();
                    Om.Address = Address.Text;
                    Om.PizzaID = basket[i].Pizza_id;
                    Om.UserID = user_id;

                    using (Stream stream = httpWebRequest.GetRequestStream())
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(JsonConvert.SerializeObject(Om));
                        }
                    }
                    string response = "";
                    WebResponse web = httpWebRequest.GetResponse();
                    using (Stream stream = web.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        response = reader.ReadToEnd();
                    }

                }
                Button_Click_1(null, null);
            }
        }

        private void ListViewForPizza_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListViewForPizza.SelectedItems.Count > 0)
            {

            }
        }
    }
}

