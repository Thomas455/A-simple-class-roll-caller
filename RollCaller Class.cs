using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace 班级点名器
{
    internal class RollCaller
    {
        //防重复
        public static int Start_Value = 0;//启动随机数
        public static string[] Name_Called = new string[21];//应为14，预留7位
        public static int Name_Called_Time = 1;
        public static string StrTemp;

        //随机数启动种子
        public static void Start_RandomValue()
        {
            Random random = new Random();
            Start_Value = random.Next(0, 512);
            Console.WriteLine("随机数启动种子：" + Start_Value);
        }

        //随机方法
        public static int Randompp(int Max_Value)
        {
            byte[] randomBytes = new byte[512+4];
            RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            rngServiceProvider.GetBytes(randomBytes);
            int result = Math.Abs(BitConverter.ToInt32(randomBytes, Start_Value));
            Random random = new Random(result);
            Console.WriteLine(result);
            result= (result % Max_Value) +1;

            return result;
        }


       



    }
}
