using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eight_homework1
{
    public partial class Form1 : Form
    {
        //采用CGCS2000参数
        /*
        public static double a = 6378137;
        public static double b = 6356752.3141;
        public static double c = 6399593.6259;
        public static double e2 = 0.00669438002290;
        public static double e12 = 0.00673949677548;
        public static double roll = 206264.806247096355;
        */
        //采用CGCS2000参数
        public static double a = 6378137;
        public static double b = 6356752.3142;
        public static double c = (a - b) / a;
        public static double e2 = 0.00669437999013;
        public static double e12 = 0.006739496742227;
        public static double roll = 206264.806247096355;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //大地主题正算
        private void button1_Click(object sender, EventArgs e)
        {
            //double l1 = double.Parse(textBox1.Text);
            //double l2 = double.Parse(textBox2.Text);
            //double l3 = double.Parse(textBox3.Text);

            //double b1 = double.Parse(textBox4.Text);
            //double b2 = double.Parse(textBox5.Text);
            //double b3 = double.Parse(textBox6.Text);

            //double a1 = double.Parse(textBox7.Text);
            //double a2 = double.Parse(textBox8.Text);
            //double a3 = double.Parse(textBox9.Text);

            //double S = double.Parse(textBox10.Text);
            double l1 = 35;
            double l2 = 49;
            double l3 = 36.33;
            double b1 = 47;
            double b2 = 46;
            double b3 = 52.647;
            double a1 = 44;
            double a2 = 12;
            double a3 = 13.664;
            double S = 44797.2826;

            double L1 = 0, B1 = 0, A12 = 0;
            dms2s(l1, l2, l3, out L1);
            dms2s(b1, b2, b3, out B1);
            dms2s(a1, a2, a3, out A12);//全部化成以秒为单位
            double sinB = Math.Sin(B1 / 60.0 / 60.0 * Math.PI / 180.0);
            double cosB = Math.Cos(B1 / 60.0 / 60.0 * Math.PI / 180.0);
            
            double V = Math.Sqrt(1 + e12 * cosB * cosB);
            double M = c / Math.Pow(V, 3);
            double N = c / V;
            double del_B0 = roll / M * S * Math.Cos(A12/60.0/60.0 * Math.PI / 180.0);
            double del_L0 = roll / N * S * sinB / cosB;
            double del_A0 = del_L0 * sinB;

            double Bm = B1 + 0.5 * del_B0;
            double Am = A12 + 0.5 * del_A0;
            
            double cosBm = Math.Cos(Bm / 60.0 / 60.0 * Math.PI / 180.0);   //把cosBm记一下
            double Vm = Math.Sqrt(1 + e12 * cosBm * cosBm);
            double Mm = c / Math.Pow(Vm, 3);
            double Nm = c / Vm;

            double del_B1=0, del_L1=0, del_A1 = 0;
            double sina = Math.Sin(Am / 60.0 / 60.0 * Math.PI / 180.0);//把SinA记一下
            double cosa = Math.Cos(Am / 60.0 / 60.0 * Math.PI / 180.0);//把CosA记一下
            double t2 = Math.Pow(Math.Tan(Bm / 60.0 / 60.0 * Math.PI / 180.0), 2);
            double n2 = e12 * cosBm * cosBm;
            //迭代一次
            del_B1 = Vm * Vm / Nm * roll * S * cosa * (1 + S * S / 24.0 / Nm / Nm * (sina * sina * (2 + 3 * t2 + 3 * n2 * t2) + 3 * n2 * cosa * cosa * (-1 + t2 - n2 - 4 * t2 * n2)));
            del_L1 = roll / Nm * S / cosB * sina * (1 + S * S / 24.0 / Nm / Nm * (sina * sina * t2 - cosa * cosa * (1 + n2 - 9 * t2 * n2 + n2 * n2)));
            del_A1 = roll / Nm * S * sina * Math.Sqrt(t2) * (1 + S * S / 24.0 / Nm / Nm * (cosa * cosa*(2 + 7 * n2 + 9 * t2 * n2 + 5 * n2 * n2) + sina * sina * (2 + t2 + 2 * n2)));
            //迭代计算
            int count = 1;
            while (Math.Abs(del_B1-del_B0)>0.001 && Math.Abs(del_L1-del_L0)>0.001 && Math.Abs(del_A1-del_A0)>0.001)
            {
                del_B0 = del_B1;
                del_L0 = del_L1;
                del_A0 = del_A1;
                Bm = B1 + 0.5 * del_B0;
                Am = A12 + 0.5 * del_A0;
                cosBm = Math.Cos(Bm / 60.0 / 60.0 * Math.PI / 180.0);
                sina = Math.Sin(Am / 60.0 / 60.0 * Math.PI / 180.0);
                cosa = Math.Cos(Am / 60.0 / 60.0 * Math.PI / 180.0);
                Vm = Math.Sqrt(1 + e12 * cosBm * cosBm);
                Mm = c / Math.Pow(Vm, 3);
                Nm = c / Vm;
                del_B1 = Vm * Vm / Nm * roll * S * cosa * (1 + S * S / 24.0 / Nm / Nm * (sina * sina * (2 + 3 * t2 + 3 * n2 * t2) + 3 * n2 * cosa * cosa * (-1 + t2 - n2 - 4 * t2 * n2)));
                del_L1 = roll / Nm * S / cosB * sina * (1 + S * S / 24.0 / Nm / Nm * (sina * sina * t2 - cosa * cosa * (1 + n2 - 9 * t2 * n2 + n2 * n2)));
                del_A1 = roll / Nm * S * sina * Math.Sqrt(t2) * (1 + S * S / 24.0 / Nm / Nm * (cosa * cosa * (2 + 7 * n2 + 9 * t2 * n2 + 5 * n2 * n2) + sina * sina * (2 + t2 + 2 * n2)));
                count += 1;
                MessageBox.Show(string.Format("1={0},2={1},3={2}", Math.Abs(del_B1 - del_B0), Math.Abs(del_L1 - del_L0), Math.Abs(del_A1 - del_A0)));
            }
            double B2 = B1 + del_B0;
            double L2 = L1 + del_L0;
            double A21 = A12 + del_A0;
            if (A12 < 180 * 60 * 60)
                A21 = A21 + 180.0 * 60 * 60;
            else
                A21 = A21 - 180 * 60 * 60;
            double b21 = 0, b22 = 0, b23 = 0;
            double l21 = 0, l22 = 0, l23 = 0;
            double A211 = 0, A212 = 0, A213 = 0;
            s2dms(B2, out b21, out b22, out b23);
            s2dms(L2, out l21, out l22, out l23);
            s2dms(A21, out A211, out A212, out A213);

            //输出
            textBox11.Text = string.Format("{0}", b21);
            textBox12.Text = string.Format("{0}", b22);
            textBox13.Text = string.Format("{0}", b23);
            textBox14.Text = string.Format("{0}", l21);
            textBox15.Text = string.Format("{0}", l22);
            textBox16.Text = string.Format("{0}", l23);
            textBox17.Text = string.Format("{0}", A211);
            textBox18.Text = string.Format("{0}", A212);
            textBox19.Text = string.Format("{0}", A213);



        }

        public void dms2s(double a, double b, double c, out double d)
        {
            d = a * 60 * 60 + b * 60 + c;
        }

        public void s2dms(double d, out double a, out double b, out double c)
        {
            a = Math.Floor(d / 3600);
            b = Math.Floor((d - a * 3600) / 60);
            c = d - a * 3600 - b * 60;
        }

        //大地主题反算
        private void button2_Click(object sender, EventArgs e)
        {
            //读入数据
            //double b1 = double.Parse(textBox20.Text);
            //double b2 = double.Parse(textBox21.Text);
            //double b3 = double.Parse(textBox22.Text);

            //double l1 = double.Parse(textBox23.Text);
            //double l2 = double.Parse(textBox24.Text);
            //double l3 = double.Parse(textBox25.Text);

            //double b4 = double.Parse(textBox26.Text);
            //double b5 = double.Parse(textBox27.Text);
            //double b6 = double.Parse(textBox28.Text);

            //double l4 = double.Parse(textBox29.Text);
            //double l5 = double.Parse(textBox30.Text);
            //double l6 = double.Parse(textBox31.Text);
            double b1 = 47;
            double b2 = 46;
            double b3 = 52.647;
            double l1 = 35;
            double l2 = 49;
            double l3 = 36.33;
            double b4 = 48;
            double b5 = 4;
            double b6 = 9.6384;
            double l4 = 36;
            double l5 = 14;
            double l6 = 45.0505;

            //度分秒转换
            double B1, L1, B2, L2;
            dms2s(b1, b2, b3, out B1);
            dms2s(l1, l2, l3, out L1);
            dms2s(b4, b5, b6, out B2);
            dms2s(l4, l5, l6, out L2);

            //把大地参数计算好
            double Bm = 0.5 * (B1 + B2);
            double sinBm = Math.Sin(Bm / 60.0 / 60.0 * Math.PI / 180.0);
            double cosBm = Math.Cos(Bm / 60.0 / 60.0 * Math.PI / 180.0);
            double tanBm = Math.Tan(Bm / 60.0 / 60.0 * Math.PI / 180.0);
            double t2 = tanBm * tanBm;
            double n2 = e12 * cosBm * cosBm;
            double Vm = Math.Sqrt(1 + e12 * cosBm * cosBm);
            double Nm = c / Vm;
            double del_L = L2 - L1;
            double del_B = B2 - B1;

            //开始计算高斯平均引数公式的系数
            double r01, r21, r03, s10, s12, s30,t01,t21,t03;
            double SsinAm, ScosAm, A12, A21, S, del_A, Am=0;
            r01 = Nm / roll * cosBm;
            r21 = Nm * cosBm / 24.0 / Math.Pow(roll, 3) / Math.Pow(Vm, 4) * (1 + n2 - 9 * n2 * t2 + n2 * n2);
            r03 = -Nm / 24.0 / Math.Pow(roll, 3) * Math.Pow(cosBm, 3) * t2;
            s10 = Nm / roll / Vm / Vm;
            s12 = -Nm / 24.0 / Math.Pow(roll, 3) / Vm / Vm * cosBm * cosBm * (2 + 3 * t2 + 2 * n2);  //加了个负号
            s30 = Nm / 8.0 / Math.Pow(roll, 3) / Math.Pow(Vm, 6) * (n2 - t2 * n2);

            SsinAm = r01 * del_L + r21 * del_B * del_B * del_L + r03 * Math.Pow(del_L, 3);
            ScosAm = s10 * del_B + s12 * del_B * del_L * del_L + s30 * Math.Pow(del_B, 3);

            t01 = tanBm * cosBm;
            //t21 = cosBm * tanBm * (2 + 7 * n2 + 9 * t2 * n2 ) / 24.0 / roll / roll / Math.Pow(Vm, 4);
            t21 = cosBm * tanBm * (2 + 7 * n2 + 9 * t2 * n2 + 5 * n2 * n2) / 24.0 / roll / roll / Math.Pow(Vm, 4);
            t03 = Math.Pow(cosBm, 3) * tanBm * (2 + t2 + 2 * n2) / 24.0 / roll / roll;

            del_A = t01 * del_L + t21 * del_B * del_B * del_L + t03 * Math.Pow(del_L, 3);
            //计算Am，从而求出S,此间有判断
            double T=0, cotAm;
            cotAm = Math.Abs(ScosAm / SsinAm);
            if (Math.Abs(del_B) >= Math.Abs(del_L))
                T = Math.Atan(Math.Abs(SsinAm / ScosAm));   //返回一个弧度值
            else
                T = Math.PI / 4.0 + Math.Atan(Math.Abs((1 - cotAm) / (1 + cotAm)));
            if (del_B > 0 && del_L >= 0)
                Am = T;
            if (del_B < 0 && del_L >= 0)
                Am = Math.PI - T;
            if (del_B <= 0 && del_L < 0)
                Am = Math.PI + T;
            if (del_B > 0 && del_L < 0)
                Am = 2 * Math.PI - T;
            if (del_B==0&& del_L>0)
                Am = Math.PI / 2.0;
            S = SsinAm / Math.Sin(Am);
            //把Am的弧度化为秒
            Am = Am / Math.PI * 180.0 * 60.0 * 60.0;
            MessageBox.Show(string.Format("Am:{0},del_A:{1}", Am/60/60,del_A/60/60));
            A12 = Am - 0.5 * del_A;
            A21 = Am + 0.5 * del_A;
            MessageBox.Show(string.Format("A12:{0},A21:{1}", A12/60/60, A21/60/60));
            double del_A2 = del_L * sinBm;
            MessageBox.Show(string.Format("del_A2={0}", del_A2 / 60 / 60));
            //del_A有问题！！
            if (A12 < 180 * 60 * 60)
                A21 = A21 + 180.0 * 60 * 60;
            else
                A21 = A21 - 180 * 60 * 60;

            double a1, a2, a3, a4, a5, a6;
            s2dms(A12, out a1, out a2, out a3);
            s2dms(A21, out a4, out a5, out a6);

            textBox32.Text = string.Format("{0}", S);
            textBox33.Text = string.Format("{0}", a1);
            textBox34.Text = string.Format("{0}", a2);
            textBox35.Text = string.Format("{0}", a3);
            textBox36.Text = string.Format("{0}", a4);
            textBox37.Text = string.Format("{0}", a5);
            textBox38.Text = string.Format("{0}", a6);

        }

        //高斯投影正算按钮
        private void button3_Click(object sender, EventArgs e)
        {
            double b1 = double.Parse(textBox39.Text);
            double b2 = double.Parse(textBox40.Text);
            double b3 = double.Parse(textBox41.Text);
            double l1 = double.Parse(textBox42.Text);
            double l2 = double.Parse(textBox43.Text);
            double l3 = double.Parse(textBox44.Text);
            double l01 = double.Parse(textBox50.Text);
            double l02 = double.Parse(textBox51.Text);
            double l03 = double.Parse(textBox52.Text);
            
            //double b1 = 30;
            //double b2 = 30;
            //double b3 = 0;
            //double l1 = 114;
            //double l2 = 20;
            //double l3 = 0;
            double B, L,L0, x=0, y=0;
            dms2s(b1, b2, b3, out B);
            dms2s(l1, l2, l3, out L);
            dms2s(l01, l02, l03, out L0);
            //子午线弧长正算
            double X = 0;
            b2x(B, out X);
            bl2xy(B, L,L0, X, out x, out y);
            textBox45.Text = string.Format("{0}", x);
            textBox46.Text = string.Format("{0}", y);
            textBox47.Text = string.Format("{0}", l01);
            textBox48.Text = string.Format("{0}", l02);
            textBox49.Text = string.Format("{0}", l03);
        }

        //高斯投影反算按钮
        private void button4_Click(object sender, EventArgs e)
        {
            double l01 = double.Parse(textBox47.Text);
            double l02 = double.Parse(textBox48.Text);
            double l03 = double.Parse(textBox49.Text);
            double L0 = 0;
            dms2s(l01, l02, l03, out L0);
            double x = double.Parse(textBox45.Text);
            double y = double.Parse(textBox46.Text);
            double Bf = 0;
            x2b(x, out Bf);
            Bf = Bf / 3600.0 * Math.PI / 180.0;
            double B = 0, l = 0;
            xy2bl(x, y, L0, Bf, out B, out l);

            double b1 = 0, b2 = 0, b3 = 0;
            s2dms(B, out b1, out b2, out b3);
            textBox39.Text = string.Format("{0}", b1);
            textBox40.Text = string.Format("{0}", b2);
            textBox41.Text = string.Format("{0}", b3);

            double l1 = 0, l2 = 0, l3 = 0;
            s2dms(l, out l1, out l2, out l3);
            textBox42.Text = string.Format("{0}", l1);
            textBox43.Text = string.Format("{0}", l2);
            textBox44.Text = string.Format("{0}", l3);
            textBox50.Text = string.Format("{0}", l01);
            textBox51.Text = string.Format("{0}", l02);
            textBox52.Text = string.Format("{0}", l03);
        }

        //子午线弧长正算
        public void b2x(double B, out double X)            
        {
            //这里B的单位是秒
            double m0 = a * (1.0 - e2);
            double m2 = 3.0 / 2.0 * e2 * m0;
            double m4 = 5.0 / 4.0 * e2 * m2;
            double m6 = 7.0 / 6.0 * e2 * m4;
            double m8 = 9.0 / 8.0 * e2 * m6;
            double a0 = m0 + m2 / 2.0 + 3.0 / 8.0 * m4 + 5.0 / 16.0 * m6 + 35.0 / 128.0 * m8;
            double a2 = m2 / 2.0 + m4 / 2.0 + 15.0 / 32.0 * m6 + 7.0 / 16.0 * m8;
            double a4 = m4 / 8.0 + 3.0 / 16.0 * m6 + 7.0 / 32.0 * m8;
            double a6 = m6 / 32.0 + m8 / 16.0;
            double a8 = m8 / 128.0;
            double B2 = B;
            B = B * Math.PI / 180.0 / 60.0 / 60.0;       //将B的单位划为弧度
            X = a0 * B - a2 / 2.0 * Math.Sin(2 * B) + a4 / 4.0 * Math.Sin(4 * B) - a6 / 6.0 * Math.Sin(6 * B) + a8 / 8.0 * Math.Sin(8 * B);
        }
        //子午线弧长反算
        public void x2b(double X, out double B)
        {
            double m0 = a * (1 - e2);
            double m2 = 3.0 / 2.0 * e2 * m0;
            double m4 = 5.0 / 4.0 * e2 * m2;
            double m6 = 7.0 / 6.0 * e2 * m4;
            double m8 = 9.0 / 8.0 * e2 * m6;
            double a0 = m0 + m2 / 2.0 + 3.0 / 8.0 * m4 + 5.0 / 16.0 * m6 + 35.0 / 128.0 * m8;
            double a2 = m2 / 2.0 + m4 / 2.0 + 15.0 / 32.0 * m6 + 7.0 / 16.0 * m8;
            double a4 = m4 / 8.0 + 3.0 / 16.0 * m6 + 7.0 / 32.0 * m8;
            double a6 = m6 / 32.0 + m8 / 16.0;
            double a8 = m8 / 128.0;
            double B0 = X / a0;
            double B1 = 1 / a0 * (X + a2 / 2.0 * Math.Sin(2 * B0) - a4 / 4.0 * Math.Sin(4 * B0) + a6 / 6.0 * Math.Sin(6 * B0) - a8 / 8.0 * Math.Sin(8 * B0));
            while (Math.Abs(B1 - B0) > Math.Pow(10, -50))
            {
                B0 = B1;
                B1 = 1 / a0 * (X + a2 / 2.0 * Math.Sin(2 * B0) - a4 / 4.0 * Math.Sin(4 * B0) + a6 / 6.0 * Math.Sin(6 * B0) - a8 / 8.0 * Math.Sin(8 * B0));
            }
            B = B1 * 180.0 / Math.PI * 60.0 * 60.0;       //将B的单位划为秒
        }
        //高斯投影正算
        public void bl2xy(double b, double L,double L0, double X, out double x, out double y)
        {
            double B = b * Math.PI / 180.0 / 60.0 / 60.0;    //将B的单位划为弧度
            double t = Math.Tan(B);
            double n2 = e12 * Math.Cos(B) * Math.Cos(B);
            double p = 180.0 / Math.PI * 3600.0;
            double sb = Math.Sin(B);
            double cb = Math.Cos(B);
            double ll = L - L0;
            double N = a * Math.Pow((1 - e2 * sb * sb), -0.5);
            x = X + N / 2.0 / p / p * sb * cb * ll * ll + N / 24.0 / Math.Pow(p, 4) * sb * Math.Pow(cb, 3) * (5 - t * t + 9 * n2 + 4 * n2 * n2) * Math.Pow(ll, 4) + N / 720.0 / Math.Pow(p, 6) * sb * Math.Pow(cb, 5) * (61 - 58 * t * t + Math.Pow(t, 4)) * Math.Pow(ll, 6);
            y = N / p * cb * ll + N / 6.0 / Math.Pow(p, 3) * Math.Pow(cb, 3) * (1 - t * t + n2) * Math.Pow(ll, 3) + N / 120.0 / Math.Pow(p, 5) * Math.Pow(cb, 5) * (5 - 18 * t * t + Math.Pow(t, 4) + 14 * n2 - 58 * n2 * t * t) * Math.Pow(ll, 5);
        }
        //高斯投影反算
        public void xy2bl(double x, double y, double L0, double Bf, out double B, out double L)
        {
            double tf = Math.Tan(Bf);
            double sbf = Math.Sin(Bf);
            double cbf = Math.Cos(Bf);
            double nf2 = e12 * cbf * cbf;
            double Nf = a * Math.Pow((1 - e2 * sbf * sbf), -0.5);
            double Mf = a * (1 - e2) * Math.Pow(1 - e2 * sbf * sbf, -3.0 / 2.0);
            double l = 0;
            B = Bf - tf / 2.0 / Mf / Nf * y * y + tf / 24.0 / Mf / Math.Pow(Nf, 3) * (5 + 3 * tf * tf + nf2 - 9 * nf2 * tf * tf) * Math.Pow(y, 4) - tf / 720.0 / Mf / Math.Pow(Nf, 5) * (61 + 90 * tf * tf + 45 * Math.Pow(tf, 4)) * Math.Pow(y, 6);
            l = 1.0 / Nf / cbf * y - 1.0 / 6.0 / Math.Pow(Nf, 3) * cbf * (1 + 2 * tf * tf + nf2) * Math.Pow(y, 3) + 1.0 / 120.0 / Math.Pow(Nf, 5) / cbf * (5 + 28 * tf * tf + 24 * Math.Pow(tf, 4) + 6 * nf2 + 8 * nf2 * tf * tf) * Math.Pow(y, 5);
            B = B * 180.0 / Math.PI * 60.0 * 60.0;       //将B的单位划为秒
            l = l * 180.0 / Math.PI * 60.0 * 60.0;       //将l的单位划为秒
            L = L0 + l;
        }

        private void textBox59_TextChanged(object sender, EventArgs e)
        {

        }
        //正轴墨卡托投影正算
        private void button5_Click(object sender, EventArgs e)
        {
            double B = 0, L = 0, B0 = 0, L0 = 0;
            //double b1 = double.Parse(textBox53.Text);
            //double b2 = double.Parse(textBox54.Text);
            //double b3 = double.Parse(textBox55.Text);

            //double l1 = double.Parse(textBox56.Text);
            //double l2 = double.Parse(textBox57.Text);
            //double l3 = double.Parse(textBox58.Text);
            if (textBox61.Text == string.Empty && textBox62.Text == string.Empty && textBox63.Text == string.Empty && textBox64.Text == string.Empty && textBox65.Text == string.Empty && textBox66.Text == string.Empty)
            {
                B0 = 30 * 60 * 60;
                L0 = 0;
            }
            else
            {
                double b4 = double.Parse(textBox61.Text);
                double b5 = double.Parse(textBox62.Text);
                double b6 = double.Parse(textBox63.Text);

                double l4 = double.Parse(textBox64.Text);
                double l5 = double.Parse(textBox65.Text);
                double l6 = double.Parse(textBox66.Text);
                dms2s(b4, b5, b6, out B0);
                dms2s(l4, l5, l6, out L0);
            }

            double b1 = 30;
            double b2 = 30;
            double b3 = 0;
            double l1 = 114;
            double l2 = 20;
            double l3 = 0;
            
            dms2s(b1, b2, b3, out B);
            dms2s(l1, l2, l3, out L);
            //把B,L,BO,LO化成弧度制
            B = B * Math.PI / 180.0 / 60 / 60;
            L = L * Math.PI / 180.0 / 60 / 60;
            B0 = B0 * Math.PI / 180.0/60/60;
            L0 = L0 * Math.PI / 180.0/60/60;

            double x, y, k;
            k = a * a / b / Math.Sqrt(1 + e2 * Math.Cos(B0) * Math.Cos(B0));
            double eee = Math.Sqrt(e2);
            double r1 = Math.Tan(Math.PI / 4 + B / 2);
            double r2 = (1 - eee * Math.Sin(B)) / (1 + eee * Math.Sin(B));
            double r3 = r1 * Math.Pow(r2, eee / 2);
            double r4 = Math.Log(r3);
            x = k * r4;
            y = k * (L - L0);
            textBox59.Text = string.Format("{0}", x);
            textBox60.Text = string.Format("{0}", y);
;        }
        //正轴墨卡托反算
        private void button6_Click(object sender, EventArgs e)
        {
            double x = double.Parse(textBox59.Text);
            double y = double.Parse(textBox60.Text);
            double B= 0;
            double L= 0;
            double B0 = Math.PI / 6, L0 = 0, B2 = 0;
            double k= a * a / b / Math.Sqrt(1 + e2 * Math.Cos(B0) * Math.Cos(B0));
            double eee = Math.Sqrt(e2);
            
            double r1 = (1 - eee * Math.Sin(B)) / (1 + eee * Math.Sin(B));
            double r2 = eee / 2 * Math.Log(r1);
            double r3 = -x / k;
            double r4 = Math.Pow(Math.E, r3);
            double r5 = Math.Pow(Math.E, r2);
            B2 = Math.PI / 2 - 2 * Math.Atan(r4 * r5);
            MessageBox.Show(string.Format("r1={0}\nr2={1}\nr3={2}\nr4={3}\nr5={4}\n", r1, r2, r3, r4, r5));
            MessageBox.Show(string.Format("进入循环前：B={0},B2={1}", B / Math.PI * 180, B2 / Math.PI * 180));
            //B2 = Math.PI / 2 - 2 * Math.Atan(Math.Pow(Math.E, -x / k) * Math.Pow(Math.E, Math.E / 2 * Math.Log(Math.E, ((1 - eee * Math.Sin(B)) / (1 + eee * Math.Sin(B))))));
            //double rr1, rr2, rr3, rr4;
            int count = 1;
            while (Math.Abs(B2 - B) > 0.001 )
            {
                MessageBox.Show(string.Format("循环前：B={0},B2={1}",B/Math.PI*180, B2 / Math.PI * 180));
                B = B2;

                //rr1 = Math.Tan(Math.PI / 4 + B / 2);
                //rr2 = (1 - eee * Math.Sin(B)) / (1 + eee * Math.Sin(B));
                //rr3 = r1 * Math.Pow(r2, eee / 2);
                //rr4 = Math.Log(Math.E, r3);
                //x = k * rr4;

                r1 = (1 - eee * Math.Sin(B)) / (1 + eee * Math.Sin(B));
                r2 = eee / 2 * Math.Log(r1);
                r3 = -x / k;
                r4 = Math.Pow(Math.E, r3);
                r5 = Math.Pow(Math.E, r2);
                B2 = Math.PI / 2 - 2 * Math.Atan(r4 * r5);
                MessageBox.Show(string.Format("循环后:B={0},B2={1}", B / Math.PI * 180, B2 / Math.PI * 180));
                count += 1;
                MessageBox.Show(string.Format("count={0}", count));
            }
            L = y / k + L0;
            //弧度制化为秒数
            B = B / Math.PI * 180 * 60 * 60;
            L = L / Math.PI * 180 * 60 * 60;
            double b1, b2, b3, l1, l2, l3;
            s2dms(B, out b1, out b2, out b3);
            s2dms(L, out l1, out l2, out l3);
            textBox53.Text = string.Format("{0}", b1);
            textBox54.Text = string.Format("{0}", b2);
            textBox55.Text = string.Format("{0}", b3);
            textBox56.Text = string.Format("{0}", l1);
            textBox57.Text = string.Format("{0}", l2);
            textBox58.Text = string.Format("{0}", l3);
        }

        private void label77_Click(object sender, EventArgs e)
        {

        }
        //Lambert投影正算（切圆锥）
        private void button8_Click(object sender, EventArgs e)
        {
            double b1 = double.Parse(textBox80.Text);
            double b2 = double.Parse(textBox79.Text);
            double b3 = double.Parse(textBox78.Text);
            double l1 = double.Parse(textBox77.Text);
            double l2 = double.Parse(textBox76.Text);
            double l3 = double.Parse(textBox75.Text);
            double b4 = double.Parse(textBox72.Text);
            double b5 = double.Parse(textBox71.Text);
            double b6 = double.Parse(textBox70.Text);
            double l4 = double.Parse(textBox69.Text);
            double l5 = double.Parse(textBox68.Text);
            double l6 = double.Parse(textBox67.Text);
            double B, L, B0, L0;
            dms2s(b1, b2, b3, out B);
            dms2s(l1, l2, l3, out L);
            dms2s(b4, b5, b6, out B0);
            dms2s(l4, l5, l6, out L0);
            //把这些角度转换为弧度制
            B = B / 180.0 / 60 / 60 * Math.PI;
            L = L / 180.0 / 60 / 60 * Math.PI;
            B0 = B0 / 180.0 / 60 / 60 * Math.PI;
            L0 = L0 / 180.0 / 60 / 60 * Math.PI;

            double beta = 0,q0 = 0, q = 0, p0 = 0,p = 0, x, y;
            beta = Math.Sin(B0);
            double N0 = a / Math.Sqrt(1 - e2 * Math.Sin(B0) * Math.Sin(B0));
            p0 = N0 / Math.Tan(B0);
            double l = L - L0;
            double gama = beta * l;
            double del_B, del_q;
            del_B = B - B0;

            double cosB0 = Math.Cos(B0);
            double n2 = e12 * cosB0 * cosB0;
            double tanB0 = Math.Tan(B0);

            double t1, t2, t3, t4, t5;
            t1 = (1 - n2 + n2 * n2 - n2 * n2 * n2) / cosB0;
            t2 = tanB0 / 2 / cosB0 * (1 + n2 - 3 * n2 * n2);
            t3 = (1 + 2 * tanB0 * tanB0 + n2 - 3 * n2 * n2 + 6 * n2 * n2 * tanB0 * tanB0) / 6 / cosB0;
            t4 = tanB0 * (5 + 6 * tanB0 * tanB0 - n2) / 24 / cosB0;
            t5 = (5 + 28 * tanB0 * tanB0 + 24 * tanB0 * tanB0 * tanB0 * tanB0) / 120 / cosB0;
            del_q = t1 * del_B + t2 * del_B * del_B + t3 * del_B * del_B * del_B + t4 * del_B * del_B * del_B * del_B + t5 * del_B * del_B * del_B * del_B * del_B;
            p = p0 * Math.Pow(Math.E, beta * del_q);
            y = p * Math.Sin(gama);
            x = p0 - p * Math.Cos(gama);
            textBox74.Text = string.Format("{0}", x);
            textBox73.Text = string.Format("{0}", y);
        }
        //Lambert投影反算（切圆锥）
        private void button7_Click(object sender, EventArgs e)
        {
            double x = double.Parse(textBox74.Text);
            double y = double.Parse(textBox73.Text);
            double B0, L0,B,L;

            double b4 = double.Parse(textBox72.Text);
            double b5 = double.Parse(textBox71.Text);
            double b6 = double.Parse(textBox70.Text);
            double l4 = double.Parse(textBox69.Text);
            double l5 = double.Parse(textBox68.Text);
            double l6 = double.Parse(textBox67.Text);
            dms2s(b4, b5, b6, out B0);
            dms2s(l4, l5, l6, out L0);
            //把这些角度转换为弧度制
            B0 = B0 / 180.0 / 60 / 60 * Math.PI;
            L0 = L0 / 180.0 / 60 / 60 * Math.PI;
            double beta, l, gama, N0, p0, p;
            beta = Math.Sin(B0);
            N0 = a / Math.Sqrt(1 - e2 * Math.Sin(B0) * Math.Sin(B0));
            p0 = N0 / Math.Tan(B0);
            gama = Math.Atan(y / (p0 - x));
            l = gama / beta;
            L = L0 + l;
            p = Math.Sqrt((p0 - x) * (p0 - x) + y * y);
            double del_q, del_B;
            del_q = -Math.Log(p / p0) / beta;
            double t1, t2, t3, t4, t5;
            double cosB0 = Math.Cos(B0);
            double n2 = e12 * cosB0 * cosB0;
            double tanB0 = Math.Tan(B0);
            t1 = cosB0 * (1 + n2);
            t2 = (-1 - 4 * n2 - 3 * n2 * n2) * cosB0 * cosB0 * tanB0 / 2;
            t3 = (-1 + tanB0 * tanB0 - 5 * n2 + 13 * n2 * tanB0 * tanB0 - 7 * n2 * n2 + 27 * n2 * n2 * tanB0 * tanB0) * cosB0 * cosB0 * cosB0 / 6;
            t4 = (5 - tanB0 * tanB0 + 56 * n2 - 40 * n2 * tanB0 * tanB0) * cosB0 * cosB0 * cosB0 * cosB0 * tanB0 / 24;
            t5 = (5 - 18 * tanB0 * tanB0 + tanB0 * tanB0 * tanB0 * tanB0) * Math.Pow(cosB0, 5) / 120;
            del_B = t1 * del_q + t2 * del_q * del_q + t3 * del_q * del_q * del_q + t4 * del_q * del_q * del_q * del_q + t5 * del_q * del_q * del_q * del_q * del_q;
            B = del_B + B0;
            //把B,L的弧度化为秒
            B = B / Math.PI * 180 * 60 * 60;
            L = L / Math.PI * 180 * 60 * 60;
            double b1, b2, b3, l1, l2, l3;
            s2dms(B, out b1, out b2, out b3);
            s2dms(L, out l1, out l2, out l3);

            textBox80.Text = string.Format("{0}", b1);
            textBox79.Text = string.Format("{0}", b2);
            textBox78.Text = string.Format("{0}", b3);
            textBox77.Text = string.Format("{0}", l1);
            textBox76.Text = string.Format("{0}", l2);
            textBox75.Text = string.Format("{0}", l3);
        
        }

        private void label84_Click(object sender, EventArgs e)
        {

        }
        //Lambert投影正算（割圆锥）
        private void button10_Click(object sender, EventArgs e)
        {
            double b1 = double.Parse(textBox94.Text);
            double b2 = double.Parse(textBox93.Text);
            double b3 = double.Parse(textBox92.Text);
            double l1 = double.Parse(textBox91.Text);
            double l2 = double.Parse(textBox90.Text);
            double l3 = double.Parse(textBox89.Text);
            double b4 = double.Parse(textBox86.Text);
            double b5 = double.Parse(textBox85.Text);
            double b6 = double.Parse(textBox84.Text);
            double l4 = double.Parse(textBox83.Text);
            double l5 = double.Parse(textBox82.Text);
            double l6 = double.Parse(textBox81.Text);
            double l7 = double.Parse(textBox97.Text);
            double l8 = double.Parse(textBox96.Text);
            double l9 = double.Parse(textBox95.Text);
            double b7 = double.Parse(textBox107.Text);
            double b8 = double.Parse(textBox106.Text);
            double b9 = double.Parse(textBox105.Text);
            double B, L, B1, B2, L0, B0, x, y;
            dms2s(b1, b2, b3, out B);
            dms2s(l1, l2, l3, out L);
            dms2s(b4, b5, b6, out B1);
            dms2s(l4, l5, l6, out B2);
            dms2s(l7, l8, l9, out L0);
            dms2s(b7, b8, b9, out B0);
            MessageBox.Show(string.Format("{0},{1},{2},{3},{4},{5}", B/60/60, L / 60 / 60, B1 / 60 / 60, B2 / 60 / 60, B0 / 60 / 60, L0 / 60 / 60));
            //把秒化成弧度制
            B = B / 180.0 / 60 / 60 * Math.PI;
            L = L / 180.0 / 60 / 60 * Math.PI;
            B1 = B1 / 180.0 / 60 / 60 * Math.PI;
            B2 = B2 / 180.0 / 60 / 60 * Math.PI;
            L0 = L0 / 180.0 / 60 / 60 * Math.PI;
            B0 = B0 / 180.0 / 60 / 60 * Math.PI;
            double eee = Math.Sqrt(e2);
            double m1 = Math.Cos(B1) / Math.Sqrt(1 - e2 * Math.Sin(B1) * Math.Sin(B1));
            double m2 = Math.Cos(B2) / Math.Sqrt(1 - e2 * Math.Sin(B2) * Math.Sin(B2));
            double t = Math.Tan(Math.PI / 4 - B / 2) / Math.Pow((1 - eee * Math.Sin(B)) / (1 + eee * Math.Sin(B)), eee / 2);
            double t0= Math.Tan(Math.PI / 4 - B0 / 2) / Math.Pow((1 - eee * Math.Sin(B0)) / (1 + eee * Math.Sin(B0)), eee / 2);
            double t1= Math.Tan(Math.PI / 4 - B1 / 2) / Math.Pow((1 - eee * Math.Sin(B1)) / (1 + eee * Math.Sin(B1)), eee / 2);
            double t2= Math.Tan(Math.PI / 4 - B2 / 2) / Math.Pow((1 - eee * Math.Sin(B2)) / (1 + eee * Math.Sin(B2)), eee / 2);
            double n = Math.Log(m1 / m2) / Math.Log(t1 / t2);
            double F = m1 / n * Math.Pow(t1, n);
            double r = a * F * Math.Pow(t, n);
            double r0 = a * F * Math.Pow(t0, n);
            double angle = n * (B - B0);
            x = r * Math.Sin(angle);
            y = r0 - r * Math.Cos(angle);



            /*

            double l, beta, N0,N1, N2, K, q1, q2, q0, q, del_q, del_B;
            l = L - L0;
            del_B = B - B0;
            double sinB0 = Math.Sin(B0);
            double cosB0 = Math.Cos(B0);
            double n2 = e12 * cosB0 * cosB0;
            double tanB0 = Math.Tan(B0);
            //用del_q计算
            //double t1, t2, t3, t4, t5;
            //t1 = (1 - n2 + n2 * n2 - n2 * n2 * n2) / cosB0;
            //t2 = tanB0 / 2 / cosB0 * (1 + n2 - 3 * n2 * n2);
            //t3 = (1 + 2 * tanB0 * tanB0 + n2 - 3 * n2 * n2 + 6 * n2 * n2 * tanB0 * tanB0) / 6 / cosB0;
            //t4 = tanB0 * (5 + 6 * tanB0 * tanB0 - n2) / 24 / cosB0;
            //t5 = (5 + 28 * tanB0 * tanB0 + 24 * tanB0 * tanB0 * tanB0 * tanB0) / 120 / cosB0;
            //del_q = t1 * del_B + t2 * del_B * del_B + t3 * del_B * del_B * del_B + t4 * del_B * del_B * del_B * del_B + t5 * del_B * del_B * del_B * del_B * del_B;

            //用q-q0计算
            double sinB = Math.Sin(B);
            double eee = Math.Sqrt(e2);
            double r1, r2;
            r1 = (1 + sinB) / (1 - sinB);
            r2 = (1 + eee * sinB) / (1 - eee * sinB);
            q = 0.5 * Math.Log(r1) - 0.5 * eee * Math.Log(r2);
            double r3, r4;
            r3 = (1 + sinB0) / (1 - sinB0);
            r4 = (1 + eee * sinB0) / (1 - eee * sinB0);
            q0 = 0.5 * Math.Log(r3) - 0.5 * eee * Math.Log(r4);
            double r5, r6;
            r5= (1 + Math.Sin(B1)) / (1 - Math.Sin(B1));
            r6= (1 + eee * Math.Sin(B1)) / (1 - eee * Math.Sin(B1));
            q1 = 0.5 * Math.Log(r5) - 0.5 * eee * Math.Log(r6);
            double r7, r8;
            r7 = (1 + Math.Sin(B2)) / (1 - Math.Sin(B2));
            r8 = (1 + eee * Math.Sin(B2)) / (1 - eee * Math.Sin(B2));
            q2 = 0.5 * Math.Log(r7) - 0.5 * eee * Math.Log(r8);

            del_q = q - q0;
            //用级数回求法算得：0.4745，用q-q0算得：0.4747

            N0 = a / Math.Sqrt(1 - e2 * sinB0 * sinB0);
            N1 = a / Math.Sqrt(1 - e2 * Math.Sin(B1) * Math.Sin(B1));
            N2 = a / Math.Sqrt(1 - e2 * Math.Sin(B2) * Math.Sin(B2));
            double m1 = N1 * Math.Cos(B1);
            double m2 = N2 * Math.Cos(B2);
            beta = Math.Log(m1 / m2) / (q2 - q1);
            K = N1 * Math.Cos(B1) / (beta * Math.Pow(Math.E, -beta * q1));
            double p1 = K * Math.Pow(Math.E, -beta * q1);
            double p2 = K * Math.Pow(Math.E, -beta * q2);
            double p0 = N0 / tanB0;
            double p = p0 * Math.Pow(Math.E, beta * del_q);
            double gama = beta * l;
            y = p * Math.Sin(gama);
            x = p0 - p * Math.Cos(gama);


    */
            textBox88.Text = string.Format("{0}",x);
            textBox87.Text = string.Format("{0}",y);

        }
    }
}