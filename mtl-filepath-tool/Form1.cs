using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace mtl_filepath_tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            folderLabel.Text = Properties.Settings.Default.folder;
            extentionTextBox.Text = Properties.Settings.Default.extention;
            searchTextBox.Text = Properties.Settings.Default.searchfor;
            replaceTextBox.Text = Properties.Settings.Default.replacewith;

        }

        private void appendToLog(string logText)
        {
            logBox.AppendText("\n" + logText);
            logBox.ScrollToCaret();
            System.Threading.Thread.Sleep(5);
        }

        private void SearchAndReplace()
        {
            //List all files in folder
            String[] filesInFolder = FileManager.ListFilesInFolder(folderLabel.Text);

            appendToLog("\n\nFiles in folder:\n");
            foreach (string file in filesInFolder)
            {
                appendToLog(file);
            }

            int changedFiles = 0;
            //Filter out non matching files
            foreach (string file in filesInFolder)
            {
                if (extentionTextBox.Text == "")
                {
                    if (ProcessFile(file))
                    {
                        changedFiles++;
                    }
                }
                else
                {
                    int comparison = String.Compare(("." + extentionTextBox.Text), Path.GetExtension(file), comparisonType: StringComparison.OrdinalIgnoreCase);
                    if (comparison == 0)
                    {

                        if (ProcessFile(file))
                        {
                            changedFiles++;
                        }

                    }
                }


            }

            if (changedFiles > 0)
            {
                MessageBox.Show((changedFiles + " files were updated."), "Search & Replace Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(("No files were updated."), "Search & Replace Done with warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private bool ProcessFile(string filePath)
        {
            appendToLog("\n\nProcessing: " + filePath);

            appendToLog("Reading file into memory");
            string[] sourceFile = FileManager.ReadFileToArray(filePath);
            appendToLog("Done");

            appendToLog("Searching and replacing...");

            int matchesCounter = 0;
            string[] destFile = new string[0];
            foreach (string line in sourceFile)
            {
                string newLine = line;

                if (line.Contains(searchTextBox.Text))
                {
                    newLine = line.Replace(searchTextBox.Text, replaceTextBox.Text);
                    appendToLog("\nOriginal line: " + line);
                    appendToLog("New line:      " + newLine);
                    matchesCounter++;
                }
                destFile = destFile.Concat(new string[] { newLine }).ToArray();

            }
            appendToLog("Lines replaced: " + matchesCounter + " / " + sourceFile.Length);


            if (matchesCounter > 0)
            {
                appendToLog("Deleting source file");
                FileManager.Delete(filePath);
                appendToLog("Done");

                appendToLog("Creating new empty file");
                FileManager.CreateEmpty(filePath);
                appendToLog("Done");

                appendToLog("Appending modified file from memory...");

                foreach (string appendLine in destFile)
                {
                    FileManager.AppendLine(filePath, appendLine);
                }

                appendToLog("Done");
                return true;
            }
            else
            {
                appendToLog("Not preforming any actions on the sourcefile since nothing was replaced");
                return false;
            }


        }

        private void runButton_Click(object sender, EventArgs e)
        {
            if (folderLabel.Text != "" && searchTextBox.Text != "")
            {
                SearchAndReplace();
                Properties.Settings.Default.folder = folderLabel.Text;
                Properties.Settings.Default.extention = extentionTextBox.Text;
                Properties.Settings.Default.searchfor = searchTextBox.Text;
                Properties.Settings.Default.replacewith = replaceTextBox.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                MessageBox.Show("You need to select a folder and enter a search phrase", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void browseFolderButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    folderLabel.Text = fbd.SelectedPath;
                    appendToLog("Target folder changed: " + fbd.SelectedPath);
                }
                else
                {
                    MessageBox.Show("There was a error opening the folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
