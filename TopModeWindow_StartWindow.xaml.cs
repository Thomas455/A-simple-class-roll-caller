using System;
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
    /// TopModeWindow_StartWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TopModeWindow_StartWindow : Window
    {
        //全局变量
        string Temp_NamePath = Properties.Settings.Default.Save_NamePath;//读取已经储存的路径
        

        public TopModeWindow_StartWindow()
        {
            InitializeComponent();
        }

        //自制随机方法
        public int Randompp(int n, int m)
        {
            //零号种子
            int Seed0;
            if (n == 0)
            {
                byte[] randomBytes = new byte[4];
                RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                rngServiceProvider.GetBytes(randomBytes);
                int result = BitConverter.ToInt32(randomBytes, 0);
                Random random = new Random(result);
                Seed0 = random.Next();
                Console.WriteLine(result);
            }
            else Seed0 = n;

            int Ramdon_num = Seed0;
            for (int i = 0; i <= m; i++)
            {
                Random random = new Random(Ramdon_num);

                byte[] randomBytes = new byte[128];
                RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
                rngServiceProvider.GetBytes(randomBytes);
                Ramdon_num = BitConverter.ToInt32(randomBytes, random.Next(124));

            }
            Console.WriteLine("调用Ramdonpp，循环：" + m);



            return Ramdon_num;
        }





        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Width = SystemParameters.PrimaryScreenWidth / 2;
                this.Height= SystemParameters.PrimaryScreenHeight / 2;
                this.Left= SystemParameters.PrimaryScreenWidth / 4;
                this.Top= SystemParameters.PrimaryScreenHeight / 4;
                
            }
            catch { }



            await Task.Delay(100);

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



            //随机点名部分
            //这一部分已经尝试做到高度随机了，让每个人都有机会被抽
            string Lucky = null;//被抽中的幸运儿

            long Long_Tick = DateTime.Now.Ticks;//获取时间刻

            string Str_Time_s = DateTime.Now.ToString("ss");//获取秒
            int Time_s = int.Parse(Str_Time_s);//str to int

            int Seed = Randompp(0, 999);



            //Console.WriteLine(Seed);


            Random Time_Random = new Random(Seed);
            int RollTime = Time_Random.Next(40, 65);//生成一个随机数，用于决定名单随机循环次数


            
            for (int i = 0; i < RollTime; i++)//循环名单，抽取幸运儿
            {
                //点一次名
                Random Name_random = new Random(Seed + Time_s * i);
                int randomIndex = Name_random.Next(NameLines.Length);//生成一个随机数，并对应到数组里的内容
                Lucky = NameLines[randomIndex];
                Name.Content = Lucky;//切换文本框

                await Task.Delay(500 / RollTime);// 等待的延迟时间

            }

            

            Console.WriteLine("幸运儿：" + Lucky);
            for (int i = 0; i < 3; i++)
            {
                await Task.Delay(100);
                Name.Content = "";
                await Task.Delay(300);
                Name.Content=Lucky;
            }
            await Task.Delay(2000);

            this.Close();

            return;
        }
    }
}
