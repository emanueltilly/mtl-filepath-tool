using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


namespace mtl_filepath_tool
{
    class FileManager
    {
        public static String[] ListFilesInFolder(string folderPath)
        {
            String[] returnList;

            try
            {
                returnList = Directory.GetFiles(folderPath);
            } catch
            {
                MessageBox.Show("There was a error listing the files in the folder", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                returnList = new String[0];
            }
            

            return returnList;
        }

        public static void Delete(string filepath)
        {
            try
            {
                if (File.Exists(filepath))
                {
                    //Console.WriteLine("File exists. Deleting file: " + filepath);
                    File.Delete(filepath);
                }
            }
            catch { MessageBox.Show("There was a error deleting a file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        public static void CreateEmpty(string filepath)
        {
            //Delete if existing
            if (File.Exists(filepath))
            {
                Delete(filepath);

            }
            //Create new empty file
            try
            {
                File.Create(filepath).Dispose();
                //Console.WriteLine("Creating new empty file: " + filepath);
            }
            catch { MessageBox.Show("There was a error creating a empty file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        public static void AppendLine(string filepath, string lineData)
        {
            try { File.AppendAllText(filepath, lineData + Environment.NewLine); }
            catch { MessageBox.Show("There was a error appending a new line to a file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        public static string[] ReadFileToArray(string filepath)
        {
            string[] returnArray;
            try
            {
                returnArray = File.ReadAllLines(filepath);
            } catch
            {
                MessageBox.Show("There was a error reading file line by line into array", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                returnArray = new string[0];
            }

            return returnArray;
             

        }






    }
}
