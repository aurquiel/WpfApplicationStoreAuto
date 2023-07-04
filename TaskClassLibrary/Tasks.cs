using System.Windows.Controls;

namespace TaskClassLibrary
{
    public class Tasks
    {
        public enum EnumTaskStatusTask { APROBADA = 4, PENDIENTE = 3, CERRADA = 2, DENEGADA = 5, ERROR = 6, NONE = 7};
        public enum EnumTaskStatusLocal { EXITOSA = 1, FALLIDA = 2, NONE = 3};
        public enum EnumTaskGroup { UNBLOCK_USER = 0, UNBLOCK_FISCAL_MACHINE, RESEND_CLOSER, CLOSE_CASH_MACHINE, EQUALS_FISCAL_NETO, UPDATE_AMOUNT_PETTY_CASH_FUND,
            UPDATE_EURO, UPDATE_DOLLAR, DISABLE_ENABLE_DISCOUNT_SAP, UPDATE_AMOUNT_MAX_PER_INVOICE, DISABLE_ENABLE_CLOSE_EMAIL, FAILURE_UPDATE_PRICES_DISCCOUNTS_TOTAL_POS,
            GLOBAL_DISCOUNT, GLOBAL_DISCOUNT_PER_SECTOR, GLOBAL_DISCOUNT_PER_ARTICLE, DISCOUNT_SECTOR_EXCLUDING_ARTICLE, DISABLE_ENABLE_SECTOR_BY_DATE, UPDATE_SECTOR_BY_ARTICLE,
            ARTICLES_DISCOUNTS, UPDATE_GENDER_ARTICLES, UPDATE_CATEGORY_ARTICLES, GLOBAL_DISCOUNT_PER_GENDERS, GLOBAL_DISCOUNT_PER_CATEGORIES, NONE
        };

        private int id;
        private EnumTaskStatusTask status = EnumTaskStatusTask.NONE;
        private EnumTaskGroup group = EnumTaskGroup.NONE;
        private string moderatorMessage;
        private GroupBox groupBox;
        private string token;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public EnumTaskStatusTask Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }

        public EnumTaskGroup Group
        {
            get
            {
                return group;
            }

            set
            {
                group = value;
            }
        }

        public string ModeratorMessage
        {
            get
            {
                return moderatorMessage;
            }
            set
            {
                moderatorMessage = value;
            }
        }

        public GroupBox GroupBox
        {
            get
            {
                return groupBox;
            }

            set
            {
                groupBox = value;
            }
        }

        public string Token
        {
            get
            {
                return token;
            }

            set
            {
                token = value;
            }
        }

        public Tasks()
        {

        }

        public Tasks(int id, string token, string moderatorMessage, EnumTaskStatusTask status, EnumTaskGroup group, GroupBox groupBox)
        {
            this.id = id;
            this.token = token;
            this.moderatorMessage = moderatorMessage;
            this.status = status;
            this.group = group;
            this.groupBox = groupBox;
        }
    }
}
