﻿using System;
using System.Net;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void databaseBackupToolBtn_Click(object sender, EventArgs e)
        {
            Backup backup = new Backup();
            backup.ShowDialog();
        }

        private void restoreBackupToolBtn_Click(object sender, EventArgs e)
        {
            Restore restore = new Restore();
            restore.ShowDialog();
        }

        private void updateButtonClick(object sender, EventArgs e)
        {
            //gets the location the program was launched from and stores it in string. removes the file:\ from the front of it
            string str = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            str = str.Remove(0, 6);

            //debug
            str = "C:\\Users\\Jerrick\\Downloads\\";

            //downloads and overwrites files in location
            try
            {
                WebClient webClient = new WebClient();
                WebClient webClient2 = new WebClient();
                webClient.DownloadFileAsync(new Uri("https://github.com/JazzGlobal/DatabaseBackupTool/raw/master/Build/DatabaseBackupTool.exe"), str + "DatabaseBackupTool.exe");                
                webClient2.DownloadFileAsync(new Uri("https://github.com/JazzGlobal/DatabaseBackupTool/raw/master/Build/SqlConnector.dll"), str + "SqlConnector.dll");
            }
            catch (Exception f)
            {
                Exception myException = new Exception("Problem with path: " + str, f);
                ErrorForm popup = new ErrorForm(myException);
                popup.Show();
            }
        }//end updateButtonClickk
    }
}
