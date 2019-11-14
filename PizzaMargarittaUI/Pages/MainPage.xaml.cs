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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        UserModel CurrentUser;

        List<Pizza> To_basket = new List<Pizza>();
        static public List<int> pizas_count = new List<int>();
        List<Pizza> listOFPizzas = new List<Pizza>();
        List<BPizza> Pizzas_toBasket = new List<BPizza>();
        static int user_id;
        public MainPage(UserModel um,int id)
        {
            InitializeComponent();
            TextUserName.Text = um.FirstName + " " + "\"" + um.Login + "\"" + " " + um.LastName;
            CurrentUser = um;
            
           
            user_id = id;

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

            var list = JsonConvert.DeserializeObject<List<Pizza>>(response);

            foreach (var item in list)
            {
                item.Image = "https://localhost:44361/api/content/PizzaImages/" + item.Image;
            }
            listOFPizzas = list;
            RefreshPizzasFromBasket();
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
       
            this.NavigationService.Navigate(new Basket(CurrentUser, user_id));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            var item = ((StackPanel)(((Button)sender).Parent)).DataContext as Pizza;
            ListViewForPizza.SelectedItem = item;
            if (ListViewForPizza.SelectedItems.Count > 0)
            {
                bool flag = false;
     
                foreach (var p in Pizzas_toBasket)
                {
                    if (p.Pizza_id == (ListViewForPizza.SelectedItems[0] as Pizza).id)
                    {
                        p.Count_in++;
                        AddPizzaToBasketEq(p);
                        flag = true;
                    }
             
                }
                if (flag == false)
                {

                    AddPizzaToBasket(listOFPizzas[ListViewForPizza.SelectedIndex]);
                }
                ListViewForPizza.SelectedItems.Clear();
            }
            
        }

        private void ListViewForPizza_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewForPizza.SelectedItem != null)
            {
                var item = (ListViewForPizza.SelectedItem as Pizza);

                BoxOFPizza.Header = item.Name;
                OurPizza.Source = new BitmapImage(new Uri(item.Image));
                gpricebox.Content = "Pizza`s price - > " + item.Price.ToString();
                gdescbox.Text = item.Description;
            }

        }

        public void AddPizzaToBasket(Pizza pizza)
        {
            BPizza pizzaModel = new BPizza();
            pizzaModel.Count_in = 1;
            pizzaModel.Description = pizza.Description;
            pizzaModel.Image = pizza.Image;
            pizzaModel.Name = pizza.Name;
            pizzaModel.Price = pizza.Price;
            pizzaModel.Pizza_id = pizza.id;
        
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/basketpizzas/post/{user_id}");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";

            

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
            RefreshPizzasFromBasket();
        }


        public void AddPizzaToBasketEq(BPizza pizza)
        {
            if (pizza.Count_in != 99)
            {
                HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/basketpizzas/edit/{user_id}");
                httpWebRequest.Method = "PUT";
                httpWebRequest.ContentType = "application/json";
                BPizza pizzaModel = pizza;
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
                RefreshPizzasFromBasket();
            }
        }

        public void RefreshPizzasFromBasket()
        {

            try
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
                if (response != "BAN")
                {
                    var list = JsonConvert.DeserializeObject<List<BPizza>>(response);

                    foreach (var item in list)
                    {
                        item.Image = "https://localhost:44361/api/content/PizzaImages/" + item.Image;
                    }
                    Pizzas_toBasket = list;
                    int lengthof = 0;
                    foreach (var p in Pizzas_toBasket)
                    {
                        lengthof += p.Count_in;
                    }
             
                    Badge.Badge = lengthof;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Orders(CurrentUser, user_id,listOFPizzas));
        }
    }
}
