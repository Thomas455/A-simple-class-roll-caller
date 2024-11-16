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
        public static int Randompp(int n, int m)
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

            int Random_num = Seed0;
            for (int i = 0; i <= m; i++)
            {

                Random random = new Random(Random_num + DateTime.Now.Second);
                Random_num = random.Next(int.MaxValue - 1);


            }
            Console.WriteLine("调用Ramdonpp，循环：" + m);



            return Random_num;
        }


       



    }
}
