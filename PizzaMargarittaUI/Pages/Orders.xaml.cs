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
    /// Логика взаимодействия для Orders.xaml
    /// </summary>
    public partial class Orders : Page
    {
        List<OrderModel> listOFOrders = new List<OrderModel>();
        UserModel User = new UserModel();
        int user_id;
        List<Pizza> Pizza = new List<Pizza>();
        public Orders(UserModel user, int user_id, List<Pizza> pizzas)
        {

            InitializeComponent();
            this.user_id = user_id;
            User = user;
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/orders/getL/{user.Login}:{user.Password}");
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            WebResponse web = httpWebRequest.GetResponse();
            string response = "";
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }

            var list = JsonConvert.DeserializeObject<List<OrderModel>>(response);

            Pizza = pizzas;

            listOFOrders = list;
            


            ListViewForOrders.ItemsSource = listOFOrders;

        }

        private void ExitIcon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new MainPage(User, user_id));
        }

        private void ListViewForOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewForOrders.SelectedItems.Count > 0)
            {

                var item = (ListViewForOrders.SelectedItem as OrderModel);
                foreach (var p in Pizza)
                {
                    if (p.id == item.PizzaID)
                    {
                        BoxOFPizza.Header = p.Name;
                        OurPizza.Source = new BitmapImage(new Uri(p.Image));
                        gpricebox.Content = "Pizza`s price - > " + p.Price.ToString();
                        gdescbox.Text = p.Description;
                    }
                }
            }
        }
    }
}
