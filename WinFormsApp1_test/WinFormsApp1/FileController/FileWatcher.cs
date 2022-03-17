using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using WinFormsApp1.FileController;
using System.Threading;

namespace WinFormsApp1.FileController
{
    internal class FileWatcher
    {
        FileSystemWatcher watcher;
        public static int count = 1;
        public String path = "";


        public FileWatcher(String path)
        {
            this.path = path;
        }

        public void watcher_start() // 생성자
        {
            watcher_start(path);
        }

        public void watcher_start(String path) 
        {
            using (watcher = new FileSystemWatcher(path))
            {
                watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;

                //watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDeleted;
                //watcher.Renamed += OnRenamed;
                //watcher.Error += OnError;

                watcher.Filter = "*.json";
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;

                //Console.WriteLine("Press enter to exit.");
                //Console.ReadLine();

                Thread.CurrentThread.Join();
            }
        }

        private static void OnCreated(object sender, FileSystemEventArgs e) // static
        {
            String sampleName = e.Name;
            String samplePath = @"C:\Users\valoo\Desktop\workspace\studyDotNet\WinFormsApp1_test\path";
            String DotNetFilePath = @"C:\Users\valoo\Desktop\workspace\studyDotNet\WinFormsApp1_test\result";
            String DotNetFileName = $"result{count}.xls"; //xlsx, xls

            JsonFileToDotNet jd = new JsonFileToDotNet(samplePath, sampleName, DotNetFilePath, DotNetFileName);
            jd.fileCreate();


            count++;
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) // static
        {
            //Console.WriteLine($"Deleted: {e.FullPath}");
         
        }

        /*private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }*/




    }
}
