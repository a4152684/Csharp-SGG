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
    public partial class Form1 : Form
    {
        double a, f, m0, m2, m4, m6, m8, e1, e2, b;

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] strings = new string[100];
            double[] origin = new double[100];
            double[] end = new double[100];
            double[] s = new double[100];
            int number = 0;
            string filename = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (DialogResult.OK == openFileDialog.ShowDialog())
            {
                filename = openFileDialog.FileName;
            }
            StreamReader reader = null;
            reader = new StreamReader(filename);
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                strings[number++] = line;
            }
            reader.Close();
            a = Convert.ToDouble(textBox1.Text); f = 1 / Convert.ToDouble(textBox2.Text);
            b = (1 - f) * a; e1 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2); e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(b, 2);
            m0 = a * (1 - e1);
            m2 = 3 * e1 * m0 / 2;
            m4 = e1 * 5 * m2 / 4;
            m6 = m4 * e1 * 7 / 6;
            m8 = e1 * 9 * m6 / 8;
            double a0, a2, a4, a6, a8;
            a0 = m0 + m2 / 2 + 3 * m4 / 8 + 5 * m6 / 16 + 35 * m8 / 128;
            a2 = m2 / 2 + m4 / 2 + 15 * m6 / 32 + 7 * m8 / 16;
            a4 = m4 / 8 + 3 * m6 / 16 + 7 * m8 / 32;
            a6 = m6 / 32 + m8 / 16;
            a8 = m8 / 128;
            double B0, s0;
            double B1, B2, l = 0;
            for (int i=1;i<number;i++)
            {
                s[i] = Convert.ToDouble(strings[i].Split(',')[2]);
                if(strings[i].Split(',')[0]!="")
                {
                    origin[i] = Convert.ToDouble(strings[i].Split(',')[0]);
                    B0 = origin[i] * Math.PI / 180;
                    s0 = a0 * B0 - Math.Sin(B0) * Math.Cos(B0) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B0), 2) + 16 * a6 * Math.Pow(Math.Sin(B0), 4) / 3);
                    l = s0 + s[i];
                }
                else
                {
                    end[i] = Convert.ToDouble(strings[i].Split(',')[1]);
                    B0 = end[i] * Math.PI / 180;
                    s0 = a0 * B0 - Math.Sin(B0) * Math.Cos(B0) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B0), 2) + 16 * a6 * Math.Pow(Math.Sin(B0), 4) / 3);
                    l = s0 - s[i];
                }
                B1 = 0; B2 = l / a0;
                if (Math.Abs(l) <= Math.Pow(10, -3))
                {
                    B2 = 0;
                }
                while (Math.Abs(B1 - B2) >= 0.000000001)
                {
                    B1 = B2;
                    B2 = (l + Math.Sin(B2) * Math.Cos(B2) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B2), 2) + 16 * a6 * Math.Pow(Math.Sin(B2), 4) / 3)) / a0;
                }
                if(strings[i].Split(',')[0]!="")
                {
                    strings[i] = strings[i].Split(',')[0] + "," + (B2 * 180 / Math.PI).ToString() + "," + strings[i].Split(',')[2];
                }
                else
                {
                    strings[i] = (B2 * 180 / Math.PI).ToString() + "," + strings[i].Split(',')[1] + "," + strings[i].Split(',')[2];
                }
            }
            string filename1 = "result2.txt";
            StreamWriter writer = null;
            writer = new StreamWriter(filename1);
            for (int i = 0; i < number; i++)
            {
                writer.WriteLine(strings[i]);
            }
            writer.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string[] strings=new string[100];
            double[] origin = new double[100];
            double[] end = new double[100];
            double[] s = new double[100];
            int number = 0;
            string filename = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(DialogResult.OK==openFileDialog.ShowDialog())
            {
                filename = openFileDialog.FileName;
            }
            StreamReader reader = null;
            reader = new StreamReader(filename);
            for(string line=reader.ReadLine();line!=null;line=reader.ReadLine())
            {
                strings[number++] = line;
            }
            reader.Close();
            a = Convert.ToDouble(textBox1.Text); f = 1 / Convert.ToDouble(textBox2.Text);
            b = (1 - f) * a; e1 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2); e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(b, 2);
            m0 = a * (1 - e1);
            m2 = 3 * e1 * m0 / 2;
            m4 = e1 * 5 * m2 / 4;
            m6 = m4 * e1 * 7 / 6;
            m8 = e1 * 9 * m6 / 8;
            double a0, a2, a4, a6, a8;
            a0 = m0 + m2 / 2 + 3 * m4 / 8 + 5 * m6 / 16 + 35 * m8 / 128;
            a2 = m2 / 2 + m4 / 2 + 15 * m6 / 32 + 7 * m8 / 16;
            a4 = m4 / 8 + 3 * m6 / 16 + 7 * m8 / 32;
            a6 = m6 / 32 + m8 / 16;
            a8 = m8 / 128;
            double B1, B2, s1, s2;
            for (int i=1;i<number;i++)
            {
                origin[i] = Convert.ToDouble(strings[i].Split(',')[0]);
                end[i] = Convert.ToDouble(strings[i].Split(',')[1]);
                B1 = origin[i] * Math.PI / 180;B2 = end[i] * Math.PI / 180;
                s1 = a0 * B1 - Math.Sin(B1) * Math.Cos(B1) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B1), 2) + 16 * a6 * Math.Pow(Math.Sin(B1), 4) / 3);
                s2 = a0 * B2 - Math.Sin(B2) * Math.Cos(B2) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B2), 2) + 16 * a6 * Math.Pow(Math.Sin(B2), 4) / 3);
                s[i] = s2 - s1;
            }
            string filename1 = "result1.txt";
            StreamWriter writer = null;
            writer = new StreamWriter(filename1);
            writer.WriteLine("弧长(m)");
            for(int i=1;i<number;i++)
            {
                writer.WriteLine(s[i].ToString());
            }
            writer.Close();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            a = Convert.ToDouble(textBox1.Text); f = 1 / Convert.ToDouble(textBox2.Text);
            b = (1 - f) * a; e1 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2); e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(b, 2);
            m0 = a * (1 - e1);
            m2 = 3 * e1 * m0 / 2;
            m4 = e1 * 5 * m2 / 4;
            m6 = m4 * e1 * 7 / 6;
            m8 = e1 * 9 * m6 / 8;
            double a0, a2, a4, a6, a8;
            a0 = m0 + m2 / 2 + 3 * m4 / 8 + 5 * m6 / 16 + 35 * m8 / 128;
            a2 = m2 / 2 + m4 / 2 + 15 * m6 / 32 + 7 * m8 / 16;
            a4 = m4 / 8 + 3 * m6 / 16 + 7 * m8 / 32;
            a6 = m6 / 32 + m8 / 16;
            a8 = m8 / 128;
            double B0, s0, s;
            string string0;
            double B1, B2, l = 0;
            if(textBox3.Text!="")
            {
                string0 = textBox3.Text;
                B0 = Convert.ToDouble(string0.Split(',')[0]) * Math.PI / 180 + Convert.ToDouble(string0.Split(',')[1]) * Math.PI / 180 / 60 + Convert.ToDouble(string0.Split(',')[2]) * Math.PI / 180 / 3600;
                s0 = a0 * B0 - Math.Sin(B0) * Math.Cos(B0) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B0), 2) + 16 * a6 * Math.Pow(Math.Sin(B0), 4) / 3);
                s = Convert.ToDouble(textBox5.Text);
                l = s + s0;
            }
            else
            {
                string0 = textBox4.Text;
                B0 = Convert.ToDouble(string0.Split(',')[0]) * Math.PI / 180 + Convert.ToDouble(string0.Split(',')[1]) * Math.PI / 180 / 60 + Convert.ToDouble(string0.Split(',')[2]) * Math.PI / 180 / 3600;
                s0 = a0 * B0 - Math.Sin(B0) * Math.Cos(B0) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B0), 2) + 16 * a6 * Math.Pow(Math.Sin(B0), 4) / 3);
                s = Convert.ToDouble(textBox5.Text);
                l = s0 - s;
            }
            B1 = 0;B2 = l / a0;
            if (Math.Abs(l) <= Math.Pow(1, -3))
            {
                B2 = 0;
            }
            while (Math.Abs(B1-B2)>=0.000000001)
            {
                B1 = B2;
                B2 = (l + Math.Sin(B2) * Math.Cos(B2) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B2), 2) + 16 * a6 * Math.Pow(Math.Sin(B2), 4) / 3)) / a0;
            }
            double b1, b2, b3;
            b1 = Math.Floor(B2 * 180 / Math.PI);
            b2 = Math.Floor(B2 * 180 * 60 / Math.PI - b1 * 60);
            b3 = B2 * 180 * 3600 / Math.PI - b1 * 3600 - b2 * 60;
            if(Math.Abs(b3)<Math.Pow(10,-3))
            {
                b3 = 0.00;
            }
            if(Math.Abs(b3-Math.Floor(b3)-1)<Math.Pow(10,-3))
            {
                b3 = Math.Floor(b3) + 1;
            }
            if(b3==60)
            {
                b3 = 0;
                b2++;
            }
            if(b2==60)
            {
                b2 = 0;
                b1++;
            }
            string ss;
            ss = b1.ToString() + "," + b2.ToString() + "," + b3.ToString();
            if(textBox3.Text != "")
            {
                textBox4.Text = ss;
            }
            else
            {
                textBox3.Text = ss;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            a = Convert.ToDouble(textBox1.Text);f = 1 / Convert.ToDouble(textBox2.Text);
            b = (1 - f) * a;e1 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(a, 2); e2 = (Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(b, 2);
            m0 = a * (1 - e1);
            m2 = 3 * e1 * m0 / 2;
            m4 = e1 * 5 * m2 / 4;
            m6 = m4 * e1 * 7 / 6;
            m8 = e1 * 9 * m6 / 8;
            double a0, a2, a4, a6, a8;
            a0 = m0 + m2 / 2 + 3 * m4 / 8 + 5 * m6 / 16 + 35 * m8 / 128;
            a2 = m2 / 2 + m4 / 2 + 15 * m6 / 32 + 7 * m8 / 16;
            a4 = m4 / 8 + 3 * m6 / 16 + 7 * m8 / 32;
            a6 = m6 / 32 + m8 / 16;
            a8 = m8 / 128;
            double B1, B2, s1, s2, s;
            string string1, string2;
            string1 = textBox3.Text;string2 = textBox4.Text;
            B1 = Convert.ToDouble(string1.Split(',')[0]) * Math.PI / 180 + Convert.ToDouble(string1.Split(',')[1]) * Math.PI / 180 / 60 + Convert.ToDouble(string1.Split(',')[2]) * Math.PI / 180 / 3600;
            B2 = Convert.ToDouble(string2.Split(',')[0]) * Math.PI / 180 + Convert.ToDouble(string2.Split(',')[1]) * Math.PI / 180 / 60 + Convert.ToDouble(string2.Split(',')[2]) * Math.PI / 180 / 3600;
            s1 = a0 * B1 - Math.Sin(B1) * Math.Cos(B1) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B1), 2) + 16 * a6 * Math.Pow(Math.Sin(B1), 4) / 3);
            s2 = a0 * B2 - Math.Sin(B2) * Math.Cos(B2) * ((a2 - a4 + a6) + (2 * a4 - 16 * a6 / 3) * Math.Pow(Math.Sin(B2), 2) + 16 * a6 * Math.Pow(Math.Sin(B2), 4) / 3);
            s = s2 - s1;
            textBox5.Text = s.ToString();
        }
    }
}
