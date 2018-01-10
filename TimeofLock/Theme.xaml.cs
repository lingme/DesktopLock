using System;
using System.Collections.Generic;
using System.IO;
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

namespace TimeofLock
{
    /// <summary>
    /// Theme.xaml 的交互逻辑
    /// </summary>
    public partial class Theme : Window
    {
        public Theme()
        {
            InitializeComponent();
        }

        private Theme _iWindows;
        public Theme IWindows
        {
            get
            {
                return _iWindows;
            }
            set
            {
                _iWindows = value;
            }
        }

        public static bool onoff = true;

        /////////////////////////////////////////////////////////////////////////////////窗口拖动函数
        #region
        private void R1_MouseMove(object sender, MouseEventArgs e)
        {
            Point pp = Mouse.GetPosition(this);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            onoff = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            bj_config.read_config_txt();
            onoff = false;
            switch(bj_config.combo[11])
            {
                case "1":B5.Margin = new Thickness(49, 72, 0, 0);
                         B5.Visibility = Visibility.Visible;
                            break;
                case "2":B5.Margin = new Thickness(210, 72, 0, 0);
                         B5.Visibility = Visibility.Visible;
                            break;
                case "3":
                         B5.Margin = new Thickness(49, 168, 0, 0);
                         B5.Visibility = Visibility.Visible;
                            break;
            }

            if(File.Exists(bj_config.combo[12]))
            {
                image4.Source = new BitmapImage(new Uri(bj_config.combo[12], UriKind.Absolute));
                B4.BorderThickness = new Thickness(1, 1, 1, 1);
                if (bj_config.combo[11] == "4")
                {
                    B5.Margin = new Thickness(210, 168, 0, 0);
                    B5.Visibility = Visibility.Visible;
                }
            }
        }

        private void E1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void E2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            B5.Margin = new Thickness(49, 72, 0, 0);
            bj_config.edit_config_txt(12, "1");

            this.Height = 290;
            R2.Height = 211;
            border1.Height = 241;
            close.Visibility = Visibility.Hidden;
            enter.Visibility = Visibility.Hidden;
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            B5.Margin = new Thickness(210, 72, 0, 0);
            bj_config.edit_config_txt(12, "2");

            this.Height = 290;
            R2.Height = 211;
            border1.Height = 241;
            close.Visibility = Visibility.Hidden;
            enter.Visibility = Visibility.Hidden;
        }

        private void image3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            B5.Margin = new Thickness(49, 168, 0, 0);
            bj_config.edit_config_txt(12, "3");

            this.Height = 290;
            R2.Height = 211;
            border1.Height = 241;
            close.Visibility = Visibility.Hidden;
            enter.Visibility = Visibility.Hidden;
        }

        private void image4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(bj_config.combo[12]))
            {
                Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "图像文件|*.png;*.jpg;*.bmp;*.jpeg;*.gif";
                if (dialog.ShowDialog() == true)
                {
                    if (File.Exists(dialog.FileName))
                    {
                        image4.Source = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));
                        bj_config.edit_config_txt(12, "4");
                        bj_config.edit_config_txt(13, dialog.FileName);
                        B5.Margin = new Thickness(210, 168, 0, 0);
                        B4.BorderThickness = new Thickness(1, 1, 1, 1);
                    }
                }
            }
            else
            {
                this.Height = 330;
                R2.Height = 251;
                border1.Height = 281;

                close.Visibility = Visibility.Visible;
                enter.Visibility = Visibility.Visible;
                close.Margin = new Thickness(120, 266, 0, 0);
                enter.Margin = new Thickness(257, 266, 0, 0);

                bj_config.edit_config_txt(12, "4");
                B5.Margin = new Thickness(210, 168, 0, 0);
            }
            
        }

        private void close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            image4.Source = new BitmapImage(new Uri("/image/theme_image4.png", UriKind.Relative));
            bj_config.edit_config_txt(12, "1");
            bj_config.edit_config_txt(13, " ");
            B5.Margin = new Thickness(49, 72, 0, 0);

            this.Height = 290;
            R2.Height = 211;
            border1.Height = 241;
            close.Visibility = Visibility.Hidden;
            enter.Visibility = Visibility.Hidden;
            B4.BorderThickness = new Thickness(0,0,0,0);
        }

        private void enter_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "图像文件|*.png;*.jpg;*.bmp;*.jpeg;*.gif";
            if (dialog.ShowDialog() == true)
            {
                if (File.Exists(dialog.FileName))
                {
                    image4.Source = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));
                    bj_config.edit_config_txt(12, "4");
                    bj_config.edit_config_txt(13, dialog.FileName);
                }
            }
        }
    }
}
