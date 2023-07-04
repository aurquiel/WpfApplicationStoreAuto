using System.Collections.Generic;

namespace NetworkClassLibrary.Models
{
    public class Data3
    {
        public int task_status_id { get; set; }
        public string task_token { get; set; }
        public string task_moderator_message { get; set; }
    }

    public class TaskStatusAnswerModel
    {
        public string message { get; set; }
        public List<Data3> data { get; set; }
        public bool statusOperation { get; set; }
    }
}
