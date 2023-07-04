using LogLibraryClassLibrary;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NetworkClassLibrary
{
    public class Get
    {
        public static async Task<Tuple<bool, string, NetworkClassLibrary.Models.StoresAnswerModel>> GetStores(string url)
        {
            try
            {
                WebRequest oRequest = WebRequest.Create(url);
                oRequest.Method = "get";
                oRequest.UseDefaultCredentials = false;
                oRequest.PreAuthenticate = true;
                oRequest.Credentials = CredentialCache.DefaultNetworkCredentials;

                WebResponse oResponse = await oRequest.GetResponseAsync();

                using (var oSR = new StreamReader(oResponse.GetResponseStream()))
                {
                    string result = await oSR.ReadToEndAsync();
                    result = result.Trim();
                    return new Tuple<bool, string, NetworkClassLibrary.Models.StoresAnswerModel>(true, "Respuesta del servidor obtenida con exito." , JsonConvert.DeserializeObject<NetworkClassLibrary.Models.StoresAnswerModel>(result));
                }
            }
            catch(Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                return new Tuple<bool, string, NetworkClassLibrary.Models.StoresAnswerModel>(false, "Error " + ex.Message.ToLower(), new NetworkClassLibrary.Models.StoresAnswerModel());
            }
        }
    }
}
