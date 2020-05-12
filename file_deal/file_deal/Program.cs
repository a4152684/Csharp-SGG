using System;
using System.IO;
using System.Linq;

namespace file_deal
{
    class Program
    {
        public static void create(string filename)
        {
            if (!Directory.Exists(filename))
            {
                Directory.CreateDirectory(filename);//无则创建文件夹
                Console.WriteLine("结果文件夹创建成功\n");
            }
        }
       

        public static void file_de(FileSystemInfo file2)
        {
            String filename = file2.FullName;
            FileInfo file = new FileInfo(filename);

            string type = file.FullName.Split("\\")[file.FullName.Split("\\").Length - 1].Split(".")[1];
            if (type != "bmp") return; //只读取bmp文件

            while(File.Exists("../结果\\" + filename.Split("\\")[filename.Split("\\").Length - 1]))//如果已经存在，则改名字
            {
                String str = filename.Split("\\")[filename.Split("\\").Length - 1].Split(".")[0] + "x." + type;
                filename = "";
                for (int i = 0; i < file.FullName.Split("\\").Length - 1; i++)
                {
                    filename += file.FullName.Split("\\")[i] + "\\";
                }
                filename += str;
            }

            if (file.Length / 1024 / 1024 <= 10)
            {
                Console.WriteLine(file.FullName + "复制完毕，大小" + (file.Length / 1024).ToString() + "Kb");
                File.Copy(file.FullName, "../结果\\" + filename.Split("\\")[filename.Split("\\").Length - 1]);
            }
            else
            {
                Console.WriteLine(file.FullName + "大小超过10M，舍弃");
            }
        }

        public static void dir_de(FileSystemInfo folder2)
        {
            DirectoryInfo folder = new DirectoryInfo(folder2.FullName);
            foreach (FileSystemInfo fileSystem in folder.GetFileSystemInfos(""))//对目录下的每个文件或目录
            {
                if (fileSystem is DirectoryInfo)//如果是目录，则递归调用
                {
                    dir_de(fileSystem);
                }
                else //如果是文件,直接存储
                {
                    file_de(fileSystem);
                }
            }
        }

         static void Main(string[] args)
         {
                create("../结果");
                Console.WriteLine("Hello World!");
                DirectoryInfo folder = new DirectoryInfo("./");
                dir_de(folder);

                Console.WriteLine("\n再见");
                Console.ReadKey();

         }
    }
}

