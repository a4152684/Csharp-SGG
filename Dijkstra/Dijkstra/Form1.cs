using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dijkstra
{
    public partial class Form1 : Form
    {
        public static Form1 MainForm = null;

        int count = 1;
        /// <summary>
        /// 顶点个数
        /// </summary>
        int n;//顶点个数
        static double inf = 9999;//表示无穷大
        static string lambta = "λ";//表示空点
        Matrix ma;//图的关系矩阵
        static string[] result;//结果储存


        public static void compute(Matrix matrix)
        {
            int n = matrix.length;//顶点个数
            bool[] flag = new bool[n];//某个顶点是否进入集合P,true为进入
            double[] l = new double[n];//每个点到v0的长度，初始值为正无穷inf
            double[] l_last = new double[n];//每个点在上一轮中到v0的长度
            int[] pj = new int[n];//记录vi最短路径的下一个点
            int[] pop = new int[n];//记录弹出的顺序
            for (int i = 0; i < n; i++)
            {
                flag[i] = false;
                l[i] = inf;
                l_last[i] = inf;
                pj[i] = 0;
            }
            

            //初始化，顶点v0进入P
            flag[0] = true;
            l[0] = 0;
            l_last[0] = 0;
            pop[0] = 0;
            int t;//次数

            //分号做分隔符
            result[0] = "1;(0," + lambta + ");";
            for (int i = 1; i < n; i++)
            {
                //顶点vi下面
                result[0] += "(+∞," + lambta + ");";
            }
            result[0] += "0;";//k

            //开始循环
            for (t=1; t < n; t++)//在第t行中
            {
                //将上一轮的l_last赋值
                for (int i = 0; i < n; i++)
                {
                    l_last[i] = l[i];
                }

                result[t] += (t+1).ToString() + ";";

                double min = inf;//最小长度
                int k = 0;//每次出来的点序号
                //对于每一个的顶点
                for (int i = 0; i < n; i++)
                {
                    if(!flag[i])//如果其不在P中
                    {
                        //则对于刚弹出来的那个顶点v[pop[t-1]],记作vj
                        //for (int j = 0; j < n; j++)
                        //{
                        int j = pop[t - 1];
                        if (i == j) continue; //i≠j
                                              //如果这个点vi和某个点vj相邻
                        if (matrix.m[i, j] != inf)//不为无穷大则相邻
                        {
                            if (l_last[i] > (l_last[j] + matrix.m[i, j]))//vi的长度更大则更新
                            {
                                l[i] = l_last[j] + matrix.m[i, j];
                                pj[i] = j;        //记录最小的顶点号
                            }
                        }
                        //}//此时l(i)是当前一轮最小的了
                        //求出不在P中点长度最小的那个
                        if (min > l[i])
                        {
                            min = l[i];
                            k = i;
                        }
                        //将结果存储
                        if(l[i]==inf)//如果为+∞，即没有更新
                        {
                            result[t]+= "(+∞," + lambta + ");";
                        }
                        else//如果更新了，不为+∞
                        {
                            result[t] += "(" + l[i].ToString() + "," + "v" + pj[i].ToString() + ");";
                        }
                        
                    }
                    else//如果其在P中，则对应的地方不填东西
                    {
                        result[t] += ";";
                    }
                }
                flag[k] = true;//将这个点放入P中
                pop[t] = k;
                //Form1.MainForm.richTextBox1.AppendText((k+1).ToString()+"\n");
                result[t] += k.ToString();//出来的点号
                result[t] += ";";

                //求出最短路径和距离
                string s = "";
                int[] p = new int[n];
                int num,kk;
                kk = pop[t];
                for (num = 0; num<n ; num++)
                {
                    p[num] = pj[kk];                 
                    if (p[num] == 0) break;
                    kk = pj[kk];
                }
                for (int i = num; i >=0; i--)
                {
                    s += "v" + p[i].ToString();
                }
                s += "v" + pop[t].ToString();
                s += ",d(v0,v" + pop[t].ToString() + ")=" + l[pop[t]].ToString()+";";
                result[t] += s;
                Form1.MainForm.richTextBox1.AppendText(s + "\n");
            }
            MessageBox.Show("计算完成！");            
            
        }

        public Form1()
        {
            InitializeComponent();
            MainForm = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //先更新关系矩阵
            int v1, v2;
            double w;
            v1 = Convert.ToInt32(textBox2.Text);
            v2 = Convert.ToInt32(textBox3.Text);
            w = Convert.ToDouble(textBox4.Text);
            ma.m[v1, v2] = w;
            ma.m[v2, v1] = w;
            //再在界面显示
            richTextBox1.AppendText(count.ToString() + ",(V" + textBox2.Text + ",V" + textBox3.Text + "),权重:" + textBox4.Text + "\n");
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            count++;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Undo();
            count--;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            compute(ma);
        }

        private void button2_Click(object sender, EventArgs e)
        {          

            //从文件中读取数据
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog()==DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("Utf-8"));
                string str = "";
                for (int i = 0; ; i++)
                {
                    str = sr.ReadLine();
                    if (str == null) break;
                    int v1, v2;
                    double w;
                    string[] PrimiryStr = new String[3];
                    PrimiryStr = str.Split(',');
                    v1 = Convert.ToInt32(PrimiryStr[0]);
                    v2 = Convert.ToInt32(PrimiryStr[1]);
                    w = Convert.ToDouble(PrimiryStr[2]);
                    ma.m[v1, v2] = w;
                    ma.m[v2, v1] = w;
                    richTextBox1.AppendText(count.ToString() + ",(V" + v1.ToString() + ",V"
                        + v2.ToString() + "),权重:" + w.ToString() + "\n");
                    count++;
                }
                sr.Close();
            }            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="")
            {
                MessageBox.Show("请输入顶点个数");
            }
            else
            {
                n = Convert.ToInt32(textBox1.Text);//读取顶点个数
                ma = new Matrix(n, n);
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        ma.m[i, j] = inf;
                    }
                }//矩阵初始化
                result = new string[n];//结果字符串初始化
                richTextBox1.Clear();//文本清空
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();
                MessageBox.Show("图的关系矩阵已初始化，顶点数为" + n.ToString());
            }        
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter= "txt文件(*.txt)|*.txt|所有文件(*.*)|*.*"; //设置“另存为文件类型”或“文件类型”框中出现的选择内容
            if (saveFileDialog.ShowDialog()==DialogResult.OK)
            {
                //先初始化标题
                string str = "";
                str += "t;";
                for (int i = 0; i < n; i++)
                {
                    str += "v" + i.ToString() + ";";
                }
                str += "k;";
                str += "最短路径及距离;";

                FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("Utf-8"));
                sw.WriteLine(str);//先写入标题
                for (int i = 0; i < result.Length; i++)
                {
                    sw.WriteLine(result[i]);
                }
                sw.Close();
                MessageBox.Show("保存成功");
            }
        }
    }
}
