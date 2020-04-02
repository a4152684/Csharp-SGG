using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace 作业3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        //初始化各种数据
        const double p = 180 * 60 * 60 / Math.PI;

        static double DMS2D(double d, double m, double s)//度分秒到°
        {
            double D;
            D = d + m / 60 + s / 3600;
            return D;
        }

        static string D2DMS(double D)//°到度分秒
        {
            string DMS;
            double d, m, s;
            d = Math.Truncate(D);
            m = Math.Truncate((D - d)*60);
            s = D * 3600 - d * 3600 - m * 60;
            DMS = d.ToString() + "," + m.ToString() + "," + s.ToString();
            return DMS;
        }
        static double ComputeMid(double L)//计算中央子午线经度,六度带
        {
            for (int i = 1; ; i++)
            {
                if(Math.Abs(L-6*i+3)<=3)
                {
                    return (6 * i - 3);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //(L,B)->(x,y)
            //c#的sin,cos采用弧度制,asin,acos返回也是弧度

            //先度分秒转换
            double degreeL=0,degreeB=0;
            string dmsTextL,dmsTextB;
            if(radioButtonD.Checked)//若以°形式输入，显示即可
            {
                if (BB.Text == "") MessageBox.Show("请输入B的值");
                if (LL.Text == "") MessageBox.Show("请输入L的值");

                degreeL = Convert.ToDouble(LL.Text);
                degreeB = Convert.ToDouble(BB.Text);

                dmsTextL = D2DMS(degreeL);
                dmsTextB = D2DMS(degreeB);

                LL2.Text = dmsTextL;
                BB2.Text = dmsTextB;
            }
            else//若以度分秒形式输入，转换为°
            {
                if (BB2.Text == "") MessageBox.Show("请输入B的值");
                if (LL2.Text == "") MessageBox.Show("请输入L的值");

                dmsTextB = BB2.Text;
                dmsTextL = LL2.Text;

                string[] PrimiryStrB = new String[100];
                PrimiryStrB = dmsTextB.Split(',');
                string[] PrimiryStrL = new String[100];
                PrimiryStrL = dmsTextL.Split(',');

                degreeB = DMS2D(Convert.ToDouble(PrimiryStrB[0]), Convert.ToDouble(PrimiryStrB[1]), Convert.ToDouble(PrimiryStrB[2]));
                degreeL = DMS2D(Convert.ToDouble(PrimiryStrL[0]), Convert.ToDouble(PrimiryStrL[1]), Convert.ToDouble(PrimiryStrL[2]));

                LL.Text = degreeL.ToString();
                BB.Text = degreeB.ToString();
            }

            double a = Convert.ToDouble(longtextBox.Text);
            double aa = Convert.ToDouble(denominatorTextBox.Text);
            double b = a * (1 - 1/aa);
            double E = Math.Sqrt(a * a - b * b) / a;
            double EE = Math.Sqrt(a * a - b * b) / b;//初始化各种数据         

            double L = degreeL;
            double B = degreeB;//单位为°
            B = B * Math.PI / 180;//化为弧度
            double t = Math.Tan(B);
            double n =Math.Sqrt( EE * EE * Math.Cos(B) * Math.Cos(B) );
            double W = Math.Sqrt(1 - E * E * Math.Sin(B) * Math.Sin(B));
            double V = Math.Sqrt(1 + n * n);
            double N = a / W;

            double X;//由赤道开始到此点的子午线弧长
            double m0, m2, m4, m6, m8;
            m0 = a * (1 - E * E);
            m2 = 1.5 * E * E * m0;
            m4 = 1.25 * E * E * m2;
            m6 = 7 * E * E * m4 / 6;
            m8 = 9 * E * E * m6 / 8;
            double a0, a2, a4, a6, a8;
            a0 = m0 + m2 / 2 + 3 * m4 / 8 + 5 * m6 / 16 + 35 * m8 / 128;
            a2 = m2 / 2 + m4 / 2 + 15 * m6 / 32 + 7 * m8 / 16;
            a4 = m4 / 8 + 3 * m6 / 16 + 7 * m8 / 32;
            a6 = m6 / 32 + m8 / 16;
            a8 = m8 / 128;
            X = a0 * B - a2 * Math.Sin(2 * B) / 2 + a4 * Math.Sin(4 * B) / 4 - a6 * Math.Sin(6 * B) / 6+a8*Math.Sin(8*B)/8;
            //这里B单位为弧度

            double l;  //此点经度与中央子午线的经度之差，在东为正，在西为负，单位″
            double L0;
            if (middleLongitude.Text != "") L0 = Convert.ToDouble(middleLongitude.Text);
            else
            {
                L0 = ComputeMid(L);
                middleLongitude.Text = L0.ToString();//输入至窗体
            }
            //中央子午线经度，单位°
            
            l = (L - L0) * 60*60;//单位″

            double x, y;
            double SB = Math.Sin(B); double CB = Math.Cos(B);
            x = X + N * SB * CB * l * l / (2 * p * p) + N * SB * Math.Pow(CB, 3) * (5 - t * t + 9 * n * n + 4 * Math.Pow(n, 4)) * Math.Pow(l, 4) / (24 * Math.Pow(p, 4))
                +N*SB*Math.Pow(CB,5)*(61-58*t*t+Math.Pow(t,4))*Math.Pow(l,6)/(720*Math.Pow(p,6)) ;
            y = N * CB * l / p + N * Math.Pow(CB, 3) * (1 - t * t + n * n) * Math.Pow(l, 3) / (6 * Math.Pow(p, 3)) + 
                N * Math.Pow(CB, 5) * (5 - 18 * t * t + Math.Pow(t, 4)+14*n*n-58*n*n*t*t) * Math.Pow(l, 5) / (120 * Math.Pow(p, 5));

            /*//电算公式,书p175
            double a0, a3, a4, a5, a6, NN;
            NN = 6399698.902 - (21562.267 - (108.973 - 0.612 * CB * CB) * CB * CB) * CB * CB;
            a0 = 32140.404 - (135.3302 - (0.7092 - 0.004 * CB * CB) * CB * CB) * CB * CB;
            a4 = (0.25 + 0.00252 * CB * CB) * CB * CB - 0.04166;
            a6 = (0.166 * CB * CB - 0.084) * CB * CB;
            a3 = (0.3333333 + 0.001123 * CB * CB) * CB * CB - 0.16666667;
            a5 = 0.0083 - (0.1667 - (0.1968 + 0.004 * CB * CB) * CB * CB) * CB * CB;
            //下面的l单位为°
            l = l / p;
            x = 6367558.4969 * B - (a0 - (0.5 + (a4 + a6 * l * l) * l * l) * l * l * NN) * SB * CB;
            y = (1 + (a3 + a5 * l * l) * l * l) * l * NN * CB;*/
            xx.Text = x.ToString();
            yy.Text = y.ToString();
        }

        double ZiwuxianFansuan(double X,double a,double E,double epu)//反算子午线长度
        {
            //epu是精度，a是长半轴长度，X是子午线长度
            double Bf;
            double m0, m2, m4, m6, m8;
            m0 = a * (1 - E * E);
            m2 = 1.5 * E * E * m0;
            m4 = 1.25 * E * E * m2;
            m6 = 7 * E * E * m4 / 6;
            m8 = 9 * E * E * m6 / 8;
            double a0, a2, a4, a6, a8;
            a0 = m0 + m2 / 2 + 3 * m4 / 8 + 5 * m6 / 16 + 35 * m8 / 128;
            a2 = m2 / 2 + m4 / 2 + 15 * m6 / 32 + 7 * m8 / 16;
            a4 = m4 / 8 + 3 * m6 / 16 + 7 * m8 / 32;
            a6 = m6 / 32 + m8 / 16;
            a8 = m8 / 128;
            Bf = X / a0;
            double FBf;
            FBf = -a2 * Math.Sin(2 * Bf) / 2 + a4 * Math.Sin(4 * Bf) / 4 - a6 * Math.Sin(6 * Bf) / 6 + a8 * Math.Sin(8 * Bf) / 8;
            while(Math.Abs((X-FBf)/a0-Bf)>=epu)
            {
                Bf = (X - FBf) / a0;
                FBf = -a2 * Math.Sin(2 * Bf) / 2 + a4 * Math.Sin(4 * Bf) / 4 - a6 * Math.Sin(6 * Bf) / 6 + a8 * Math.Sin(8 * Bf) / 8;
            }
            return Bf;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //(x,y)->(B,L)
            //c#的sin,cos采用弧度制,asin,acos返回也是弧度
            double a = Convert.ToDouble(longtextBox.Text);
            double aa = Convert.ToDouble(denominatorTextBox.Text);
            double b = a * (1 - 1 / aa);
            double E = Math.Sqrt(a * a - b * b) / a;
            double EE = Math.Sqrt(a * a - b * b) / b;//初始化各种数据\

            if (xx.Text == "") MessageBox.Show("请输入x的值");
            if (yy.Text == "") MessageBox.Show("请输入y的值");
            if (middleLongitude.Text == "") MessageBox.Show("请输入中央子午线经度的值");

            double x = Convert.ToDouble(xx.Text);
            double y = Convert.ToDouble(yy.Text);
            double L0 = Convert.ToDouble(middleLongitude.Text);

            //子午线弧长反算纬度Bf
            double Bf;
            Bf = ZiwuxianFansuan(x, a, E, Math.Pow(10, -6));//单位弧度

            double Nf, Mf, tf, nf;
            double SBf, CBf;
            SBf = Math.Sin(Bf); CBf = Math.Cos(Bf);
            tf = Math.Tan(Bf);
            Nf = a * Math.Pow(1 - E * E * SBf * SBf,-0.5);
            Mf = a * (1 - E * E) * Math.Pow(1 - E * E * SBf * SBf, -1.5);
            nf =Math.Sqrt( EE * EE * CBf * CBf );

            double B, l, L;
            B = Bf - tf * y * y / (2 * Mf * Nf) + tf * (5 + 3 * tf * tf + nf * nf - 9 * nf * nf * tf * tf) * Math.Pow(y, 4) / (24 * Mf * Math.Pow(Nf, 3))
                - tf * (61 + 90 * tf * tf + 45 * Math.Pow(tf, 4)) * Math.Pow(y, 6) / (720 * Mf * Math.Pow(Nf, 5));
            l = y / (Nf * CBf) - (1 + 2 * tf * tf + nf * nf) * Math.Pow(y, 3) / (6 * Math.Pow(Nf, 3) * CBf)
                + (5 + 28 * tf * tf + 24 * Math.Pow(tf, 4) + 6 * nf * nf + 8 * nf * nf * tf * tf) * Math.Pow(y, 5) / (120 * Math.Pow(Nf, 5) * CBf);


            //化为度
            l = l * 180 / Math.PI;
            B = B * 180 / Math.PI;
            L = l + L0;


            BB.Text = B.ToString();
            LL.Text = L.ToString();
            BB2.Text = D2DMS(B);
            LL2.Text = D2DMS(L);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            longtextBox.Text = "6378245";
            denominatorTextBox.Text = "298.3";
            LL.Text = "";
            BB.Text = "";
            middleLongitude.Text = "";
            xx.Text = "";
            yy.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            longtextBox.Text = "6378245";
            denominatorTextBox.Text = "298.3";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            longtextBox.Text = "6378140";
            denominatorTextBox.Text = "298.257";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            longtextBox.Text = "6378137";
            denominatorTextBox.Text = "298.257223563";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            longtextBox.Text = "6378137";
            denominatorTextBox.Text = "298.257222101";
        }

        private void ReadFileZS_Click(object sender, EventArgs e)
        {
            double[] x = new double[100];
            double[] y = new double[100];
            double[] L0 = new double[100];
            double[] L = new double[100];
            double[] B = new double[100];
            double[] l = new double[100];

            double a = Convert.ToDouble(longtextBox.Text);
            double aa = Convert.ToDouble(denominatorTextBox.Text);
            double b = a * (1 - 1 / aa);
            double E = Math.Sqrt(a * a - b * b) / a;
            double EE = Math.Sqrt(a * a - b * b) / b;//初始化各种数据

            double X;//由赤道开始到此点的子午线弧长
            double m0, m2, m4, m6, m8;
            m0 = a * (1 - E * E);
            m2 = 1.5 * E * E * m0;
            m4 = 1.25 * E * E * m2;
            m6 = 7 * E * E * m4 / 6;
            m8 = 9 * E * E * m6 / 8;
            double a0, a2, a4, a6, a8;
            a0 = m0 + m2 / 2 + 3 * m4 / 8 + 5 * m6 / 16 + 35 * m8 / 128;
            a2 = m2 / 2 + m4 / 2 + 15 * m6 / 32 + 7 * m8 / 16;
            a4 = m4 / 8 + 3 * m6 / 16 + 7 * m8 / 32;
            a6 = m6 / 32 + m8 / 16;
            a8 = m8 / 128;         

            OpenFileDialog openDlg = new OpenFileDialog();
            // 指定打开文本文件（后缀名为txt）
            openDlg.Filter = "文本文件(*.txt)|*.txt";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                // 读出文本文件的所有行
                FileStream fs = new FileStream(openDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
                string str = "";
                string[] PrimiryStr = new String[100];
                int count = 0;
                for (int i = 0; ; i++)
                {
                    str = sr.ReadLine();
                    if (str == null) break;
                    PrimiryStr = str.Split(',');
                    L[i] = Convert.ToDouble(PrimiryStr[0]);
                    B[i] = Convert.ToDouble(PrimiryStr[1]);
                    L0[i] = Convert.ToDouble(PrimiryStr[2]);

                    double B2 = B[i] * Math.PI / 180;
                    X = a0 * B2 - a2 * Math.Sin(2 * B2) / 2 + a4 * Math.Sin(4 * B2) / 4 - a6 * Math.Sin(6 * B2) / 6 + a8 * Math.Sin(8 * B2) / 8;
                    double t = Math.Tan(B2);
                    double n = Math.Sqrt(EE * EE * Math.Cos(B2) * Math.Cos(B2));
                    double W = Math.Sqrt(1 - E * E * Math.Sin(B2) * Math.Sin(B2));
                    double V = Math.Sqrt(1 + n * n);
                    double N = a / W;

                    l[i] = (L[i] - L0[i]) * 60 * 60;//单位″
                    double SB = Math.Sin(B2); double CB = Math.Cos(B2);
                    x[i] = X + N * SB * CB * l[i] * l[i] / (2 * p * p) + N * SB * Math.Pow(CB, 3) * (5 - t * t + 9 * n * n + 4 * Math.Pow(n, 4)) * Math.Pow(l[i], 4) / (24 * Math.Pow(p, 4))
                        + N * SB * Math.Pow(CB, 5) * (61 - 58 * t * t + Math.Pow(t, 4)) * Math.Pow(l[i], 6) / (720 * Math.Pow(p, 6));
                    y[i] = N * CB * l[i] / p + N * Math.Pow(CB, 3) * (1 - t * t + n * n) * Math.Pow(l[i], 3) / (6 * Math.Pow(p, 3)) +
                        N * Math.Pow(CB, 5) * (5 - 18 * t * t + Math.Pow(t, 4) + 14 * n * n - 58 * n * n * t * t) * Math.Pow(l[i], 5) / (120 * Math.Pow(p, 5));

                    count++;
                }
                sr.Close();

                //写入当前路径
                FileStream fout = new FileStream("./高斯投影正算结果.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(fout, System.Text.Encoding.GetEncoding(936));
                sw.WriteLine(" B(°)        L(°)       L0(°)       x(m)       y(m)");
                for (int i = 0; i < count; i++)
                {
                    sw.WriteLine("{0},{1},{2},{3},{4}", B[i], L[i], L0[i], x[i], y[i]);
                }
                MessageBox.Show("读取成功");
                sw.Close();
            }
        }

        private void ReadFileFS_Click(object sender, EventArgs e)
        {
            double[] x = new double[100];
            double[] y = new double[100];
            double[] L0 = new double[100];
            double[] L = new double[100];
            double[] B = new double[100];
            double[] l = new double[100];

            double a = Convert.ToDouble(longtextBox.Text);
            double aa = Convert.ToDouble(denominatorTextBox.Text);
            double b = a * (1 - 1 / aa);
            double E = Math.Sqrt(a * a - b * b) / a;
            double EE = Math.Sqrt(a * a - b * b) / b;//初始化各种数据

            OpenFileDialog openDlg = new OpenFileDialog();
            // 指定打开文本文件（后缀名为txt）
            openDlg.Filter = "文本文件(*.txt)|*.txt";
            if (openDlg.ShowDialog() == DialogResult.OK)
            {
                // 读出文本文件的所有行
                FileStream fs = new FileStream(openDlg.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
                string str = "";
                string[] PrimiryStr = new String[100];
                int count = 0;
                for (int i = 0; ; i++)
                {
                    str = sr.ReadLine();
                    if (str == null) break;
                    PrimiryStr = str.Split(',');                   
                    x[i] = Convert.ToDouble(PrimiryStr[0]);
                    y[i] = Convert.ToDouble(PrimiryStr[1]);
                    L0[i] = Convert.ToDouble(PrimiryStr[2]);
                    
                    //子午线弧长反算纬度Bf
                    double Bf;
                    Bf = ZiwuxianFansuan(x[i], a, E, Math.Pow(10, -6));//单位弧度

                    double Nf, Mf, tf, nf;
                    double SBf, CBf;
                    SBf = Math.Sin(Bf); CBf = Math.Cos(Bf);
                    tf = Math.Tan(Bf);
                    Nf = a * Math.Pow(1 - E * E * SBf * SBf, -0.5);
                    Mf = a * (1 - E * E) * Math.Pow(1 - E * E * SBf * SBf, -1.5);
                    nf = Math.Sqrt(EE * EE * CBf * CBf);

                    B[i] = Bf - tf * y[i] * y[i] / (2 * Mf * Nf) + tf * (5 + 3 * tf * tf + nf * nf - 9 * nf * nf * tf * tf) * Math.Pow(y[i], 4) / (24 * Mf * Math.Pow(Nf, 3))
                        - tf * (61 + 90 * tf * tf + 45 * Math.Pow(tf, 4)) * Math.Pow(y[i], 6) / (720 * Mf * Math.Pow(Nf, 5));
                    l[i] = y[i] / (Nf * CBf) - (1 + 2 * tf * tf + nf * nf) * Math.Pow(y[i], 3) / (6 * Math.Pow(Nf, 3) * CBf)
                        + (5 + 28 * tf * tf + 24 * Math.Pow(tf, 4) + 6 * nf * nf + 8 * nf * nf * tf * tf) * Math.Pow(y[i], 5) / (120 * Math.Pow(Nf, 5) * CBf);
                    B[i] = B[i] * 180 / Math.PI;
                    l[i] = l[i] * 180 / Math.PI;
                    L[i] = L0[i] + l[i];

                    count++;
                }
                sr.Close();

                //写入当前路径
                FileStream fout = new FileStream("./高斯投影反算结果.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(fout, System.Text.Encoding.GetEncoding(936));
                sw.WriteLine("x(m)       y(m)       L0(°)       B(°)        L(°)");
                for (int i = 0;i < count ; i++)
                {
                    sw.WriteLine("{0},{1},{2},{3},{4}", x[i], y[i], L0[i], B[i], L[i]);
                }
                MessageBox.Show("读取成功");
                sw.Close();
            }       
        }
    }
}
