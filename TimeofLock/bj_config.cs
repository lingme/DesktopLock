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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;

namespace TimeofLock
{
    class bj_config
    {
        #region
        public static string[] combo = new string[13];

        public static bool is_have()
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\" + "tol.config.txt"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void edit_config_txt(int line_num,string line_content)
        {
            read_config_txt();
            if (is_have())
            {
                combo[line_num - 1] = line_content;

                StreamWriter sw = new StreamWriter((Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) + "\\" + "tol.config.txt", false);
                for (int a = 0; a < 13; a++)
                {
                    sw.WriteLine(combo[a]);
                }
                sw.Flush();
                sw.Close();
            }
            else
            {
                return;
            }
            read_config_txt();
        }

        public static void read_config_txt()
        {
            if(is_have())
            {
                StreamReader sr = new StreamReader((Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)) + "\\" + "tol.config.txt");
                for (int a = 0; a < 13; a++)
                {
                    combo[a] = sr.ReadLine();
                }
                sr.Close();
            }
            else
            {
                return;
            }
        }

        public static string GetMD5HashFromString(string str)

        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bytValue, bytHash;

            bytValue = System.Text.Encoding.UTF8.GetBytes(str);

            bytHash = md5.ComputeHash(bytValue);

            md5.Clear();

            string sTemp = "";

            for (int i = 0; i < bytHash.Length; i++)

            {

                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');

            }

            return sTemp.ToUpper();

        }
        #endregion
    }


}
