using System;
using System.Collections.Generic;
using System.Linq;
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

namespace PizzaMargarittaUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoginForm lf = new LoginForm();
            lf.Owner = this;
            lf.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RefisterForm rg = new RefisterForm();
            rg.Owner = this;
            rg.Show();
        }
    }
}
