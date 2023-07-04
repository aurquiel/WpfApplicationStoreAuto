using System.Data;
using NetworkClassLibrary.Models;

namespace IniitalDataClassLibrary
{
    public class InitalData
    {
        private DataTable usersMYSQL = new DataTable();
        private DataTable salePointsMYSQL = new DataTable();
        private NetworkClassLibrary.Models.StoreEmployeeAnswerModel storeEmployee = new NetworkClassLibrary.Models.StoreEmployeeAnswerModel();
        private readonly string ip_web_service = string.Empty;
        private readonly string time_interval_checking_task = string.Empty;
        private readonly string sectors = string.Empty;
        private readonly string genders = string.Empty;
        private readonly string categories = string.Empty;
        private readonly string mysql_time_out = string.Empty;

        public DataTable UsersMYSQL
        {
            get
            {
                return usersMYSQL;
            }
        }

        public DataTable SalePointsMYSQL
        {
            get
            {
                return salePointsMYSQL;
            }
        }

        public StoreEmployeeAnswerModel StoreEmployee
        {
            get
            {
                return storeEmployee;
            }
        }

        public string IP_WEB_SERVICE
        {
            get
            {
                return ip_web_service;
            }
        }

        public string TIME_INTERVAL_CHECKING_TASK
        {
            get
            {
                return time_interval_checking_task;
            }
        }

        public string Sectors
        {
            get
            {
                return sectors;
            }
        }

        public string Genders
        {
            get
            {
                return genders;
            }
        }

        public string Categories
        {
            get
            {
                return categories;
            }
        }

        public string MYSQL_TIME_OUT
        {
            get
            {
                return mysql_time_out;
            }
        }

        public InitalData()
        {

        }

        public InitalData(DataTable usersMYSQL, DataTable salePointsMYSQL, NetworkClassLibrary.Models.StoreEmployeeAnswerModel storeEmployee, string ip_web_service, string time_interval_checking_task, string sectors, string genders, string categories, string mysql_time_out)
        {
            this.usersMYSQL = usersMYSQL;
            this.salePointsMYSQL = salePointsMYSQL;
            this.storeEmployee = storeEmployee;
            this.ip_web_service = ip_web_service;
            this.time_interval_checking_task = time_interval_checking_task;
            this.sectors = sectors;
            this.genders = genders;
            this.categories = categories;
            this.mysql_time_out = mysql_time_out;
        }
    }
}
