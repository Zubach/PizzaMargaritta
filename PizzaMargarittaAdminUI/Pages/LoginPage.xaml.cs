using Newtonsoft.Json;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Login();
        }

        async Task Login()
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp($"https://localhost:44361/api/admins/{LoginTextBox.Text}:{PasswordBox.Password}");
            webRequest.Method = "GET";
            WebResponse webResponse = (await webRequest.GetResponseAsync());
            string response = "";
            using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if (response != "BAN")
            {
                AdminModel admin = JsonConvert.DeserializeObject<AdminModel>(response);


                this.NavigationService.Navigate(new MainPage(admin));
               


            }
        }
    }
}
