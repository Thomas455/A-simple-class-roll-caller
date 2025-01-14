﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        


        Point pressedPosition;//鼠标位置
        bool isDragMoved = false;

        

        //检查方法
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
            if (extension != ".txt")
            {
                System.Windows.MessageBox.Show("设定的路径\"" + path + "\"似乎不是一个合法的路径，已经重置设置", "路径错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                path = string.Empty;
                Properties.Settings.Default.Save_NamePath = path;
                Properties.Settings.Default.PathSettingTime = string.Empty;
                Properties.Settings.Default.Save();//重置所有保存的内容
                return false;
            }
            else
                ;
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
            pressedPosition = e.GetPosition(this);//获取按下鼠标位置
            Console.WriteLine("LeftButtonDown"+pressedPosition.ToString());
            return;
        }

        

        void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && pressedPosition != e.GetPosition(this))
            {
                isDragMoved = true;
                DragMove();
            }
            return ;
        }

        //抬起鼠标时
        void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragMoved)
            {
                isDragMoved = false;
                e.Handled = true;
            }
            return;
        }

        private void Window_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            return ;
        }

        private void Window_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            return;
        }


        //点名事件
        private async void start_Click(object sender, RoutedEventArgs e)
        {
            //打开窗口
            start.IsEnabled = false;
            if(TopWindowPathCheck(Properties.Settings.Default.Save_NamePath)==false) return;
            
            


            //实际点名
            //读取名单
            string FileNameToRead = @Properties.Settings.Default.Save_NamePath;
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
                System.Windows.MessageBox.Show("文件读取错误: " + error.Message, "读取错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                return;
            }
            // 读取文件的所有行，并将它们存储到字符串数组中
            NameLines = System.IO.File.ReadAllLines(FileNameToRead);



            //随机点名部分
            //这一部分已经尝试做到高度随机了，让每个人都有机会被抽
            string Lucky = null;//被抽中的幸运儿

            long Long_Tick = DateTime.Now.Ticks;//获取时间刻

            string Str_Time_s = DateTime.Now.ToString("ss");//获取秒
            int Time_s = int.Parse(Str_Time_s);//str to int

            int Seed = RollCaller.Randompp(0, 999);



            //Console.WriteLine(Seed);

            Random Name_random = new Random(Seed);
            int randomIndex = Name_random.Next(NameLines.Length);//生成一个随机数，并对应到数组里的内容
            Lucky = NameLines[randomIndex];

            //Console.WriteLine("幸运儿："+Lucky);

            //点名防重复
            short ReCalled_time = 0;//再生成次数
            for (int j = 1; j <= RollCaller.Name_Called.Length - 7; j++)
            {
                
                while (Lucky == RollCaller.Name_Called[j])//相同时再生成
                {
                    ReCalled_time++;
                    Console.WriteLine("__替换" + Lucky);
                    Name_random = new Random(RollCaller.Randompp(0, 200));
                    randomIndex = Name_random.Next(NameLines.Length);//生成一个随机数，并对应到数组里的内容
                    Lucky = NameLines[randomIndex];
                    j = 1;//重新检查

                    if (ReCalled_time > 15) break;
                }
                if(ReCalled_time > 15) break;

            }

            Console.WriteLine("幸运儿：" + Lucky);
            if (RollCaller.Name_Called_Time > RollCaller.Name_Called.Length - 7) RollCaller.Name_Called_Time = 1;
            RollCaller.Name_Called[RollCaller.Name_Called_Time] = Lucky;
            RollCaller.Name_Called_Time++;

            //准备发送至新窗口
            RollCaller.StrTemp = Lucky;
            


            //打开窗口
            var window = new TopModeWindow_StartWindow();
            window.Owner = this;
            window.Topmost = true;
            window.Show();


            await Task.Delay(3810);
            window.Close();//关闭窗口
            
            start.IsEnabled = true;
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
