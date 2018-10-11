using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjectVersionUpgrader
{
    class Program
    {
        static void RecurseList(string parentDirectory, List<string> projectFiles)
        {
            foreach(string directory in Directory.GetDirectories(parentDirectory))
            {
                RecurseList(directory, projectFiles);

                foreach (string file in Directory.GetFiles(directory))
                {
                    if(file.ToLower().EndsWith(".csproj"))
                    {
                        projectFiles.Add(file); 
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Solution root directory: ");
            string rootDir = Console.ReadLine();
            Console.WriteLine("Upgrade to what .Net version: ");
            string newVersion = Console.ReadLine(); 

            if(Directory.Exists(rootDir))
            {
                List<string> projectFiles = new List<string>();
                RecurseList(rootDir, projectFiles); 

                foreach(string projectFile in projectFiles)
                {
                    string content = File.ReadAllText(projectFile);

                    Regex regex = new Regex(@"\<TargetFrameworkVersion\>v(\d+|.)+<\/TargetFrameworkVersion>");
                    string output = regex.Replace(content, $"<TargetFrameworkVersion>v{newVersion}</TargetFrameworkVersion>");
                    File.WriteAllText(projectFile, output); 
                }
            }
        }
    }
}
