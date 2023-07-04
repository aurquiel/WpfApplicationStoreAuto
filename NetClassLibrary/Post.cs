using LogLibraryClassLibrary;
using NetworkClassLibrary.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NetworkClassLibrary
{
    public class Post
    {
        public static async Task<Tuple<bool, string, NetworkClassLibrary.Models.StoreEmployeeAnswerModel>> LoginUserStore(string url, string alias, string storeCode)
        {
            try
            {
                WebRequest oRequest = WebRequest.Create(url);
                oRequest.Method = "post";
                oRequest.ContentType = "application/json;charset=UTF-8";
                oRequest.UseDefaultCredentials = false;
                oRequest.PreAuthenticate = true;
                oRequest.Credentials = CredentialCache.DefaultNetworkCredentials;

                using (var oSW = new StreamWriter(await oRequest.GetRequestStreamAsync()))
                {
                    string json = "{\"stremp_alias\":\"" + alias + "\",\"store_code\":" + storeCode + "}";

                    await oSW.WriteAsync(json);
                    oSW.Flush();
                    oSW.Close();
                }

                WebResponse oResponse = await oRequest.GetResponseAsync();

                using (var oSR = new StreamReader(oResponse.GetResponseStream()))
                {
                    string result = await oSR.ReadToEndAsync();
                    result = result.Trim();
                    return new Tuple<bool, string, NetworkClassLibrary.Models.StoreEmployeeAnswerModel>(true, "Respuesta del servidor obtenida con exito.", JsonConvert.DeserializeObject<NetworkClassLibrary.Models.StoreEmployeeAnswerModel>(result));
                }
            }
            catch(Exception ex)
            {
                Logger.WriteToLog("Metodo: " +ex.TargetSite + ", Error: " + ex.Message.ToLower());
                return new Tuple<bool, string, NetworkClassLibrary.Models.StoreEmployeeAnswerModel>(false, "Error " + ex.Message.ToLower(), new NetworkClassLibrary.Models.StoreEmployeeAnswerModel());
            }
        }

        public static async Task<Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel>> InsertTask(string url, TaskModel taskmodel)
        {
            try
            {
                WebRequest oRequest = WebRequest.Create(url);
                oRequest.Method = "post";
                oRequest.ContentType = "application/json;charset=UTF-8";
                oRequest.UseDefaultCredentials = false;
                oRequest.PreAuthenticate = true;
                oRequest.Credentials = CredentialCache.DefaultNetworkCredentials; //visto en stackoverflow autentification de windows

                using (var oSW = new StreamWriter(await oRequest.GetRequestStreamAsync()))
                {
                    string json = JsonConvert.SerializeObject(taskmodel);
                    await oSW.WriteAsync(json);
                    oSW.Flush();
                    oSW.Close();
                }

                WebResponse oResponse = await oRequest.GetResponseAsync();

                using (var oSR = new StreamReader(oResponse.GetResponseStream()))
                {
                    string result = await oSR.ReadToEndAsync();
                    result = result.Trim();
                    return new Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel>(true, "Respuesta del servidor obtenida con exito.", JsonConvert.DeserializeObject<NetworkClassLibrary.Models.TaskAnswerModel>(result));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                return new Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel>(false, "Error " + ex.Message.ToLower(), new NetworkClassLibrary.Models.TaskAnswerModel());
            }
        }

        public static async Task<Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel>> GetStatusTask(string url, string taskId)
        {
            try
            {
                WebRequest oRequest = WebRequest.Create(url);
                oRequest.Method = "post";
                oRequest.ContentType = "application/json;charset=UTF-8";
                oRequest.UseDefaultCredentials = false;
                oRequest.PreAuthenticate = true;
                oRequest.Credentials = CredentialCache.DefaultNetworkCredentials; //visto en stackoverflow autentification de windows

                using (var oSW = new StreamWriter(await oRequest.GetRequestStreamAsync()))
                {
                    string json = "{\"task_id\":\"" + taskId + "\"" + "}";
                    await oSW.WriteAsync(json);
                    oSW.Flush();
                    oSW.Close();
                }

                WebResponse oResponse = await oRequest.GetResponseAsync();

                using (var oSR = new StreamReader(oResponse.GetResponseStream()))
                {
                    string result = await oSR.ReadToEndAsync();
                    result = result.Trim();
                    return new Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel>(true, "Respuesta del servidor obtenida con exito.", JsonConvert.DeserializeObject<NetworkClassLibrary.Models.TaskStatusAnswerModel>(result));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                return new Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel>(false, "Error " + ex.Message.ToLower(), new NetworkClassLibrary.Models.TaskStatusAnswerModel());
            }
        }

        public static async Task<Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel>> SetStatusTask(string url, TaskStatusModel taskStatusModel)
        {
            try
            {
                WebRequest oRequest = WebRequest.Create(url);
                oRequest.Method = "post";
                oRequest.ContentType = "application/json;charset=UTF-8";
                oRequest.UseDefaultCredentials = false;
                oRequest.PreAuthenticate = true;
                oRequest.Credentials = CredentialCache.DefaultNetworkCredentials; //visto en stackoverflow autentification de windows

                using (var oSW = new StreamWriter(await oRequest.GetRequestStreamAsync()))
                {
                    string json = JsonConvert.SerializeObject(taskStatusModel);
                    await oSW.WriteAsync(json);
                    oSW.Flush();
                    oSW.Close();
                }

                WebResponse oResponse = await oRequest.GetResponseAsync();

                using (var oSR = new StreamReader(oResponse.GetResponseStream()))
                {
                    string result = await oSR.ReadToEndAsync();
                    result = result.Trim();
                    return new Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel>(true, "Respuesta del servidor obtenida con exito.", JsonConvert.DeserializeObject<NetworkClassLibrary.Models.TaskStatusAnswerModel>(result));
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                return new Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel>(false, "Error " + ex.Message.ToLower(), new NetworkClassLibrary.Models.TaskStatusAnswerModel());
            }
        }
    }
}
