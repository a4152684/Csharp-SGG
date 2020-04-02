using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace szw
{
    class Program
    {
        static void Main(string[] args)
        {
            //定义观测值、必要观测值、多余观测量个数等各种数
            const int n = 37; const int t = 23; const int r = n - t;
            double m0;//单位权中误差
            const int N1 = 0; const int N2 = 20;//基准点
            double H1 = 216.118; double H2 = 205.379;//基准点高程，单位m
            int minPointIndex;//精度最弱点点号
            double mMinPoint;//精度最弱点中误差
            double[] height = new double[n];//高差

            //初始化各类矩阵
            Matrix B = new Matrix(n, t);
            Matrix V = new Matrix(n, 1);
            Matrix x = new Matrix(t, 1);
            Matrix L = new Matrix(n, 1);
            Matrix P = new Matrix(n, n);
            Matrix Q = new Matrix(n, n);
            Matrix NBB = new Matrix(t, t);
            Matrix W = new Matrix(t, 1);
            Matrix Qxx = new Matrix(t, t);

            //导入数据至矩阵 
            FileStream fs = new FileStream("E:\\曹臻个人\\2017301610095\\水准网2.csv", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
            string str = "";
            sr.ReadLine();
            for (int i = 0; i < n; i++)
            {
                str = sr.ReadLine();
                string[] PrimiryStr = new String[100];
                PrimiryStr = str.Split(',');
                //协因数阵Q的构造
                Q.m[i, i] = Convert.ToDouble(PrimiryStr[1]);
                //处理参数点号并构造B
                //同时构造L，其中t个参数近似值均取0
                string[] num = PrimiryStr[0].Split(new char[2] { '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
                height[i] = Convert.ToDouble(PrimiryStr[2]);//读取高差观测值
                int num1 = int.Parse(num[0].Trim());
                int num2 = int.Parse(num[1].Trim());//读取文件中的点号
                L.m[i, 0] = height[i];
                //B矩阵中每一行点号在前面的为-1，后面的为1，其余对应的数值为0
                //已知点点号除外
                //若两点均未知，L矩阵对应行中的值即为高差，否则需要根据点的前后做调整
                if (num1 != N1 && num1 != N2)
                {
                    if (num1 > N1 && num1 < N2) num1 -= 1; else if (num1 > N2) num1 -= 2;
                    B.m[i, num1] = -1;
                }
                else if (num1 == N1) L.m[i, 0] = height[i] + H1;
                else if (num1 == N2) L.m[i, 0] = height[i] + H2;
                if (num2 != N1 && num2 != N2)
                {
                    if (num2 > N1 && num2 < N2) num2 -= 1; else if (num2 > N2) num2 -= 2;
                    B.m[i, num2] = 1;
                }
                else if (num2 == N1) L.m[i, 0] = height[i] - H1;
                else if (num2 == N2) L.m[i, 0] = height[i] - H2;


            }
            sr.Close();

            //进行计算
            P = Matrix.inv(Q);
            NBB = Matrix.transposs(B) * P * B;
            W = Matrix.transposs(B) * P * L;
            x = Matrix.inv(NBB) * W;
            V = B * x - L;
            m0 = Math.Sqrt(((Matrix.transposs(V) * P * V).m[0, 0]) / r);
            Qxx = Matrix.inv(NBB);
            //找出精度最低点的点号
            double max = Qxx.m[0, 0];
            minPointIndex = 0;
            for (int i = 0; i < t; i++)
            {
                if (Qxx.m[i, i] > max)
                {
                    max = Qxx.m[i, i]; minPointIndex = i;
                }
            }
            mMinPoint = m0 * Math.Sqrt(Qxx.m[minPointIndex, minPointIndex]);//精度最弱点中误差
            Console.WriteLine("1 公里高差观测值的中误差为:{0}mm", 1000 * m0);
            Console.WriteLine("最弱点中误差:{0}mm", 1000 * mMinPoint);
            Console.WriteLine("各未知点的高程平差值及中误差为：");
            for (int i = 0; i < t; i++)
            {
                Console.WriteLine("{0}m {1}mm", x.m[i, 0], 1000 * m0 * Math.Sqrt(Qxx.m[i, i]));
            }
            Console.WriteLine("高差平差值为:");
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine("{0}m", V.m[i, 0] + height[i]);
            }
            Console.WriteLine("{0}", (Matrix.transposs(B) * P * V).m[0, 0]); //检核
            Console.ReadKey();
        }
    }
}
