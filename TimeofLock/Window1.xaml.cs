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
using System.Windows.Interop;
using Microsoft.Win32;

namespace TimeofLock
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent(); 
        }

        public static bool on_or_off = true;
        public bool u_drive = false;
        public bool a_kill = false;


        ////////////////////////////////////////////////////////////////////实例化一个计时器/////////////////////////////////////////////////////////////////////////////

        DispatcherTimer clock = new DispatcherTimer();

        ////////////////////////////////////////////////////////////////////////键盘钩子///////////////////////////////////////////////////////////////////////////////

        #region
        /// 声明回调函数委托  
        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

        /// 委托实例  
 
        HookProc KeyboardHookProcedure;

        /// 键盘钩子句柄  
        static int hKeyboardHook = 0;

        //装置钩子的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //卸下钩子的函数   
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //获取某个进程的句柄函数  
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        // 普通按键消息  
        private const int WM_KEYDOWN = 0x100;

        // 系统按键消息  
        private const int WM_SYSKEYDOWN = 0x104;

        //鼠标常量   
        public const int WH_KEYBOARD_LL = 13;

        //声明键盘钩子的封送结构类型   
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;               //表示一个在1到254间的虚似键盘码   
            public int scanCode;             //表示硬件扫描码   
            public int flags;
            public int time;
            public int dwExtraInfo;
        }


        /// 启动键盘钩子  
        /// 截取全局按键，发送新按键，返回  

        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN)
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                System.Windows.Forms.Keys keyData = (System.Windows.Forms.Keys)MyKeyboardHookStruct.vkCode;

                ///////////////////////////////////////////////////////////键盘钩子 屏蔽按键列表//////////////////////////////////////////////////////
                if (keyData == System.Windows.Forms.Keys.LWin)
                {
                    //  左win
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.RWin)
                {
                    //  右win
                    return 1;
                }
                if(keyData==System.Windows.Forms.Keys.LControlKey)
                {
                    // 左Ctrl
                    return 1;
                }
                if(keyData==System.Windows.Forms.Keys.RControlKey)
                {
                    // 右Ctrl
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Alt)
                {
                    // Alt
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Tab)
                {
                    // Tab
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.F4)
                {
                    // F4
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.RShiftKey)
                {
                    // R shift
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.LShiftKey)
                {
                    // L shift
                    return 1;
                }
                if (keyData >= System.Windows.Forms.Keys.F1 && keyData<=System.Windows.Forms.Keys.F12)
                {
                    // F1  --  F12
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Escape && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control)
                {
                    //  ctrl + esc (开始菜单)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.F4 && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Alt)
                {
                    // alt + F4 (关闭)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Tab && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Alt)
                {
                    // alt + tab (切换)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.E && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    // win + e (资源管理器)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Space && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Alt)
                {
                    // alt + space (打开快捷方式菜单)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Tab && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    // ctrl + alt + tab(可以在打开的项目中切换)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Space && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    // win + space (预览桌面)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Up && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    // win + ↑ (调整窗口大小)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Down && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    // win + ↓ (调整窗口大小)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Left && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    // win + ← (调整窗口大小)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Right && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LWin)
                {
                    // win + →(调整窗口大小)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Up && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    // ctrl + alt + ↑( 屏幕旋转)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Down && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    // ctrl + alt + ↓ ( 屏幕旋转)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Left && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    // ctrl + alt + ←( 屏幕旋转)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Right && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    // ctrl + alt + →( 屏幕旋转)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.A && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    // ctrl + alt + a(qq截图)
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Z && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control + (int)System.Windows.Forms.Keys.Alt)
                {
                    // ctrl + alt + a (qq主窗口)
                    return 1;
                }
                if(keyData == System.Windows.Forms.Keys.V && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Control)
                {
                    // ctrl + V
                    return 1;
                }
                if(keyData==System.Windows.Forms.Keys.Control&&(int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Space)
                {
                    //Ctrl + Space
                    return 1;
                }
                if(keyData == System.Windows.Forms.Keys.Alt && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Escape)
                {
                    //alt+esc
                    return 1;
                }
                if(keyData == System.Windows.Forms.Keys.Alt && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.LShiftKey + (int)System.Windows.Forms.Keys.Escape)
                {
                    //alt+shift+esc
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Alt && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.RShiftKey + (int)System.Windows.Forms.Keys.Escape)
                {
                    //alt+shift+esc
                    return 1;
                }
                if (keyData == System.Windows.Forms.Keys.Alt && (int)System.Windows.Forms.Control.ModifierKeys == (int)System.Windows.Forms.Keys.Shift + (int)System.Windows.Forms.Keys.Tab)
                {
                    //alt+shift+esc
                    return 1;
                }
            }
            return 0;
        }

        #endregion

        //////////////////////////////////////////////////////////////////////关闭窗口函数/////////////////////////////////////////////////////////////////////////////

        #region
        public void close_lock()
        {
            //关闭计时器
            clock.Stop();
            //卸载钩子
            bool retKeyboard = true;

            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }

            if (!(retKeyboard)) throw new Exception("Keyboard hook uninstall Error!");
            //关闭窗
            Close();

            on_or_off = true;

            bj_config.edit_config_txt(1, "true");
            if (bj_config.combo[1]=="true")
            {
                RegistryKey reg = null;
                reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                reg.DeleteValue("ANTI", false);
                reg.Close();
            }
        }
        #endregion

        /////////////////////////////////////////////////////////窗口初始化创建计时器、初始化属性、安装键盘钩子///////////////////////////////////////////////////////////////

        #region
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            on_or_off = false;
            bj_config.read_config_txt();
            //设置计时器属性
            clock.Interval = new TimeSpan(0, 0, 0, 0, 10);
            clock.Tick += new EventHandler(timer_kkick);
            clock.Start();

            //设置控件image与分辨率相同
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;

            image.Width = SystemParameters.PrimaryScreenWidth;
            image.Height = SystemParameters.PrimaryScreenHeight;

            R1.Width = SystemParameters.PrimaryScreenWidth;
            R1.Height = SystemParameters.PrimaryScreenHeight;

            WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 0;
            this.Top = 0;

            //让 passwordbox 在窗体载入同时获得输入焦点
            passwordBox.Focus();     
            
            //装载钩子
            if (hKeyboardHook == 0)
            {
                //实例化委托  
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                Process curProcess = Process.GetCurrentProcess();
                ProcessModule curModule = curProcess.MainModule;
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(curModule.ModuleName), 0);
            }

            //初始化窗口UI
            switch(bj_config.combo[11])
            {
                case "1":
                    image.Source = new BitmapImage(new Uri("/image/theme_image1.jpg", UriKind.Relative));
                    break;
                case "2":
                    image.Source = new BitmapImage(new Uri("/image/theme_image2.png", UriKind.Relative));
                    break;
                case "3":
                    image.Source = new BitmapImage(new Uri("/image/theme_image3.jpg", UriKind.Relative));
                    break;
                case "4":
                    if(File.Exists(bj_config.combo[12]))
                    {
                        image.Source = new BitmapImage(new Uri(bj_config.combo[12], UriKind.Absolute));
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri("/image/theme_image1.jpg", UriKind.Relative));
                    }
                    break;
            }


            //初始化top line坐标、h w 
            re1.Width = SystemParameters.PrimaryScreenWidth;
            re1.Height = 50;
            re1.Margin = new Thickness(0, 0, 0, 0);

            //初始化时间坐标
            textBlock.Margin = new Thickness(20, 10, 0, 0);

            //初始化留言条坐标
            textBlock1.Width= SystemParameters.PrimaryScreenWidth-224;
            textBlock1.Margin = new Thickness(106, 15, 0, 0);

            //初始化Center Black Background
            re2.Width = 301;
            re2.Height = 336;
            re2.Margin = new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2, (SystemParameters.PrimaryScreenHeight - re2.Height) / 2, 0, 0);

            // 初始化密码框

            passwordBox.Width = 187;
            passwordBox.Height = 28;

            passwordBox.Margin = new Thickness(((SystemParameters.PrimaryScreenWidth - re2.Width) / 2) + 57, ((SystemParameters.PrimaryScreenHeight - re2.Height) / 2) + 265, 0, 0);
            passwordBox.Background = new SolidColorBrush(Color.FromArgb(100,0,0,0));
            passwordBox.Foreground = new SolidColorBrush(Color.FromRgb(86,157,229));
            passwordBox.CaretBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));

            textBlock2.Margin= new Thickness(((SystemParameters.PrimaryScreenWidth - re2.Width) / 2) + 57, ((SystemParameters.PrimaryScreenHeight - re2.Height) / 2) + 269, 0, 0);
            textBlock2.Foreground = new SolidColorBrush(Color.FromRgb(86, 157, 229));

            //初始化线条
            fu1.Margin=new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2+0.5,((SystemParameters.PrimaryScreenHeight - re2.Height) / 2) + 49,0,0);
            fu2.Margin = new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2 + 150, (SystemParameters.PrimaryScreenHeight - re2.Height) / 2, 0, 0);

            fu3.Margin = new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2+1, ((SystemParameters.PrimaryScreenHeight - re2.Height) / 2) + 49, 0, 0);

            //初始化按钮
            image1.Margin = new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2, (SystemParameters.PrimaryScreenHeight - re2.Height) / 2+1, 0, 0);
            image2.Margin = new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2+150, (SystemParameters.PrimaryScreenHeight - re2.Height) / 2+1, 0, 0);

            //初始化图标
            image3.Source = new BitmapImage(new Uri("/image/lock_1.png", UriKind.Relative));
            image3.Margin = new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2+97.5, (SystemParameters.PrimaryScreenHeight - re2.Height) / 2+111, 0, 0);


            //挂载U盘检测
            HwndSource hwndSource = PresentationSource.FromVisual(this) as HwndSource;//窗口过程
            if (hwndSource != null)
            {
                hwndSource.AddHook(new HwndSourceHook(WndProc));//挂钩
            }


            //创建自启
            bj_config.edit_config_txt(1, "false");

            if(bj_config.combo[1]=="true")
            {
                string a = "\"" + System.Windows.Forms.Application.StartupPath + "\\" + System.IO.Path.GetFileName(System.Reflection.Assembly.GetEntryAssembly().GetName().Name) + ".exe" + "\"";
                RegistryKey reg = null;
                reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (reg == null)
                {
                    reg = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                }
                reg.SetValue("ANTI", a);
                reg.Close();
            }
            
        }
        #endregion

        /////////////////////////////////////////////////////----解锁----、计时器、KILL进程、判断密码、卸载钩子/////////////////////////////////////////////////////////////

        #region
        public void timer_kkick(object sender, EventArgs e)
        {
            //textBox.Text = "当前时间：" + DateTime.Now.ToLongTimeString();
            Topmost = true;                                                                   //窗口置顶

            Process[] kill = Process.GetProcesses();                                          //获取当前任务管理器所有运行中程序
            foreach(Process kill1 in kill)
            {
                try
                {
                    if(kill1.ProcessName.ToLower().Trim()=="taskmgr")
                    {
                        kill1.Kill();
                        return;
                    }
                }
                catch
                {
                    return;
                }
            }

            string a = "";
            DateTime t = new DateTime();
            t = DateTime.Now;
            a = t.ToString("yyyyMdHm");
            textBlock.Text = t.ToString("HH:mm");
            

            if (bj_config.combo[2]=="true")                                                 //关闭锁屏
            {
                if(passwordBox.Password==a)
                {
                    close_lock();
                }
            }
            else
            {
                if(bj_config.GetMD5HashFromString(passwordBox.Password)==bj_config.combo[3])
                {
                    close_lock();
                }
            }

            if(u_drive&&a_kill)
            {
                Close();
            }

        }
        #endregion

        ///////////////////////////////////////////////////////////////////passwordBox输入事件////////////////////////////////////////////////////////////////////////
 
        #region
        private void passwordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }
        #endregion

        ///////////////////////////////////////////////////////////////////留言字体颜色、缩放////////////////////////////////////////////////////////////////////////

        #region
        private void textBlock1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch (bj_config.combo[6])
            {
                case "1":
                    textBlock1.Foreground = new SolidColorBrush(Color.FromArgb(235, 255, 255, 255));
                    break;
                case "2":
                    textBlock1.Foreground = new SolidColorBrush(Color.FromArgb(255, 177, 76, 240));
                    break;
                case "3":
                    textBlock1.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 237, 127));
                    break;
                case "4":
                    textBlock1.Foreground = new SolidColorBrush(Color.FromArgb(255, 92, 135, 235));
                    break;
                case "5":
                    textBlock1.Foreground = new SolidColorBrush(Color.FromArgb(255, 200,11, 107));
                    break;
                case "6":
                    textBlock1.Foreground = new SolidColorBrush(Color.FromArgb(255, 235, 237, 47));
                    break;
                case "7":
                    textBlock1.Foreground = new SolidColorBrush(Color.FromArgb(255, 234, 60, 83));
                    break;
            }

            if (textBlock1.Text==">")
            {
                textBlock1.Text = bj_config.combo[5] + " <";
            }
            else
            {
                textBlock1.Text = ">";
                textBlock1.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////秘钥盘、密码锁切换////////////////////////////////////////////////////////////////////////

        #region
        private void passwordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (passwordBox.Password.Length == 1)
                {
                    textBlock2.Visibility = Visibility.Visible;
                }
            }
            else
            {
                textBlock2.Visibility = Visibility.Hidden;
            }
        }

        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            u_drive = false;

            image3.Source = new BitmapImage(new Uri("/image/lock_1.png", UriKind.Relative));
            fu3.Margin = new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2 + 1, ((SystemParameters.PrimaryScreenHeight - re2.Height) / 2) + 49, 0, 0);
            passwordBox.Visibility = Visibility.Visible;
            passwordBox.Clear();
            passwordBox.Focus();
            textBlock2.Text = "请输入密码";

            biankuang.Visibility = Visibility.Hidden;
        }

        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            u_drive = true;

            image3.Source = new BitmapImage(new Uri("/image/lock_2.png", UriKind.Relative));
            fu3.Margin = new Thickness((SystemParameters.PrimaryScreenWidth - re2.Width) / 2 + 152, ((SystemParameters.PrimaryScreenHeight - re2.Height) / 2) + 49, 0, 0);
            passwordBox.Visibility = Visibility.Hidden;
            textBlock2.Text = "请插入U盘";

            biankuang.Visibility = Visibility.Visible;
            biankuang.Margin= new Thickness(((SystemParameters.PrimaryScreenWidth - re2.Width) / 2) + 57, ((SystemParameters.PrimaryScreenHeight - re2.Height) / 2) + 265, 0, 0);
            biankuang.BorderBrush = new SolidColorBrush(Color.FromRgb(86, 157, 229));
        }
        #endregion

        /////////////////////////////////////////////////////////////////////秘钥盘检索监听////////////////////////////////////////////////////////////////////////

        #region
        string panfu = "";
        public const int WM_DEVICECHANGE = 0x219;
        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        public const uint GENERIC_READ = 0x80000000;
        public const int GENERIC_WRITE = 0x40000000;
        public const int FILE_SHARE_READ = 0x1;
        public const int FILE_SHARE_WRITE = 0x2;
        public const int IOCTL_STORAGE_EJECT_MEDIA = 0x2d4808;

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
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
                                if (File.Exists(panfu + bj_config.GetMD5HashFromString(System.Environment.MachineName) + ".key"))
                                {
                                    a_kill = true;
                                }
                                else
                                {
                                    textBlock2.Text = "U盘不存在秘钥";
                                }
                                return true;
                            }
                            return false;
                        });
                        break;
                    case DBT_DEVICEREMOVECOMPLETE:
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
    }
}
