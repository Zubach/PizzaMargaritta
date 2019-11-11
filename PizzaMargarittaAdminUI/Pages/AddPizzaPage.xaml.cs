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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PizzaMargarittaAdminUI.Pages
{
    /// <summary>
    /// Interaction logic for AddPizzaPage.xaml
    /// </summary>
    public partial class AddPizzaPage : Page
    {
        AdminModel admin;
        public AddPizzaPage()
        {
            InitializeComponent();
        }

        public AddPizzaPage(AdminModel admin)
        {
            InitializeComponent();
            this.admin = admin;
        }

        public string ImageToBase64(string path)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PathTextBox.Text != "")
            {
                PizzaModel model = new PizzaModel()
                {
                    Name = NameTextBox.Text,
                    Price = decimal.Parse(PriceTextBox.Text),
                    Description = DescriptionTextBox.Text,
                    Image = ImageToBase64(PathTextBox.Text)
                };

                HttpWebRequest httpWebRequest = WebRequest.CreateHttp($"https://localhost:44361/api/pizzas/add/{admin.Login}:{admin.Password}");
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                
              

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
               
            }
            this.NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
            
         }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            var result = openFile.ShowDialog();
            if(result == DialogResult.OK)
            {
                PathTextBox.Text = openFile.FileName;
                var converter = new ImageSourceConverter();
                
                Image.Source = (ImageSource)converter.ConvertFromString(PathTextBox.Text);

            }

        }
    }
}
