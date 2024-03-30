using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using static System.Windows.Forms.LinkLabel;


namespace 班级点名器
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //全局变量
        string Temp_NamePath = Properties.Settings.Default.Save_NamePath;//读取已经储存的路径
        string Temp_NamePath_Time = Properties.Settings.Default.PathSettingTime;//读取已经储存的路径
        bool Can_start;




        public MainWindow()
        {
            

            InitializeComponent();
        }

        //界面加载完成后
        public void Window_Loaded(object sender, EventArgs e)
        {
            //程序初始化

            //检查配置文件中路径的合法性
            if (Temp_NamePath == "")
            {
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                Can_start = false;
                return;//如果没有设定，就退出检查
            }
            string extension = System.IO.Path.GetExtension(Temp_NamePath);
            if (extension == ".txt")
            {
                NamePath.Text = Temp_NamePath;//若路径合法，则自动设定这个路径
                File.Content = "当前使用的名单：" + Temp_NamePath + "    设定于:" + Temp_NamePath_Time;
                Name.Content = "点名已就绪";
                Can_start= true;
            }
            else
            {
                System.Windows.MessageBox.Show("先前设定的路径" + Temp_NamePath + "似乎不是一个合法的路径，已经重置设置");//弹出提示框
                NamePath.Text = "";//重置文本框
                Temp_NamePath = string.Empty;
                Temp_NamePath_Time = string.Empty;
                Properties.Settings.Default.Save_NamePath = Temp_NamePath;
                Properties.Settings.Default.PathSettingTime = Temp_NamePath_Time;
                Properties.Settings.Default.Save();//重置所有保存的内容
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                Can_start = false;
                return;
            }
        }



        //点名按钮
        private async void start_Click(object sender, RoutedEventArgs e)
        {
            if (Can_start == false)
            {
                System.Windows.MessageBox.Show("请先完成设置");//弹出提示框
                return;
            }
            File.Content = "当前使用的名单：" + Temp_NamePath + "    设定于:" + Temp_NamePath_Time;

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
                    Console.WriteLine(line);
                }
            }
            catch (IOException error)
            {
                // 处理文件读取时可能出现的异常，例如文件不存在、没有读取权限等
                Console.WriteLine("文件读取错误: " + error.Message);
                System.Windows.MessageBox.Show("文件读取错误: " + error.Message);//弹出提示框
                return;
            }
            // 读取文件的所有行，并将它们存储到字符串数组中
            NameLines = System.IO.File.ReadAllLines(FileNameToRead);


            string Lucky;//被抽中的幸运儿
            for (int i = 0; i<30; i++)
            {
                //点一次名
                Random random = new Random();
                int randomIndex = random.Next(NameLines.Length);
                Lucky = NameLines[randomIndex];
                Name.Content = Lucky;

                // 等待指定的延迟时间
                await Task.Delay(20);

            }







        }
        //路径选择按钮事件
        private void GetPathButton_Click(object sender, RoutedEventArgs e)
        {
            string GetPath = string.Empty;
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Files (*.txt)|*.txt"//选择txt文件
            };
            //var result = openFileDialog.ShowDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetPath = openFileDialog.FileName;
            }
            else
            {
                return;
            }
            NamePath.Text = GetPath;//覆盖输入框
            return;
        }

        //帮助按钮
        private void TipsButton_Click(object sender, RoutedEventArgs e)
        {
            string Tips = "温馨提示：\n 1.目前只支持txt文件。 \n 2.txt文件格式：一个名字换行一次。\n 3.路径请使用相对路径，不需要双引号。 \n 4.txt内的信息应尽量采用UTF-8编码 \n";
            System.Windows.MessageBox.Show(Tips);//弹出提示框
        }


        //设定路径按钮
        private void SetButton_Click(object sender, RoutedEventArgs e)
        {
            Temp_NamePath = NamePath.Text;//获取文本框中路径
            Properties.Settings.Default.Save_NamePath = Temp_NamePath;
            Properties.Settings.Default.Save();//保存文件路径至配置文件

            //保存设定名单的时间
            string Time= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//获取时间
            Temp_NamePath_Time = Time;//保存至全局变量
            Properties.Settings.Default.PathSettingTime = Time;
            Properties.Settings.Default.Save();//保存设定名单的时间至配置文件

            //以上路径的合法性
            string extension = System.IO.Path.GetExtension(Temp_NamePath);
            if (extension == ".txt")
            {
                NamePath.Text = Temp_NamePath;//若路径合法，则自动设定这个路径
                File.Content = "当前使用的名单：" + Temp_NamePath + "    设定于:" + Temp_NamePath_Time;
                Name.Content = "点名已就绪";
                Can_start = true;
                System.Windows.MessageBox.Show("设定成功！");//弹出提示框
            }
            else
            {
                System.Windows.MessageBox.Show(Temp_NamePath + "似乎不是一个合法的路径，已经重置设置");//弹出提示框
                NamePath.Text = "";//重置文本框
                Temp_NamePath = string.Empty;
                Temp_NamePath_Time = string.Empty;//清空变量
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                Can_start = false;
                return;
            }
        }


        //预览文件
        private void view_Click(object sender, RoutedEventArgs e)
        {
            if (Temp_NamePath == "")
            {
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                System.Windows.MessageBox.Show("请先设置路径！");//弹出提示框
                Can_start = false;
                return;//如果没有设定，就退出检查
            }

            System.Diagnostics.Process.Start(Temp_NamePath);//打开文件


        }
    }
}
