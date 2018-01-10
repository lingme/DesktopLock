using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TimeofLock
{
    /// <summary>
    /// Window3.xaml 的交互逻辑
    /// </summary>
    public partial class Window3 : Window
    {
        public Window3()
        {
            InitializeComponent();
        }

        /////////////////////////////////////////////////////////变量集合///////////////////////////////////////////////////////

        #region
        public static bool onoff = true;
        public bool suoping_onoff = true;
        public bool qiangli_onoff = true;
        public bool jishiqi_onoff = true;
        public bool miyaopan_onoff = true;
        public bool admin_onoff = true;

        string panfu = null;
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const uint GENERIC_READ = 0x80000000;
        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int IOCTL_STORAGE_EJECT_MEDIA = 0x2d4808;

        DispatcherTimer clock = new DispatcherTimer();

        #endregion

        //////////////////////////////////////////////////////U盘监听挂载函数/////////////////////////////////////////////////////

        #region
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            string a = null;
            if (msg == WM_DEVICECHANGE)
            {
                switch (wParam.ToInt32())
                {
                    case DBT_DEVICEARRIVAL:
                        DriveInfo[] s = DriveInfo.GetDrives();
                        s.Any(t =>
                        {
                            if (t.DriveType == DriveType.Removable)
                            {
                                panfu = t.Name;
                                edit_textblock("miyaopan_textblock1", "U盘："+panfu);
                                a = bj_config.GetMD5HashFromString(System.Environment.MachineName) + ".key";
                                if(File.Exists(panfu+a))
                                {
                                    edit_textblock("miyaopan_textblock2", "已存在秘钥");
                                }
                                else
                                {
                                    edit_textblock("miyaopan_textblock2", "不存在秘钥");
                                }
                                return true;
                            }
                            return false;
                        });
                        break;
                    case DBT_DEVICEREMOVECOMPLETE:
                        edit_textblock("miyaopan_textblock1", "请插入U盘");
                        edit_textblock("miyaopan_textblock2", "U盘已弹出");
                        break;
                    default:
                        break;
                }
            }
            return IntPtr.Zero;
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr CreateFile(
         string lpFileName,
         uint dwDesireAccess,
         uint dwShareMode,
         IntPtr SecurityAttributes,
         uint dwCreationDisposition,
         uint dwFlagsAndAttributes,
         IntPtr hTemplateFile);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped
        );
        #endregion

        ////////////////////////////////////////////////动态设置控制添加容器状态函数集///////////////////////////////////////////////

        #region
        bool is_number(string a)
        {
            for(int i=0;i<a.Length;i++)
            {
                if(!Char.IsNumber(a[i]))
                {
                    return false;
                }
            }
            return true;
        }

        void find_rec2(string id,int Ta)
        {
            foreach (var c in abc.Children)
            {
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == id)
                    {
                        tb.Margin = new Thickness(10, Ta, 0, 0);
                    }
                }

                if (c is Rectangle)
                {
                    Rectangle tb = (Rectangle)c;
                    if (tb.Name == id)
                    {
                        tb.Margin = new Thickness(Ta, 439, 0, 0);
                    }
                }
            }
        }

        string take_pass(string anti)
        {
            string a = "";
            foreach (var c in abc.Children)
            {
                if (c is PasswordBox)
                {
                    PasswordBox tb = (PasswordBox)c;
                    if (tb.Name == anti)
                    {
                        a=tb.Password;
                    }
                }
            }

            return a;
        }

        string take_textbox(string anti)
        {
            string a = "";
            foreach (var c in abc.Children)
            {
                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;
                    if (tb.Name == anti)
                    {
                        a = tb.Text;
                    }
                }
            }
            return a;
        }

        void edit_textblock(string id_textblock,string neirong)
        {
            foreach (var c in abc.Children)
            {
                if (c is TextBlock)
                {
                    TextBlock tb = (TextBlock)c;
                    if (tb.Name == id_textblock)
                    {
                        tb.Text = neirong;
                    }
                }

                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;
                    if (tb.Name == id_textblock)
                    {
                        tb.Text = neirong;
                    }
                }
            }
        }

        void clear_passwordbox(string id_passwordbox)
        {
            foreach (var c in abc.Children)
            {
                if (c is PasswordBox)
                {
                    PasswordBox tb = (PasswordBox)c;
                    if (tb.Name == id_passwordbox)
                    {
                        tb.Clear();
                    }
                }
            }
        }

        void focus_passwordbox(string id_passwordbox)
        {
            foreach (var c in abc.Children)
            {
                if (c is PasswordBox)
                {
                    PasswordBox tb = (PasswordBox)c;
                    if (tb.Name == id_passwordbox)
                    {
                        tb.Focus();
                    }
                }
            }

            foreach (var c in abc.Children)
            {
                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;
                    if (tb.Name == id_passwordbox)
                    {
                        tb.Focus();
                    }
                }
            }
        }

        void edit_set_image(string id_image,string neirong)
        {
            foreach (var c in abc.Children)
            {
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == id_image)
                    {
                        tb.Source = new BitmapImage(new Uri(neirong, UriKind.Relative));
                    }
                }
            }
        }

        void hide_kongjian(string a)
        {
            foreach (var c in abc.Children)
            {
                if (c is TextBlock)
                {
                    TextBlock tb = (TextBlock)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }

                if (c is PasswordBox)
                {
                    PasswordBox tb = (PasswordBox)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }

                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }

                if (c is Ellipse)
                {
                    Ellipse tb = (Ellipse)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }

                if (c is Rectangle)
                {
                    Rectangle tb = (Rectangle)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        void show_kongjian(string a)
        {
            foreach (var c in abc.Children)
            {
                if (c is TextBlock)
                {
                    TextBlock tb = (TextBlock)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }
                if (c is Image)
                {
                    Image tb = (Image)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }

                if (c is PasswordBox)
                {
                    PasswordBox tb = (PasswordBox)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }

                if (c is TextBox)
                {
                    TextBox tb = (TextBox)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }

                if (c is Ellipse)
                {
                    Ellipse tb = (Ellipse)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }

                if (c is Rectangle)
                {
                    Rectangle tb = (Rectangle)c;
                    if (tb.Name == a)
                    {
                        tb.Visibility = Visibility.Visible;
                    }
                }
            }
        }
        #endregion

        ////////////////////////////////////////////////////锁屏留言界面添加元素////////////////////////////////////////////////////

        #region
        void add_liuyan()
        {
            bj_config.read_config_txt();
            if (suoping_onoff)
            {
                suoping_onoff = false;
                bj_config.read_config_txt();
                TextBox tb2 = new TextBox
                {
                    Name = "suoping_textbox2",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 95,
                    Width = 407,
                    Margin = new Thickness(196, 279, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    TextAlignment = TextAlignment.Left,
                    Foreground = new SolidColorBrush(Color.FromArgb(30, 255, 255, 255)),
                    Text = bj_config.combo[5].Length == 0 ? "请输入留言......" : "",
                    FontSize = 14,
                    CaretBrush = new SolidColorBrush(Color.FromArgb(30, 255, 255, 255)),
                    SnapsToDevicePixels = true,
                    IsReadOnly = true,
                };
                abc.Children.Add(tb2);

                TextBox tb1 = new TextBox
                {
                    Name = "suoping_textbox1",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 95,
                    Width = 407,
                    Margin = new Thickness(196, 279, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    TextAlignment = TextAlignment.Left,
                    Foreground = new SolidColorBrush(Color.FromArgb(90, 255, 255, 255)),
                    Text = bj_config.combo[5],
                    FontSize = 14,
                    CaretBrush = new SolidColorBrush(Color.FromArgb(90, 255, 255, 255)),
                    SnapsToDevicePixels = true,
                };
                tb1.PreviewKeyDown += new KeyEventHandler(suoping_textboxkey);
                abc.Children.Add(tb1);



                byte[,] el_color = new byte[7, 4] {{ 178,187,189,191},
                                               { 178,123,23,185 },
                                               { 178,96,146,108 },
                                               { 178,27,78,183  },
                                               { 178,180,88,83  },
                                               { 178,185,187,17 },
                                               { 178,181,16,38  },};

                Rectangle ELL_bg = new Rectangle
                {
                    Name = "ell_8",
                    Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255)),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 9,
                    Height = 4,
                    Margin = new Thickness(305, 439, 0, 0),
                    Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                };
                abc.Children.Add(ELL_bg);
                switch (Convert.ToInt16(bj_config.combo[6]))
                {
                    case 1:
                        ell_1(null, null);
                        break;
                    case 2:
                        ell_2(null, null);
                        break;
                    case 3:
                        ell_3(null, null);
                        break;
                    case 4:
                        ell_4(null, null);
                        break;
                    case 5:
                        ell_5(null, null);
                        break;
                    case 6:
                        ell_6(null, null);
                        break;
                    case 7:
                        ell_7(null, null);
                        break;
                }

                Ellipse[] ELL = new Ellipse[7];
                for (int a = 0; a < 7; a++)
                {
                    ELL[a] = new Ellipse
                    {
                        Name = "ell_" + a.ToString(),
                        Fill = new SolidColorBrush(Color.FromArgb(el_color[a, 0], el_color[a, 1], el_color[a, 2], el_color[a, 3])),
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Width = 25,
                        Height = 25,
                        Margin = new Thickness(297 + 44 * a, 411, 0, 0),
                        Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                        Cursor = Cursors.Hand,
                    };
                    switch (a)
                    {
                        case 0:
                            ELL[a].MouseDown += new MouseButtonEventHandler(ell_1);
                            break;
                        case 1:
                            ELL[a].MouseDown += new MouseButtonEventHandler(ell_2);
                            break;
                        case 2:
                            ELL[a].MouseDown += new MouseButtonEventHandler(ell_3);
                            break;
                        case 3:
                            ELL[a].MouseDown += new MouseButtonEventHandler(ell_4);
                            break;
                        case 4:
                            ELL[a].MouseDown += new MouseButtonEventHandler(ell_5);
                            break;
                        case 5:
                            ELL[a].MouseDown += new MouseButtonEventHandler(ell_6);
                            break;
                        case 6:
                            ELL[a].MouseDown += new MouseButtonEventHandler(ell_7);
                            break;
                    }
                    abc.Children.Add(ELL[a]);
                }
            }
        }
        #endregion

        ////////////////////////////////////////////////////强力模式界面添加元素////////////////////////////////////////////////////

        #region
        void add_qiangli()
        {
            bj_config.read_config_txt();
            if(qiangli_onoff)
            {
                qiangli_onoff = false;

                Image Im4 = new Image
                {
                    Name = "qiangli_close",
                    Height = 24,
                    Width = 24,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(219, 413, 0, 0),
                    Source = new BitmapImage(new Uri("/image/kk1.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                Im4.MouseDown += new MouseButtonEventHandler(qiangli_close);
                abc.Children.Add(Im4);

                Image Im5 = new Image
                {
                    Name = "qiangli_enter",
                    Height = 24,
                    Width = 24,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(265, 413, 0, 0),
                    Source = new BitmapImage(new Uri("/image/kk2.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                Im5.MouseDown += new MouseButtonEventHandler(qiangli_enter);
                abc.Children.Add(Im5);

                TextBlock TB1 = new TextBlock
                {
                    Name = "qiangli_textblock1",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 40,
                    Width = 315,
                    Margin = new Thickness(329, 414, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Text = bj_config.combo[1] == "true" ? "已开启强力模式" : "已关闭强力模式",
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(200, 123, 192, 135)),
                    FontSize = 15,
                    SnapsToDevicePixels = true,
                };
                abc.Children.Add(TB1);
            }
        }
        #endregion

        /////////////////////////////////////////////////////计时器界面添加元素////////////////////////////////////////////////////

        #region
        void add_jishiqi()
        {
            bj_config.read_config_txt();
            if(jishiqi_onoff)
            {
                jishiqi_onoff = false;

                TextBox tb1 = new TextBox
                {
                    Name = "jishiqi_textbox1",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 57,
                    Width = 103,
                    Margin = new Thickness(497, 153, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(200, 123, 192, 135)),
                    Text = "0",
                    FontSize = 20,
                    CaretBrush = new SolidColorBrush(Color.FromArgb(200, 123, 192, 135)),
                    SnapsToDevicePixels = true,
                    MaxLength=4,
                };
                tb1.PreviewKeyDown += new KeyEventHandler(jishiqi_textbox1);
                abc.Children.Add(tb1);

                TextBox tb2 = new TextBox
                {
                    Name = "jishiqi_textbox2",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 57,
                    Width = 103,
                    Margin = new Thickness(497, 336, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(200, 123, 192, 135)),
                    Text = "0",
                    FontSize = 20,
                    CaretBrush = new SolidColorBrush(Color.FromArgb(200, 123, 192, 135)),
                    SnapsToDevicePixels = true,
                    MaxLength=4,
                };
                tb2.PreviewKeyDown += new KeyEventHandler(jishiqi_textbox2);

                abc.Children.Add(tb2);

                TextBlock TB1 = new TextBlock
                {
                    Name = "jishiqi_textblock1",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 40,
                    Width = 315,
                    Margin = new Thickness(391, 222, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Text = Welcome.lock_timer?"计时中":"未创建",
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(200, 123, 192, 135)),
                    FontSize = 15,
                    SnapsToDevicePixels = true,
                };
                abc.Children.Add(TB1);

                TextBlock TB2 = new TextBlock
                {
                    Name = "jishiqi_textblock2",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 40,
                    Width = 315,
                    Margin = new Thickness(391, 401, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Text = Welcome.shutdown_timer ? "计时中" : "未创建",
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(200, 123, 192, 135)),
                    FontSize = 15,
                    SnapsToDevicePixels = true,
                };
                abc.Children.Add(TB2);

                Image Im5 = new Image
                {
                    Name = "jishiqi_close1",
                    Height = 18,
                    Width = 18,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(435, 106, 0, 0),
                    Source = new BitmapImage(new Uri("/image/close_timer.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                Im5.MouseDown += new MouseButtonEventHandler(timer_close1);
                abc.Children.Add(Im5);

                Image Im6 = new Image
                {
                    Name = "jishiqi_close2",
                    Height = 18,
                    Width = 18,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(435, 296, 0, 0),
                    Source = new BitmapImage(new Uri("/image/close_timer.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                Im6.MouseDown += new MouseButtonEventHandler(timer_close2);
                abc.Children.Add(Im6);
            }
        }
        #endregion

        /////////////////////////////////////////////////////秘钥盘界面添加元素////////////////////////////////////////////////////

        #region
        void add_miyaopan()
        {
            if(miyaopan_onoff)
            {
                miyaopan_onoff = false;
                TextBlock TB1 = new TextBlock
                {
                    Name = "miyaopan_textblock1",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 40,
                    Width = 315,
                    Margin = new Thickness(121, 377, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Text = "请插入U盘",
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(200, 188, 188, 188)),
                    FontSize = 12,
                    SnapsToDevicePixels = true,
                };
                abc.Children.Add(TB1);

                TextBlock TB2 = new TextBlock
                {
                    Name = "miyaopan_textblock2",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 40,
                    Width = 315,
                    Margin = new Thickness(122,411, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Text = "未写入",
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(200, 188, 188, 188)),
                    FontSize = 14,
                    SnapsToDevicePixels = true,
                };
                abc.Children.Add(TB2);

                Image Im5 = new Image
                {
                    Name = "miyaopan_write",
                    Height = 83,
                    Width = 85,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(395, 354, 0, 0),
                    Source = new BitmapImage(new Uri("/image/write_u.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                Im5.MouseDown += new MouseButtonEventHandler(write_u_click);
                abc.Children.Add(Im5);

                Image Im6 = new Image
                {
                    Name = "miyaopan_out",
                    Height = 83,
                    Width = 85,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(500, 354, 0, 0),
                    Source = new BitmapImage(new Uri("/image/out_u.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                Im6.MouseDown += new MouseButtonEventHandler(out_u_click);
                abc.Children.Add(Im6);
            }
        }
        #endregion

        /////////////////////////////////////////////////////管理员界面添加元素////////////////////////////////////////////////////

        #region
        void add_admin()
        {
            bj_config.read_config_txt();
            if(admin_onoff)
            {
                admin_onoff = false;

                PasswordBox ps1 = new PasswordBox
                {
                    Name = "admin_passwordBox1",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(268, 333, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 185,
                    Height = 24,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    Background = new SolidColorBrush(Color.FromArgb(0,255,255,255)),
                    PasswordChar = '*',
                    FontSize = 15,
                    CaretBrush = new SolidColorBrush(Color.FromRgb(123, 192, 135)),
                    MaxLength = 20,
                    Foreground = new SolidColorBrush(Color.FromRgb(123, 192, 135)),
                };
                abc.Children.Add(ps1);

                PasswordBox ps2 = new PasswordBox
                {
                    Name = "admin_passwordBox2",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(268, 372, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 185,
                    Height = 24,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    PasswordChar = '*',
                    FontSize = 15,
                    CaretBrush = new SolidColorBrush(Color.FromRgb(123, 192, 135)),
                    MaxLength = 20,
                    Foreground = new SolidColorBrush(Color.FromRgb(123, 192, 135)),
                };
                abc.Children.Add(ps2);

                PasswordBox ps3 = new PasswordBox
                {
                    Name = "admin_passwordBox3",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(268, 410, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = 185,
                    Height = 24,
                    BorderThickness = new Thickness(0, 0, 0, 0),
                    Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                    PasswordChar = '*',
                    FontSize = 15,
                    CaretBrush = new SolidColorBrush(Color.FromRgb(123, 192, 135)),
                    MaxLength = 20,
                    Foreground = new SolidColorBrush(Color.FromRgb(123, 192, 135)),
                };
                abc.Children.Add(ps3);

                TextBlock TB1 = new TextBlock
                {
                    Name = "admin_textblock",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 40,
                    Width = 315,
                    Margin = new Thickness(377, 398, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    Text = "开始重置",
                    VerticalAlignment = VerticalAlignment.Top,
                    Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                    TextAlignment = TextAlignment.Center,
                    Foreground = new SolidColorBrush(Color.FromArgb(200, 123, 192, 135)),
                    FontSize = 14,
                    SnapsToDevicePixels = true,
                };
                abc.Children.Add(TB1);

                Image Im5 = new Image
                {
                    Name = "admin_btn",
                    Height = 28,
                    Width = 28,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(520, 350, 0, 0),
                    Source = new BitmapImage(new Uri("/image/kk2.png", UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                Im5.MouseDown += new MouseButtonEventHandler(admin_click);
                abc.Children.Add(Im5);
            }
        }
        #endregion

        ///////////////////////////////////////////////////密码输入正确后初始化设置界面///////////////////////////////////////////////

        #region
        void start_setting_win()
        {
            //隐藏管理员登陆界面元素
            image.Visibility = Visibility.Hidden;
            image1.Visibility = Visibility.Hidden;
            textBox.Visibility = Visibility.Hidden;
            passwordBox1.Visibility = Visibility.Hidden;

            wsm.Opacity = 1;

            //初始化窗口 W  H
            Width = 680;
            Height = 481;
            //初始化border1   W  H
            border1.Width = 630;
            border1.Height = 431;
            //初始化窗口顶栏W
            R1.Width = 630;
            //初始化关闭、最小化按钮 坐标
            E1.Margin = new Thickness(590,18,0,0);
            E2.Margin = new Thickness(610,16,0,0);
            
            //初始化设置界面左边栏背景
            Image Im1 = new Image
            {
                Name = "Rec1",
                Height = 430,
                Width = 151,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 39, 0, 0),
                Source = new BitmapImage(new Uri("/image/setting_left_bg.png", UriKind.Relative)),
            };
            abc.Children.Add(Im1);
            //点击按钮选定背景
            Image Im2 = new Image
            {
                Name = "Mouse_sel",
                Height = 430,
                Width = 151,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 218, 0, 0),
                Source = new BitmapImage(new Uri("/image/Mouse_selected.png", UriKind.Relative)),
            };
            abc.Children.Add(Im2);
            //设置按钮图片地址字符串数组
            string[] s_bt = new string[6];
            s_bt[0] = "/image/s_bt1.png";
            s_bt[1] = "/image/s_bt2.png";
            s_bt[2] = "/image/s_bt3.png";
            s_bt[3] = "/image/s_bt4.png";
            s_bt[4] = "/image/s_bt5.png";
            s_bt[5] = "/image/s_bt6.png";
            //实例化Image控件数组
            Image[] SBT = new Image[6];
            
            //动态添加Image
            for(int i=0;i<6;i++)
            {
                SBT[i] = new Image
                {
                    Name = "bu_img" + i.ToString(),
                    Height = 30,
                    Width = 151,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(10, 218 + 36 * i, 0, 0),
                    Source = new BitmapImage(new Uri(s_bt[i], UriKind.Relative)),
                    Cursor = Cursors.Hand,
                };
                switch(i)
                {
                    case 0:
                        SBT[i].MouseDown += new MouseButtonEventHandler(sbt_MouseDown1);
                        break;
                    case 1:
                        SBT[i].MouseDown += new MouseButtonEventHandler(sbt_MouseDown2);
                        break;
                    case 2:
                        SBT[i].MouseDown += new MouseButtonEventHandler(sbt_MouseDown3);
                        break;
                    case 3: 
                        SBT[i].MouseDown += new MouseButtonEventHandler(sbt_MouseDown4);
                        break;
                    case 4:
                        SBT[i].MouseDown += new MouseButtonEventHandler(sbt_MouseDown5);
                        break;
                    case 5:
                        SBT[i].MouseDown += new MouseButtonEventHandler(sbt_MouseDown6);
                        break;
                }
                abc.Children.Add(SBT[i]);
            }
            //功能区
            ////////////////解锁方式
            Image Im3 = new Image
            {
                Name = "right_bg",
                Height = 387,
                Width = 437,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(181,60,0,0),
                Source = new BitmapImage(new Uri("/image/set1.png", UriKind.Relative)),
            };
            abc.Children.Add(Im3);

            PasswordBox ps1 = new PasswordBox
            {
                Name = "jiesuofangshi_passwordBox2",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(290,316,0,0),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 185,
                Height = 24,
                BorderThickness = new Thickness(0, 0, 0, 0),
                Background=new SolidColorBrush(Color.FromArgb(0,0,0,0)),
                PasswordChar='*',
                FontSize=14,
                CaretBrush=new SolidColorBrush(Color.FromRgb(123,192,135)),
                MaxLength=20,
                Foreground=new SolidColorBrush(Color.FromRgb(123, 192, 135)),
            };
            abc.Children.Add(ps1);

            PasswordBox ps2 = new PasswordBox
            {
                Name = "jiesuofangshi_passwordBox3",
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(290, 358, 0, 0),
                VerticalAlignment = VerticalAlignment.Top,
                Width = 185,
                Height = 24,
                BorderThickness = new Thickness(0, 0, 0, 0),
                Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
                PasswordChar = '*',
                FontSize = 14,
                CaretBrush = new SolidColorBrush(Color.FromRgb(123, 192, 135)),
                MaxLength = 20,
                Foreground = new SolidColorBrush(Color.FromRgb(123, 192, 135)),
            };
            abc.Children.Add(ps2);

            Image Im4 = new Image
            {
                Name = "jiesuofangshi_close",
                Height = 24,
                Width = 24,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(209, 416, 0, 0),
                Source = new BitmapImage(new Uri("/image/re1.png", UriKind.Relative)),
                Cursor = Cursors.Hand,
            };
            Im4.MouseDown += new MouseButtonEventHandler(kk1_MouseDown);
            abc.Children.Add(Im4);

            Image Im5 = new Image
            {
                Name = "jiesuofangshi_enter",
                Height = 24,
                Width = 24,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(255, 416, 0, 0),
                Source = new BitmapImage(new Uri("/image/kk2.png", UriKind.Relative)),
                Cursor=Cursors.Hand,
            };
            Im5.MouseDown += new MouseButtonEventHandler(kk2_MouseDown);
            abc.Children.Add(Im5);

            TextBlock TB1 = new TextBlock
            {
                Name = "textblock1",
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 40,
                Width = 315,
                Margin = new Thickness(310, 418, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                Text = bj_config.combo[2] == "true" ? "现解锁模式：时间锁": "现解锁模式：自定义密码",
                VerticalAlignment =VerticalAlignment.Top,
                Background=new SolidColorBrush(Color.FromArgb(0,0,0,0)),
                TextAlignment=TextAlignment.Center,
                Foreground= new SolidColorBrush(Color.FromArgb(200,123, 192, 135)),
                FontSize=15,
                SnapsToDevicePixels=true,
            };
            abc.Children.Add(TB1);
        }
        #endregion

        /////////////////////////////////////////////////////////窗口拖动事件//////////////////////////////////////////////////////

        #region
        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            Point pp = Mouse.GetPosition(this);
            if(e.LeftButton==MouseButtonState.Pressed)
            {
                DragMove();
            }

        }
        #endregion

        /////////////////////////////////////////////////////窗口载入事件、计时器事件/////////////////////////////////////////////////

        #region
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            onoff = false;
            passwordBox1.Focus();

            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;//窗口过程
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(WndProc));//挂钩
            }

            clock.Interval = new TimeSpan(0, 0, 1);
            clock.Tick += new EventHandler(timer_kkick);
            clock.Start();
        }

        public void timer_kkick(object sender, EventArgs e)
        {
            if(Welcome.lock_timer)
            {
                edit_textblock("jishiqi_textbox1",(Math.Ceiling((float)Welcome.lock_m/60)).ToString());
                if(Welcome.lock_m==1)
                {
                    edit_textblock("jishiqi_textblock1","计时结束");
                }
            }
            if (Welcome.shutdown_timer)
            {
                edit_textblock("jishiqi_textbox2",(Math.Ceiling((float)Welcome.shutdown_m / 60)).ToString());
            }
        }

        #endregion

        ////////////////////////////////////////////////////////窗口最小化事件//////////////////////////////////////////////////////

        #region
        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        ////////////////////////////////////////////////////////窗口关闭按钮事件////////////////////////////////////////////////////

        #region
        private void Ellipse_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            clock.Stop();
            bj_config.edit_config_txt(6, take_textbox("suoping_textbox1"));
            Close();
            onoff = true;
        }
        #endregion

        /////////////////////////////////////////////////////////窗口关闭事件//////////////////////////////////////////////////////

        #region
        private void Window_Closed(object sender, EventArgs e)
        {
            bj_config.edit_config_txt(6, take_textbox("suoping_textbox1"));
            onoff = true;
        }
        #endregion

        /////////////////////////////////////////////////////passwordbox键盘事件//////////////////////////////////////////////////

        #region
        private void passwordBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Back)
            {   
                if(passwordBox1.Password.Length == 1)
                {
                    textBox.Visibility = Visibility.Visible;
                }
            }
            else
            {
                textBox.Visibility = Visibility.Hidden;
            }

            if(e.Key==Key.Enter)
            {
                if (bj_config.GetMD5HashFromString(passwordBox1.Password) == bj_config.combo[4])
                {
                    start_setting_win();
                }
                else
                {
                    textBox.Visibility = Visibility.Visible;
                    passwordBox1.Clear();
                    textBox.Text = "密码错误";
                }
            }
        }
        #endregion

        ///////////////////////////////////////////////////////确认密码按钮事件/////////////////////////////////////////////////////

        #region
        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (bj_config.GetMD5HashFromString(passwordBox1.Password) == bj_config.combo[4])
            {
                start_setting_win();
            }
            else
            {
                textBox.Visibility = Visibility.Visible;
                passwordBox1.Clear();
                textBox.Text = "密码错误";
            }
        }
        #endregion

        ///////////////////////////////////////////////////////程序组：UI元素事件//////////////////////////////////////////////////

        #region
        //****************************************************************************************解锁模式
        private void sbt_MouseDown1(object sender, MouseButtonEventArgs e)
        {
            find_rec2("Mouse_sel", 218);
            edit_set_image("right_bg","/image/set1.png");

            //显示解锁模式界面元素
            show_kongjian("jiesuofangshi_passwordBox2");
            show_kongjian("jiesuofangshi_passwordBox3");
            show_kongjian("jiesuofangshi_close");
            show_kongjian("jiesuofangshi_enter");
            show_kongjian("textblock1");
            focus_passwordbox("jiesuofangshi_passwordBox2");

            //隐藏锁屏留言界面元素
            hide_kongjian("suoping_textbox2");
            hide_kongjian("suoping_textbox1");
            hide_kongjian("ell_8");
            for (int i = 0; i < 7; i++)
            {
                hide_kongjian("ell_" + i.ToString());
            }

            //隐藏强力模式界面元素
            hide_kongjian("qiangli_close");
            hide_kongjian("qiangli_enter");
            hide_kongjian("qiangli_textblock1");

            //隐藏计时器界面元素
            hide_kongjian("jishiqi_textbox1");
            hide_kongjian("jishiqi_textbox2");
            hide_kongjian("jishiqi_textblock1");
            hide_kongjian("jishiqi_textblock2");
            hide_kongjian("jishiqi_close1");
            hide_kongjian("jishiqi_close2");

            //隐藏秘钥盘界面元素
            hide_kongjian("miyaopan_textblock1");
            hide_kongjian("miyaopan_textblock2");
            hide_kongjian("miyaopan_write");
            hide_kongjian("miyaopan_out");

            //隐藏管理员界面元素
            hide_kongjian("admin_passwordBox1");
            hide_kongjian("admin_passwordBox2");
            hide_kongjian("admin_passwordBox3");
            hide_kongjian("admin_textblock");
            hide_kongjian("admin_btn");
            clear_passwordbox("admin_passwordBox1");
            clear_passwordbox("admin_passwordBox2");
            clear_passwordbox("admin_passwordBox3");


        }

        //****************************************************************************************锁屏留言
        private void sbt_MouseDown2(object sender, MouseButtonEventArgs e)
        {
            find_rec2("Mouse_sel", 254);
            edit_set_image("right_bg","/image/set2.png");

            //显示锁屏留言界面元素
            show_kongjian("suoping_textbox2");
            show_kongjian("suoping_textbox1");
            show_kongjian("ell_8");
            for(int i=0;i<7;i++)
            {
                show_kongjian("ell_" + i.ToString());
            }

            //隐藏解锁模式界面元素
            hide_kongjian("jiesuofangshi_passwordBox2");
            hide_kongjian("jiesuofangshi_passwordBox3");
            hide_kongjian("jiesuofangshi_close");
            hide_kongjian("jiesuofangshi_enter");
            hide_kongjian("textblock1");
            clear_passwordbox("jiesuofangshi_passwordBox2");
            clear_passwordbox("jiesuofangshi_passwordBox3");

            //隐藏强力模式界面元素
            hide_kongjian("qiangli_close");
            hide_kongjian("qiangli_enter");
            hide_kongjian("qiangli_textblock1");

            //隐藏计时器界面元素
            hide_kongjian("jishiqi_textbox1");
            hide_kongjian("jishiqi_textbox2");
            hide_kongjian("jishiqi_textblock1");
            hide_kongjian("jishiqi_textblock2");
            hide_kongjian("jishiqi_close1");
            hide_kongjian("jishiqi_close2");

            //隐藏秘钥盘界面元素
            hide_kongjian("miyaopan_textblock1");
            hide_kongjian("miyaopan_textblock2");
            hide_kongjian("miyaopan_write");
            hide_kongjian("miyaopan_out");

            //隐藏管理员界面元素
            hide_kongjian("admin_passwordBox1");
            hide_kongjian("admin_passwordBox2");
            hide_kongjian("admin_passwordBox3");
            hide_kongjian("admin_textblock");
            hide_kongjian("admin_btn");
            clear_passwordbox("admin_passwordBox1");
            clear_passwordbox("admin_passwordBox2");
            clear_passwordbox("admin_passwordBox3");


            //添加
            add_liuyan();

        }

        //****************************************************************************************强力模式
        private void sbt_MouseDown3(object sender, MouseButtonEventArgs e)
        {
            find_rec2("Mouse_sel", 290);
            edit_set_image("right_bg","/image/set4.png");

            //显示强力模式界面元素
            show_kongjian("qiangli_close");
            show_kongjian("qiangli_enter");
            show_kongjian("qiangli_textblock1");

            //隐藏解锁模式界面元素
            hide_kongjian("jiesuofangshi_passwordBox2");
            hide_kongjian("jiesuofangshi_passwordBox3");
            hide_kongjian("jiesuofangshi_close");
            hide_kongjian("jiesuofangshi_enter");
            hide_kongjian("textblock1");
            clear_passwordbox("jiesuofangshi_passwordBox2");
            clear_passwordbox("jiesuofangshi_passwordBox3");

            //隐藏锁屏留言界面元素
            hide_kongjian("suoping_textbox2");
            hide_kongjian("suoping_textbox1");
            hide_kongjian("ell_8");
            for (int i = 0; i < 7; i++)
            {
                hide_kongjian("ell_" + i.ToString());
            }

            //隐藏计时器界面元素
            hide_kongjian("jishiqi_textbox1");
            hide_kongjian("jishiqi_textbox2");
            hide_kongjian("jishiqi_textblock1");
            hide_kongjian("jishiqi_textblock2");
            hide_kongjian("jishiqi_close1");
            hide_kongjian("jishiqi_close2");

            //隐藏秘钥盘界面元素
            hide_kongjian("miyaopan_textblock1");
            hide_kongjian("miyaopan_textblock2");
            hide_kongjian("miyaopan_write");
            hide_kongjian("miyaopan_out");

            //隐藏管理员界面元素
            hide_kongjian("admin_passwordBox1");
            hide_kongjian("admin_passwordBox2");
            hide_kongjian("admin_passwordBox3");
            hide_kongjian("admin_textblock");
            hide_kongjian("admin_btn");
            clear_passwordbox("admin_passwordBox1");
            clear_passwordbox("admin_passwordBox2");
            clear_passwordbox("admin_passwordBox3");

            //添加
            add_qiangli();
        }

        //****************************************************************************************计时器
        private void sbt_MouseDown4(object sender, MouseButtonEventArgs e)
        {
            find_rec2("Mouse_sel", 326);
            edit_set_image("right_bg","/image/set5.png");
            //显示计时器界面元素
            show_kongjian("jishiqi_textbox1");
            show_kongjian("jishiqi_textbox2");
            show_kongjian("jishiqi_textblock1");
            show_kongjian("jishiqi_textblock2");
            show_kongjian("jishiqi_close1");
            show_kongjian("jishiqi_close2");


            //隐藏解锁模式界面元素
            hide_kongjian("jiesuofangshi_passwordBox2");
            hide_kongjian("jiesuofangshi_passwordBox3");
            hide_kongjian("jiesuofangshi_close");
            hide_kongjian("jiesuofangshi_enter");
            hide_kongjian("textblock1");
            clear_passwordbox("jiesuofangshi_passwordBox2");
            clear_passwordbox("jiesuofangshi_passwordBox3");

            //隐藏锁屏留言界面元素
            hide_kongjian("suoping_textbox2");
            hide_kongjian("suoping_textbox1");
            hide_kongjian("ell_8");
            for (int i = 0; i < 7; i++)
            {
                hide_kongjian("ell_" + i.ToString());
            }

            //隐藏强力模式界面元素
            hide_kongjian("qiangli_close");
            hide_kongjian("qiangli_enter");
            hide_kongjian("qiangli_textblock1");

            //隐藏秘钥盘界面元素
            hide_kongjian("miyaopan_textblock1");
            hide_kongjian("miyaopan_textblock2");
            hide_kongjian("miyaopan_write");
            hide_kongjian("miyaopan_out");

            //隐藏管理员界面元素
            hide_kongjian("admin_passwordBox1");
            hide_kongjian("admin_passwordBox2");
            hide_kongjian("admin_passwordBox3");
            hide_kongjian("admin_textblock");
            hide_kongjian("admin_btn");
            clear_passwordbox("admin_passwordBox1");
            clear_passwordbox("admin_passwordBox2");
            clear_passwordbox("admin_passwordBox3");

            //添加
            add_jishiqi();
        }

        //****************************************************************************************秘钥盘
        private void sbt_MouseDown5(object sender, MouseButtonEventArgs e)
        {
            find_rec2("Mouse_sel", 362);
            edit_set_image("right_bg","/image/set3.png");

            //隐藏解锁模式界面元素
            hide_kongjian("jiesuofangshi_passwordBox2");
            hide_kongjian("jiesuofangshi_passwordBox3");
            hide_kongjian("jiesuofangshi_close");
            hide_kongjian("jiesuofangshi_enter");
            hide_kongjian("textblock1");
            clear_passwordbox("jiesuofangshi_passwordBox2");
            clear_passwordbox("jiesuofangshi_passwordBox3");

            //隐藏锁屏留言界面元素
            hide_kongjian("suoping_textbox2");
            hide_kongjian("suoping_textbox1");
            hide_kongjian("ell_8");
            for (int i = 0; i < 7; i++)
            {
                hide_kongjian("ell_" + i.ToString());
            }

            //隐藏强力模式界面元素
            hide_kongjian("qiangli_close");
            hide_kongjian("qiangli_enter");
            hide_kongjian("qiangli_textblock1");

            //隐藏计时器界面元素
            hide_kongjian("jishiqi_textbox1");
            hide_kongjian("jishiqi_textbox2");
            hide_kongjian("jishiqi_textblock1");
            hide_kongjian("jishiqi_textblock2");
            hide_kongjian("jishiqi_close1");
            hide_kongjian("jishiqi_close2");

            //隐藏秘钥盘界面元素
            show_kongjian("miyaopan_textblock1");
            show_kongjian("miyaopan_textblock2");
            show_kongjian("miyaopan_write");
            show_kongjian("miyaopan_out");

            //隐藏管理员界面元素
            hide_kongjian("admin_passwordBox1");
            hide_kongjian("admin_passwordBox2");
            hide_kongjian("admin_passwordBox3");
            hide_kongjian("admin_textblock");
            hide_kongjian("admin_btn");
            clear_passwordbox("admin_passwordBox1");
            clear_passwordbox("admin_passwordBox2");
            clear_passwordbox("admin_passwordBox3");

            //添加
            add_miyaopan();
        }

        //****************************************************************************************管理员
        private void sbt_MouseDown6(object sender, MouseButtonEventArgs e)
        {
            find_rec2("Mouse_sel", 398);
            edit_set_image("right_bg","/image/set6.png");

            //显示管理员界面元素
            show_kongjian("admin_passwordBox1");
            show_kongjian("admin_passwordBox2");
            show_kongjian("admin_passwordBox3");
            show_kongjian("admin_textblock");
            show_kongjian("admin_btn");
            focus_passwordbox("admin_passwordBox1");

            //隐藏解锁模式界面元素
            hide_kongjian("jiesuofangshi_passwordBox2");
            hide_kongjian("jiesuofangshi_passwordBox3");
            hide_kongjian("jiesuofangshi_close");
            hide_kongjian("jiesuofangshi_enter");
            hide_kongjian("textblock1");
            clear_passwordbox("jiesuofangshi_passwordBox2");
            clear_passwordbox("jiesuofangshi_passwordBox3");

            //隐藏锁屏留言界面元素
            hide_kongjian("suoping_textbox2");
            hide_kongjian("suoping_textbox1");
            hide_kongjian("ell_8");
            for (int i = 0; i < 7; i++)
            {
                hide_kongjian("ell_" + i.ToString());
            }

            //隐藏强力模式界面元素
            hide_kongjian("qiangli_close");
            hide_kongjian("qiangli_enter");
            hide_kongjian("qiangli_textblock1");

            //隐藏计时器界面元素
            hide_kongjian("jishiqi_textbox1");
            hide_kongjian("jishiqi_textbox2");
            hide_kongjian("jishiqi_textblock1");
            hide_kongjian("jishiqi_textblock2");
            hide_kongjian("jishiqi_close1");
            hide_kongjian("jishiqi_close2");

            //隐藏秘钥盘界面元素
            hide_kongjian("miyaopan_textblock1");
            hide_kongjian("miyaopan_textblock2");
            hide_kongjian("miyaopan_write");
            hide_kongjian("miyaopan_out");

            //添加
            add_admin();
        }

        #endregion

        ///****************///////////////////////////////////解锁方式界面按钮事件集////////////////////////////////////////////////

        #region
        private void kk1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (bj_config.combo[2] == "false")
            {
                if(bj_config.combo[3].Length<32)
                {
                    edit_textblock("textblock1", "切换失败，自定义密码为空");
                }
                else
                {
                    bj_config.edit_config_txt(3, "true");
                    edit_textblock("textblock1", "已切换时间密码解锁");
                }     
            }
            else
            {
                bj_config.edit_config_txt(3, "false");
                edit_textblock("textblock1", "已切换自定义密码解锁");
            }
        }
        private void kk2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string a1 = take_pass("jiesuofangshi_passwordBox2");
            string a2 = take_pass("jiesuofangshi_passwordBox3");

            if(string.Equals(a1,a2)&&a1.Length!=0&&a2.Length!=0)
            {
                bj_config.edit_config_txt(3, "false");
                bj_config.edit_config_txt(4,bj_config.GetMD5HashFromString(a1));
                if(bj_config.combo[2]=="true")
                {
                    edit_textblock("textblock1", "已修改解锁模式：自定义解锁");
                }
                if (bj_config.combo[2] == "false")
                {
                    edit_textblock("textblock1", "已修改自定义解锁密码");
                }
            }
            if(a1.Length==0||a2.Length==0)
            {
                edit_textblock("textblock1","密码不能为空");
            }
            if (a1!=a2)
            {
                edit_textblock("textblock1","请核对两次密码输出");
            }
        }
        #endregion

        ///****************///////////////////////////////////锁屏留言界面按钮事件集////////////////////////////////////////////////

        #region
        private void suoping_textboxkey(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (take_textbox("suoping_textbox1").Length == 0)
                {
                    show_kongjian("suoping_textbox2");
                }
            }
            else
            {
                hide_kongjian("suoping_textbox2");
            }
        }

        private void ell_1(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(7, "1");
            find_rec2("ell_8", 305);
        }
        private void ell_2(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(7, "2");
            find_rec2("ell_8", 349);
        }
        private void ell_3(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(7, "3");
            find_rec2("ell_8", 393);
        }
        private void ell_4(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(7, "4");
            find_rec2("ell_8", 437);
        }
        private void ell_5(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(7, "5");
            find_rec2("ell_8", 481);
        }
        private void ell_6(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(7, "6");
            find_rec2("ell_8", 525);
        }
        private void ell_7(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(7, "7");
            find_rec2("ell_8", 569);
        }
        #endregion

        ///****************//////////////////////////////////强力模式界面按钮事件集////////////////////////////////////////////////

        #region
        private void qiangli_close(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(2, "false");
            edit_textblock("qiangli_textblock1", "已关闭强力模式");
        }

        private void qiangli_enter(object sender, MouseButtonEventArgs e)
        {
            bj_config.edit_config_txt(2, "true");
            edit_textblock("qiangli_textblock1", "已开启强力模式");
        }
        #endregion

        ///****************///////////////////////////////////计时器界面按钮事件集////////////////////////////////////////////////

        #region
        private void jishiqi_textbox1(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key == Key.Back)
            {
            }
            else if (e.Key>=Key.A&&e.Key<=Key.Z||e.Key==Key.Decimal)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if(is_number(take_textbox("jishiqi_textbox1")))
                {
                    if (Convert.ToInt16(take_textbox("jishiqi_textbox1")) > 0)
                    {
                        Welcome.lock_timer = true;
                        Welcome.lock_m = Convert.ToInt16(take_textbox("jishiqi_textbox1")) * 60;
                        edit_textblock("jishiqi_textblock1", "创建成功");
                    }
                    else
                    {
                        edit_textblock("jishiqi_textblock1", "无效数值");
                    } 
                }
                else
                {
                    edit_textblock("jishiqi_textblock1", "无效数值");
                }
            }   
        }

        private void jishiqi_textbox2(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key == Key.Back)
            {
            }
            else if (e.Key >= Key.A && e.Key <= Key.Z||e.Key==Key.Decimal)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (is_number(take_textbox("jishiqi_textbox2")))
                {
                    if (Convert.ToInt16(take_textbox("jishiqi_textbox2")) > 0)
                    {
                        Welcome.shutdown_timer = true;
                        Welcome.shutdown_m = Convert.ToInt16(take_textbox("jishiqi_textbox2")) * 60;
                        edit_textblock("jishiqi_textblock2", "创建成功");
                    }
                    else
                    {
                        edit_textblock("jishiqi_textblock2", "无效数值");   
                    }
                }
                else
                {
                    edit_textblock("jishiqi_textblock2", "无效数值");
                }
            }
        }

        private void timer_close1(object sender, MouseButtonEventArgs e)
        {
            Welcome.lock_timer = false;
            edit_textblock("jishiqi_textbox1", "");
            edit_textblock("jishiqi_textblock1", "已删除");
        }
        private void timer_close2(object sender, MouseButtonEventArgs e)
        {
            Welcome.shutdown_timer = false;
            edit_textblock("jishiqi_textbox2", "");
            edit_textblock("jishiqi_textblock2", "已删除");
        }

        #endregion

        ///****************///////////////////////////////////秘钥盘界面按钮事件集////////////////////////////////////////////////

        #region
        private void write_u_click(object sender, MouseButtonEventArgs e)
        {
            string a = null;
            if(panfu!=null)
            {
                a = bj_config.GetMD5HashFromString(System.Environment.MachineName)+ ".key";
                File.Create(panfu + a).Close();
                edit_textblock("miyaopan_textblock2","秘钥已写入");
            }
            else
            {
                edit_textblock("miyaopan_textblock2", "无可用U盘");
            }
        }

        private void out_u_click(object sender, MouseButtonEventArgs e)
        {
            if (panfu!=null)
            {
                string filename = @"\\.\" + panfu.Remove(2);
                IntPtr handle = CreateFile(filename, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ | FILE_SHARE_WRITE, IntPtr.Zero, 0x3, 0, IntPtr.Zero);
                uint byteReturned;
                bool result = DeviceIoControl(handle, IOCTL_STORAGE_EJECT_MEDIA, IntPtr.Zero, 0, IntPtr.Zero, 0, out byteReturned, IntPtr.Zero);
                edit_textblock("miyaopan_textblock2", "U盘已弹出");
                edit_textblock("miyaopan_textblock1", "请插入U盘");
                panfu = null;
            }
            else
            {
                edit_textblock("miyaopan_textblock1", "请插入U盘");
                edit_textblock("miyaopan_textblock2", "未检查到U盘");
            }
        }
        #endregion

        ///****************///////////////////////////////////管理员界面按钮事件集////////////////////////////////////////////////

        #region
        private void admin_click(object sender, MouseButtonEventArgs e)
        {
            bj_config.read_config_txt();
            if (bj_config.GetMD5HashFromString(take_pass("admin_passwordBox1")) == bj_config.combo[4])
            {
                if(take_pass("admin_passwordBox2")== take_pass("admin_passwordBox3"))  
                {
                    bj_config.edit_config_txt(5, bj_config.GetMD5HashFromString(take_pass("admin_passwordBox2")));
                    edit_textblock("admin_textblock", "修改成功");
                }
                else
                {
                    edit_textblock("admin_textblock", "请确认两次密码");
                }
            }
            else
            {
                edit_textblock("admin_textblock", "旧密码错误");
            }
        }
        #endregion
    }
}
