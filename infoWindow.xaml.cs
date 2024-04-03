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

namespace 班级点名器
{
    /// <summary>
    /// infoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class infoWindow : Window
    {
        public infoWindow()
        {
            InitializeComponent();
        }

        private void infoWindow_Loaded(object sender, RoutedEventArgs e)
        {
            info.Text = "一个很简陋的点名器\n\n作者B站：\n逗比Thomas";
        }

        private void GithubButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://github.com/Thomas455/A-simple-class-roll-caller");//尝试打开文件

            }
            catch (IOException error)
            {
                // 处理文件读取时可能出现的异常，例如文件不存在、没有读取权限等
                Console.WriteLine("网页打开错误：" + error.Message);
                System.Windows.MessageBox.Show("网页打开错误：" + error.Message);//弹出提示框
                return;
            }
        }
    }
}
