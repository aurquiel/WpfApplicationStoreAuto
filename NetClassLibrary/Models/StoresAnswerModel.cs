using System.Collections.Generic;

namespace NetworkClassLibrary.Models
{
    public class Data2
    {
        public int store_id { get; set; }
        public int store_code { get; set; }
    }

    public class StoresAnswerModel
    {
        public string message { get; set; }
        public List<Data2> data { get; set; }
        public bool statusOperation { get; set; }
    }
}
