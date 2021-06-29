using NotSoHotSeats_.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotSoHotSeats_
{
    class Manager
    {
        SeatsDBContext context = new SeatsDBContext();
        List<DependencyObject> neededChilds = new List<DependencyObject>();

        public User Login(string username, string password)
        {
            var user = context.Login(username, password);

            if (user == null)
                throw new Exception("Złe hasło lub login!");
            context.AddLoggedUser(user.IdUser);

            return user;
        }

        public User RegisterUser(string firstName, string lastName, string login, string password)
        {
            var user = context.RegisterUser(firstName, lastName, login, password);
            if (user == null)
                throw new Exception("Nie udało się stworzyć użyszkodnika");

            return user;
        }

        public string CreateReservation(DateTime pickedDate, Seat seat)
        {
            var result = context.CreateReservation(UserLogged.IdUser, pickedDate, seat);
            if (String.IsNullOrEmpty(result))
                throw new Exception("Nie udało się stworzyć rezewacji");

            return result;
        }

        public Seat GetSeat(string seatSymbol)
        {
            var result = context.GetSeat(seatSymbol);
            if (result == null)
                throw new Exception("Nie udało się stworzyć rezewacji");

            return result;
        }   

        public void SetContentForSeats(DateTime pickedDate, DependencyObject obj)
        {

            var contentControls = ContentControls(obj);
            Thickness thicknessTaken = new Thickness(2);
            Thickness thickness = new Thickness(1);
            foreach (ContentControl control in contentControls)
            {
                SolidColorBrush brush = new SolidColorBrush();
                SolidColorBrush borderBrush = new SolidColorBrush();
                Seat seat = context.GetSeat(control.Name);
                bool seatTaken= context.IsSeatTaken(seat, pickedDate);
                var takebBy = context.GetUserFromSeat(seat, pickedDate);

                var levelBeforeBorder = VisualTreeHelper.GetChild(control, 0);
                var border = VisualTreeHelper.GetChild(levelBeforeBorder, 0) as Border;
                var childForGetTextbox = border.Child;
                var txtBlock = VisualTreeHelper.GetChild(childForGetTextbox, 0) as TextBlock;
                var txtBox = VisualTreeHelper.GetChild(childForGetTextbox, 1) as TextBox;

                if (!seatTaken)
                {
                    txtBlock.Text = seat.SeatSymbol;
                    txtBox.Text = "";
                    brush.Color = Colors.Green;
                    border.BorderThickness = thickness;
                    border.BorderBrush= brush;
                }
                
                if (seatTaken && takebBy.IdUser != UserLogged.IdUser)
                {
                    var user = context.GetUserFromSeat(seat, pickedDate);
                    txtBlock.Text = seat.SeatSymbol;
                    txtBox.Text = (user.FirstName + ' ' + user.LastName);
                    brush.Color = Colors.MediumVioletRed;
                    border.BorderThickness = thicknessTaken;
                    border.BorderBrush = brush;
                }
                if (seatTaken && takebBy.IdUser == UserLogged.IdUser)
                {
                    var user = context.GetUserFromSeat(seat, pickedDate);
                    txtBlock.Text = seat.SeatSymbol;
                    txtBox.Text = (user.FirstName + ' ' + user.LastName);
                    brush.Color = Colors.Aquamarine;
                    border.BorderThickness = thicknessTaken; 
                    border.BorderBrush = brush;
                }
            }
        }
        public void SetContentForSeat(DateTime pickedDate, DependencyObject obj)
        {

            var control = obj as ContentControl;
            Thickness thicknessTaken = new Thickness(2);
            Thickness thickness = new Thickness(1);

                SolidColorBrush brush = new SolidColorBrush();
                SolidColorBrush borderBrush = new SolidColorBrush();
                Seat seat = context.GetSeat(control.Name);
                bool seatTaken = context.IsSeatTaken(seat, pickedDate);
                var takebBy = context.GetUserFromSeat(seat, pickedDate);

                var levelBeforeBorder = VisualTreeHelper.GetChild(control, 0);
                var border = VisualTreeHelper.GetChild(levelBeforeBorder, 0) as Border;
                var childForGetTextbox = border.Child;
                var txtBlock = VisualTreeHelper.GetChild(childForGetTextbox, 0) as TextBlock;
                var txtBox = VisualTreeHelper.GetChild(childForGetTextbox, 1) as TextBox;

                if (!seatTaken)
                {
                    txtBlock.Text = seat.SeatSymbol;
                    txtBox.Text = "";
                    brush.Color = Colors.Green;
                    border.BorderThickness = thickness;
                    border.BorderBrush = brush;
                }

                if (seatTaken && takebBy.IdUser != UserLogged.IdUser)
                {
                    var user = context.GetUserFromSeat(seat, pickedDate);
                    txtBlock.Text = seat.SeatSymbol;
                    txtBox.Text = (user.FirstName + ' ' + user.LastName);
                    brush.Color = Colors.MediumVioletRed;
                    border.BorderThickness = thicknessTaken;
                    border.BorderBrush = brush;
                }
                if (seatTaken && takebBy.IdUser == UserLogged.IdUser)
                {
                    var user = context.GetUserFromSeat(seat, pickedDate);
                    txtBlock.Text = seat.SeatSymbol;
                    txtBox.Text = (user.FirstName + ' ' + user.LastName);
                    brush.Color = Colors.Aquamarine;
                    border.BorderThickness = thicknessTaken;
                    border.BorderBrush = brush;
                }

        }

        public void VisualizationWhenMouseEnters(DateTime pickedDate, DependencyObject obj, bool mouseLeaves = false)
        {

            SolidColorBrush brush = new SolidColorBrush();
            SolidColorBrush borderBrush = new SolidColorBrush();
            var seatBtn = obj as ContentControl;
            var levelBeforeBorder = VisualTreeHelper.GetChild(seatBtn, 0);
            var border = VisualTreeHelper.GetChild(levelBeforeBorder, 0) as Border;
            Seat seat = context.GetSeat(seatBtn.Name);
            bool seatTaken = context.IsSeatTaken(seat, pickedDate);
            var takebBy = context.GetUserFromSeat(seat, pickedDate);
            Thickness thicknessTaken = new Thickness(2);
            Thickness thickness = new Thickness(1);
            if (!seatTaken)
            {
                ColorAnimationUsingKeyFrames myColorAnimation = new ColorAnimationUsingKeyFrames();
                if (mouseLeaves)
                {
                    Brush newColor = border.Background;
                    SolidColorBrush newBrush = (SolidColorBrush)newColor;
                    Color myColorFromBrush = newBrush.Color;
                    myColorAnimation.ColorAnimationWhenMouseLeave(myColorFromBrush);
                }
                else
                    myColorAnimation.ColorAnimationWhenBtnEmpty();

                brush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
                brush.Color = Colors.Green;
                border.BorderBrush = brush;
                brush.Color = Colors.GreenYellow;
                border.Background = brush;
                border.UpdateLayout();

            }

            if (seatTaken)
            {
                 ColorAnimationUsingKeyFrames myColorAnimation = new ColorAnimationUsingKeyFrames();
                 if (mouseLeaves && takebBy.IdUser != UserLogged.IdUser)
                 {
                     Brush newColor = border.Background;
                     SolidColorBrush newBrush = (SolidColorBrush)newColor;
                     Color myColorFromBrush = newBrush.Color;
                     myColorAnimation.ColorAnimationWhenMouseLeave(myColorFromBrush);
                    border.BorderThickness = thicknessTaken;
                }
                if (mouseLeaves && takebBy.IdUser == UserLogged.IdUser)
                {
                    Brush newColor = border.Background;
                    SolidColorBrush newBrush = (SolidColorBrush)newColor;
                    Color myColorFromBrush = newBrush.Color;
                    myColorAnimation.ColorAnimationWhenMouseLeaveUserLoggedReservation(myColorFromBrush);
                    border.BorderThickness = thicknessTaken;
                }

                if (!mouseLeaves && takebBy.IdUser != UserLogged.IdUser)
                     myColorAnimation.ColorAnimationWhenBtnReserved();

                if (!mouseLeaves && takebBy.IdUser == UserLogged.IdUser)
                    myColorAnimation.ColorAnimationWhenBtnUnReserved();

                brush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
                 brush.Color = Colors.Green;
                 border.BorderBrush = brush;
                 brush.Color = Colors.GreenYellow;
                 border.Background = brush;
            }
            
        }
        public void VisualizationWhenMouseLeaves(DateTime pickedDate, DependencyObject obj)
        {

            SolidColorBrush brush = new SolidColorBrush();
            SolidColorBrush borderBrush = new SolidColorBrush();
            var seatBtn = obj as ContentControl;
            var levelBeforeBorder = VisualTreeHelper.GetChild(seatBtn, 0);
            var border = VisualTreeHelper.GetChild(levelBeforeBorder, 0) as Border;
            Seat seat = context.GetSeat(seatBtn.Name);
            bool seatTaken = context.IsSeatTaken(seat, pickedDate);
            if (!seatTaken)
            {
                ColorAnimationUsingKeyFrames myColorAnimation = new ColorAnimationUsingKeyFrames();
                myColorAnimation.ColorAnimationWhenBtnEmpty();
                brush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
                brush.Color = Colors.Green;
                border.BorderBrush = brush;
                brush.Color = Colors.GreenYellow;
                border.Background = brush;
            }

            if (seatTaken)
            {
                ColorAnimationUsingKeyFrames myColorAnimation = new ColorAnimationUsingKeyFrames();
                myColorAnimation.ColorAnimationWhenBtnReserved();
                brush.BeginAnimation(SolidColorBrush.ColorProperty, myColorAnimation);
                brush.Color = Colors.Green;
                border.BorderBrush = brush;
                brush.Color = Colors.GreenYellow;
                border.Background = brush;
            }

        }

        private childItem FindVisualChildrensOfContentControl<childItem>(DependencyObject obj)
        where childItem : DependencyObject
        {
            List<DependencyObject> childs = new List<DependencyObject>();
            var on = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);

                childItem childOfChild = FindVisualChildrensOfContentControl<childItem>(child);
                if (child != null && child.GetType() == typeof(ContentControl))
                {
                    childs.Add(child);
                }
            }
            foreach (ContentControl child in childs)
            {
                if (child.Name.Length == 1)
                    neededChilds.Add(child);
            }
            return null;
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
        public List<DependencyObject> ContentControls(DependencyObject obj)
        {
            neededChilds.Clear();
            FindVisualChildrensOfContentControl<ContentControl>(obj);
            return neededChilds;
        }
    }
}
