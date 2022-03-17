using System.Threading;
using WinFormsApp1.FileController;
using System.ComponentModel;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {

        private FileWatcher watcher = new FileWatcher(@"C:\Users\valoo\Desktop\workspace\studyDotNet\WinFormsApp1_test\path");
        private BackgroundWorker back;

        public Form1()
        {
            InitializeComponent();

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread t1 = new Thread(new ThreadStart(thread1)) { IsBackground = true };
            t1.Start();
            Thread t2 = new Thread(new ThreadStart(thread2)) { IsBackground = true };
            t2.Start();
        }

        private void thread1()
        {
            watcher.watcher_start();
        }

        private void thread2()
        {
            FileSystemWatcher fw = new FileSystemWatcher(@"C:\Users\valoo\Desktop\workspace\studyDotNet\WinFormsApp1_test\result");

            fw.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;

            fw.Created += OnCreated;
            fw.Deleted += OnDeleted;

            //fw.Filter = "*.*";
            fw.IncludeSubdirectories = true;
            fw.EnableRaisingEvents = true;

            Thread.CurrentThread.Join();

        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            setText_Control(label1, "count : " + Directory.GetFiles(@"C:\Users\valoo\Desktop\workspace\studyDotNet\WinFormsApp1_test\result").Length.ToString());
        }

        delegate void ChangeLabel(Control ctr, string text);

        public void setText_Control(Control ctr, string txtValue)
        {
            if (ctr.InvokeRequired)
            {
                ChangeLabel changelabel = new ChangeLabel(setText_Control);
                ctr.Invoke(changelabel, ctr, txtValue);
            }
            else
            {
                ctr.Text = txtValue;
            }
        }


        private void OnDeleted(object sender, FileSystemEventArgs e) =>
            setText_Control(label1, "count : " + Directory.GetFiles(@"C:\Users\valoo\Desktop\workspace\studyDotNet\WinFormsApp1_test\result").Length.ToString());




    }
}