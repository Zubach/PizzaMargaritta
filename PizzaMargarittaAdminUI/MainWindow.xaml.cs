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

namespace PizzaMargarittaAdminUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<UserModel> users = new List<UserModel>();
        AdminModel admin;
        public MainWindow()
        {
            InitializeComponent();
           
            
        }
        public MainWindow(AdminModel model)
        {
            InitializeComponent();
            admin = model;
            HttpWebRequest webRequest = WebRequest.CreateHttp($"https://localhost:44361/api/clients/get/{admin.Login}:{admin.Password}");
            webRequest.Method = "GET";
            WebResponse webResponse = webRequest.GetResponse();
            string response = "";
            using (Stream stream = webResponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream);
                response = reader.ReadToEnd();
            }
            if (response != "Ti ne admin")
            {
                users = JsonConvert.DeserializeObject<List<UserModel>>(response);
                listView.ItemsSource = users;

            }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if((sender as ListView).SelectedItem != null){
              var model =  (sender as ListView).SelectedItem as UserModel;
                EditUserWindow window = new EditUserWindow(model,admin);
                window.Show();
            }
        }
    }
}
