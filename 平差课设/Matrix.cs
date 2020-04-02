using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Matrix
{
    public double[,] m;
    public int length, width;
    public Matrix(int l, int w)
    {
        length = l; width = w;
        m = new double[l, w];
    }
    public double this[int x, int y]
    {
        get
        {
            return m[x, y];
        }
        set
        {
            m[x, y] = value;
        }
    }
    public int Length
    {
        get { return length; }
    }
    public int Width
    {
        get { return width; }
    }
    public static Matrix operator +(Matrix a, Matrix b)
    {
        if (a.Width != b.Width || a.Length != b.Length)
        {
            return null;
        }
        Matrix c = new Matrix(a.Length, a.Width);
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < a.Width; j++)
            {
                c[i, j] = a[i, j] + b[i, j];
            }
        }
        return c;
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        if (a.Width != b.Width || a.Length != b.Length)
        {
            return null;
        }
        Matrix c = new Matrix(a.Length, a.Width);
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < a.Width; j++)
            {
                c[i, j] = a[i, j] - b[i, j];
            }
        }
        return c;
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        //Console.WriteLine(" a:{0}X{1}  b:{2}X{3}", a.Length, a.Width, b.Length, b.Width);

        if (a.Width != b.Length)
        {
            //  Console.WriteLine("error a:{0}X{1}  b:{2}X{3}", a.Length, a.Width, b.Length, b.Width);
            return null;
        }
        Matrix c = new Matrix(a.Length, b.Width);
        for (int i = 0; i < c.Length; i++)
        {
            for (int j = 0; j < c.Width; j++)
            {
                c[i, j] = 0;
                for (int k = 0; k < a.Width; k++)
                {
                    c[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        return c;
    }

    //求逆
    public static Matrix inv(Matrix m)
    {
        if (m.Length != m.Width)
        {
            return null;
        }
        //clone
        Matrix a = new Matrix(m.Length, m.Width);
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < a.Width; j++)
            {
                a[i, j] = m[i, j];
            }
        }
        Matrix c = new Matrix(a.Length, a.Width);
        for (int i = 0; i < a.Length; i++)
        {
            for (int j = 0; j < a.Width; j++)
            {
                if (i == j) { c[i, j] = 1; }
                else { c[i, j] = 0; }
            }
        }

        //i表示第几行，j表示第几列
        for (int j = 0; j < a.Length; j++)
        {
            bool flag = false;
            for (int i = j; i < a.Length; i++)
            {
                if (a[i, j] != 0)
                {
                    flag = true;
                    double temp;
                    //交换i,j,两行
                    if (i != j)
                    {
                        for (int k = 0; k < a.Length; k++)
                        {
                            temp = a[j, k];
                            a[j, k] = a[i, k];
                            a[i, k] = temp;

                            temp = c[j, k];
                            c[j, k] = c[i, k];
                            c[i, k] = temp;
                        }
                    }
                    //第j行标准化
                    double d = a[j, j];
                    for (int k = 0; k < a.Length; k++)
                    {
                        a[j, k] = a[j, k] / d;
                        c[j, k] = c[j, k] / d;
                    }
                    //消去其他行的第j列
                    d = a[j, j];
                    for (int k = 0; k < a.Length; k++)
                    {
                        if (k != j)
                        {
                            double t = a[k, j];
                            for (int n = 0; n < a.Length; n++)
                            {
                                a[k, n] -= (t / d) * a[j, n];
                                c[k, n] -= (t / d) * c[j, n];
                            }
                        }
                    }
                }
            }
            if (!flag) return null;
        }
        return c;
    }

    public void print()
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Console.Write(m[i, j].ToString("0.00") + " ");
            }
            Console.WriteLine("");
        }
        Console.WriteLine();
    }
    public static Matrix transposs(Matrix A)
    {
        int i = 0;
        int j = 0;
        Matrix B = new Matrix(A.width, A.length);
        //运算    
        for (i = 0; i < B.length; i++)
        {

            for (j = 0; j < B.width; j++)
            {
                B.m[i, j] = A.m[j, i];
            }
        }

        return B;
    }

}