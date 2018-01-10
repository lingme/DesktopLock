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

namespace TimeofLock
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2 : Window
    {
        public static bool on_or_off = true;   //判断窗口是否打开

        public Window2()
        {
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////// /////初始化窗口/////////////////////////////////////////////////////////////////////////////////////

        #region
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            on_or_off = false;
        }
        #endregion

        ////////////////////////////////////////////////////////////////////////关闭事件//////////////////////////////////////////////////////////////////////////////////////

        #region
        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            on_or_off = true ;
            Close();
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////Blog点击事件///////////////////////////////////////////////////////////////////////////////////

        #region
        private void image1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process ie = new System.Diagnostics.Process();
            ie.StartInfo.FileName = "IEXPLORE.EXE";
            ie.StartInfo.Arguments = "http://lingminme.lofter.com/";
            ie.Start();
        }
        #endregion

        //////////////////////////////////////////////////////////////////////窗口拖动事件////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////发送邮件拖动事件//////////////////////////////////////////////////////////////////////////////////

        #region
        private void image2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            sentEmail("kid--l@hotmail.com", "", "");
        }

        private void sentEmail(string ToUsers, string Title, string Body)
        {
            string sEmailMSG = "mailto:" + ToUsers + "?subject=" + Title + "&body=" + Body;
            System.Diagnostics.Process.Start(sEmailMSG);

        }
        #endregion

        //////////////////////////////////////////////////////////////////////窗口关闭事件//////////////////////////////////////////////////////////////////////////////////

        #region
        private void Window_Closed(object sender, EventArgs e)
        {
            on_or_off = true;
        }
        #endregion
    }
}
