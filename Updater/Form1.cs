using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace Updater
{
    public partial class Form1 : Form
    {
        //variables
        string version = "1.5"; //replace this to the version of your program
        string pastebinversion = "https://pastebin.com/raw/gBJ0EAZk"; //replace the pastebin with one that is yours
        string pastebintitle = "https://pastebin.com/raw/hDW5uA8D"; //replace the pastebin with one that is yours
        string pastebindescription = "https://pastebin.com/raw/0eLEfK0k"; //replace the pastebin with one that is yours
        string downloadlink = ""; //put the link with one that is yours.

        //webclient
        WebClient client = new WebClient();
        public Form1()
        {
            InitializeComponent();
        }

        private void bunifuGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        //bunifuGradientPanel1.GradientBottomLeft = System.Drawing.Color.Red;
        //bunifuGradientPanel1.GradientBottomRight = System.Drawing.Color.Firebrick;
        private void Form1_Load(object sender, EventArgs e)
        {
            bunifuGradientPanel1.GradientBottomLeft = System.Drawing.Color.Gold;
            bunifuGradientPanel1.GradientBottomRight = System.Drawing.Color.Yellow;
            updateLabel.Text = "You may need to check for updates";
            updatesButton.Text = "Check for updates";
        }

        void CheckForUpdates()
        {
            string whatsthelatestversion = client.DownloadString(pastebinversion);
            string whatsthepastebintitle = client.DownloadString(pastebintitle);
            string whatsthepastebindescription = client.DownloadString(pastebindescription);
            if (whatsthelatestversion.Contains(version))
            {
                bunifuGradientPanel1.GradientBottomLeft = System.Drawing.Color.LimeGreen;
                bunifuGradientPanel1.GradientBottomRight = System.Drawing.Color.Lime;
                updateLabel.Text = "This program is up to date";
                updatesButton.Text = "Check for updates";
            }
            else
            {
                bunifuGradientPanel1.GradientBottomLeft = System.Drawing.Color.Gold;
                bunifuGradientPanel1.GradientBottomRight = System.Drawing.Color.Yellow;
                updateLabel.Text = "There are some updates pending";
                updatesButton.Text = "Install updates";
                MessageBox.Show(whatsthepastebindescription, whatsthepastebintitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void updatesButton_Click(object sender, EventArgs e)
        {
            if (updatesButton.Text == "Check for updates")
            {
                CheckForUpdates();
            }
            else if (updatesButton.Text == "Install updates")
            {
                label2.Visible = true;
                progressBar1.Visible = true;
                updatesButton.Enabled = false;
                StartDownload();
            }
        }

        private void StartDownload()
        {
            Thread thread = new Thread(() => {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                client.DownloadFileAsync(new Uri(downloadlink), "program.rar");
            });
            thread.Start();
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                label2.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
                progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                bunifuGradientPanel1.GradientBottomLeft = System.Drawing.Color.LimeGreen;
                bunifuGradientPanel1.GradientBottomRight = System.Drawing.Color.Lime;
                updateLabel.Text = "This program is up to date";
                label2.Text = "Completed, however, you may need to restart the program or extract the file.";
            });
        }
    }
}
