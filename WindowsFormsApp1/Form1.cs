using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, EventArgs e)
        {
            var progress = new Progress<string>(s => label.Text = s);
            await Task.Factory.StartNew(() => LongWork(progress), TaskCreationOptions.LongRunning);
            label.Text = "completed";
        }
        public void LongWork(IProgress<string> progress)
        {
            // Perform a long running work...
            for (var i = 0; i < 1000; i++)
            {
                Task.Delay(500).Wait();
                progress.Report(i.ToString());
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            try
            {
                var progress = new Progress<string>(s => button1.Text = s);
                await Task.Run(() => FailingWork(progress));
                button1.Text = "Completed";
            }
            catch (Exception exception)
            {
                button1.Text = "Failed: " + exception.Message;
            }
            button1.Enabled = true;
        }
        public static void FailingWork(IProgress<string> progress)
        {
            progress.Report("I will fail in...");
            Task.Delay(500).Wait();

            for (var i = 0; i < 3; i++)
            {
                progress.Report((3 - i).ToString());
                Task.Delay(500).Wait();
            }
            throw new Exception("Oops...");
        }
    }
}
