using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;

namespace XamlTest
{
    class XamlWindow : Window
    {
        AnimationClock myClock;
        public XamlWindow()
        {
            Dispatcher d1 = Dispatcher.CurrentDispatcher;
            Dispatcher d2 = Dispatcher.CurrentDispatcher;


            SolidColorBrush solidBrush = new SolidColorBrush();

            StackPanel stack = new StackPanel();
            Label label = new Label();
            label.Content = "Hello world";
            label.Margin = new Thickness(20, 0, 0, 0);
            stack.Children.Add(label);
            label.ForeBrush = solidBrush;

            label = new Label();
            label.Content = "Hello world";
            label.HorizontalAlignment = HorizontalAlignment.Right;
            label.Margin = new Thickness(0, 0, 20, 0);
            stack.Children.Add(label);

            label = new Label();
            label.Content = "Hello world";
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Margin = new Thickness(60, 0, 0, 0);
            Font font = new Font(FontFamily.GenericSerif, 24, FontStyle.Regular);
            label.Font = font;
            stack.Children.Add(label);

            Rectangle rect = new Rectangle();
            rect.Width = 100;
            rect.Height = 100;
            rect.HorizontalAlignment = HorizontalAlignment.Right;
            GradientBrush gradient = new GradientBrush();
            gradient.StartPoint = new Point(0, .5f);
            gradient.EndPoint = new Point(1, .5f);
            gradient.GradientStops.Add(new GradientStop(new Color(1f, 1f, 0f, 1f), 0));
            gradient.GradientStops.Add(new GradientStop(new Color(0, 0, 1f, 1f), 1));
            rect.Fill = gradient;
            stack.Children.Add(rect);

            BitmapSource bitmap;
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("XamlTest.Flames.bmp"))
            {
                bitmap = BitmapSource.Create(stream);
            }
            Image image = new Image();
            image.Stretch = Stretch.None;
            image.ImageSource = bitmap;
            stack.Children.Add(image);

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = bitmap;
            brush.Stretch = Stretch.None;
            label.ForeBrush = brush;

            Content = stack;

            FloatAnimation anim = new FloatAnimation();
            anim.From = 50;
            anim.To = 300;
            anim.Duration = new Duration(TimeSpan.FromMilliseconds(10000));
            anim.RepeatBehavior = RepeatBehavior.Forever;
            anim.AutoReverse = true;
            myClock = anim.CreateClock();
            myClock.Animate(rect, Rectangle.WidthProperty);
            

            //new Thread(() =>
            //    {
            //        //Thread.Sleep(2000);
            //        Random rand = new Random();
            //        while (true)
            //        {
            //            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Send, new EmptyDelegate(() =>
            //                {
            //                    Color newColor = new Color((byte)(rand.Next() % 256), (byte)(rand.Next() % 256), (byte)(rand.Next() % 256), (byte)(rand.Next() % 256));
            //                    solidBrush.Color = newColor;
            //                    rect.Width += 5;
            //                    if (rect.Width > 300)
            //                        rect.Width = 200;
            //                }));
            //        }
            //    }
            //).Start();
        }
    }
}
