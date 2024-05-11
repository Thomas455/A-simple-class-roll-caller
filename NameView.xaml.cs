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
using static System.Windows.Forms.LinkLabel;

namespace 班级点名器
{
    /// <summary>
    /// NameView.xaml 的交互逻辑
    /// </summary>
    public partial class NameView : Window
    {
        string Temp_NamePath = Properties.Settings.Default.Save_NamePath;//读取已经储存的路径
        string Temp_NamePath_Time = Properties.Settings.Default.PathSettingTime;//读取已经储存的路径

        public NameView()
        {
            InitializeComponent();
        }

        

        //窗口加载完成
        private void NameViewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Path.Content = "路径："+Temp_NamePath;
            NameShow.Text = "当前名单：";//初始化后面会用到的属性
            NameShow.Height = 16;

            //读取名单
            string FileNameToRead = @Temp_NamePath;
            //用文件里每一行的内容创建一个字符串数组
            string[] NameLines;
            try
            {
                // 读取文件的所有行，并将它们存储到字符串数组中
                NameLines = System.IO.File.ReadAllLines(FileNameToRead);

                // 遍历数组并输出每一行的内容
                foreach (string line in NameLines)
                {
                    //Console.WriteLine("当前名单："+line);
                }
            }
            catch (IOException error)
            {
                // 处理文件读取时可能出现的异常，例如文件不存在、没有读取权限等
                Console.WriteLine("文件读取错误: " + error.Message);
                System.Windows.MessageBox.Show("文件读取错误: " + error.Message);//弹出提示框
                this.Close();//关闭窗口
                return;
            }
            // 读取文件的所有行，并将它们存储到字符串数组中
            NameLines = System.IO.File.ReadAllLines(FileNameToRead);

            //尝试读出文件
            try
            {
                foreach (string line in NameLines)
                {
                    NameShow.Text += "\n"+line;//逐行输出名字
                    NameShow.Height += 16;
                }

            }
            catch(IOException error)
            {
                System.Windows.MessageBox.Show("文件读取错误: " + error.Message);
                this.Close();//关闭窗口
                return;//结束进程
            }
            
            //同步滚动条与文本框高度
            NameRoll.Height=NameShow.Height;

            return;
        }

        

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Temp_NamePath);//尝试打开文件

            }
            catch (IOException error)
            {
                // 处理文件读取时可能出现的异常，例如文件不存在、没有读取权限等
                Console.WriteLine("打开错误：" + error.Message);
                System.Windows.MessageBox.Show("打开错误：" + error.Message,"", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                return;
            }
            
            return;
        }
    }
}
