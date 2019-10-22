using PizzaMargaritta.Models;
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
using System.Windows.Shapes;

namespace PizzaMargarittaAdminUI
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        UserModel user;
        public EditUserWindow()
        {
            InitializeComponent();
        }

        public EditUserWindow(UserModel model)
        {
            InitializeComponent();
            user = model;

            LogintextBox.Text = user.Login;
            FirstNameTextBox.Text = user.FirstName;
            LastNameTextBox.Text = user.LastName;
            NumberTextBox.Text = user.Number;
            RadioButtonFalse.IsChecked = !user.IsBanned;

          
        }

    }
}
