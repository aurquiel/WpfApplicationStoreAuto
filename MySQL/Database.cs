using LogLibraryClassLibrary;
using System;
using System.Configuration;
using System.IO;
using System.Linq;

namespace MySQLClassLibrary
{
    public class Database
    {
        public int SQL_TIMEOUT_EXECUTION_COMMAND = 5; //By default is 5 seconds
        private static string CONNECTION_STRING = string.Empty;

        public Database(string time_out)
        {
            try
            {
                SQL_TIMEOUT_EXECUTION_COMMAND = Int32.Parse(time_out);
            }
            catch (Exception ex)
            {
                SQL_TIMEOUT_EXECUTION_COMMAND = 5;
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
            }
        }

        protected string GetSettingsConenction()
        {
            try
            {
                if (CONNECTION_STRING == string.Empty)
                {
                    string[] lines = File.ReadAllLines(Logger.MYSQL_FILE_PATH);
                    lines = lines.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                    if (lines.Length > 0)
                    {
                        CONNECTION_STRING = lines[0];
                    }
                    return CONNECTION_STRING;
                }
                else
                {
                    return CONNECTION_STRING;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Error al tomar string conmexion Mysql del archivo de configuracion. Excepcion: " + ex.Message.ToLower());
                return string.Empty;
            }
        }
    }
}
