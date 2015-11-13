using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckIncludedFiles
{
    class Program
    {
        static List<string> files;
        static List<string> srcfiles;
        static string root = @"c:\buildtools\src\mame\";

        static void ParseFile(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            foreach (string line in lines)
            {
                // regular files
                if (line.Trim().StartsWith("MAME_DIR .. \""))
                {
                    string l = line.Trim().Replace("MAME_DIR .. \"", "").Replace("\",", "").Replace("\"","");
                    files.Add(l);
                }
                // commented like some test disassms and similar
                if (line.Trim().StartsWith("--MAME_DIR .. \""))
                {
                    string l = line.Trim().Replace("--MAME_DIR .. \"", "").Replace("\",", "").Replace("\"", "");
                    files.Add(l);
                }
                // disassebler extraction
                if (line.Trim().StartsWith("table.insert(disasm_files , "))
                {
                    string l = line.Trim().Replace("table.insert(disasm_files , ","").Replace("MAME_DIR .. \"", "").Replace("\")", "");
                    files.Add(l);
                }
            }
        }

        public static void ScanSource(string path)
        {
            srcfiles = new List<string>();

            if (File.Exists(path))
            {
                // This path is a file
                ProcessFile(path);
            }
            else if (Directory.Exists(path))
            {
                // This path is a directory
                ProcessDirectory(path);
            }
        }

        public static void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void ProcessFile(string path)
        {
            string l = path.Replace(root, "").Replace(@"\",@"/");
            if (!files.Contains(l))
                srcfiles.Add(l);
        }

        static void Main(string[] args)
        {
            files = new List<string>();

            ParseFile(root + @"scripts\src\bus.lua");
            ParseFile(root + @"scripts\src\cpu.lua");
            ParseFile(root + @"scripts\src\devices.lua");
            ParseFile(root + @"scripts\src\emu.lua");
            ParseFile(root + @"scripts\src\lib.lua");
            ParseFile(root + @"scripts\src\machine.lua");
            ParseFile(root + @"scripts\src\main.lua");
            ParseFile(root + @"scripts\src\netlist.lua");
            ParseFile(root + @"scripts\src\sound.lua");
            ParseFile(root + @"scripts\src\tools.lua");
            ParseFile(root + @"scripts\src\video.lua");

            ParseFile(root + @"scripts\src\osd\modules.lua");
            ParseFile(root + @"scripts\src\osd\osdmini.lua");
            ParseFile(root + @"scripts\src\osd\sdl.lua");
            ParseFile(root + @"scripts\src\osd\windows.lua");

            ParseFile(root + @"scripts\target\ldplayer\ldplayer.lua");
 
            ParseFile(root + @"scripts\target\mame\arcade.lua");
            ParseFile(root + @"scripts\target\mame\dummy.lua");
            ParseFile(root + @"scripts\target\mame\mame.lua");
            ParseFile(root + @"scripts\target\mame\mess.lua");
            ParseFile(root + @"scripts\target\mame\nl.lua");
            ParseFile(root + @"scripts\target\mame\tiny.lua");

            ScanSource(root + @"src\");

            foreach (string file in srcfiles) 
            {
                if (file.EndsWith(".h") || file.EndsWith(".cpp") || file.EndsWith(".inc"))
                    Console.WriteLine(file);
            }
            
        }
    }
}
