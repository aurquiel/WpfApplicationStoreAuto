using IniitalDataClassLibrary;
using LogLibraryClassLibrary;
using MySQLClassLibrary;
using NetworkClassLibrary;
using System;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WpfApplicationStoreAuto
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly string IP_WEB_SERVICE;
        private readonly string TIME_INTERVAL_CHECKING_TASK;
        private readonly string SECTORS;
        private readonly string GENDERS;
        private readonly string CATEGORIES;
        private readonly string MYSQL_TIME_OUT;

        private NetworkClassLibrary.Models.StoresAnswerModel stores = new NetworkClassLibrary.Models.StoresAnswerModel();
        private NetworkClassLibrary.Models.StoreEmployeeAnswerModel storeEmployee = new NetworkClassLibrary.Models.StoreEmployeeAnswerModel();

        private InitialInfoMYSQL initialInfoMYSQL;
        private DataTable usersMYSQL = new DataTable();
        private DataTable salePointsMYSQL = new DataTable();

        public LoginWindow()
        {
            InitializeComponent();
            Logger.CreateLog();
            Logger.CreateMySQLFile();

            try
            {
                IP_WEB_SERVICE = ConfigurationManager.AppSettings["IP_WEB_SERVICE"].ToString();
            }
            catch(Exception ex)
            {
                if(Logger.LoggerExists())
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                }
                System.Windows.MessageBox.Show("Error al obtener la IP del Servivo Web del app.config, " + ex.Message.ToLower(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                TIME_INTERVAL_CHECKING_TASK = ConfigurationManager.AppSettings["TIME_INTERVAL_CHECKING_TASK"].ToString();
            }
            catch (Exception ex)
            {
                TIME_INTERVAL_CHECKING_TASK = "5000";
                if (Logger.LoggerExists())
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                }
                System.Windows.MessageBox.Show("Error al obtener el tiempo de intervalo del app.config, " + ex.Message.ToLower(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                SECTORS = ConfigurationManager.AppSettings["SECTORS"];
            }
            catch (Exception ex)
            {
                if (Logger.LoggerExists())
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                }
                System.Windows.MessageBox.Show("Error al cargar sectores del App.config, " + ex.Message.ToLower(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                SECTORS = ConfigurationManager.AppSettings["SECTORS"];
            }
            catch (Exception ex)
            {
                if (Logger.LoggerExists())
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                }
                System.Windows.MessageBox.Show("Error al cargar sectores del App.config, " + ex.Message.ToLower(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                GENDERS = ConfigurationManager.AppSettings["GENDERS"];
            }
            catch (Exception ex)
            {
                if (Logger.LoggerExists())
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                }
                System.Windows.MessageBox.Show("Error al cargar generos del App.config, " + ex.Message.ToLower(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                CATEGORIES = ConfigurationManager.AppSettings["CATEGORIES"];
            }
            catch (Exception ex)
            {
                if (Logger.LoggerExists())
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                }
                System.Windows.MessageBox.Show("Error al cargar categorias del App.config, " + ex.Message.ToLower(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            try
            {
                MYSQL_TIME_OUT = ConfigurationManager.AppSettings[" MYSQL_TIME_OUT"];
            }
            catch (Exception ex)
            {
                if (Logger.LoggerExists())
                {
                    Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                }
                MYSQL_TIME_OUT = "5"; //5 segundos by default
                System.Windows.MessageBox.Show("Error al cargar timeout mysql del App.config, " + ex.Message.ToLower(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            initialInfoMYSQL = new InitialInfoMYSQL(MYSQL_TIME_OUT);
            Load();
        }

        private async void Load()
        {
            await LoadInitialData();
        }

        private async Task LoadStores()
        {
            try
            {
                buttonLogin.IsEnabled = false;
                buttonLogin.ToolTip= "Boton deshabilitado";
                TextBoxStatusUpdate("Espere obteniendo tiendas.", Brushes.Green);
                Tuple<bool, string, NetworkClassLibrary.Models.StoresAnswerModel> result = await NetworkClassLibrary.Get.GetStores(IP_WEB_SERVICE + "/api/Store/GetStores");
                stores = result.Item3;
                if (result.Item1)
                {
                    if (stores.statusOperation)
                    {
                        LoadComboboxStores();
                        UpdateUiFromLoadStore(new Tuple<bool, string>(true, stores.message));
                    }
                    else
                    {
                        UpdateUiFromLoadStore(new Tuple<bool, string>(false, stores.message));
                    }
                }
                else
                {
                    UpdateUiFromLoadStore(new Tuple<bool, string>(false, result.Item2));
                }
            }
            catch(Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                UpdateUiFromLoadStore(new Tuple<bool, string>(false, "Error al cargar tiendas, " + ex.Message.ToLower()));
            }
        }

        private async Task LoadInitialData()
        {
            this.usersMYSQL = new DataTable();
            Tuple<bool, string> result = await initialInfoMYSQL.GetUsers(this.usersMYSQL);

            if(result.Item1)
            {
                UpdateUiFromLoadStore(new Tuple<bool, string>(true, result.Item2));
                this.salePointsMYSQL = new DataTable();
                Tuple<bool, string> result2 = await initialInfoMYSQL.GetSalePoints(this.salePointsMYSQL);

                if(result2.Item1)
                {
                    UpdateUiFromLoadStore(new Tuple<bool, string>(true, result2.Item2));
                    await LoadStores();
                }
                else
                {
                    UpdateUiFromLoadStore(new Tuple<bool, string>(false, result2.Item2));
                }
            }
            else
            {
                UpdateUiFromLoadStore(new Tuple<bool, string>(false, result.Item2));
            }
        }

        private void UpdateUiFromLoadStore(Tuple<bool, string> result)
        {
            if(result.Item1)
            {
                TextBoxStatusUpdate(result.Item2, Brushes.Green);
                buttonLogin.ToolTip = "Entrar a la aplicación";
                buttonLogin.IsEnabled = true;
            }
            else
            {
                TextBoxStatusUpdate(result.Item2, Brushes.Red);
                buttonLogin.ToolTip = "Boton deshabilitado";
                buttonLogin.IsEnabled = false;
            }
        }

        private void LoadComboboxStores()
        {
            comboBoxStores.Items.Clear();
            foreach (NetworkClassLibrary.Models.Data2 store in stores.data)
            {
                comboBoxStores.Items.Add(store.store_code);
            } 
        }

        private async void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                buttonLogin.IsEnabled = false;
                if(string.IsNullOrWhiteSpace(textBoxUser.Text))
                {
                    TextBoxStatusUpdate("Error nombre de ususario vacio.", Brushes.Red);
                }
                else if(comboBoxStores.SelectedIndex == -1)
                {
                    TextBoxStatusUpdate("Error debe seleccionar una tienda.", Brushes.Red);
                }
                else
                {
                    await Task.Run(() => TextBoxStatusUpdate("Entrando al sistema espere.", Brushes.Green));
                    Tuple<bool, string, NetworkClassLibrary.Models.StoreEmployeeAnswerModel> result = await Post.LoginUserStore(IP_WEB_SERVICE + "/api/Access/login", textBoxUser.Text, comboBoxStores.SelectedValue.ToString());
                    storeEmployee = result.Item3;
                    if (result.Item1)
                    {
                        if (storeEmployee.statusOperation)
                        {
                            this.Hide();
                            textBoxUser.Text = string.Empty;
                            TextBoxStatusUpdate(string.Empty, Brushes.White);
                            comboBoxStores.SelectedIndex = -1;
                            MainWindow mainForm = new MainWindow(new InitalData(this.usersMYSQL, this.salePointsMYSQL, storeEmployee, IP_WEB_SERVICE, TIME_INTERVAL_CHECKING_TASK, SECTORS, GENDERS, CATEGORIES, MYSQL_TIME_OUT));
                            mainForm.Closed += (s, args) => this.Show();
                            mainForm.ShowDialog();
                            await LoadInitialData(); 
                        }
                        else
                        {
                            TextBoxStatusUpdate(storeEmployee.message, Brushes.Red);
                        }
                    }
                    else
                    {
                        TextBoxStatusUpdate(result.Item2, Brushes.Red);
                    }
                }
                buttonLogin.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                buttonLogin.IsEnabled = true;
                TextBoxStatusUpdate("Error: " +ex.Message.ToLower(), Brushes.Red);
            }
        }

        private void TextBoxStatusUpdate(string text, Brush colorBrush)
        {
            textBoxStatus.Dispatcher.Invoke(() =>
            {
                textBoxStatus.Foreground = colorBrush;
                textBoxStatus.Text = text;
            });
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void buttonRefreshStores_Click(object sender, RoutedEventArgs e)
        {
            await LoadInitialData();
        }

    }
}
