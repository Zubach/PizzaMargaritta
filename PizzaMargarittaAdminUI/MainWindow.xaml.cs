using Newtonsoft.Json;
using PizzaMargaritta.Models;
using PizzaMargarittaAdminUI.Models;
using PizzaMargarittaAdminUI.Pages;
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
        private AdminModel admin;

        public MainWindow()
        {
            InitializeComponent();
           
            
        }
        public MainWindow(AdminModel admin)

        {
            InitializeComponent();
            this.admin = admin;
            frame.Navigate(new MainPage(admin));
        }
       
        
    }
}
