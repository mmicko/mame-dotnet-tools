using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PreventFormating
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
            bool indefine = false;
            bool inmacro = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (indefine && !lines[i].TrimEnd().EndsWith("\\") && !lines[i].Trim().StartsWith("#define"))
                {
                    lines[i] = lines[i] + "\r\n// clang-format on";
                    indefine = false;
                }

                if (lines[i].Trim().StartsWith("#define") && !inmacro && !indefine)
                {
                    lines[i] = "// clang-format off\r\n" + lines[i];
                    changed = true;
                    indefine = true;
                    continue;
                }




                if (lines[i].Trim().Contains("MACHINE_CONFIG_START") || lines[i].Trim().Contains("MACHINE_CONFIG_FRAGMENT") || lines[i].Trim().Contains("MACHINE_CONFIG_DERIVED")
                    || lines[i].Trim().Contains("MACHINE_CONFIG_DERIVED_CLASS"))
                {
                    lines[i] = "// clang-format off\r\n" + lines[i];
                    changed = true;
                    inmacro = true;
                    continue;
                }

                if (inmacro && lines[i].Trim().Contains("MACHINE_CONFIG_END"))
                {
                    lines[i] = lines[i] + "\r\n// clang-format on";
                    inmacro = false;
                }



                if (lines[i].Trim().Contains("ROM_START"))
                {
                    lines[i] = "// clang-format off\r\n" + lines[i];
                    changed = true;
                    inmacro = true;
                    continue;
                }

                if (inmacro && lines[i].Trim().Contains("ROM_END"))
                {
                    lines[i] = lines[i] + "\r\n// clang-format on";
                    inmacro = false;
                }


                if (lines[i].Trim().Contains("ADDRESS_MAP_START"))
                {
                    lines[i] = "// clang-format off\r\n" + lines[i];
                    changed = true;
                    inmacro = true;
                    continue;
                }

                if (inmacro && lines[i].Trim().Contains("ADDRESS_MAP_END"))
                {
                    lines[i] = lines[i] + "\r\n// clang-format on";
                    inmacro = false;
                }


                if (lines[i].Trim().Contains("INPUT_PORTS_START"))
                {
                    lines[i] = "// clang-format off\r\n" + lines[i];
                    changed = true;
                    inmacro = true;
                    continue;
                }

                if (inmacro && lines[i].Trim().Contains("INPUT_PORTS_END"))
                {
                    lines[i] = lines[i] + "\r\n// clang-format on";
                    inmacro = false;
                }

                if (lines[i].Trim().Contains("DISCRETE_SOUND_START"))
                {
                    lines[i] = "// clang-format off\r\n" + lines[i];
                    changed = true;
                    inmacro = true;
                    continue;
                }

                if (inmacro && lines[i].Trim().Contains("DISCRETE_SOUND_END"))
                {
                    lines[i] = lines[i] + "\r\n// clang-format on";
                    inmacro = false;
                }

                if (lines[i].Trim().Contains("SLOT_INTERFACE_START"))
                {
                    lines[i] = "// clang-format off\r\n" + lines[i];
                    changed = true;
                    inmacro = true;
                    continue;
                }

                if (inmacro && lines[i].Trim().Contains("SLOT_INTERFACE_END"))
                {
                    lines[i] = lines[i] + "\r\n// clang-format on";
                    inmacro = false;
                }

                if (lines[i].Trim().StartsWith("GAME("))
                {
                    lines[i] = "// clang-format off\r\n" + lines[i] + "\r\n// clang-format on";
                    inmacro = false;
                }
            }
            if (changed)
            {
                System.IO.File.WriteAllLines(filename, lines);
            }
            return lines;
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
