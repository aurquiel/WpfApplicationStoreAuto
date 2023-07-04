using System.Collections.Generic;

namespace NetworkClassLibrary.Models
{

    public class Data
    {
        public int stremp_id { get; set; }
        public string stremp_alias { get; set; }
        public int stremp_store_id { get; set; }
        public int store_code { get; set; }
    }

    public class StoreEmployeeAnswerModel
    {
        public string message { get; set; }
        public List<Data> data { get; set; }
        public bool statusOperation { get; set; }
    }

}