using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogErrorChange
{
    class Program
    {

        public static string[] changefile(string fileContent, string filename)
        {
            fileContent = fileContent.Substring(0, fileContent.Length - 2);
            string[] lines = fileContent.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Replace("\n", "").Replace("\r", "");
            }

            bool changed = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("logerror(") && !lines[i].Contains("logerror(LOG_"))
                {
                    lines[i] = lines[i].Replace("logerror(", "logerror(LOG_DEFAULT,");
                    changed = true;
                }
               

            }
            if (changed)
            {
                System.IO.File.WriteAllLines(filename, lines);
            }
            return lines;
        }

        static void Analize(string path)
        {
            string[] filePaths = Directory.GetFiles(path, "*.cpp");
            
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
                    //Console.WriteLine(fn);
                    //Console.WriteLine("-----------");

                    string[] lines = changefile(fileContent, filename);
                }

            }
        }
        static void AnalizeDir(string path)
        {
            string[] dirs = Directory.GetDirectories(path);          
            foreach (string dir in dirs)
            {
                AnalizeDir(dir);
                Analize(dir);
            }
        }
        static void Main(string[] args)
        {
            string root = @"c:\buildtools\src\mame\";
            AnalizeDir(root + @"\src\devices\");
        }
    }
}
