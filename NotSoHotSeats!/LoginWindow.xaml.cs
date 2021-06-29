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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        Manager _manager;
        public LoginWindow()
        {
            InitializeComponent();
            _manager = new Manager();
        }

        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void LoginBtnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = _manager.Login(Login.Text, Password.Password);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                GetWindow(sender as Button).Close();
            }
            catch(Exception ex)
            {
                dialogHost.IsOpen = true;
                var txtBlock = dialogHost.FindName("textBlockDialog") as TextBlock;
                txtBlock.Text = ex.Message;
            }

        }

        private void RegisterButton(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            GetWindow(sender as Button).Close();
            register.Show();
        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
        
    }
}
