using NotSoHotSeats_.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NotSoHotSeats_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SeatsDBContext dataContext;
        Manager _manager;
        public DateTime date { get ; set; }
        public MainWindow()
        {
            InitializeComponent();
            SetDate();
            _manager = new Manager();
            dataContext = new SeatsDBContext();
            _manager.SetContentForSeats(date, this);
        }

        private void SetDate()
        {
            date = DateTime.Now;
        }

        private void MouseEnters(object sender, RoutedEventArgs e)
        {
            _manager.VisualizationWhenMouseEnters(date, sender as ContentControl);
        }
        private void MouseLeaves(object sender, RoutedEventArgs e)
        {
            _manager.VisualizationWhenMouseEnters(date, sender as ContentControl, true);
            _manager.SetContentForSeats(date, GetWindow(this));
        }

        private childItem FindVisualChild<childItem>(DependencyObject obj)
        where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                {
                    return (childItem)child;
                }
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }


        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var clickedButton = sender as ContentControl;
                Seat seat = _manager.GetSeat(clickedButton.Name.ToString());
                string message = _manager.CreateReservation(date, seat);
                dialogHost.IsOpen = true;
                var txtBlock = dialogHost.FindName("textBlockDialog") as TextBlock;
                txtBlock.Text =message;
                SoundPlayer sound = new SoundPlayer(Environment.CurrentDirectory + "\\reservation.wav");
                sound.Play();
            }
            catch (Exception ex)
            {
                dialogHost.IsOpen = true;
                var txtBlock = dialogHost.FindName("textBlockDialog") as TextBlock;
                txtBlock.Text = ex.Message;
                SoundPlayer sound = new SoundPlayer(Environment.CurrentDirectory + "\\wrong.wav");
                sound.Play();
            }

        }

        private void MaterialCalendar_SelectedDatesChanged(object sender, RoutedEventArgs e)
        {
            SetDate();
            var clickedButton = sender as ContentControl;
            date = Convert.ToDateTime(sender.ToString());
            _manager.SetContentForSeats(date, GetWindow(this));
            (FindName("TxtBlockDate") as TextBlock).Text = date.ToShortDateString();
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            SetDate();
            Binding myNewBindDef = new Binding();
            myNewBindDef.Mode = BindingMode.OneWay;
            myNewBindDef.Source = date.ToShortDateString();
            BindingOperations.SetBinding(FindName("TxtBlockDate") as TextBlock, TextBlock.TextProperty, myNewBindDef);
            _manager.SetContentForSeats(date, sender as DependencyObject);

        }

    }


    
}
