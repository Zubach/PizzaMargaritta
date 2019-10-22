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
using System.Windows.Shapes;

namespace PizzaMargarittaAdminUI
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpWebRequest webRequest = WebRequest.CreateHttp($"https://localhost:44361/api/admins/{LoginTextBox.Text}:{PasswordBox.Password}");
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            string response = "";
            using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if(response != "BAN")
            {
                AdminModel admin = JsonConvert.DeserializeObject<AdminModel>(response);
                MainWindow window = new MainWindow(admin);
                window.Show();
                this.Close();
            }
        }
    }
}
