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
using System.Windows.Shapes;

namespace PizzaMargarittaAdminUI
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        UserModel user;
        AdminModel admin;
        public EditUserWindow()
        {
            InitializeComponent();
        }

        public EditUserWindow(UserModel model, AdminModel adminModel)
        {
            InitializeComponent();
            user = model;

            admin = adminModel;

            LogintextBox.Text = user.Login;
            FirstNameTextBox.Text = user.FirstName;
            LastNameTextBox.Text = user.LastName;
            NumberTextBox.Text = user.Number;
            RadioButtonFalse.IsChecked = !user.IsBanned;

          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            HttpWebRequest httpWebRequest= WebRequest.CreateHttp($"https://localhost:44361/api/clients/{user.Login}/edit/{admin.Login}:{admin.Password}");
            httpWebRequest.Method = "PUT";
            UserModel userModel = new UserModel()
            {
                Login = LogintextBox.Text,
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Number = NumberTextBox.Text,
                IsBanned = (RadioButtonTrue.IsChecked == true) ? true : false
            };

            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(userModel));
                }
            }

            string response = "";
            WebResponse web = httpWebRequest.GetResponse();
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            MessageBox.Show(response);
        }
    }
}
