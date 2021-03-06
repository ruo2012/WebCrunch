﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace WebCrunch.Utilities
{
    class Updates
    {
        /// <summary>
        /// Check application for update. Installs latest installer to user downloads folder and opens it and closes this instance
        /// </summary>
        public static void checkForUpdate()
        {
            try
            {
                Program.log.Info("Checking for internet connection");

                Version newVersion = null;
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(MainForm.urlLatestVersion);
                stream.ReadTimeout = 10000;
                using (StreamReader reader = new StreamReader(stream))
                {
                    newVersion = new Version(reader.ReadToEnd());
                    Version curVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                    if (curVersion.CompareTo(newVersion) < 0)
                    {
                        Program.log.Info("Update found - preparing to update");
                        MessageBox.Show(MainForm.form, "WebCrunch " + newVersion.ToString() + " is ready to be installed.", "WebCrunch - Update Available");

                        client.DownloadFile(MainForm.getUrlLatestInstaller(newVersion), MainForm.userDownloadsDirectory + MainForm.pathInstallerFileName);
                        Program.log.Info("Installer downloaded successfully - opening installer...");
                        Directory.Delete(MainForm.pathData, true);
                        Process.Start(MainForm.userDownloadsDirectory + MainForm.pathInstallerFileName);
                        Process.Start(Application.StartupPath + @"\AutoUpdater.exe");
                        Application.Exit();
                    }
                }                    
            } 
            catch (Exception ex)
            {
                Program.log.Error("Unable to update", ex);
                MessageBox.Show("There was an error checking for update. You will be directed to the latest available version download page.");
                Process.Start(MainForm.urlLatestDownload);
                Application.Exit();
            }
        }
    }
}
