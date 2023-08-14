using LogLibraryClassLibrary;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MySQLClassLibrary
{
    public class ExcecuteTaskMySQL : Database
    {
        public ExcecuteTaskMySQL(string time_out) : base(time_out)
        {

        }

        public async Task<Tuple<bool, string>> UnblockUser(string user)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command;
                    if (user.ToLower() == "encargado")
                    {
                        command = new MySqlCommand("update usuario set password = '1509442', bloqueado = 0, debeCambiarPassword = 1 where login = " + "'" + user + "';", mysqlConnection);
                    }
                    else
                    {
                        command = new MySqlCommand("update usuario set password = '1509442', bloqueado = 0 where login = " + "'" + user + "';", mysqlConnection);
                    }
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if(result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> UnblockFailureFiscalMachine(string cashMachine)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update punto_de_venta set habilitada = 1 where identificador =" + "'" + cashMachine + "';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> UpdateAmountPettyCashFund(string amount)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update configuracion c set c.value = '" + amount + "' where c.key like 'pettyCashFund';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> UpdateEuroRate(string amount)
        {
            using (MySqlConnection connection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await connection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update configuracion c set c.value = '" + amount + "' where c.key like '%euroRate%';", connection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> UpdateDollarRate(string amount)
        {
            using (MySqlConnection connection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await connection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update configuracion c set c.value = '" + amount + "' where c.key like '%dollarRate%';", connection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> EnbaleDisableGlobalDisccountSap(string option)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update configuracion c set c.value = " + option + " where c.key = 'descglobalsap';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> UpdateAmountMaxPerInvoice(string amount)
        {
            using (MySqlConnection connection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await connection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update configuracion c set c.value ='" + amount + "' where c.key = 'purchaseMaxAmount';", connection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> DisableEnableCloseEmail(string option)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update configuracion c set c.value = "+ option + " where c.key = 'activateSendEmails';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> FailureUpdatePricesDisccountsTotalPos(string days)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update configuracion c set value = replace(date_sub(current_date(), interval "+ days +" day),'-','') where c.key = 'LastPriceUpdate';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> ResendCloser(string dateString)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update dia_operativo set compensado=0 where date(fecha) = '" + dateString + "';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> CloseCashMachineIfExistRegister(string dateString, string salePoint)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("select COUNT(*) from dia_operativo where fecha = '" + dateString + "' and codigo_punto_de_venta='" + salePoint + "';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    MySqlDataReader reader = command.ExecuteReader();
                    
                    if(reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int records = Int32.Parse(reader[0].ToString());

                            if (records == 0)
                            {
                                reader.Close();
                                await mysqlConnection.CloseAsync();
                                return new Tuple<bool, string>(false, "Error: No exites el registro en la base de datos, fecha inexistente o caja inexistente.");
                            }
                            else
                            {
                                reader.Close();
                                await mysqlConnection.CloseAsync();
                                return new Tuple<bool, string>(true, "Exites el registro.");
                            }
                        }

                        await mysqlConnection.CloseAsync();
                        return new Tuple<bool, string>(false, "Error: No exites el registro en la base de datos, fecha inexistente o caja inexistente.");
                    }
                    else
                    {
                        reader.Close();
                        await mysqlConnection.CloseAsync();
                        return new Tuple<bool, string>(false, "Error: No exites el registro en la base de datos, fecha inexistente o caja inexistente.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string, double>> CloseCashMachineSumSales(string dateString, string salePoint)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("select total_ventas from dia_operativo where fecha = '" + dateString + "' and codigo_punto_de_venta='" + salePoint + "';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    MySqlDataReader reader = command.ExecuteReader();
                    double resultSum = 0;

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            string result = reader[0].ToString();

                            if (string.IsNullOrEmpty(result))
                            {
                                resultSum = 0;
                            }
                            else
                            {
                                resultSum = Double.Parse(result);
                            }
                        }
                    }

                    reader.Close();
                    await mysqlConnection.CloseAsync();
                    return new Tuple<bool, string, double>(true, "Operacion exitosa.", resultSum);
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string, double>(false, "Error: " + ex.Message.ToLower(), 0);
                }
            }
        }

        public async Task<Tuple<bool, string>> CloseCashMachine(string dateString, string salesPoint)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("DELETE FROM dia_operativo WHERE (total_ventas = '0' OR total_ventas IS NULL) AND (num_facturas = '0' OR num_facturas IS NULL) AND (numero_notas_credito = '0' OR numero_notas_credito IS NULL) AND 0 = (SELECT COUNT(1) FROM factura WHERE DATE(fecha_creacion) = DATE(fecha) AND identificador_pos = codigo_punto_de_venta AND estado = 'Facturada') AND fecha ='" + dateString +"' and codigo_punto_de_venta='"+ salesPoint +"';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> CloseCashMachineZReport(string dateString, string salesPoint)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update dia_operativo set reporteZ='1' where fecha='" + dateString + "' and codigo_punto_de_venta='" + salesPoint + "';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> EqualsFicalNeto(string dateString, string cashMachine)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("UPDATE dia_operativo c SET total_ventas = (SELECT SUM(f.total_con_iva+(SELECT IFNULL(SUM(fi.igtf),0) FROM factura_igtf fi WHERE fi.codigo_interno_factura = f.codigo_interno)) - (SELECT IFNULL(SUM(n.total_con_iva),0) FROM nota_de_credito n WHERE n.estado = 'Nota' AND n.identificador_pos = c.codigo_punto_de_venta AND DATE(n.fecha_creacion) = DATE(c.fecha)) FROM factura f WHERE DATE(f.fecha_creacion)= c.fecha AND f.identificador_pos = c.codigo_punto_de_venta AND f.estado = 'Facturada') WHERE c.fecha = '" + dateString + "' AND c.codigo_punto_de_venta = '" + cashMachine + "' AND total_ventas = 0;", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> GlobalDiscount(string discountValue)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update articulo set descuento = '"+ discountValue + "'", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> GlobalDiscountPerSectors(string discountValue, string sectors)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update articulo set descuento = '" + discountValue + "' where sector in (" + sectors + ");", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> GlobalDiscountPerArticles(string discountValue, List<string> articles)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    foreach(string article in articles)
                    {
                        MySqlCommand command = new MySqlCommand("update articulo set descuento = '" + discountValue + "' where codigo in ('" + article + "');", mysqlConnection);
                        command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                        int result = await command.ExecuteNonQueryAsync();

                        if (result <= 0)
                        {
                            Logger.WriteToLog("Error: Operacion fallida descuento global por articulo: " + article);
                        }
                    }
                    await mysqlConnection.CloseAsync();
                    return new Tuple<bool, string>(true, "Operacion exitosa.");
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> DiscountSectorExcludingArticle(string discountValue, string sectors, string articles)
        {
            try
            {
                using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update articulo set descuento = '" + discountValue + "' where sector in (" + sectors + ") and codigo not in (" + articles + ");", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result <= 0)
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }

                return new Tuple<bool, string>(true, "Operacion exitosa.");
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
            }
        }

        public async Task<Tuple<bool, string>> DisableEnableSectorByDate(string option)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update configuracion c set c.value = " + option + " where c.key = 'updateSectorxDate';", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }


        public async Task<Tuple<bool, string>> UpdateSectorByArticle(List<Tuple<string,string>> data)
        {
            string failures = string.Empty;
            int counter = 1;
            foreach(Tuple<string, string> line in data)
            {
                using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
                {
                    try
                    {
                        await mysqlConnection.OpenAsync();
                        MySqlCommand command = new MySqlCommand("update articulo set sector = " + line.Item2 + " where codigo in ('" + line.Item1 + "');", mysqlConnection);
                        command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                        int result = await command.ExecuteNonQueryAsync();
                        await mysqlConnection.CloseAsync();
                        if (result <= 0)
                        {
                            Logger.WriteToLog("Error: Operacion actualizar sector por articulo data: " + line.Item1 + "," + line.Item2 + " no afecto ninguna fila en la base de datos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                        return new Tuple<bool,string>(false, "Error, operacion base de datos fallida con data: " + line.Item1 + "," + line.Item2 + ". Excepcion: " + ex.Message.ToLower());
                    }
                }

                counter++;
            }

            return new Tuple<bool, string>(true, "Operacion exitosa.");
        }

        public async Task<Tuple<bool, string>> CheckArticlesExist(List<string> listOfArticles)
        {
            string failures = string.Empty;
            foreach (string article in listOfArticles)
            {
                using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
                {
                    try
                    {
                        await mysqlConnection.OpenAsync();
                        MySqlCommand command = new MySqlCommand("select COUNT(articulo.codigo) from articulo where codigo = '"+ article + "'", mysqlConnection);
                        command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                        var result = await command.ExecuteScalarAsync();
                        if (Convert.ToInt32(result) <= 0)
                        {
                            return new Tuple<bool, string>(false, "Error: el articulo " + article + " no existe en la base de datos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                        return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                    }
                }
            }

            return new Tuple<bool, string>(true, "Operacion exitosa.");
        }

        public async Task<Tuple<bool, string>> UpdateDiscocuntsByArticles(List<Tuple<string, string>> data)
        {
            string failures = string.Empty;
            int counter = 1;
            foreach (Tuple<string, string> line in data)
            {
                using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
                {
                    try
                    {
                        await mysqlConnection.OpenAsync();
                        MySqlCommand command = new MySqlCommand("update articulo set descuento = '" + line.Item2 + "' where codigo in ('" + line.Item1 + "');", mysqlConnection);
                        command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                        int result = await command.ExecuteNonQueryAsync();
                        await mysqlConnection.CloseAsync();
                        if (result <= 0)
                        {
                            Logger.WriteToLog("Error: Operacion actualizar descuento por articulo data: " + line.Item1 + "," + line.Item2 + " no afecto ninguna fila en la base de datos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                        return new Tuple<bool, string>(false, "Error, operacion base de datos fallida con data: " + line.Item1 + "," + line.Item2 + ". Excepcion: " + ex.Message.ToLower());
                    }
                }

                counter++;
            }

            return new Tuple<bool, string>(true, "Operacion exitosa.");
        }

        public async Task<Tuple<bool, string>> UpdateGendersByArticles(List<Tuple<string, string>> data)
        {
            string failures = string.Empty;
            int counter = 1;
            foreach (Tuple<string, string> line in data)
            {
                using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
                {
                    try
                    {
                        await mysqlConnection.OpenAsync();
                        MySqlCommand command = new MySqlCommand("update articulo set genero = '" + line.Item2 + "' where codigo in ('" + line.Item1 + "');", mysqlConnection);
                        command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                        int result = await command.ExecuteNonQueryAsync();
                        await mysqlConnection.CloseAsync();
                        if (result <= 0)
                        {
                            Logger.WriteToLog("Error: Operacion actualizar descuento por articulo data: " + line.Item1 + "," + line.Item2 + " no afecto ninguna fila en la base de datos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                        return new Tuple<bool, string>(false, "Error, operacion base de datos fallida con data: " + line.Item1 + "," + line.Item2 + ". Excepcion: " + ex.Message.ToLower());
                    }
                }

                counter++;
            }

            return new Tuple<bool, string>(true, "Operacion exitosa.");
        }

        public async Task<Tuple<bool, string>> UpdateCategoriesByArticles(List<Tuple<string, string>> data)
        {
            string failures = string.Empty;
            int counter = 1;
            foreach (Tuple<string, string> line in data)
            {
                using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
                {
                    try
                    {
                        await mysqlConnection.OpenAsync();
                        MySqlCommand command = new MySqlCommand("update articulo set categoria = '" + line.Item2 + "' where codigo in ('" + line.Item1 + "');", mysqlConnection);
                        command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                        int result = await command.ExecuteNonQueryAsync();
                        await mysqlConnection.CloseAsync();
                        if (result <= 0)
                        {
                            Logger.WriteToLog("Error: Operacion actualizar descuento por articulo data: " + line.Item1 + "," + line.Item2 + " no afecto ninguna fila en la base de datos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                        return new Tuple<bool, string>(false, "Error, operacion base de datos fallida con data: " + line.Item1 + "," + line.Item2 + ". Excepcion: " + ex.Message.ToLower());
                    }
                }

                counter++;
            }

            return new Tuple<bool, string>(true, "Operacion exitosa.");
        }

        public async Task<Tuple<bool, string>> GlobalDiscountPerGenders(string discountValue, string genders)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update articulo set descuento = '" + discountValue + "' where genero in (" + genders + ");", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> GlobalDiscountPerCategories(string discountValue, string categories)
        {
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update articulo set descuento = '" + discountValue + "' where categoria in (" + categories + ");", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result >= 1)
                    {
                        return new Tuple<bool, string>(true, "Operacion exitosa.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(false, "Error: Operacion fallida, filas no afectadas en la base de datos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error: " + ex.Message.ToLower());
                }
            }
        }

        public async Task<Tuple<bool, string>> SetIva(string articles)
        {
            string failures = string.Empty;
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update articulo set iva='.16', ivaTipo='0' where codigo in (" + articles + ");", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result <= 0)
                    {
                        Logger.WriteToLog("Error: Operacion fijar iva de articulos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error, operacion base de datos fallida. Excepcion: " + ex.Message.ToLower());
                }
            }


            return new Tuple<bool, string>(true, "Operacion exitosa.");
        }

        public async Task<Tuple<bool, string>> SetExento(string articles)
        {
            string failures = string.Empty;
            using (MySqlConnection mysqlConnection = new MySqlConnection(GetSettingsConenction()))
            {
                try
                {
                    await mysqlConnection.OpenAsync();
                    MySqlCommand command = new MySqlCommand("update articulo set iva = '0.0', ivaTipo = '3' where codigo in (" + articles + ");", mysqlConnection);
                    command.CommandTimeout = SQL_TIMEOUT_EXECUTION_COMMAND;
                    int result = await command.ExecuteNonQueryAsync();
                    await mysqlConnection.CloseAsync();
                    if (result <= 0)
                    {
                        Logger.WriteToLog("Error: Operacion fijar exento de articulos.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                    return new Tuple<bool, string>(false, "Error, operacion base de datos fallida. Excepcion: " + ex.Message.ToLower());
                }
            }


            return new Tuple<bool, string>(true, "Operacion exitosa.");
        }
    }
}
