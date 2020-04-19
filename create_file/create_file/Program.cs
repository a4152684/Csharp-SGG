using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace create_file
{
    class Program
    {
        public static void create(string filename)
        {
            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);//无则创建文件夹
            }
        }
        private static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {

                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
               
                        File.Copy(file, srcfileName);
                   
                }
            }//foreach 
        }

        public static void file_deal(string filename)
        {
            FileStream fs = new FileStream("./"+filename+".txt", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.GetEncoding("Utf-8"));
            string str = "";
            Console.WriteLine("下面开始复制"+filename+"类型");
            for (int i = 0; ; i++)
            {
                str = sr.ReadLine();
                if (str == null) break;
                if (Directory.Exists(str) && !Directory.Exists("./结果/" + filename+"/"+str))
                {
                    CopyDirectory(str, "./结果/"+filename);
                    Console.WriteLine(str + "复制完毕");
                }
            }
            sr.Close();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("创建结果文件夹");
            string filename = "./结果";
            create(filename);
            create(filename + "/BENIGN");
            create(filename + "/BENIGN_WITHOUT_CALLBACK");
            create(filename + "/MALIGNANT");
            Console.WriteLine("文件夹创建完毕");

            //string src = "./test";
            //string dist = filename;
            //CopyDirectory(src, dist);

            Console.WriteLine();
            file_deal("BENIGN");

            Console.WriteLine();
            file_deal("BENIGN_WITHOUT_CALLBACK");

            Console.WriteLine();
            file_deal("MALIGNANT");


            Console.WriteLine("所有文件已经复制在结果文件夹中");
            Console.ReadKey();
        }
    }
}
