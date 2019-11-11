using Newtonsoft.Json;
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
        List<Pizza> forbasket = new List<Pizza>();
        List<int> pizzas_count = new List<int>();
        List<BPizza> basket = new List<BPizza>();
       
        public Basket(List<Pizza> l, List<int> counts, int user_id)
        {
            InitializeComponent();
            forbasket = l;
            pizzas_count = counts;

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

            var list = JsonConvert.DeserializeObject<List<BPizza>>(response);

            foreach (var item in list)
            {
                item.Image = "https://localhost:44361/api/content/PizzaImages/" + item.Image;
            }
            basket = list;
     
            //Pizza pizza = new Pizza() { Name = "Mozzarella", Description = "Жоста з сирком і памідорками", Image = @"D:\Mozarella-pizza_cr.jpg", Price = 200 };
            //Pizza pizza2 = new Pizza() { Name = "Paperoni", Description = "Тупа с острими калбасками", Image = @"D:\Mozarella - pizza_cr.jpg", Price = 300 };

            //listOFPizzas.Add(pizza); listOFPizzas.Add(pizza2);
            ListViewForPizza.ItemsSource = basket;
            //for (int i = 0; i < forbasket.Count; i++)
            //{
            //    BPizza bp = new BPizza();
            //    bp.Description = forbasket[i].Description;
            //    bp.Image = forbasket[i].Image;
            //    bp.Name = forbasket[i].Name;
            //    bp.Price = forbasket[i].Price;
            //    bp.Count_in = pizzas_count[i];
            //    basket.Add(bp);
            //}
            //ListViewForPizza.ItemsSource = basket;

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
          
            this.NavigationService.GoBack();
        }
    }
}

