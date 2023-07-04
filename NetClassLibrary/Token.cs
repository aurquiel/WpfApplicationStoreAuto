using LogLibraryClassLibrary;
using System;

namespace NetworkClassLibrary
{
    public class Token
    {
        public static Tuple<bool, string> IsTokenAlive(string recivedToken)
        {
            try
            {
                if (string.IsNullOrEmpty(recivedToken))
                {
                    return new Tuple<bool, string>(false, "Token vacio o nulo.");
                }
                else
                {
                    byte[] data = Convert.FromBase64String(recivedToken);
                    DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
                    if (when < DateTime.Now)
                    {
                        return new Tuple<bool, string>(false, "Token ha expirado.");
                    }
                    else
                    {
                        return new Tuple<bool, string>(true, "Token valido.");
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                return new Tuple<bool, string>(false, "Error al verificar Token: " + ex.Message.ToLower());
            }
        }
    }
}
