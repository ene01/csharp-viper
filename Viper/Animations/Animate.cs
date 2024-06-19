using System;
using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Vector = System.Windows.Vector;

namespace Viper.Animations
{
    public static class Animate
    {
        public static void Double(DependencyObject targetElement, DependencyProperty property, double toValue, TimeSpan duration, IEasingFunction easing = null)
        {
            if (targetElement != null && property != null)
            {
                var currentValue = (double)targetElement.GetValue(property);

                DoubleAnimation animation = new()
                {
                    From = currentValue,
                    To = toValue,
                    Duration = duration
                };

                if (easing != null)
                {
                    animation.EasingFunction = easing;
                }

                StartAnimation(targetElement, property, animation);
            }
        }

        public static void Color(DependencyObject targetElement, DependencyProperty property, Color toColor, TimeSpan duration, IEasingFunction easing = null, bool useCurrentValueAsFromValue = false)
        {
            if (targetElement != null && property != null && targetElement is Animatable)
            {
                var currentValue = (Color)targetElement.GetValue(property);

                ColorAnimation animation = new()
                {
                    From = currentValue,
                    To = toColor,
                    Duration = duration
                };

                if (easing != null)
                {
                    animation.EasingFunction = easing;
                }

                StartAnimation(targetElement, property, animation);
            }
        }

        public static void Point(DependencyObject targetElement, DependencyProperty property, Point toPoint, Duration duration, IEasingFunction easing = null)
        {
            if (targetElement != null && property != null && targetElement is Animatable)
            {
                var currentValue = (Point)targetElement.GetValue(property);

                PointAnimation animation = new PointAnimation
                {
                    From = currentValue,
                    To = toPoint,
                    Duration = duration
                };

                if (easing != null)
                {
                    animation.EasingFunction = easing;
                }

                StartAnimation(targetElement, property, animation);
            }
        }

        public static void AnimateRect(DependencyObject targetElement, DependencyProperty property, Rect toValue, Duration duration, IEasingFunction easing = null)
        {
            if (targetElement != null && property != null)
            {
                var currentValue = (Rect)targetElement.GetValue(property);

                RectAnimation animation = new RectAnimation
                {
                    From = currentValue,
                    To = toValue,
                    Duration = duration
                };

                if (easing != null)
                {
                    animation.EasingFunction = easing;
                }

                StartAnimation(targetElement, property, animation);
            }
        }

        public static void AnimateThickness(DependencyObject targetElement, DependencyProperty property, Thickness toValue, Duration duration, IEasingFunction easing = null)
        {
            if (targetElement != null && property != null)
            {
                var currentValue = (Thickness)targetElement.GetValue(property);

                ThicknessAnimation animation = new ThicknessAnimation
                {
                    From = currentValue,
                    To = toValue,
                    Duration = duration
                };

                if (easing != null)
                {
                    animation.EasingFunction = easing;
                }

                StartAnimation(targetElement, property, animation);
            }
        }

        private static void StartAnimation(DependencyObject dependencyObject, DependencyProperty dependencyProperty, AnimationTimeline animation)
        {
            switch (dependencyObject)
            {
                case UIElement uiElement:
                    uiElement.BeginAnimation(dependencyProperty, animation);
                    break;
                case Geometry geometry:
                    geometry.BeginAnimation(dependencyProperty, animation);
                    break;
                case Brush brush:
                    brush.BeginAnimation(dependencyProperty, animation);
                    break;
                case Transform transform:
                    transform.BeginAnimation(dependencyProperty, animation);
                    break;
                case Animatable animatable:
                    animatable.BeginAnimation(dependencyProperty, animation);
                    break;
                default:
                    break;
            }
        }
    }
}
