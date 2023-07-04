namespace NetworkClassLibrary.Models
{
    public class TaskModel
    {
        public int task_store_id { get; set; }
        public int task_stremp_id { get; set; }
        public int task_status_id { get; set; }
        public string task_token { get; set; }
        public System.DateTime task_date { get; set; }
        public string task_description { get; set; }
        public string task_moderator_message { get; set; }
        public int task_status_local { get; set; }
        public string task_status_local_message { get; set; }
        public int task_audit_id { get; set; }
        public System.DateTime task_audit_date { get; set; }
        public bool task_audit_deleted { get; set; }

        public TaskModel(int task_store_id, int task_stremp_id, int task_status_id, string task_token, System.DateTime task_date, string task_description, string task_moderator_message,
            int task_status_local, string task_status_local_message, int task_audit_id, System.DateTime task_audit_date, bool task_audit_deleted)
        {
            this.task_store_id = task_store_id;
            this.task_stremp_id = task_stremp_id;
            this.task_status_id = task_status_id;
            this.task_token = task_token;
            this.task_date = task_date;
            this.task_description = task_description;
            this.task_moderator_message = task_moderator_message;
            this.task_status_local = task_status_local;
            this.task_status_local_message = task_status_local_message;
            this.task_audit_id = task_audit_id;
            this.task_audit_date = task_audit_date;
            this.task_audit_deleted = task_audit_deleted;
        }
    }
}
