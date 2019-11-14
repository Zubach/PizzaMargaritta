using Newtonsoft.Json;
using PizzaMargaritta;
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
    /// Interaction logic for SearchPizzaPage.xaml
    /// </summary>
    public partial class SearchPizzaPage : Page
    {
        AdminModel admin;
        List<PizzaModel> pizzas;
        public SearchPizzaPage()
        {
            InitializeComponent();
        }

        public SearchPizzaPage(AdminModel model)
        {
            InitializeComponent();
            admin = model;
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            MultyFilter filter = new MultyFilter();
            if(NameTextBox.Text != "")
            {
                List<string> conditions = new List<string>() { "=="};
                List<string> values = new List<string>() { NameTextBox.Text };
                filter.Filters.Add(new Filter() {PropertyName="Name",Conditions=conditions,Values= values});
            
            }
            if(FromTextBox.Text != "" || ToTextBox.Text != "")
            {
                List<string> conditions = new List<string>();
                List<string> values = new List<string>();
                if (FromTextBox.Text != "")
                {
                    conditions.Add(">=");
                    values.Add(FromTextBox.Text);
                }
                if(ToTextBox.Text != "")
                {
                    conditions.Add("<=");
                    values.Add(ToTextBox.Text);
                }

                
                filter.Filters.Add(new Filter() { PropertyName = "Price", Conditions = conditions, Values = values });
            }
           
              
              
             
            

            HttpWebRequest webRequest = WebRequest.CreateHttp($"https://localhost:44361/api/pizzas/filtered/get");
            webRequest.Method = "PUT";
            webRequest.ContentType = "application/json";
                    
            using (Stream stream = webRequest.GetRequestStream())
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(filter));
                }
            }
            var webResponse = webRequest.GetResponse();
            string response = "";
            using(Stream stream = webResponse.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    response = reader.ReadToEnd();
                }
            }

            pizzas = (JsonConvert.DeserializeObject<List<PizzaModel>>(response));
            listViewPizza.ItemsSource = pizzas;
        }

        private void DeletePizza_Click(object sender, RoutedEventArgs e)
        {
            if (listViewPizza.SelectedItem != null)
            {
                var selected = listViewPizza.SelectedItem as PizzaModel;

                HttpWebRequest webRequest = WebRequest.CreateHttp($"https://localhost:44361/api/pizzas/delete/{selected.Name}/{admin.Login}:{admin.Password}");
                webRequest.Method = "DELETE";
                var webResponse = webRequest.GetResponse();
                string response = "";
                using (Stream stream = webResponse.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        response = reader.ReadToEnd();
                    }
                }
                // listViewPizza.SelectedItem = null;
                pizzas.Remove(selected);

                listViewPizza.SelectedItem = null;
                listViewPizza.Items.Refresh();
            }
        }

        private void ListViewPizza_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((sender as ListView).SelectedItem != null)
            {
                var model = (sender as ListView).SelectedItem as UserModel;
                this.NavigationService.Navigate(new EditUserPage(model, admin));
            }
        }

        private void GoBackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
