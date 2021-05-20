﻿using System;
using System.Net;
using System.Windows.Forms;

namespace DatabaseBackupTool
{
    public partial class Dashboard : Form
    {
        public static SqlConnectorInfo.SqlConnectionInfoData SqlInfoData; // Globally accessible instance of the XML loaded SQL Connection Info.
        private string xmlPath = "SqlConnectorData.xml";
        public Dashboard()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
            loadSqlConnectorXml();
        }

        private void loadSqlConnectorXml()
        {
            SqlInfoData = SqlConnectorInfo.LoadSqlConnectionData(xmlPath);
        }
        private void databaseBackupToolBtn_Click(object sender, EventArgs e)
        {
            loadSqlConnectorXml();
            Backup backup = new Backup();
            backup.ShowDialog();
        }

        private void restoreBackupToolBtn_Click(object sender, EventArgs e)
        {
            loadSqlConnectorXml();
            Restore restore = new Restore();
            restore.ShowDialog();
        }
    }
}
