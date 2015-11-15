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
            string[] lines = fileContent.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace("\n", "").Replace("\r", "");
            }

            List<string> output = new List<string>();

            int j = 0;
            do
            {
                if (lines[j].Contains("// clang-format on"))
                {
                    j+=2;
                    continue;
                }
                if (lines[j].Contains("// clang-format off"))
                {
                    j++;
                    continue;
                }
                output.Add(lines[j]);
                j++;
            }
            while (j < lines.Length) ;
            System.IO.File.WriteAllLines(filename, output.ToArray());
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
            string root = @"c:\buildtools\src\mame-split";
            AnalizeDir(root + @"\src\");
        }
    }
}
