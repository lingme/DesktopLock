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
using System.Windows.Threading;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Windows.Navigation;

namespace TimeofLock
{
    /// <summary>
    /// Welcome.xaml 的交互逻辑
    /// </summary>
    public partial class Welcome : Window
    {
        public Welcome()
        {
            InitializeComponent();
            { 
            }

            //////////////////////////////////////////////////创建配置文件/////////////////////////////////////////////////

            #region
            bj_config.read_config_txt();
            if (bj_config.is_have()&&bj_config.combo[4]!="anto")
            {
                main_window();
                if(bj_config.combo[0]=="false"&&bj_config.combo[1]=="true")
                {
                    Window1 kk = new Window1();
                    kk.Show();
                }
            }

            if(!bj_config.is_have())
            {
                File.Create(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "tol.config.txt").Close();
                StreamWriter wr = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "tol.config.txt",true);
                wr.WriteLine("true");            // 1.是否正常关闭         （true：是            false：否）
                wr.WriteLine("false");           // 2.强力模式            （true：开启          false：关闭）
                wr.WriteLine("true");            // 3.锁屏密码模式         （true：时间密码      false：自定义密码） 
                wr.WriteLine("anto");            // 4.自定义锁屏密码       （自定义密码 MD5.32加密）
                wr.WriteLine("anto");            // 5.管理员密码           （管理员密码 MD5.32加密）
                wr.WriteLine(" ");               // 6.锁屏留言            
                wr.WriteLine("1");               // 7.锁屏留言字体颜色   
                wr.WriteLine("false");           // 8.计时器-锁屏         (true：开启    false：关闭）
                wr.WriteLine("0");               // 9.时间 
                wr.WriteLine("false");           //10.计时器-关机         (true：开启    false：关闭）
                wr.WriteLine("0");               //11.时间
                wr.WriteLine("1");               //12.主题模式            （1、2、3默认     4：自定义）
                wr.WriteLine("anto");            //13.自定义壁纸路径
                wr.Flush();
                wr.Close();
            }
            #endregion

        }
        public static int lock_m = 0;
        public static bool lock_timer = false;
        public static int shutdown_m = 0;
        public static bool shutdown_timer = false;

        public bool ansi = true;
        public bool tme = true;

        DispatcherTimer clock = new DispatcherTimer();

        //////////////////////////////////////////////////////关机函数/////////////////////////////////////////////////////

        #region
        public void shutdown_computer()
        {
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = "cmd.exe";
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.RedirectStandardInput = true;
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.RedirectStandardError = true;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.Start();
            myProcess.StandardInput.WriteLine("shutdown -s -f -t 0");
        }
        #endregion

        //////////////////////////////////////////////////////计时器事件/////////////////////////////////////////////////////

        #region
        public void timer_kkick(object sender, EventArgs e)
        {
            if (lock_timer)
            {
               if ((lock_m--)==0)
                {
                    if(Window1.on_or_off)
                    {
                        Window1 pp = new Window1();
                        pp.Show();
                    }
                    clock.Stop();
                    lock_timer = false;
                }
            }

            if(shutdown_timer)
            {
                if((shutdown_m--)== 0)
                {
                    shutdown_timer = false;
                    clock.Stop();
                    shutdown_computer();
                }
            }
        }
        #endregion

        //////////////////////////////////////////////////////主界面初始化/////////////////////////////////////////////////////

