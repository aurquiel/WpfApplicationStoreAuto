using System;
using System.IO;
using System.Windows.Forms;

namespace LogLibraryClassLibrary
{
    public static class Logger
    {
        public static readonly string LOGGER_FILE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AutogestionTiendas/Logger.log";
        public static readonly string MYSQL_FILE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AutogestionTiendas/mysql.txt";
        public static readonly string LOGGER_DIRECTORY_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AutogestionTiendas";

        public static void CreateLog()
        {
            try
            {
                if(!Directory.Exists(LOGGER_DIRECTORY_PATH))
                {
                    Directory.CreateDirectory(LOGGER_DIRECTORY_PATH);
                }

                if(!File.Exists(LOGGER_FILE_PATH))
                {
                    FileStream fs = File.Open(LOGGER_FILE_PATH, FileMode.Create);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Logger Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void CreateMySQLFile()
        {
            try
            {
                if (!Directory.Exists(LOGGER_DIRECTORY_PATH))
                {
                    Directory.CreateDirectory(LOGGER_DIRECTORY_PATH);
                }

                if (!File.Exists(MYSQL_FILE_PATH))
                {
                    FileStream fs = File.Open(MYSQL_FILE_PATH, FileMode.Create);
                    var sw = new StreamWriter(fs);
                    sw.WriteLine("server=172.16.10.25;user=root;database=gt99;port=3306;password=.Gt99Administrador.;SSL Mode=None");
                    sw.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Logger Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void WriteToLog(string message)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                using (StreamWriter w = File.AppendText(LOGGER_FILE_PATH))
                {
                    w.WriteLine(dateTime.ToString() + ": " + message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Write To Log Exception " + ex.Message.ToLower(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool LoggerExists()
        {
            return File.Exists(LOGGER_FILE_PATH);
        }
    }
}
