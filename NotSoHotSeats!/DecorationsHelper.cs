using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NotSoHotSeats_
{
    static class DecorationsHelper
    {
        public static ColorAnimationUsingKeyFrames CustomColorAnimation(this ColorAnimationUsingKeyFrames colorAnimation, Color colorFrom, Color colorTo, int timeInMiliseconds, Color? endingColor = null, double? repeatBehaviorDuration = null, bool? isReversed = false)
        {
            colorAnimation.Duration = TimeSpan.FromMilliseconds(timeInMiliseconds);
            SolidColorBrush animatedBrush = new SolidColorBrush();
            animatedBrush.Color = Color.FromArgb(255, 0, 255, 0);
            colorAnimation.KeyFrames.Add(
            new LinearColorKeyFrame(
                colorFrom, // Target value (KeyValue)
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.0))) // KeyTime
            );
                        colorAnimation.KeyFrames.Add(
            new LinearColorKeyFrame(
                colorTo,
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0.5)))
            );

            if(endingColor != null)
            colorAnimation.KeyFrames.Add(
            new LinearColorKeyFrame(
                endingColor.HasValue ? endingColor.Value : Colors.White, 
                KeyTime.FromTimeSpan(TimeSpan.FromSeconds(7.0))) 
            );

            colorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(timeInMiliseconds));
            colorAnimation.AutoReverse = isReversed.Value;
            RepeatBehavior repeatBehavior = new RepeatBehavior( repeatBehaviorDuration.HasValue ? repeatBehaviorDuration.Value : 1);
            colorAnimation.RepeatBehavior = repeatBehavior;
            return colorAnimation;
        }

        public static ColorAnimationUsingKeyFrames ColorAnimationWhenBtnEmpty(this ColorAnimationUsingKeyFrames colorAnimation)
        {
            return colorAnimation.CustomColorAnimation(Colors.Transparent, Colors.AliceBlue, 1000);
        }
        public static ColorAnimationUsingKeyFrames ColorAnimationWhenBtnReserved(this ColorAnimationUsingKeyFrames colorAnimation)
        {
            return colorAnimation.CustomColorAnimation(Colors.IndianRed, Colors.HotPink, 500,null,3,true);
        }
        public static ColorAnimationUsingKeyFrames ColorAnimationWhenBtnClicked(this ColorAnimationUsingKeyFrames colorAnimation)
        {
            return colorAnimation.CustomColorAnimation(Colors.Transparent, Colors.Green, 2000);
        }
        public static ColorAnimationUsingKeyFrames ColorAnimationWhenBtnUnReserved(this ColorAnimationUsingKeyFrames colorAnimation)
        {
            return colorAnimation.CustomColorAnimation(Colors.GreenYellow, Colors.LightSeaGreen, 500, null, 3, true);
        }
        public static ColorAnimationUsingKeyFrames ColorAnimationWhenMouseLeave(this ColorAnimationUsingKeyFrames colorAnimation, Color colorFromEndingOfBeforeAnim)
        {

            return colorAnimation.CustomColorAnimation(colorFromEndingOfBeforeAnim, Colors.Transparent, 1000);
        }
        public static ColorAnimationUsingKeyFrames ColorAnimationWhenMouseLeaveUserLoggedReservation(this ColorAnimationUsingKeyFrames colorAnimation, Color colorFromEndingOfBeforeAnim)
        {

            return colorAnimation.CustomColorAnimation(colorFromEndingOfBeforeAnim, Colors.Transparent, 1000);
        }
    }
}
