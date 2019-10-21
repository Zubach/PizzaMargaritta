using Newtonsoft.Json;
using PizzaMargaritta.Models;
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
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/clients/{LoginBox.Text}:{PasswordBox.Password.ToString()}");
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json";
            WebResponse web = httpWebRequest.GetResponse();
            string response = "";
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            UserModel currentUser;
            if ("Login failed" == response)
                MessageBox.Show("Hello");

            else
            {
                currentUser = JsonConvert.DeserializeObject<UserModel>(response);
         
                UserWindow uw = new UserWindow(currentUser);
                uw.Owner = this;
                this.Hide();
                uw.Show();

            }

           
        }
    }
}