        #region
        void main_window()
        {
            //隐藏欢迎界面元素
            image.Visibility = Visibility.Hidden;
            image1.Visibility = Visibility.Hidden;
            passwordBox.Visibility = Visibility.Hidden;
            passwordBox1.Visibility = Visibility.Hidden;
            textBox.Visibility = Visibility.Hidden;
            //重置窗口大小
            Width = 320;
            Height = 283;
            f_hand.Width = 302;
            e_mini.Margin = new Thickness(259, 18, 0, 0);
            e_close.Margin = new Thickness(281, 16, 0, 0);
            b_1.Width = 302;
            b_1.Height = 236;
            kkn.Opacity = 0.95;
            //添加元素
            Image T1 = new Image
            {
                Name = "image3",
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 59,
                Width = 48,
                Margin = new Thickness(62, 69, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Source = new BitmapImage(new Uri("/image/lock.png", UriKind.Relative)),
                Cursor = Cursors.Hand,
            };
            T1.MouseDown += new MouseButtonEventHandler(image3_MouseDown);
            abc.Children.Add(T1);

            Image T2 = new Image
            {
                Name = "image4",
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 56,
                Width = 67,
                Margin = new Thickness(203,73, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Source = new BitmapImage(new Uri("/image/theme.png", UriKind.Relative)),
                Cursor = Cursors.Hand,
            };
            T2.MouseDown += new MouseButtonEventHandler(image4_MouseDown);
            abc.Children.Add(T2);

            Image T3 = new Image
            {
                Name = "image5",
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 55,
                Width = 60,
                Margin = new Thickness(57,191,0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Source = new BitmapImage(new Uri("/image/setting.png", UriKind.Relative)),
                Cursor = Cursors.Hand,
            };
            T3.MouseDown += new MouseButtonEventHandler(image5_MouseDown);
            abc.Children.Add(T3);

            Image T4 = new Image
            {
                Name = "image6",
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 55,
                Width = 67,
                Margin = new Thickness(206,192, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Source = new BitmapImage(new Uri("/image/about.png", UriKind.Relative)),
                Cursor = Cursors.Hand,
            };
            T4.MouseDown += new MouseButtonEventHandler(image6_MouseDown);
            abc.Children.Add(T4);

            Rectangle R1 = new Rectangle
            {
                Name = "r1",
                Fill = new SolidColorBrush(Color.FromArgb(255, 29, 33, 41)),
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 235,
                Width=3,
                Margin = new Thickness(156, 38, 0, 0),
                Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                VerticalAlignment = VerticalAlignment.Top,
            };
            abc.Children.Add(R1);

            Rectangle R2 = new Rectangle
            {
                Name = "r2",
                Fill = new SolidColorBrush(Color.FromArgb(255, 29, 33, 41)),
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 3,
                Width = 300,
                Margin = new Thickness(11,159, 0, 0),
                Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                VerticalAlignment = VerticalAlignment.Top,
            };
            abc.Children.Add(R2);

        }
        #endregion
   
        ///////////////////////////////////////////////窗口载入事件、初始化、计时器创建////////////////////////////////////////////

        #region
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //让Pbox 获得输入焦点
            passwordBox.Focus();                                                             
            
            //创建计时器
            clock.Interval = new TimeSpan(0, 0, 1);
            clock.Tick += new EventHandler(timer_kkick);
            clock.Start();
        }
        #endregion

        ////////////////////////////////////////////////////关闭应用程序函数////////////////////////////////////////////////////

        #region
        void app_close()
        {
            if (shutdown_timer || lock_timer)
            {
                //隐藏界面元素
                foreach (var c in abc.Children)
                {
                    if (c is Image)
                    {
                        Image tb = (Image)c;
                        if (tb.Name == "image3"|| tb.Name == "image4"|| tb.Name == "image5"|| tb.Name == "image6")
                        {
                            tb.Visibility = Visibility.Hidden;
                        }
                    }

                    if (c is Rectangle)
                    {
                        Rectangle tb = (Rectangle)c;
                        if (tb.Name == "r1" || tb.Name == "r2")
                        {
                            tb.Visibility = Visibility.Hidden;
                        }
                    }
                }

                e_close.Visibility = Visibility.Hidden;
                e_mini.Visibility = Visibility.Hidden;

                //显示
                image.Visibility = Visibility.Visible;
                image.Height = 129;
                image.Width = 249;
                image.Source = new BitmapImage(new Uri("/image/tixing.png", UriKind.Relative));
                image.Margin = new Thickness(36, 63, 0, 0);

                Image kid1 = new Image
                {
                    Name = "close1",
                    Height = 30,
                    Width = 30,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(93, 220, 0, 0),
                    Source = new BitmapImage(new Uri("/image/kk1.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                kid1.MouseDown += new MouseButtonEventHandler(shur_close);
                abc.Children.Add(kid1);

                Image kid2 = new Image
                {
                    Name = "enter1",
                    Height = 30,
                    Width = 30,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(193, 220, 0, 0),
                    Source = new BitmapImage(new Uri("/image/kk2.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                kid2.MouseDown += new MouseButtonEventHandler(shur_enter);
                abc.Children.Add(kid2);

            }
            else
            {
                Application.Current.Shutdown();
            }
            
        }
        #endregion

        //////////////////////////////////////////////////////窗口拖动函数//////////////////////////////////////////////////////

        #region
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            Point pp = Mouse.GetPosition(this);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        #endregion

        ////////////////////////////////////////////////////窗口最小化时间//////////////////////////////////////////////////////

        #region
        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        ////////////////////////////////////////////////关闭按钮关闭应用程序事件//////////////////////////////////////////////////

        #region
        private void Ellipse_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            app_close();
        }
        #endregion

        //////////////////////////////////////////////////屏蔽掉Alt+F4关闭窗口//////////////////////////////////////////////////

        #region
        bool AltDown = false;
        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.SystemKey == Key.LeftAlt || e.SystemKey == Key.RightAlt)
            {
                AltDown = true;
            }
            else if (e.SystemKey == Key.F4 && AltDown)
            {
                e.Handled = true;
            }
        }
        #endregion

        /////////////////////////////////////////////////////密码框输入事件/////////////////////////////////////////////////////

        #region
        private void passwordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void passwordBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
        #endregion
    
        ////////////////////////////////////////////////////前进按钮点击事件////////////////////////////////////////////////////

        #region
        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(passwordBox.Password.Length!=0&&passwordBox1.Password.Length!=0)
            {
                if(passwordBox.Password==passwordBox1.Password)
                {
                    bj_config.edit_config_txt(5,bj_config.GetMD5HashFromString(passwordBox.Password));
                    main_window();
                }
                else
                {
                    textBox.Text = "请确保两次密码输入相同";
                }
            }
            else
            {
                textBox.Text = "请输入管理员密码完成设置";
            }
        }
        #endregion

        //*******************///////////////////////////////锁屏按钮点击事件////////////////////////////////////////////////////

        #region
        private void image3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e_close.Visibility = Visibility.Hidden;
            e_mini.Visibility = Visibility.Hidden;
            //重置image 长宽、显示、坐标
            image.Visibility = Visibility.Visible;
            image.Height = 129;
            image.Width = 249;
            image.Margin = new Thickness(36, 63, 0, 0);

            //隐藏主界面元素
            foreach (var c in abc.Children)
            {
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == "image3" || tb.Name == "image4" || tb.Name == "image5" || tb.Name == "image6")
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }

                if (c is Rectangle)
                {
                    Rectangle tb = (Rectangle)c;
                    if (tb.Name == "r1" || tb.Name == "r2")
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }
            }

            //显示提醒界面元素
            foreach (var c in abc.Children)
            {
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == "shijiansuoping_close1" || tb.Name == "shijiansuoping_enter1")
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }

                if (c is TextBlock)
                {
                    TextBlock tb = (TextBlock)c;
                    if (tb.Name == "shijiansuoping__textblock")
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }
            }

            if(bj_config.combo[2]=="true")
            {
                image.Source = new BitmapImage(new Uri("/image/tixing2.png", UriKind.Relative));

                if(tme)
                {
                    tme = false;
                    TextBlock TB1 = new TextBlock
                    {
                        Name = "shijiansuoping__textblock",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Height = 40,
                        Width = 315,
                        Margin = new Thickness(145, 139, 0, 0),
                        TextWrapping = TextWrapping.Wrap,
                        Text = "",
                        VerticalAlignment = VerticalAlignment.Top,
                        Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                        TextAlignment = TextAlignment.Left,
                        Foreground = new SolidColorBrush(Color.FromArgb(200, 242, 109, 99)),
                        FontSize = 14,
                        SnapsToDevicePixels = true,
                    };
                    abc.Children.Add(TB1);
                }
            }
            else
            {
                image.Source = new BitmapImage(new Uri("/image/tixing3.png", UriKind.Relative));
                foreach (var c in abc.Children)
                {
                    if (c is TextBlock)
                    {
                        TextBlock tb = (TextBlock)c;
                        if (tb.Name == "shijiansuoping__textblock")
                        {
                            tb.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }


            if (ansi)
            {
                ansi = false;

                //添加

                Image kid1 = new Image
                {
                    Name = "shijiansuoping_close1",
                    Height = 30,
                    Width = 30,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(93, 220, 0, 0),
                    Source = new BitmapImage(new Uri("/image/kk1.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                kid1.MouseDown += new MouseButtonEventHandler(moren_close);
                abc.Children.Add(kid1);

                Image kid2 = new Image
                {
                    Name = "shijiansuoping_enter1",
                    Height = 30,
                    Width = 30,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(193, 220, 0, 0),
                    Source = new BitmapImage(new Uri("/image/kk2.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                kid2.MouseDown += new MouseButtonEventHandler(moren_enter);
                abc.Children.Add(kid2);
            }
        }
        #endregion

        //*******************///////////////////////////////主题按钮点击事件////////////////////////////////////////////////////

        #region
        private void image4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(Theme.onoff)
            {
                Theme nn = new Theme();
                nn.Show();
            }
        }
        #endregion

        //*******************///////////////////////////////设置按钮点击事件////////////////////////////////////////////////////

        #region
        private void image5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (Window3.onoff)
            //{
            //    Window3 pp = new Window3();
            //    pp.Show();
            //}
            
            if(Window3.onoff)
            {
                Window3 pp = new Window3();
                pp.Show();
            }    
        }
        #endregion

        //*******************///////////////////////////////关于按钮点击事件////////////////////////////////////////////////////

        #region
        private void image6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Window2.on_or_off)
            {
                Window2 about_window = new Window2();
                about_window.Show();
            }
        }
        #endregion

        //////////////////////////////////////////////////计时器已创建按钮事件////////////////////////////////////////////////////

        #region
        private void shur_close(object sender,MouseButtonEventArgs e)
        {
            image.Visibility = Visibility.Hidden;
            e_close.Visibility = Visibility.Visible;
            e_mini.Visibility = Visibility.Visible;
            foreach (var c in abc.Children)
            {
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == "image3" || tb.Name == "image4" || tb.Name == "image5" || tb.Name == "image6")
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                    if(tb.Name== "close1"||tb.Name== "enter1")
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }

                if (c is Rectangle)
                {
                    Rectangle tb = (Rectangle)c;
                    if (tb.Name == "r1" || tb.Name == "r2")
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void shur_enter(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion

        //////////////////////////////////////////////////默认时间锁屏触发事件////////////////////////////////////////////////////

        #region
        private void moren_close(object sender, MouseButtonEventArgs e)
        {
            image.Visibility = Visibility.Hidden;
            foreach (var c in abc.Children)
            {
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == "image3" || tb.Name == "image4" || tb.Name == "image5" || tb.Name == "image6")
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                    if (tb.Name == "shijiansuoping_close1" || tb.Name == "shijiansuoping_enter1")
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }

                if (c is Rectangle)
                {
                    Rectangle tb = (Rectangle)c;
                    if (tb.Name == "r1" || tb.Name == "r2")
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }

                if (c is TextBlock)
                {
                    TextBlock tb = (TextBlock)c;
                    if (tb.Name == "shijiansuoping__textblock")
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }
            }
            e_close.Visibility = Visibility.Visible;
            e_mini.Visibility = Visibility.Visible;
        }

        private void moren_enter(object sender, MouseButtonEventArgs e)
        {
            Window1 kdn = new Window1();
            kdn.Show();

            foreach (var c in abc.Children)
            {
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == "image3" || tb.Name == "image4" || tb.Name == "image5" || tb.Name == "image6")
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }

                if (c is Rectangle)
                {
                    Rectangle tb = (Rectangle)c;
                    if (tb.Name == "r1" || tb.Name == "r2")
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }
            }

            //显示提醒界面元素
            foreach (var c in abc.Children)
            {
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == "shijiansuoping_close1" || tb.Name == "shijiansuoping_enter1")
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }

                if (c is TextBlock)
                {
                    TextBlock tb = (TextBlock)c;
                    if (tb.Name == "shijiansuoping__textblock")
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }
            }
            image.Visibility = Visibility.Hidden;

            e_close.Visibility = Visibility.Visible;
            e_mini.Visibility = Visibility.Visible;
        }
        #endregion
    }
}
