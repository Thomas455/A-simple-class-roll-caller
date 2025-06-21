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
        public static string[] Name_Called = new string[21];//应为14，预留7位
        public static int Name_Called_Time = 1;
        public static string StrTemp;


        //随机方法
        public static int Randompp(int Max_Value)
        {
            if (Max_Value == 0) //如果传入的最大值为0，则返回1
            {
                return 1;
            }
            byte[] randomBytes = new byte[10];
            RNGCryptoServiceProvider rngServiceProvider = new RNGCryptoServiceProvider();
            rngServiceProvider.GetBytes(randomBytes);
            int result = Math.Abs(BitConverter.ToInt32(randomBytes,1));
            Random random = new Random(result);
            Console.WriteLine(result);
            result= (result % Max_Value) +1;

            return result;
        }


       



    }
}
