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
    /// Логика взаимодействия для RefisterForm.xaml
    /// </summary>
    public partial class RefisterForm : Window
    {
        public RefisterForm()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Confirm Password Failed");
                return;
            }
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/client/add");
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            UserModel usermodel = new UserModel();
            usermodel.Password = PasswordBox.Password.ToString();
            usermodel.Number = PhoneBox.Text.ToString();
            usermodel.Login = LoginBox.Text;
            usermodel.LastName = SurnameBox.Text;
            usermodel.FirstName = NameBox.Text;
           
            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(usermodel));
                }
            }
            string response = "";
            WebResponse web = httpWebRequest.GetResponse();
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if ("Kozak poplakav" == response)
                MessageBox.Show("Pagana");
            else if ("Phone number is busy" == response)
                MessageBox.Show("Number or login is busy");
            else
            {
                MessageBox.Show("Register SUccsesfuly");
                this.Close();
            }


        }
    }
}
