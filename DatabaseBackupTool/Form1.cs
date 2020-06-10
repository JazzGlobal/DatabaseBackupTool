﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SqlConnector; 

namespace DatabaseBackupTool
{
    public partial class Form1 : Form
    {
        SQLConnector connector;
        ErrorForm ef;
        public Form1()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Connect to (local)\SQLEXPRESS
            // Find list of all database names 
            connector = new SQLConnector("");
            connector.InitializeConnection();
            foreach(String s in GetDatabases())
            {
                databaseList.Items.Add(s);
            }
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Console.WriteLine("Closing"); 
        }

        private List<String> GetDatabases()
        {
            List<String> databases = new List<String>();
            if (connector.Open()) {

                var reader = connector.ReadResults(connector.CreateCommand("SELECT name FROM master.sys.databases where name NOT IN ('master','model','msdb','tempdb')"));
                while (reader.Read())
                {
                    databases.Add(reader[0].ToString());
                }
                reader.Close();
                if (connector.GetConnectionState() == ConnectionState.Open)
                {
                    connector.Close();
                }
            }
            return databases;
        }
        
        private void MoveSelectedItems(string direction, bool all=false)
        {

            List<String> movedList = new List<String>();
            if (direction == "right")
            {
                if (all) // If moving all items to the right.
                {
                    for (int entry = 0; entry < databaseList.Items.Count; entry++)
                    {
                        databaseList.SetSelected(entry, true);
                    }
                }
                foreach(String s in databaseList.SelectedItems)
                {
                    if (!backupList.Items.Contains(s))
                    {
                        movedList.Add(s);
                    }
                }
                foreach(String s in movedList)
                {
                    backupList.Items.Add(s);
                    if (databaseList.Items.Contains(s)){
                        databaseList.Items.Remove(s);
                    }
                }
                
            } 
            else if (direction == "left")
            {
                if(all) // If moving all items left.
                {
                    for (int entry = 0; entry < backupList.Items.Count; entry++)
                    {
                        backupList.SetSelected(entry, true);
                    }
                }
                foreach(String s in backupList.SelectedItems)
                {
                    if (!databaseList.Items.Contains(s))
                    {
                        movedList.Add(s);
                    }
                }
                foreach(String s in movedList)
                {
                    databaseList.Items.Add(s);
                    if (backupList.Items.Contains(s))
                    {
                        backupList.Items.Remove(s);
                    }
                }
            }
        }
        private void MoveSelectRight_Click(object sender, EventArgs e)
        {
            MoveSelectedItems("right");
        }

        private void MoveSelectLeft_Click(object sender, EventArgs e)
        {
            MoveSelectedItems("left");
        }

        private void MoveAllRight_Click(object sender, EventArgs e)
        {
            MoveSelectedItems("right", true);
        }

        private void MoveAllLeft_Click(object sender, EventArgs e)
        {
            MoveSelectedItems("left", true);
        }

        private void RefreshDBs_Click(object sender, EventArgs e)
        {
            databaseList.Items.Clear();
            backupList.Items.Clear();

            foreach(String s in GetDatabases())
            {
                databaseList.Items.Add(s);
            }

        }

        private void startBackUp_Click(object sender, EventArgs e)
        {
            BackupDatabases();
        }

        private void BackupDatabases()
        {
            // Start Backing up selected DBs
            foreach (String s in backupList.Items)
            {

                if (connector.Open())
                {
                    try
                    {
                        string sql = $"BACKUP DATABASE \"{s}\" TO DISK = \'{backupDirectoryTextBox.Text}\\{s}.BAK\'";
                        var reader = connector.ReadResults(connector.CreateCommand(sql));
                        if (connector.GetConnectionState() == ConnectionState.Open)
                        {
                            connector.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        connector.Close();
                        ef = new ErrorForm(e);
                        ef.Show();
                        break;
                    }
                }
            }
        }

        private void chooseDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            backupDirectoryTextBox.Text = fbd.SelectedPath;
        }

        private void databaseList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void backupDirectoryTextBox_TextChanged(object sender, EventArgs e)
        {
            if(backupDirectoryTextBox.Text == "")
            {
                backupDirectoryTextBox.Text = "C:\\CEMDAS\\temp";
            }
        }
    }
}
