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
                listViewPizza.Items.Remove(selected);

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
    }
}
