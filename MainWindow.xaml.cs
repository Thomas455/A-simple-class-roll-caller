//一个简单的点名器
//Thomas出品
//作者B站：
//          逗比Thomas
//
//2024.6.6
//
//
//


using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.Reflection;
using System.Security.Cryptography;
using System.Runtime.InteropServices;



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

        
        
        
        

        

        //检查方法
        public bool PathCheck(string path)
        {
            //检查配置文件中路径的合法性

            //空白检测
            if (path == "")
            {
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                Can_start = false;
                Console.WriteLine("路径为空！");
                
                return false;//如果没有设定，就退出检查
            }

            //冒号检测
            if (path.Contains("\"") == true)
            {
                Console.WriteLine("文件读取错误: 不能有冒号");
                System.Windows.MessageBox.Show("文件读取错误: 不能有冒号！，已经重置设置", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                NamePath.Text = "";//重置文本框
                path= string.Empty;
                Temp_NamePath_Time = string.Empty;
                Properties.Settings.Default.Save_NamePath = path;
                Properties.Settings.Default.PathSettingTime = Temp_NamePath_Time;
                Properties.Settings.Default.Save();//重置所有保存的内容
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                Can_start = false;
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
                NamePath.Text = "";//重置文本框
                path = string.Empty;
                Temp_NamePath_Time = string.Empty;
                Properties.Settings.Default.Save_NamePath = path;
                Properties.Settings.Default.PathSettingTime = Temp_NamePath_Time;
                Properties.Settings.Default.Save();//重置所有保存的内容
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                Can_start = false;
                return false;
            }

            extension = System.IO.Path.GetExtension(path);
            if (extension == ".txt")
            {
                NamePath.Text = path;//若路径合法，则自动设定这个路径
                File.Content = "当前使用的名单：" + path + "    设定于:" + Temp_NamePath_Time;
                Name.Content = "点名已就绪";
                Can_start = true;
            }
            else
            {
                System.Windows.MessageBox.Show("设定的路径\"" + path + "\"似乎不是一个合法的路径，已经重置设置", "路径错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                NamePath.Text = "";//重置文本框
                path = string.Empty;
                Temp_NamePath_Time = string.Empty;
                Properties.Settings.Default.Save_NamePath = path;
                Properties.Settings.Default.PathSettingTime = Temp_NamePath_Time;
                Properties.Settings.Default.Save();//重置所有保存的内容
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                Can_start = false;
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
                Can_start = false;
                return false;
            }
            return true;

        }

        public MainWindow()
        {
            

            InitializeComponent();
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            
            return;
        }

        //获取版本号
        public void GetVersion(object sender, EventArgs e)
        {
            AssemblyName assemblyName = this.GetType().Assembly.GetName();//获取当前程序名
            Version version = assemblyName.Version;//获取版本号
            VersionShow.Content = version;//替换文本
            Console.WriteLine("版本信息:" + assemblyName);

            return;
        }

        //界面加载完成后
        public void Window_Loaded(object sender, EventArgs e)
        {
            //程序初始化
            

            //检查配置文件中路径的合法性

            PathCheck(Temp_NamePath);
            return;
        }



        //点名按钮
        private async void start_Click(object sender, RoutedEventArgs e)
        {
            if (Can_start == false)
            {
                System.Windows.MessageBox.Show("请先完成设置","错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
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
                    //Console.WriteLine("当前名单："+line);
                }
            }
            catch (IOException error)
            {
                // 处理文件读取时可能出现的异常，例如文件不存在、没有读取权限等
                Console.WriteLine("文件读取错误: " + error.Message);
                System.Windows.MessageBox.Show("文件读取错误: " + error.Message,"读取错误",MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                return;
            }
            // 读取文件的所有行，并将它们存储到字符串数组中
            NameLines = System.IO.File.ReadAllLines(FileNameToRead);



            //随机点名部分
            //这一部分已经尝试做到高度随机了，让每个人都有机会被抽
            string Lucky = null;//被抽中的幸运儿

            long Long_Tick = DateTime.Now.Ticks;//获取时间刻

            string Str_Time_s = DateTime.Now.ToString("ss");//获取秒
            int Time_s=int.Parse(Str_Time_s);//str to int

            int Seed = RollCaller.Randompp(0, 999);
            


            //Console.WriteLine(Seed);


            Random Time_Random = new Random(Seed);
            int RollTime = Time_Random.Next(40, 65);//生成一个随机数，用于决定名单随机循环次数
            
            
            start.IsEnabled = false;//锁定按钮
            for (int i = 0; i<RollTime; i++)//循环名单，抽取幸运儿
            {
                //点一次名
                Random Name_random = new Random(Seed + Time_s*i);
                int randomIndex = Name_random.Next(NameLines.Length);//生成一个随机数，并对应到数组里的内容
                Lucky = NameLines[randomIndex];
                Name.Content = Lucky;//切换文本框

                await Task.Delay(500/RollTime);// 等待的延迟时间

            }


            //点名防重复
            short ReCalled_time = 0;//再生成次数
            for (int j = 1; j <= RollCaller.Name_Called.Length-7; j++)
            {
                
                while (Lucky == RollCaller.Name_Called[j])//相同时再生成
                {
                    ReCalled_time++;
                    Console.WriteLine("__替换" + Lucky);
                    Random Name_random = new Random(RollCaller.Randompp(0,200));
                    int randomIndex = Name_random.Next(NameLines.Length);//生成一个随机数，并对应到数组里的内容
                    Lucky = NameLines[randomIndex];
                    j = 1;//重新检查
                    Name.Content = Lucky;
                    if (ReCalled_time > 15) break;
                }
                if (ReCalled_time > 15) break;

            }
            if (RollCaller.Name_Called_Time > RollCaller.Name_Called.Length-7) RollCaller.Name_Called_Time = 1;
            RollCaller.Name_Called[RollCaller.Name_Called_Time] = Lucky;
            RollCaller.Name_Called_Time++;

            Console.WriteLine("幸运儿：" + Lucky);
            start.IsEnabled = true;//解锁按钮
            return;
        }





        //批量点名
        private async void start_MoreName_Click(object sender, RoutedEventArgs e)
        {

            if (Can_start == false)
            {
                System.Windows.MessageBox.Show("请先完成设置", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
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





            //随机批量点名部分
            //这一部分已经尝试做到高度随机了，让每个人都有机会被抽
            

            long Long_Tick = DateTime.Now.Ticks;//获取时间刻

            string Str_Time_s = DateTime.Now.ToString("ss");//获取秒
            int Time_s = int.Parse(Str_Time_s);//str to int

            int Seed = 0;
            /*随机算法
            for (int i = 0; i <= 1000; i++)
            {
                Random Seed_Ramdom = new Random();
                Seed += Seed_Ramdom.Next(1000);//随机种子
            }
            */
            //Console.WriteLine(Seed);
            


            //获取点名数量
            int NameNum=0;


            if (!int.TryParse(NameNumIn.Text, out NameNum)) {
                Console.WriteLine("批量点名设定错误" );
                System.Windows.MessageBox.Show("设定错误: 点名数量应给是一个数字！", "设置错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                NameNumIn.Text = string.Empty;
                return;

            }
            //尝试转换str to int

            

            if (NameNum > 99)
            {

                System.Windows.MessageBoxResult Result=System.Windows.MessageBox.Show("你设定的点名数量是："+NameNum+"\n这可能会引起设备卡顿或程序崩溃！如果你希望继续执行，请点击确定。", "设置错误", MessageBoxButton.OKCancel, MessageBoxImage.Warning);//弹出提示框
                if (Result != System.Windows.MessageBoxResult.OK) return;//如果不点确定，终止进程
            }

            //是否允许重复
            bool Can_reName = (bool)reName.IsChecked;
            Console.WriteLine(Can_reName);

            //防止重复
            string[] HaveNamed;
            HaveNamed = new string[NameLines.Length+7];
            if(NameNum > NameLines.Length && Can_reName != true)
            {
                System.Windows.MessageBox.Show("在关闭“允许重复”选项时,点名数量不可大于名单中名字数量！", "设置错误", MessageBoxButton.OKCancel, MessageBoxImage.Warning);//弹出提示框
                return;
            }
            





            //点名
            string Lucky = null;//被抽中的幸运儿
            start_2.IsEnabled = false;//锁定按钮
            LuckyName.Text=string.Empty;//清空显示名单
            LuckyName.Height = 16;
            for (int i = 1; i <= NameNum; i++)//循环名单，抽取幸运儿
            {
                //点一次名
                //获取种子
                Seed = RollCaller.Randompp(0,999);
                



                //生成次级种子
                Random Name_random = new Random(RollCaller.Randompp(Seed + Time_s * i,200));
                int randomIndex = Name_random.Next(NameLines.Length);//生成一个随机数，并对应到数组里的内容
                Lucky = NameLines[randomIndex];

                //关闭允许重复时执行
                if (Can_reName != true)
                {
                    //
                    for (int j = 1; j <= i; j++)
                    {

                        while (Lucky == HaveNamed[j])//相同时再生成
                        {
                            randomIndex = Name_random.Next(NameLines.Length);//生成一个随机数，并对应到数组里的内容
                            Lucky = NameLines[randomIndex];
                            Console.WriteLine(Lucky);
                            j = 1;
                        }


                    }
                    HaveNamed[i] = Lucky;
                }


                
                LuckyName.Text += "" + Lucky + "\n";
                LuckyName.Height += 20;
                LuckyNameRoll.Height += 22;
                LuckyNameRoll.Height = LuckyName.Height;//同步滚动条与文本框高度
                if (NameNum > 10) await Task.Delay(700 / NameNum);
                if (NameNum <= 10) await Task.Delay(80);
                Console.WriteLine("(批量)幸运儿：" + Lucky);

            }
            
            LuckyNameRoll.Height=LuckyName.Height;//同步滚动条与文本框高度
            start_2.IsEnabled = true;//解锁按钮



            return;
        }

        //路径选择按钮事件
        private void GetPathButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()//打开路径选择对话框
            {
                Multiselect = false,//不可选择多个
                Title = "请选择你的名单",
                Filter = "Files (*.txt)|*.txt"//选择txt文件
            };
            DialogResult dialogResult = openFileDialog.ShowDialog();//获取返回结果
            Console.WriteLine(dialogResult);
            if (dialogResult == System.Windows.Forms.DialogResult.OK)//判断返回结果
            {
                Console.WriteLine(openFileDialog.FileName);
                NamePath.Text = openFileDialog.FileName;//覆盖输入框
            }
            else
            {
                Console.WriteLine("用户关闭"+dialogResult) ;
                return;
            }
            System.Windows.MessageBoxResult Result = System.Windows.MessageBox.Show("是否设定当前路径为名单路径", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);//弹出提示框
            if (Result == System.Windows.MessageBoxResult.OK) SetButton_Click(sender,e);//如果点确定，设定名单
            return;
        }



        //帮助按钮
        private void TipsButton_Click(object sender, RoutedEventArgs e)
        {
            string Tips = "温馨提示：\n 1.目前只支持txt文件。 \n 2.txt文件格式：一个名字换行一次。\n 3.路径请使用相对路径，不需要双引号。 \n 4.txt内的信息应尽量采用UTF-8编码 \n";
            System.Windows.MessageBox.Show(Tips,"提示",MessageBoxButton.OK, MessageBoxImage.Information);//弹出提示框
            return;
        }


        //设定路径按钮
        public void SetButton_Click(object sender, RoutedEventArgs e)
        {
            Temp_NamePath = NamePath.Text;//获取文本框中路径
            

            //保存设定名单的时间
            string Time= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//获取时间
            Temp_NamePath_Time = Time;//保存至全局变量


            //以上路径的合法性
            if (PathCheck(Temp_NamePath) == false) return;


            //设定路径
            //保存配置
            Properties.Settings.Default.Save_NamePath = Temp_NamePath;
            Properties.Settings.Default.Save();//保存文件路径至配置文件
            Properties.Settings.Default.PathSettingTime = Time;
            Properties.Settings.Default.Save();//保存设定名单的时间至配置文件



            NamePath.Text = Temp_NamePath;//若路径合法，则自动设定这个路径
            File.Content = "当前使用的名单：" + Temp_NamePath + "    设定于:" + Temp_NamePath_Time;
            Name.Content = "点名已就绪";
            Can_start = true;
            System.Windows.MessageBox.Show("设定成功！", "提示");//弹出提示框
            start.IsEnabled = true;//解锁按钮
        }

        //快速设定按钮
        private void NameNum_3_Click(object sender, RoutedEventArgs e)
        {
            NameNumIn.Text = "3";
            return;
        }
        private void NameNum_5_Click(object sender, RoutedEventArgs e)
        {
            NameNumIn.Text = "5";
            return;
        }
        private void NameNum_8_Click(object sender, RoutedEventArgs e)
        {
            NameNumIn.Text = "8";
            return;
        }
        private void NameNum_10_Click(object sender, RoutedEventArgs e)
        {
            NameNumIn.Text = "10";
            return;
        }
        private void NameNum_15_Click(object sender, RoutedEventArgs e)
        {
            NameNumIn.Text = "15";
            return;
        }


        //预览名单
        private void View_Click(object sender, RoutedEventArgs e)
        {
            //检查名单路径是否空白
            if (Temp_NamePath == "")
            {
                File.Content = "路径未设定";
                Name.Content = "请设置点名";
                System.Windows.MessageBox.Show("请先设置路径！", "",MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                Can_start = false;
                return;//如果没有设定，就退出检查
            }

            //检查是否已经准备好点名字
            if (Can_start == true)
            {
                //打开预览窗口
                var window = new NameView();
                window.Owner = this;
                window.Show();


            }
            else
            {
                System.Windows.MessageBox.Show("请先完成设置", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;

            }

            return;


        }

        //打开软件信息窗口
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new infoWindow();

            window.Owner = this;
            window.Show();
            return;
        }
        private void TopMode_Click(object sender, RoutedEventArgs e)
        {
            if (Can_start == false)
            {
                System.Windows.MessageBox.Show("请先完成设置", "错误", MessageBoxButton.OK, MessageBoxImage.Warning);//弹出提示框
                return;
            }
            var window = new TopModeWindow();

            
            window.Topmost = true;
            window.Activate();
            window.Show();
            this.Close();
            return;
        }


        public void TabItemRestart()
        {
            TabItemButton1.FontWeight = FontWeights.Normal;
            TabItemButton2.FontWeight = FontWeights.Normal;
            TabItemButton3.FontWeight = FontWeights.Normal;

            TabItemButton1.BorderBrush = Brushes.WhiteSmoke;
            TabItemButton2.BorderBrush = Brushes.WhiteSmoke;
            TabItemButton3.BorderBrush = Brushes.WhiteSmoke;
            return;
        }
        //标签页切换按钮
        private void TabItem1Button_Click(object sender, RoutedEventArgs e)
        {
            TabItemRestart();
            MyTabControl.SelectedIndex = 0;
            TabItemButton1.FontWeight = FontWeights.Heavy;
            TabItemButton1.BorderBrush = Brushes.DarkGray;
            return;
        }
        private void TabItem2Button_Click(object sender, RoutedEventArgs e)
        {
            TabItemRestart();
            MyTabControl.SelectedIndex = 1;
            TabItemButton2.FontWeight = FontWeights.Heavy;
            TabItemButton2.BorderBrush = Brushes.DarkGray;
            return;
        }
        private void TabItem3Button_Click(object sender, RoutedEventArgs e)
        {
            TabItemRestart();
            MyTabControl.SelectedIndex = 2;
            TabItemButton3.FontWeight = FontWeights.Heavy;
            TabItemButton3.BorderBrush = Brushes.DarkGray;
            return;
        }

        


    }
}
