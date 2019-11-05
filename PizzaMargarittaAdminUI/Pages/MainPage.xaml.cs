using Newtonsoft.Json;
using PizzaMargaritta.Models;
using PizzaMargarittaAdminUI.Models;
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

namespace PizzaMargarittaAdminUI.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private AdminModel admin;
        private List<UserModel> users = new List<UserModel>();
        private List<PizzaModel> pizzas = new List<PizzaModel>();
        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(AdminModel model)
        {
            InitializeComponent();
            admin = model;
            HttpWebRequest webRequest = WebRequest.CreateHttp($"https://localhost:44361/api/clients/get/{admin.Login}:{admin.Password}");
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            string response = "";
            using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if (response != "Ti ne admin")
            {
                users = JsonConvert.DeserializeObject<List<UserModel>>(response);
                listViewUser.ItemsSource = users;

            }
        }

        private void ListViewPizza_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedItem != null)
            {
                var model = (sender as ListView).SelectedItem as PizzaModel;
                this.NavigationService.Navigate(new EditPizzaPage(admin, model));
            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedItem != null)
            {
                var model = (sender as ListView).SelectedItem as UserModel;
                this.NavigationService.Navigate(new EditUserPage(model,admin));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            listViewPizza.Visibility = Visibility.Visible;
            listViewUser.Visibility = Visibility.Hidden;
            BtnAddPizza.Visibility = Visibility.Visible;



            HttpWebRequest webRequest = WebRequest.CreateHttp($"https://localhost:44361/api/pizzas/");
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            string response = "";
            using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if (response != "Ti ne admin")
            {
                pizzas = JsonConvert.DeserializeObject<List<PizzaModel>>(response);
                listViewPizza.ItemsSource = pizzas;

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            listViewPizza.Visibility = Visibility.Hidden;
            listViewUser.Visibility = Visibility.Visible;
            BtnAddPizza.Visibility = Visibility.Hidden;

            HttpWebRequest webRequest = WebRequest.CreateHttp($"https://localhost:44361/api/clients/get/{admin.Login}:{admin.Password}");
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            string response = "";
            using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if (response != "Ti ne admin")
            {
                users = JsonConvert.DeserializeObject<List<UserModel>>(response);
                listViewUser.ItemsSource = users;

            }
        }
    }
}
