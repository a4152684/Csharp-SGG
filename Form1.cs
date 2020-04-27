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
        public static double e12 = 0.00673949677548;
        public static double c = 6399593.6259;
        public static double roll = 206264.806247096355;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)//大地主题正算
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
            if (A21 < 0)
                A21 = A21 + 180.0 * 60 * 60;
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

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}