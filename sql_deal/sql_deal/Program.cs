using System;
using System.Collections.Generic;
using System.IO;

namespace sql_deal
{
    class Program
    {
        struct Road //路结构体
        {
            public string R_name;
            public string R_num;
            public string R_type;
            public string R_length;
            //public string sql;
        };
        public static void road_deal()
        {
            //Road[] roads = new Road[N];
            List<string> sql = new List<string>();
            List<Road> roads = new List<Road>();
            List<double> length = new List<double>();
            List<string> Name = new List<string>();
            List<string> Num = new List<string>();
            List<string> Type = new List<string>();
            List<string> Key = new List<string>();//联合主键，由名字，编号，类型组成

            string filename = "E:\\曹臻个人\\专业课\\数据库实习\\程序\\road.txt";
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs);
            string str = "";
            int i = 0;
            sr.ReadLine();
            str = sr.ReadLine();

            do
            {
                string[] pri = str.Split(',');
                string name = pri[1];
                string num = pri[2];
                string type = pri[3];
                double leng = Convert.ToDouble(pri[4]);
                string key = name + "," + num + "," + type;

                if (Key.Contains(key)) //如果这条路已经读取过一部分
                {
                    int index = Key.IndexOf(key);
                    length[index] += leng; //则将长度相加
                }
                else //否则创建一个新的路
                {
                    Name.Add(name);
                    length.Add(leng);
                    Num.Add(num);
                    Type.Add(type);
                    Key.Add(key);
                }

                str = sr.ReadLine();
                i++;
            } while (str != null);

            sr.Close();

            for (i = 0; i < Name.Count; i++)//创建road对象
            {
                Road road = new Road();
                road.R_name = Name[i];
                road.R_num = Num[i];
                road.R_type = Type[i];
                road.R_length = length[i].ToString();
                roads.Add(road);
            }

            i = 0;
            foreach (Road road in roads)
            {
                List<string> p = new List<string>();
                List<string> Orig_fid = new List<string>();
                string s = "";

                string filename2 = "E:\\曹臻个人\\专业课\\数据库实习\\程序\\roadtopoints.txt";
                FileStream fs2 = new FileStream(filename2, FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader sr2 = new StreamReader(fs2);
                sr2.ReadLine();
                str = sr2.ReadLine();

                do
                {
                    string[] pri = str.Split(',');
                    string name = pri[1];
                    string num = pri[2];
                    string type = pri[3];
                    string point_x = pri[7];
                    string point_y = pri[8];
                    string orig_fid = pri[6];

                    if (name == road.R_name && num == road.R_num && type == road.R_type)//在这条路下
                    {
                        if(Orig_fid.Contains(orig_fid))//如果这条路的这段路已经添加进去
                        {
                            int index = Orig_fid.IndexOf(orig_fid);
                            p[index] += "," + point_x + " " + point_y; //则将点坐标添加
                        }
                        else
                        {
                            Orig_fid.Add(orig_fid);
                            p.Add(point_x + " " + point_y);//否则创建新路段
                        }            
                    }
                    str = sr2.ReadLine();
                } while (str != null);
                sr2.Close();

                for (i = 0; i < Orig_fid.Count; i++)
                {
                    s += "(" + p[i] + "),";
                }
                sql.Add(s.Remove(s.Length - 1, 1));
                i++;
            }

            //写入txt
            string filename3 = "E:\\曹臻个人\\专业课\\数据库实习\\程序\\roadsql.txt";
            FileStream fs3 = new FileStream(filename3, FileMode.Create, FileAccess.Write, FileShare.None);
            StreamWriter sw = new StreamWriter(fs3);

            for (i = 0; i < sql.Count; i++)
            {
                Road road = roads[i];
                string s = "insert into roads values(";
                s += (i+1).ToString() + ",";
                s += "'"+road.R_name+"'";
                s += "," + road.R_num + "";
                s += ",'" + road.R_type + "'";
                s += "," + road.R_length;
                s += ",geometry::STGeomFromText('MULTILINESTRING(";
                s += sql[i];
                s += ")',0))";
                sw.WriteLine(s);
            }

            sw.Close();
        }
        public static void building_deal()
        {
            List<string> Length = new List<string>();
            List<string> Area = new List<string>();
            List<string> Name = new List<string>();
            List<string> Fid = new List<string>();
            List<string> Point = new List<string>();
            List<string> Height = new List<string>();
            int i = 0;

            string filename = "E:\\曹臻个人\\专业课\\数据库实习\\程序\\build.txt";
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs);
            string str = "";

            sr.ReadLine();
            str = sr.ReadLine();

            do
            {
                string[] pri = str.Split(',');
                string name = pri[1];
                string orig_fid = pri[6];
                string height = pri[3];
                string length = pri[4];
                string area = pri[5];
                string pointx = pri[7];
                string pointy = pri[8];

                if(Fid.Contains(orig_fid))
                {
                    int index = Fid.IndexOf(orig_fid);
                    Point[index] += "," + pointx + " " + pointy;
                }
                else
                {
                    Name.Add(name);
                    Height.Add(height);
                    Length.Add(length);
                    Area.Add(area);
                    Fid.Add(orig_fid);
                    Point.Add(pointx + " " + pointy);
                }

                str = sr.ReadLine();
            } while (str != null);

            sr.Close();

            string filename3 = "E:\\曹臻个人\\专业课\\数据库实习\\程序\\buildingsql.txt";
            FileStream fs3 = new FileStream(filename3, FileMode.Create, FileAccess.Write, FileShare.None);
            StreamWriter sw = new StreamWriter(fs3);

            for (i = 0; i < Fid.Count; i++)
            {
                string s = "insert into buildings values(";
                s += Fid[i] + ",";
                s += "'" + Name[i] + "'";
                s += "," + Area[i] + "";
                s += "," + Length[i]+"";
                s += "," + Height[i] + "";
                s += ",geometry::STGeomFromText('POLYGON((";
                s += Point[i];
                s += "))',0))";
                sw.WriteLine(s);
            }

            sw.Close();
        }
        static void Main(string[] args)
        {
            //road_deal();
            building_deal();
        }
    }
}
