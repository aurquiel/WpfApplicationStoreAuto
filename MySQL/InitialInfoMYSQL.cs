using LogLibraryClassLibrary;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MySQLClassLibrary
{
    public class InitialInfoMYSQL : Database
    {
        public InitialInfoMYSQL(string time_out) : base(time_out)
        {

        }

        public async Task<Tuple<bool, string>> GetUsers(DataTable users)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("SELECT usuario.login FROM `usuario` ORDER BY usuario.login ASC", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    command.CommandType = System.Data.CommandType.Text;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    await Task.Run(() => adapter.Fill(users));
                    if (users != null && users.Rows.Count > 0)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa usuarios MYSQL obtenidos.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(true, "Error al obtener los usuarios de MYSQL.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> GetSalePoints(DataTable salePoints)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("SELECT punto_de_venta.identificador, punto_de_venta.descripcion FROM `punto_de_venta` ORDER BY punto_de_venta.descripcion ASC", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    command.CommandType = System.Data.CommandType.Text;

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    await Task.Run(() => adapter.Fill(salePoints));
                    if (salePoints != null && salePoints.Rows.Count > 0)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa cajas MYSQL obtenidas.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(true, "Error al obtener las cajas de MYSQL.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }
    }
}
