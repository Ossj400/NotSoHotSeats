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

namespace NotSoHotSeats_
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        Manager _manager;
        public Register()
        {
            InitializeComponent();
            _manager = new Manager();
        }


        private void BackButton(object sender, RoutedEventArgs e)
        {
            LoginWindow login = new LoginWindow();
            GetWindow(sender as Button).Close();
            login.Show();
        }

        private void RegisterBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _manager.RegisterUser(FirstName.Text, LastName.Text, Login.Text, Password.Text);
                dialogHost.IsOpen = true;
                var txtBlock = dialogHost.FindName("textBlockDialog") as TextBlock;
                txtBlock.Text = "Zarejestrowano";
            }

            catch (Exception ex)
            {
                dialogHost.IsOpen = true;
                var txtBlock = dialogHost.FindName("textBlockDialog") as TextBlock;
                txtBlock.Text = ex.Message;
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
