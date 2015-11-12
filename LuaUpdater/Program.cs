using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LuaUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            string root = @"c:\buildtools\src\mame\";
            ParseProjectFiles(root + @"scripts\target\mame\arcade.lua", root);
            ParseProjectFiles(root + @"scripts\target\mame\dummy.lua", root);
            ParseProjectFiles(root + @"scripts\target\mame\mame.lua", root);
            ParseProjectFiles(root + @"scripts\target\mame\mess.lua", root);
            ParseProjectFiles(root + @"scripts\target\mame\nl.lua", root);
            ParseProjectFiles(root + @"scripts\target\mame\tiny.lua", root);
        }

        private static void ParseProjectFiles(string filename, string root)
        {
            string[] lines = System.IO.File.ReadAllLines(filename);
            List<string> output = new List<string>();
            List <string> includes = new List<string>();
            foreach (string line in lines)
            {
                string l = line.Trim();
                if (l.StartsWith(("MAME_DIR ..")) && l.EndsWith((".h\",")))
                {
                    l = l.Replace("MAME_DIR .. \"", "");
                    l = l.Replace("\",", "");
                    includes.Add(l);
                }
            }

            foreach (string line in lines)
            {
                output.Add(line);
                string l = line.Trim();
                if (l.StartsWith(("MAME_DIR ..")) && l.EndsWith((".cpp\",")))
                {
                    l = l.Replace("MAME_DIR .. \"", "");
                    l = l.Replace("\",", "");
                    if (l.StartsWith("src/mame/drivers/"))
                    {
                        string inc = l.Replace("/drivers/", "/includes/").Replace(".cpp", ".h");
                        if (includes.Contains(inc)) continue;
                        if (File.Exists(root + inc))
                        {
                            output.Add("	MAME_DIR .. \"" + inc + "\",");
                        }
                    }
                    if (l.StartsWith("src/mame/audio/"))
                    {
                        string inc = l.Replace(".cpp", ".h");
                        if (includes.Contains(inc)) continue;
                        if (File.Exists(root + inc))
                        {
                            output.Add("	MAME_DIR .. \"" + inc + "\",");
                        }
                    }
                    if (l.StartsWith("src/mame/machine/"))
                    {
                        string inc = l.Replace(".cpp", ".h");
                        if (includes.Contains(inc)) continue;
                        if (File.Exists(root + inc))
                        {
                            output.Add("	MAME_DIR .. \"" + inc + "\",");
                        }
                    }
                    if (l.StartsWith("src/mame/video/"))
                    {
                        string inc = l.Replace(".cpp", ".h");
                        if (includes.Contains(inc)) continue;
                        if (File.Exists(root + inc))
                        {
                            output.Add("	MAME_DIR .. \"" + inc + "\",");
                        }
                    }
                }
            }
            System.IO.File.WriteAllLines(filename, output.ToArray());
        }
    }
}
