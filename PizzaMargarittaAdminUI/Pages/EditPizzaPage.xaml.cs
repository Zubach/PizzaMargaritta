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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PizzaMargarittaAdminUI.Pages
{
    /// <summary>
    /// Interaction logic for EditPizzaPage.xaml
    /// </summary>
    public partial class EditPizzaPage : Page
    {
        private AdminModel admin;
        private PizzaModel model;

        public EditPizzaPage()
        {
            InitializeComponent();
        }

        public EditPizzaPage(AdminModel admin,PizzaModel model)
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
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Edit();
        }

        public async Task Edit()
        {
            HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/pizzas/edit/{model.Name}/{admin.Login}:{admin.Password}");
            httpWebRequest.Method = "PUT";

            model.Name = NameTextBox.Text;
            model.Price = decimal.Parse(PriceTextBox.Text);
            model.Description = DescriptionTextBox.Text;

            using (Stream stream = await httpWebRequest.GetRequestStreamAsync())
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(JsonConvert.SerializeObject(model));
                }
            }

            string response = "";
            WebResponse web = await httpWebRequest.GetResponseAsync();
            using (Stream stream = web.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            this.NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
