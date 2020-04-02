namespace dxw
{
    class Program
    {
        const int N1 = 3, N2 = 10, N3 = 11;//三个已知点标号
        static int[] NN = new int[] { N1, N2, N3 };
        static int PointDeal(int k)
        {
            if (k < N2 && k > N1) k -= 1;
            else if (k < N3 && k > N2) k -= 2;
            else if (k > N3) k -= 3;
            return k;
        }
        static double ComputeL(double a1, double a2)
        {
            if (a1 - a2 >= 0) return (a1 - a2);
            else return (a1 - a2 + 2 * Math.PI);
        }

        static double InvAzimuth(double ang)//反方位角计算
        {
            if (ang < Math.PI) ang += Math.PI;
            else if (ang >= Math.PI) ang -= Math.PI;
            return ang;
        }

        static double ComputeAzimuth(double x1, double y1, double x2, double y2)//计算方位角
        {
            double detY = y2 - y1, detX = x2 - x1;
            double t = Math.Atan(detY / detX);
            if (detY > 0 && detX > 0) return t;
            else if (detX > 0 && detY < 0) return (t + 2 * Math.PI);
            else return (t + Math.PI);
        }


        static void Main(string[] args)
        {
            //定义观测值、必要观测值、多余观测量个数等各种数
            const int n1 = 19; const int n2 = 26;//n1个边观测值，n2个角观测值
            const int n3 = 18;//点个数
            const int n = 45; const int t = 30; const int r = n - t;
            double m0;//后验单位权中误差
            double mB = 12;//测角先验误差12″
            double mD = 0.5;//测边先验误差1/2000m，即1m误差为0.5mm            
            const double x1 = 234.717, y1 = 307.224, x2 = 169.287, y2 = 113.116, x3 = 233.641, y3 = 112.029;//三个已知点坐标(单位：m)，其中N3=N2+1,即边N2N3为已知边
            double[] length = new double[n1];//存储n1个边观测值
            double[,] Length = new double[n3, n3];//存储边观测值方便计算近似值
            int[,] number1 = new int[n1, 2];//储存n1个边的编号(起码有一个点为未知点）
            double[] ang = new double[n2];//存储n2个角观测值
            double[,,] Ang = new double[n3, n3, n3];//存储角观测值方便计算近似值
            int[,] number2 = new int[n2, 3];//存储n2个角的名称编号，如角123
            double[] Xm = new double[t / 2] { 302.7957, 300.4014, 263.3049, 173.5139, 123.8708, 102.4403, 102.5686, 118.8243, 140.7522, 253.4806, 292.3873, 165.8046, 199.3172, 217.9976, 252.7601 };
            double[] Ym = new double[t / 2] { 208.5665, 249.7059, 264.7234, 294.7579, 266.5995, 233.4518, 205.4521, 159.2027, 110.3886, 128.8458, 168.9883, 233.5258, 199.0122, 154.6054, 211.4802 };//存储t个参数的近似值，这里Xm=[X0 X1 X2...]T，Ym=[Y0 Y1 Y2...]T
            double[,] azimuth = new double[t / 2 + 3, t / 2 + 3];//方位角近似值
            int minSideIndex;//精度最弱点点号
            double mMinSide;//精度最弱边中误差
            const double p = 180 * 60 * 60 / Math.PI;
            //初始化各类矩阵
            Matrix B = new Matrix(n, t);//前面t/2列为坐标参数x，后面为坐标参数y
            Matrix V = new Matrix(n, 1);
            Matrix x = new Matrix(t, 1);
            Matrix L = new Matrix(n, 1);
            Matrix P = new Matrix(n, n);
            Matrix Q = new Matrix(n, n);
            Matrix NBB = new Matrix(t, t);
            Matrix W = new Matrix(t, 1);
            Matrix Qxx = new Matrix(t, t);
            Matrix QLL = new Matrix(n, n);


            //读取原始数据
            {
                //先读边长

                FileStream fs = new FileStream("E:\\曹臻个人\\2017301610095\\导线网.csv", FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding(936));
                string str = "";
                sr.ReadLine();
                for (int i = 0; i < n1; i++)
                {
                    str = sr.ReadLine();
                    string[] PrimiryStr = new String[100];
                    PrimiryStr = str.Split(',');
                    if (PrimiryStr[1] == "已知值")
                    {
                        i--;
                        continue;
                    };//读到已知边则跳出本层循环
                    length[i] = Convert.ToDouble(PrimiryStr[1]);//读取边长(单位m)
                    string[] num = PrimiryStr[0].Split(new char[2] { '-', '-' }, StringSplitOptions.RemoveEmptyEntries);
                    number1[i, 0] = int.Parse(num[0].Trim());
                    number1[i, 1] = int.Parse(num[1].Trim());//读取边编号
                    Length[number1[i, 0], number1[i, 1]] = length[i];
                    Length[number1[i, 1], number1[i, 0]] = length[i];
                }
                //读边完成

                //再读取角度及其对应的角编号
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                sr.DiscardBufferedData();//返回第一行           
                sr.ReadLine();
                for (int i = 0; i < n2; i++)
                {
                    str = sr.ReadLine();
                    string[] PrimiryStr = new String[100];
                    PrimiryStr = str.Split(',');
                    ang[i] = Convert.ToDouble(PrimiryStr[6]);//读取角度(单位rad)
                    string[] num = PrimiryStr[5].Split('-');
                    number2[i, 0] = int.Parse(num[0].Replace(num[0][0], ' ').Trim());//去除角编号前的'∠'，这样能把数字给导出到number中
                    number2[i, 1] = int.Parse(num[1]);
                    number2[i, 2] = int.Parse(num[2]);//读取角编号
                    Ang[number2[i, 0], number2[i, 1], number2[i, 2]] = ang[i];
                    Ang[number2[i, 2], number2[i, 1], number2[i, 0]] = 2 * Math.PI - ang[i];
                }
                //读取完成
                sr.Close();
            }

            //求坐标参数近似值和方位角近似值
            {
                azimuth[N2, N3] = ComputeAzimuth(x2, y2, x3, y3);
                if (InvAzimuth(azimuth[N3 - 1, N3]) + Ang[N3 - 1, N3, N3 + 1] < 2 * Math.PI)
                    azimuth[N3, N3 + 1] = Ang[N3 - 1, N3, N3 + 1] + InvAzimuth(azimuth[N3 - 1, N3]);
                else azimuth[N3, N3 + 1] = Ang[N3 - 1, N3, N3 + 1] + InvAzimuth(azimuth[N3 - 1, N3]) - 2 * Math.PI;
                Xm[PointDeal(N3 + 1)] = x3 + Length[N3, N3 + 1] * Math.Cos(azimuth[N3, N3 + 1]);
                Ym[PointDeal(N3 + 1)] = y3 + Length[N3, N3 + 1] * Math.Sin(azimuth[N3, N3 + 1]);
                for (int i = N3 + 1; i != N1 - 1; i++)//从N2,N3边开始求，先求到点N1的导线
                {
                    int j = i + 1, k = i - 1;
                    if (i > 13) i -= 14;
                    if (j > 13) j -= 14;
                    if (InvAzimuth(azimuth[k, i]) + Ang[k, i, j] < 2 * Math.PI)
                        azimuth[i, j] = Ang[k, i, j] + InvAzimuth(azimuth[k, i]);
                    else azimuth[i, j] = Ang[k, i, j] + InvAzimuth(azimuth[k, i]) - 2 * Math.PI;
                    Xm[PointDeal(j)] = Xm[PointDeal(i)] + Length[i, j] * Math.Cos(azimuth[i, j]);
                    Ym[PointDeal(j)] = Ym[PointDeal(i)] + Length[i, j] * Math.Sin(azimuth[i, j]);
                }
                if (InvAzimuth(azimuth[N1 - 2, N1 - 1]) + Ang[N1 - 2, N1 - 1, N1] < 2 * Math.PI)
                    azimuth[N1 - 1, N1] = Ang[N1 - 2, N1 - 1, N1] + InvAzimuth(azimuth[N1 - 2, N1 - 1]);
                else azimuth[N1 - 1, N1] = Ang[N1 - 2, N1 - 1, N1] + InvAzimuth(azimuth[N1 - 2, N1 - 1]) - 2 * Math.PI;
                for (int i = N1; i != N2 - 1; i++)//再从点N1开始求，求到点N2的导线
                {
                    int j = i + 1, k = i - 1;
                    if (i > 13) i -= 14;
                    if (j > 13) j -= 14;
                    if (InvAzimuth(azimuth[k, i]) + Ang[k, i, j] < 2 * Math.PI)
                        azimuth[i, j] = Ang[k, i, j] + InvAzimuth(azimuth[k, i]);
                    else azimuth[i, j] = Ang[k, i, j] + InvAzimuth(azimuth[k, i]) - 2 * Math.PI;
                    Xm[PointDeal(j)] = Xm[PointDeal(i)] + Length[i, j] * Math.Cos(azimuth[i, j]);
                    Ym[PointDeal(j)] = Ym[PointDeal(i)] + Length[i, j] * Math.Sin(azimuth[i, j]);
                }
                if (InvAzimuth(azimuth[N2 - 2, N2 - 1]) + Ang[N2 - 2, N2 - 1, N2] < 2 * Math.PI)
                    azimuth[N2 - 1, N2] = Ang[N2 - 2, N2 - 1, N2] + InvAzimuth(azimuth[N2 - 2, N2 - 1]);
                else azimuth[N2 - 1, N2] = Ang[N2 - 2, N2 - 1, N2] + InvAzimuth(azimuth[N2 - 2, N2 - 1]) - 2 * Math.PI;
                //再求中间边的方位角与坐标近似值，此处为手算
                azimuth[5, 14] = 5.6154;
                azimuth[14, 15] = 5.4831;
                azimuth[15, 16] = 5.1106;
                azimuth[16, 12] = 5.6552;
                azimuth[15, 17] = 0.2292;
                azimuth[17, 1] = 0.6762;
            }

            //读入反方位角
            for (int i = 0; i < t / 2 + 3; i++)
            {
                for (int j = 0; j < t / 2 + 3; j++)
                {
                    while (azimuth[i, j] > 2 * Math.PI) azimuth[i, j] -= 2 * Math.PI;
                    if (azimuth[i, j] != 0) azimuth[j, i] = InvAzimuth(azimuth[i, j]);
                }
            }

            for (int i = 0; i < t / 2 + 3; i++)
            {
                for (int j = 0; j < t / 2 + 3; j++)
                {
                    Console.WriteLine("{0} {1} {2}", i, j, azimuth[i, j]);
                }
            }

            //读入P矩阵
            for (int i = 0; i < n1; i++)
            {
                P.m[i, i] = (mB * mB) / (mD * length[i] * mD * length[i]);
            }//边观测值在前面的行，单位：秒²/mm²
            for (int i = n1; i < n; i++)
            {
                P.m[i, i] = 1;
            }//角观测值在后面的行
             //P矩阵读入完成

            //迭代开始
            for (int count = 0; count < 100; count++)
            {


                //读入B与L矩阵
                //先读前面n1条边观测值形成的数值
                for (int i = 0; i < n1; i++)
                {
                    int j = number1[i, 0]; int k = number1[i, 1];
                    double S = new double();//近似边长
                                            //处理点号问题，并在不同的情况下读入B,L矩阵
                    if (Array.IndexOf(NN, j) == -1 && Array.IndexOf(NN, k) == -1)//j，k两点均为未知点时
                    {
                        j = PointDeal(j);
                        k = PointDeal(k);
                        S = Math.Sqrt((Xm[j] - Xm[k]) * (Xm[j] - Xm[k]) + (Ym[j] - Ym[k]) * (Ym[j] - Ym[k]));//近似距离
                        B.m[i, j] = (Xm[j] - Xm[k]) / S; B.m[i, k] = -(Xm[j] - Xm[k]) / S;
                        B.m[i, j + t / 2] = (Ym[j] - Ym[k]) / S; B.m[i, k + t / 2] = -(Ym[j] - Ym[k]) / S;//单位1
                        L.m[i, 0] = 1000 * (length[i] - S);//单位mm
                    }
                    else if (Array.IndexOf(NN, j) != -1 && Array.IndexOf(NN, k) == -1)//j已知而k未知时
                    {
                        k = PointDeal(k);
                        if (j == N1)
                        {
                            S = Math.Sqrt((x1 - Xm[k]) * (x1 - Xm[k]) + (y1 - Ym[k]) * (y1 - Ym[k]));
                            B.m[i, k] = -(x1 - Xm[k]) / S;
                        }
                        else if (j == N2)
                        {
                            S = Math.Sqrt((x2 - Xm[k]) * (x2 - Xm[k]) + (y2 - Ym[k]) * (y2 - Ym[k]));
                            B.m[i, k] = -(x2 - Xm[k]) / S;
                        }
                        else if (j == N3)
                        {
                            S = Math.Sqrt((x3 - Xm[k]) * (x3 - Xm[k]) + (y3 - Ym[k]) * (y3 - Ym[k]));
                            B.m[i, k] = -(x3 - Xm[k]) / S;
                        }
                        L.m[i, 0] = 1000 * (length[i] - S);//单位mm
                    }
                    else if (Array.IndexOf(NN, j) == -1 && Array.IndexOf(NN, k) != -1)//j未知而k已知时
                    {

                        j = PointDeal(j);
                        if (k == N1)
                        {
                            S = Math.Sqrt((x1 - Xm[j]) * (x1 - Xm[j]) + (y1 - Ym[j]) * (y1 - Ym[j]));
                            B.m[i, j] = -(x1 - Xm[j]) / S;
                        }
                        else if (k == N2)
                        {
                            S = Math.Sqrt((x2 - Xm[j]) * (x2 - Xm[j]) + (y2 - Ym[j]) * (y2 - Ym[j]));
                            B.m[i, j] = -(x2 - Xm[j]) / S;
                        }
                        else if (k == N3)
                        {
                            S = Math.Sqrt((x3 - Xm[j]) * (x3 - Xm[j]) + (y3 - Ym[j]) * (y3 - Ym[j]));
                            B.m[i, j] = -(x3 - Xm[j]) / S;
                        }
                        L.m[i, 0] = 1000 * (length[i] - S);//单位mm
                    }
                }
                //前n1行读取结束
                //再读取后面n2个角观测值形成的数值
                for (int i = n1; i < n; i++)
                {
                    int h, j, k;
                    h = number2[i - n1, 0]; j = number2[i - n1, 1]; k = number2[i - n1, 2];
                    double S1, S2 = new double();//S1为hj距离的平方，S2为jk距离的平方
                    if (Array.IndexOf(NN, j) == -1 && Array.IndexOf(NN, k) == -1 && Array.IndexOf(NN, h) == -1)//当h,j,k均为未知点时
                    {
                        int h1 = h, k1 = k, j1 = j;
                        j = PointDeal(j);
                        k = PointDeal(k);
                        h = PointDeal(h);
                        S1 = (Xm[j] - Xm[h]) * (Xm[j] - Xm[h]) + (Ym[j] - Ym[h]) * (Ym[j] - Ym[h]);
                        S2 = (Xm[j] - Xm[k]) * (Xm[j] - Xm[k]) + (Ym[j] - Ym[k]) * (Ym[j] - Ym[k]);
                        //B将角度系数的单位化成″/mm；
                        B.m[i, j] = p * ((Ym[k] - Ym[j]) / S2 - (Ym[h] - Ym[j]) / S1) / 1000;//Xj前面系数
                        B.m[i, j + t / 2] = -p * ((Xm[k] - Xm[j]) / S2 - (Xm[h] - Xm[j]) / S1) / 1000;//Yj前面系数
                        B.m[i, k] = -p * (Ym[k] - Ym[j]) / S2 / 1000;//Xk前面系数
                        B.m[i, k + t / 2] = p * (Xm[k] - Xm[j]) / S2 / 1000;//Yk前面系数
                        B.m[i, h] = p * (Ym[h] - Ym[j]) / S1 / 1000;//Xh前面系数
                        B.m[i, h + t / 2] = -p * (Xm[h] - Xm[j]) / S1 / 1000;//Xh前面系数
                        L.m[i, 0] = p * (ang[i - n1] - ComputeL(azimuth[j1, k1], azimuth[j1, h1]));//L,单位：″
                    }
                    else if (Array.IndexOf(NN, j) != -1 && Array.IndexOf(NN, k) == -1 && Array.IndexOf(NN, h) == -1)//当j已知而h,k未知时
                    {
                        int h1 = h, k1 = k, j1 = j;
                        k = PointDeal(k);
                        h = PointDeal(h);
                        L.m[i, 0] = p * (ang[i - n1] - ComputeL(azimuth[j1, k1], azimuth[j1, h1]));//L,单位：″
                        if (j == N1)
                        {
                            S1 = (x1 - Xm[h]) * (x1 - Xm[h]) + (y1 - Ym[h]) * (y1 - Ym[h]);
                            S2 = (x1 - Xm[k]) * (x1 - Xm[k]) + (y1 - Ym[k]) * (y1 - Ym[k]);
                            B.m[i, k] = -p * (Ym[k] - y1) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - x1) / S2 / 1000;//Yk前面系数
                            B.m[i, h] = p * (Ym[h] - y1) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - x1) / S1 / 1000;//Xh前面系数
                        }
                        else if (j == N2)
                        {
                            S1 = (x2 - Xm[h]) * (x2 - Xm[h]) + (y2 - Ym[h]) * (y2 - Ym[h]);
                            S2 = (x2 - Xm[k]) * (x2 - Xm[k]) + (y2 - Ym[k]) * (y2 - Ym[k]);
                            B.m[i, k] = -p * (Ym[k] - y2) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - x2) / S2 / 1000;//Yk前面系数
                            B.m[i, h] = p * (Ym[h] - y2) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - x2) / S1 / 1000;//Xh前面系数
                        }
                        else if (j == N3)
                        {
                            S1 = (x3 - Xm[h]) * (x3 - Xm[h]) + (y3 - Ym[h]) * (y3 - Ym[h]);
                            S2 = (x3 - Xm[k]) * (x3 - Xm[k]) + (y3 - Ym[k]) * (y3 - Ym[k]);
                            B.m[i, k] = -p * (Ym[k] - y3) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - x3) / S2 / 1000;//Yk前面系数
                            B.m[i, h] = p * (Ym[h] - y3) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - x3) / S1 / 1000;//Xh前面系数
                        }
                    }
                    else if (Array.IndexOf(NN, j) == -1 && Array.IndexOf(NN, k) != -1 && Array.IndexOf(NN, h) == -1)//当k已知而h,j未知时
                    {
                        int h1 = h, k1 = k, j1 = j;
                        h = PointDeal(h);
                        j = PointDeal(j);
                        L.m[i, 0] = p * (ang[i - n1] - ComputeL(azimuth[j1, k1], azimuth[j1, h1]));//L,单位：″
                        if (k == N1)
                        {
                            S1 = (Xm[j] - Xm[h]) * (Xm[j] - Xm[h]) + (Ym[j] - Ym[h]) * (Ym[j] - Ym[h]);
                            S2 = (Xm[j] - x1) * (Xm[j] - x1) + (Ym[j] - y1) * (Ym[j] - y1);
                            B.m[i, j] = p * ((y1 - Ym[j]) / S2 - (Ym[h] - Ym[j]) / S1) / 1000;//Xj前面系数
                            B.m[i, j + t / 2] = -p * ((x1 - Xm[j]) / S2 - (Xm[h] - Xm[j]) / S1) / 1000;//Yj前面系数
                            B.m[i, h] = p * (Ym[h] - Ym[j]) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - Xm[j]) / S1 / 1000;//Xh前面系数
                        }
                        else if (k == N2)
                        {
                            S1 = (Xm[j] - Xm[h]) * (Xm[j] - Xm[h]) + (Ym[j] - Ym[h]) * (Ym[j] - Ym[h]);
                            S2 = (Xm[j] - x2) * (Xm[j] - x2) + (Ym[j] - y2) * (Ym[j] - y2);
                            B.m[i, j] = p * ((y2 - Ym[j]) / S2 - (Ym[h] - Ym[j]) / S1) / 1000;//Xj前面系数
                            B.m[i, j + t / 2] = -p * ((x2 - Xm[j]) / S2 - (Xm[h] - Xm[j]) / S1) / 1000;//Yj前面系数
                            B.m[i, h] = p * (Ym[h] - Ym[j]) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - Xm[j]) / S1 / 1000;//Xh前面系数
                        }
                        else if (k == N3)
                        {
                            S1 = (Xm[j] - Xm[h]) * (Xm[j] - Xm[h]) + (Ym[j] - Ym[h]) * (Ym[j] - Ym[h]);
                            S2 = (Xm[j] - x3) * (Xm[j] - x3) + (Ym[j] - y3) * (Ym[j] - y3);
                            B.m[i, j] = p * ((y3 - Ym[j]) / S2 - (Ym[h] - Ym[j]) / S1) / 1000;//Xj前面系数
                            B.m[i, j + t / 2] = -p * ((x3 - Xm[j]) / S2 - (Xm[h] - Xm[j]) / S1) / 1000;//Yj前面系数
                            B.m[i, h] = p * (Ym[h] - Ym[j]) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - Xm[j]) / S1 / 1000;//Xh前面系数
                        }
                    }
                    else if (Array.IndexOf(NN, j) == -1 && Array.IndexOf(NN, k) == -1 && Array.IndexOf(NN, h) != -1)//当h已知而k,j未知时
                    {
                        int h1 = h, k1 = k, j1 = j;
                        k = PointDeal(k);
                        j = PointDeal(j);
                        L.m[i, 0] = p * (ang[i - n1] - ComputeL(azimuth[j1, k1], azimuth[j1, h1]));//L,单位：″
                        if (h == N1)
                        {
                            S1 = (Xm[j] - x1) * (Xm[j] - x1) + (Ym[j] - y1) * (Ym[j] - y1);
                            S2 = (Xm[j] - Xm[k]) * (Xm[j] - Xm[k]) + (Ym[j] - Ym[k]) * (Ym[j] - Ym[k]);
                            B.m[i, j] = p * ((Ym[k] - Ym[j]) / S2 - (y1 - Ym[j]) / S1) / 1000;//Xj前面系数
                            B.m[i, j + t / 2] = -p * ((Xm[k] - Xm[j]) / S2 - (x1 - Xm[j]) / S1) / 1000;//Yj前面系数
                            B.m[i, k] = -p * (Ym[k] - Ym[j]) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - Xm[j]) / S2 / 1000;//Yk前面系数
                        }
                        else if (h == N2)
                        {
                            S1 = (Xm[j] - x2) * (Xm[j] - x2) + (Ym[j] - y2) * (Ym[j] - y2);
                            S2 = (Xm[j] - Xm[k]) * (Xm[j] - Xm[k]) + (Ym[j] - Ym[k]) * (Ym[j] - Ym[k]);
                            B.m[i, j] = p * ((Ym[k] - Ym[j]) / S2 - (y2 - Ym[j]) / S1) / 1000;//Xj前面系数
                            B.m[i, j + t / 2] = -p * ((Xm[k] - Xm[j]) / S2 - (x2 - Xm[j]) / S1) / 1000;//Yj前面系数
                            B.m[i, k] = -p * (Ym[k] - Ym[j]) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - Xm[j]) / S2 / 1000;//Yk前面系数
                        }
                        else if (h == N3)
                        {
                            S1 = (Xm[j] - x2) * (Xm[j] - x2) + (Ym[j] - y2) * (Ym[j] - y2);
                            S2 = (Xm[j] - Xm[k]) * (Xm[j] - Xm[k]) + (Ym[j] - Ym[k]) * (Ym[j] - Ym[k]);
                            B.m[i, j] = p * ((Ym[k] - Ym[j]) / S2 - (y2 - Ym[j]) / S1) / 1000;//Xj前面系数
                            B.m[i, j + t / 2] = -p * ((Xm[k] - Xm[j]) / S2 - (x2 - Xm[j]) / S1) / 1000;//Yj前面系数
                            B.m[i, k] = -p * (Ym[k] - Ym[j]) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - Xm[j]) / S2 / 1000;//Yk前面系数
                        }
                    }
                    else if (Array.IndexOf(NN, j) != -1 && Array.IndexOf(NN, k) != -1 && Array.IndexOf(NN, h) == -1)//当j,k已知而h未知时
                    {
                        int h1 = h, k1 = k, j1 = j;
                        h = PointDeal(h);
                        L.m[i, 0] = p * (ang[i - n1] - ComputeL(azimuth[j1, k1], azimuth[j1, h1]));//L,单位：″
                        if (j == N1)
                        {
                            S1 = (x1 - Xm[h]) * (x1 - Xm[h]) + (y1 - Ym[h]) * (y1 - Ym[h]);
                            B.m[i, h] = p * (Ym[h] - y1) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - x1) / S1 / 1000;//Xh前面系数
                        }
                        else if (j == N2)
                        {
                            S1 = (x2 - Xm[h]) * (x2 - Xm[h]) + (y2 - Ym[h]) * (y2 - Ym[h]);
                            B.m[i, h] = p * (Ym[h] - y2) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - x2) / S1 / 1000;//Xh前面系数
                        }
                        else if (j == N3)
                        {
                            S1 = (x3 - Xm[h]) * (x3 - Xm[h]) + (y3 - Ym[h]) * (y1 - Ym[h]);
                            B.m[i, h] = p * (Ym[h] - y3) / S1 / 1000;//Xh前面系数
                            B.m[i, h + t / 2] = -p * (Xm[h] - x3) / S1 / 1000;//Xh前面系数
                        }
                    }
                    else if (Array.IndexOf(NN, j) != -1 && Array.IndexOf(NN, k) == -1 && Array.IndexOf(NN, h) != -1)//当j,h已知而k未知时
                    {
                        int h1 = h, k1 = k, j1 = j;
                        k = PointDeal(k);
                        L.m[i, 0] = p * (ang[i - n1] - ComputeL(azimuth[j1, k1], azimuth[j1, h1]));//L,单位：″
                        if (j == N1)
                        {
                            S2 = (x1 - Xm[k]) * (x1 - Xm[k]) + (y1 - Ym[k]) * (y1 - Ym[k]);
                            B.m[i, k] = -p * (Ym[k] - y1) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - x1) / S2 / 1000;//Yk前面系数
                        }
                        else if (j == N2)
                        {
                            S2 = (x2 - Xm[k]) * (x2 - Xm[k]) + (y2 - Ym[k]) * (y2 - Ym[k]);
                            B.m[i, k] = -p * (Ym[k] - y2) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - x2) / S2 / 1000;//Yk前面系数
                        }
                        else if (j == N3)
                        {
                            S2 = (x3 - Xm[k]) * (x3 - Xm[k]) + (y3 - Ym[k]) * (y3 - Ym[k]);
                            B.m[i, k] = -p * (Ym[k] - y3) / S2 / 1000;//Xk前面系数
                            B.m[i, k + t / 2] = p * (Xm[k] - x3) / S2 / 1000;//Yk前面系数
                        }
                    }
                    else if (Array.IndexOf(NN, j) == -1 && Array.IndexOf(NN, k) != -1 && Array.IndexOf(NN, h) != -1)//当h,k已知而j未知时
                    {
                        int h1 = h, k1 = k, j1 = j;
                        h = PointDeal(h);
                        k = PointDeal(k);
                    }
                }
                //B,L矩阵读取完成

                //进行计算              
                NBB = Matrix.transposs(B) * P * B;
                W = Matrix.transposs(B) * P * L;
                x = Matrix.inv(NBB) * W;
                for (int i = 0; i < t / 2; i++)//近似值获取
                {
                    Xm[i] = Xm[i] + x.m[i, 0] / 1000;
                    Ym[i] = Ym[i] + x.m[i + t / 2, 0] / 1000;
                }
                for (int i = 0; i < t / 2 + 3; i++)//方位角近似值获取
                {
                    for (int j = 0; j < t / 2 + 3; j++)
                    {
                        if (azimuth[i, j] != 0)
                        {
                            if (Array.IndexOf(NN, j) == -1 && Array.IndexOf(NN, i) == -1)//i,j未知
                            {
                                int i1 = i, j1 = j;
                                int i2 = PointDeal(i), j2 = PointDeal(j);
                                azimuth[i1, j1] = ComputeAzimuth(Xm[i2], Ym[i2], Xm[j2], Ym[j2]);
                            }
                            else if (Array.IndexOf(NN, j) == -1 && Array.IndexOf(NN, i) != -1)//j未知，i已知
                            {
                                int i1 = i, j1 = j;
                                int j2 = PointDeal(j);
                                if (i == N1) azimuth[i1, j1] = ComputeAzimuth(x1, y1, Xm[j2], Ym[j2]);
                                else if (i == N2) azimuth[i1, j1] = ComputeAzimuth(x2, y2, Xm[j2], Ym[j2]);
                                else if (i == N3) azimuth[i1, j1] = ComputeAzimuth(x3, y3, Xm[j2], Ym[j2]);
                            }
                            else if (Array.IndexOf(NN, j) != -1 && Array.IndexOf(NN, i) == -1)//i未知，j已知
                            {
                                int i1 = i, j1 = j;
                                int i2 = PointDeal(i);
                                if (j == N1) azimuth[i1, j1] = ComputeAzimuth(Xm[i2], Ym[i2], x1, y1);
                                else if (j == N2) azimuth[i1, j1] = ComputeAzimuth(Xm[i2], Ym[i2], x2, y2);
                                else if (j == N3) azimuth[i1, j1] = ComputeAzimuth(Xm[i2], Ym[i2], x3, y3);
                            }
                        }
                    }
                }
            }
            //迭代完成

            Q = Matrix.inv(P);
            V = B * x - L;
            m0 = Math.Sqrt(((Matrix.transposs(V) * P * V).m[0, 0]) / r);
            Qxx = Matrix.inv(NBB);
            QLL = B * Matrix.inv(NBB) * Matrix.transposs(B);
            //找出边长精度最低点的点号，也即边长最大点点号
            double max = QLL.m[0, 0];
            minSideIndex = 0;
            for (int i = 0; i < n1; i++)
            {
                if (QLL.m[i, i] > max)
                {
                    max = QLL.m[i, i]; minSideIndex = i;
                }
            }
            mMinSide = m0 * Math.Sqrt(QLL.m[minSideIndex, minSideIndex]);//精度最弱边中误差
            Console.WriteLine("单位权中误差为:{0}″", m0);
            Console.WriteLine("各个待定点坐标平差值和中误差为：");
            for (int i = 0; i < t / 2; i++)
            {
                double mx, my, mp;
                mx = m0 * Math.Sqrt(Qxx.m[i, i]); my = m0 * Math.Sqrt(Qxx.m[i + t / 2, i + t / 2]);
                mp = Math.Sqrt(mx * mx + my * my);
                Console.WriteLine("{0}m {1}m {2}mm ", Xm[i] + x.m[i, 0] / 1000, Ym[i] + x.m[i + t / 2, 0] / 1000, mp);
            }
            Console.WriteLine("各观测值的平差值为：");
            Console.WriteLine("{0}条边观测值平差值：", n1);
            for (int i = 0; i < n1; i++)
            {
                Console.WriteLine("{0}m", V.m[i, 0] / 1000 + length[i]);
            }
            Console.WriteLine("{0}个角观测值平差值：", n2);
            for (int i = n1; i < n; i++)
            {
                double an = V.m[i, 0] + p * ang[i - n1];//单位秒
                Console.WriteLine("{0}°{1}′{2}″即 {3} rad ", (int)(an / 3600), (int)(an / 60 - 60 * (int)(an / 3600)), an - 3600 * (int)(an / 3600) - 60 * (int)(an / 60 - 60 * (int)(an / 3600)), an / p);
            }
            Console.WriteLine("最弱边边长相对中误差为：{0}", mMinSide / (1000 * length[minSideIndex]));
            //Console.WriteLine( "{0}", (Matrix.transposs(B) * P * V).m[0, 0]);
            Console.WriteLine("观测值改正数：");
            Console.WriteLine("边长观测值：");
            for (int i = 0; i < n1; i++)
            {
                Console.WriteLine("{0}mm", V.m[i, 0]);
            }
            Console.WriteLine("角观测值：", n2);
            for (int i = n1; i < n; i++)
            {
                Console.WriteLine("{0}″", V.m[i, 0]);
            }
            Console.WriteLine("坐标改正数：");
            Console.WriteLine("x坐标");
            for (int i = 0; i < t / 2; i++)
            {
                Console.WriteLine("{0}mm", x.m[i, 0]);
            }
            Console.WriteLine("y坐标");
            for (int i = t / 2; i < t; i++)
            {
                Console.WriteLine("{0}mm", x.m[i, 0]);
            }
            Console.WriteLine("{0}", (Matrix.transposs(B) * P * V).m[0, 0]);
            Console.ReadKey();
        }
    }