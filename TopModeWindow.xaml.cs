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
    /// TopModeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TopModeWindow : Window
    {
        bool StartWindowOpen = false;//点名窗口打开
        Point pressedPosition;
        bool isDragMoved = false;

        public bool TopWindowPathCheck(string path)
        {
            //检查配置文件中路径的合法性

            //空白检测
            if (path == "")
            {
                Console.WriteLine("路径为空！");

                return false;//如果没有设定，就退出检查
            }

            //冒号检测
            if (path.Contains("\"") == true)
            {
                Console.WriteLine("文件读取错误: 不能有冒号");
                System.Windows.MessageBox.Show("文件读取错误: 不能有冒号！，已经重置设置", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                path = string.Empty;
                Properties.Settings.Default.Save_NamePath = path;
                Properties.Settings.Default.Save();//重置所有保存的内容
                return false;//退出

            }

            //后缀检测
            string extension;
            try
            {
                extension = System.IO.Path.GetExtension(path);
            }
            catch (IOException error)
            {
                // 处理文件读取时可能出现的异常，例如文件不存在、没有读取权限等
                Console.WriteLine("文件读取错误: " + error.Message);
                System.Windows.MessageBox.Show("文件读取错误: " + error.Message, "后缀错误", MessageBoxButton.OK, MessageBoxImage.Error);//弹出提示框
                System.Windows.MessageBox.Show("先前设定的路径\"" + path + "\"似乎不是一个合法的路径，已经重置设置");//弹出提示框
                path = string.Empty;
                Properties.Settings.Default.Save_NamePath = path;
                Properties.Settings.Default.PathSettingTime = string.Empty;
                Properties.Settings.Default.Save();//重置所有保存的内容
                return false;
            }

            extension = System.IO.Path.GetExtension(path);
            if (extension == ".txt") ;
            else
            {
                System.Windows.MessageBox.Show("设定的路径\"" + path + "\"似乎不是一个合法的路径，已经重置设置", "路径错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                path = string.Empty;
                Properties.Settings.Default.Save_NamePath = path;
                Properties.Settings.Default.PathSettingTime = string.Empty;
                Properties.Settings.Default.Save();//重置所有保存的内容
                return false;
            }
            try
            {
                // 读取文件的所有行，并将它们存储到字符串数组中
                String[] NameLines = System.IO.File.ReadAllLines(path);

                // 遍历数组并输出每一行的内容
                Console.WriteLine("当前名单:");
                foreach (string line in NameLines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (IOException error)
            {
                // 处理文件读取时可能出现的异常，例如文件不存在、没有读取权限等
                Console.WriteLine("文件读取错误: " + error.Message);
                System.Windows.MessageBox.Show("文件读取错误: " + error.Message, "路径错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                return false;
            }
            return true;

        }

        //窗口加载完成后
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //获取适配的高度
            double Width = SystemParameters.PrimaryScreenWidth / 15;
            double Height = SystemParameters.PrimaryScreenHeight / 15;

            //设定高度
            this.MinHeight = Height;
            this.MaxHeight = Height;
            this.MinWidth = Width;
            this.MaxWidth = Width;
            this.Width = Width;
            this.Height = Height;
            this.Left = SystemParameters.PrimaryScreenWidth / 1.3;
            this.Top = SystemParameters.PrimaryScreenHeight / 1.5;
        }
        
        //判断是点击操作还是拖动
        public TopModeWindow()
        {
            InitializeComponent();
            
        }

        //获取点击坐标
        void Window_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            pressedPosition = e.GetPosition(this);
            
        }


        void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && pressedPosition != e.GetPosition(this))
            {
                isDragMoved = true;
                DragMove();
            }
        }

        //抬起鼠标时
        void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragMoved)
            {
                isDragMoved = false;
                e.Handled = true;
            }
        }


        //点名事件
        private async void start_Click(object sender, RoutedEventArgs e)
        {
            if(TopWindowPathCheck(Properties.Settings.Default.Save_NamePath)==false) return;
            if (StartWindowOpen == true) return;//如果已经打开了点名窗口，退出
            var window= new TopModeWindow_StartWindow();
            window.Owner = this;
            window.Topmost = true;
            window.Show();
            StartWindowOpen = true;
            await Task.Delay(3810);
            StartWindowOpen = false;
            return;
        }

        //关闭事件
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();

            this.Close();
            return;
        }

        
    }
}
