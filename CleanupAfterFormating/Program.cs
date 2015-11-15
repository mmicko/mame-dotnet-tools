using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanupAfterFormating
{
    class Program
    {

        public static void changefile(string fileContent, string filename)
        {
            fileContent = fileContent.Substring(0, fileContent.Length - 2);
            fileContent = fileContent.Replace("\r\n// clang-format on\r\n}", "");
            fileContent = fileContent.Replace("// clang-format off\r\n", "") + "\r\n";
            File.WriteAllText(filename, fileContent);
        }

        static void Analize(string path, string ext)
        {
            string[] filePaths = Directory.GetFiles(path, ext);

            foreach (string filename in filePaths)
            {
                string fileContent;
                if (File.Exists(filename))
                {
                    using (StreamReader sr = new StreamReader(filename))
                    {
                        fileContent = sr.ReadToEnd();
                    }
                    string fn = Path.GetFileName(filename);

                    changefile(fileContent, filename);
                }

            }
        }
        static void AnalizeDir(string path)
        {
            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                AnalizeDir(dir);
            }
            Analize(path, "*.h");
            Analize(path, "*.cpp");
        }
        static void Main(string[] args)
        {
            string root = @"c:\buildtools\src\mame";
            AnalizeDir(root + @"\src\devices\bus\");
        }
    }
}
