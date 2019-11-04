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
    /// Interaction logic for EditPizzaWindow.xaml
    /// </summary>
    public partial class EditPizzaWindow : Window
    {
        AdminModel admin;
        PizzaModel model;
        public EditPizzaWindow()
        {
            InitializeComponent();

        }


        public EditPizzaWindow(AdminModel admin, PizzaModel model)
        {
            InitializeComponent();

            var converter = new ImageSourceConverter();
            this.admin = admin;
            this.model = model;
            NameTextBox.Text = model.Name;
            PriceTextBox.Text = model.Price.ToString();
            Image.Source = (ImageSource)converter.ConvertFromString(@"https://localhost:44361/api/content/pizzaimages/" + model.Image);
             
            DescriptionTextBox.Text = model.Description;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/pizzas/edit/{model.Name}/{admin.Login}:{admin.Password}");
            httpWebRequest.Method = "PUT";

            model.Name = NameTextBox.Text;
            model.Price = decimal.Parse(PriceTextBox.Text);
            model.Description = DescriptionTextBox.Text;

            using (Stream stream = httpWebRequest.GetRequestStream())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(model));
                }
            }

            string response = "";
            WebResponse web = httpWebRequest.GetResponse();
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
