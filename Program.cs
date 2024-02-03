using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmptyFileDeleter {

    internal class Program {

        private class DirInfo {
            public string Dir;
            public bool IsEmpty;
            public DirInfo(string dir) {
                this.Dir = dir;
                this.IsEmpty = false;
            }
        }

        static void Main(string[] args) {

            string driveRoot = Path.GetPathRoot(@"C:\");

            DeleteEmptyFolders(@"C:\Program Files (x86)", 0);
            Console.ReadLine();
        }

        static string GetDepthSpacing(int depth) {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < depth; i++) {
                sb.Append("  ");
            }
            return sb.ToString();
        }

        // returns true if empty
        static bool DeleteEmptyFolders(string currentDir, int depth) {

            List<DirInfo> subDirs = new List<DirInfo>();
            try {
                subDirs = Directory.GetDirectories(currentDir).Select(subDir => {
                    return new DirInfo(subDir);
                }).ToList();
            }
            catch (UnauthorizedAccessException) {
                return false;
            }

            bool hasRestrictedFiles = false;
            string[] allFiles = Array.Empty<string>();
            try {
                allFiles = Directory.GetFiles(currentDir);
            }
            catch (System.UnauthorizedAccessException) {
                hasRestrictedFiles = true;
            }

            if (!subDirs.Any() && !hasRestrictedFiles && !allFiles.Any())
                return true;

            foreach(DirInfo subDir in subDirs) {
                if (DeleteEmptyFolders(subDir.Dir, depth + 1)) {
                    //Directory.Delete(subDir.Dir);
                    Console.WriteLine($"{GetDepthSpacing(depth)}Deleting {subDir.Dir}");
                    subDir.IsEmpty = true;
                }
            }

            return subDirs.All(sd => sd.IsEmpty) && !hasRestrictedFiles && !allFiles.Any();
        }

    }
}
