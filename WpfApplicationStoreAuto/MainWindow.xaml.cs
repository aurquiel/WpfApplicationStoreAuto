using IniitalDataClassLibrary;
using LogLibraryClassLibrary;
using System;
using System.Data;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using TaskClassLibrary;
using System.ComponentModel;
using System.Threading;
using FilesManagerClassLibrary;
using System.Threading.Tasks;

namespace WpfApplicationStoreAuto
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private enum OPERATIONS_ADD_SUBTRAC { ADD = 0, SUBTRAC };
        private enum ENABLE_DISABLE_COMBOBOX_OPTIONS { DESHABILITAR = 0, HABILITAR = 1 };

        private InitalData initialData = new InitalData();
        private NotifyIcon notificationIcon = new NotifyIcon();

        private MySQLClassLibrary.ExcecuteTaskMySQL mysql;

        private readonly BackgroundWorker backgroundWorker = new BackgroundWorker();

        private readonly string MODERATOR_MESSAGE_EMPTY = "----------";

        public MainWindow(InitalData initialData)
        {
            InitializeComponent();
            this.initialData = initialData;
            mysql = new MySQLClassLibrary.ExcecuteTaskMySQL(initialData.MYSQL_TIME_OUT);
            LoadUnblockUser(initialData.UsersMYSQL);
            LoadSalePoints(initialData.SalePointsMYSQL);
            LoadEnableDisableComboBox();
            LoadQuickNavigation();
            LoadSectors();
            LoadGenders();
            LoadCategories();
            backgroundWorker.DoWork += worker_DoWork;
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.ProgressChanged += worker_ProgressChanged;
            backgroundWorker.RunWorkerAsync();
        }

        private void LoadUnblockUser(DataTable data)
        {
            comboBoxUnblockUser.Items.Clear();
            foreach (DataRow row in data.Rows)
            {
                if (row[0].ToString().Contains("caja") || row[0].ToString().Contains("encargado") || row[0].ToString().Contains("captahuellas") || row[0].ToString().Contains("regente"))
                {
                    comboBoxUnblockUser.Items.Add(row[0].ToString());
                }
            }
        }

        private void LoadSalePoints(DataTable data)
        {
            comboBoxCloseCashMachine.Items.Clear();
            comboBoxUnblockFiscalMachine.Items.Clear();
            comboBoxEqualsFiscalNeto.Items.Clear();
            foreach (DataRow row in data.Rows)
            {
                comboBoxCloseCashMachine.Items.Add(new MyDataComboBox(row[0].ToString(), row[1].ToString()));
                comboBoxUnblockFiscalMachine.Items.Add(new MyDataComboBox(row[0].ToString(), row[1].ToString()));
                comboBoxEqualsFiscalNeto.Items.Add(new MyDataComboBox(row[0].ToString(), row[1].ToString()));
            }
        }

        private void LoadSectors()
        {
            try
            {
                string[] sectors = initialData.Sectors.Split(',');
                listBoxGlobalDiscountPerSector.Items.Clear();
                listBoxDiscountSectorExcludingArticle.Items.Clear();
                foreach (string sector in sectors)
                {
                    listBoxGlobalDiscountPerSector.Items.Add(sector);
                    listBoxDiscountSectorExcludingArticle.Items.Add(sector);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                System.Windows.MessageBox.Show("Error al cargar sectores listbox." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadGenders()
        {
            try
            {
                string[] genders = initialData.Genders.Split(',');
                listBoxGlobalDiscountPerGender.Items.Clear();
                foreach (string gender in genders)
                {
                    listBoxGlobalDiscountPerGender.Items.Add(gender);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                System.Windows.MessageBox.Show("Error al cargar generos listbox." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listBoxGlobalDiscountPerGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxNumberSelectedSectorsGlobalDiscountPerGender.Text = "Generos Seleccionados: " + GetSelectedItemsOfListBox(((System.Windows.Controls.ListBox)sender));
        }

        private void LoadCategories()
        {
            try
            {
                string[] categories = initialData.Categories.Split(',');
                listBoxGlobalDiscountPerCategories.Items.Clear();
                foreach (string gender in categories)
                {
                    listBoxGlobalDiscountPerCategories.Items.Add(gender);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                System.Windows.MessageBox.Show("Error al cargar categorias listbox." + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void listBoxGlobalDiscountPerCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxNumberSelectedSectorsGlobalDiscountPerCategories.Text = "Categorias Seleccionadas: " + GetSelectedItemsOfListBox(((System.Windows.Controls.ListBox)sender));
        }

        private void LoadEnableDisableComboBox()
        {
            comboBoxDisableEnableDiscountSap.Items.Clear();
            comboBoxDisableEnableDiscountSap.Items.Add(new MyDataComboBox(((int)ENABLE_DISABLE_COMBOBOX_OPTIONS.HABILITAR).ToString(), ENABLE_DISABLE_COMBOBOX_OPTIONS.HABILITAR.ToString()));
            comboBoxDisableEnableDiscountSap.Items.Add(new MyDataComboBox(((int)ENABLE_DISABLE_COMBOBOX_OPTIONS.DESHABILITAR).ToString(), ENABLE_DISABLE_COMBOBOX_OPTIONS.DESHABILITAR.ToString()));
            comboBoxDisableEnableCloseEmail.Items.Clear();
            comboBoxDisableEnableCloseEmail.Items.Add(new MyDataComboBox(((int)ENABLE_DISABLE_COMBOBOX_OPTIONS.HABILITAR).ToString(), ENABLE_DISABLE_COMBOBOX_OPTIONS.HABILITAR.ToString()));
            comboBoxDisableEnableCloseEmail.Items.Add(new MyDataComboBox(((int)ENABLE_DISABLE_COMBOBOX_OPTIONS.DESHABILITAR).ToString(), ENABLE_DISABLE_COMBOBOX_OPTIONS.DESHABILITAR.ToString()));
            comboBoxDisableEnableSectorByDate.Items.Clear();
            comboBoxDisableEnableSectorByDate.Items.Add(new MyDataComboBox(((int)ENABLE_DISABLE_COMBOBOX_OPTIONS.HABILITAR).ToString(), ENABLE_DISABLE_COMBOBOX_OPTIONS.HABILITAR.ToString()));
            comboBoxDisableEnableSectorByDate.Items.Add(new MyDataComboBox(((int)ENABLE_DISABLE_COMBOBOX_OPTIONS.DESHABILITAR).ToString(), ENABLE_DISABLE_COMBOBOX_OPTIONS.DESHABILITAR.ToString()));
        }

        private void listBoxGlobalDiscountPerSector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxNumberSelectedSectorsGlobalDiscountPerSector.Text = "Sectores Seleccionados: " + GetSelectedItemsOfListBox(((System.Windows.Controls.ListBox)sender));
        }

        private string GetSelectedItemsOfListBox(System.Windows.Controls.ListBox listBox)
        {
            string itemsSelected = string.Empty;
            foreach (object item in listBox.SelectedItems)
            {
                itemsSelected += item.ToString() + ",";
            }
            return itemsSelected.TrimEnd(',');
        }

        private string GetSelectedItemsOfListBoxWhitQuotes(System.Windows.Controls.ListBox listBox)
        {
            string itemsSelected = string.Empty;
            foreach (object item in listBox.SelectedItems)
            {
                itemsSelected += "'" + item.ToString() + "'" + ",";
            }
            return itemsSelected.TrimEnd(',');
        }

        private void listBoxDiscountSectorExcludingArticle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxNumberSelectedSectorsDiscountSectorExcludingArticle.Text = "Sectores Seleccionados: " + GetSelectedItemsOfListBox(((System.Windows.Controls.ListBox)sender));
        }

        private void LoadQuickNavigation()
        {
            comboBoxQuickNavigation.Items.Clear();
            foreach (var grupBox in stackPanel.Children)
            {
                comboBoxQuickNavigation.Items.Add(new MyDataComboBox(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)((System.Windows.Controls.GroupBox)grupBox).Header).Children[0]).Tag.ToString(), ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)((System.Windows.Controls.GroupBox)grupBox).Header).Children[0]).Text));
            }
        }

        private void QuickNavigationAproved(MyDataComboBox newData, OPERATIONS_ADD_SUBTRAC operarion)
        {
            if(operarion == OPERATIONS_ADD_SUBTRAC.ADD)
            {
                comboBoxQuickNavigationAprove.Items.Add(newData);
            }
            else
            {
                int i = 0;
                foreach (MyDataComboBox item in comboBoxQuickNavigationAprove.Items)
                {
                    if (item.DisplayValue == newData.DisplayValue)
                    {
                        comboBoxQuickNavigationAprove.Items.RemoveAt(i);
                        break;
                    }
                    i++;
                }
            }
        }

        private void QuickNavigationDenied(MyDataComboBox newData, OPERATIONS_ADD_SUBTRAC operarion)
        {
            if (operarion == OPERATIONS_ADD_SUBTRAC.ADD)
            {
                comboBoxQuickNavigationrDenied.Items.Add(newData);
            }
            else
            {
                int i = 0;
                foreach (MyDataComboBox item in comboBoxQuickNavigationrDenied.Items)
                {
                    if (item.DisplayValue == newData.DisplayValue)
                    {
                        comboBoxQuickNavigationrDenied.Items.RemoveAt(i);
                        break;
                    }
                    i++;
                }
            }
        }

        private System.Windows.Controls.GroupBox GetGruopBoxQuickNavigation(string tag)
        {
            foreach (var groupBox in stackPanel.Children)
            {
                if(((System.Windows.Controls.GroupBox)groupBox).Tag.ToString() == tag)
                {
                    return (System.Windows.Controls.GroupBox)groupBox;
                }
            }
            return new System.Windows.Controls.GroupBox();
        }

        private void comboBoxQuickNavigation_DropDownClosed(object sender, EventArgs e)
        {
            if (((System.Windows.Controls.ComboBox)sender).SelectedItem != null)
            {
                string item = ((MyDataComboBox)((System.Windows.Controls.ComboBox)sender).SelectedItem).Value.ToString();
                ScrollToGroupBox(GetGruopBoxQuickNavigation(item));
            }
        }

        private void comboBoxQuickNavigationAprove_DropDownClosed(object sender, EventArgs e)
        {
            if(((System.Windows.Controls.ComboBox)sender).SelectedItem != null)
            {
                string item = ((MyDataComboBox)((System.Windows.Controls.ComboBox)sender).SelectedItem).Value.ToString();
                ScrollToGroupBox(GetGruopBoxQuickNavigation(item));
            }
        }

        private void comboBoxQuickNavigationrDenied_DropDownClosed(object sender, EventArgs e)
        {
            if (((System.Windows.Controls.ComboBox)sender).SelectedItem != null)
            {
                string item = ((MyDataComboBox)((System.Windows.Controls.ComboBox)sender).SelectedItem).Value.ToString();
                ScrollToGroupBox(GetGruopBoxQuickNavigation(item));
            }
        }

        private void RepaintGroupBoxes()
        {
            int i = 0;
            foreach(System.Windows.Controls.GroupBox groupBox in stackPanel.Children)
            {
                if (i % 2 == 0)//par
                {
                    ((StackPanel)groupBox.Content).Background = System.Windows.Media.Brushes.LightBlue;
                }
                else//impar
                {
                    ((StackPanel)groupBox.Content).Background = System.Windows.Media.Brushes.White;
                }
                i++;
            }
        }

        private bool scrollControl = false;

        private void ScrollToGroupBox(System.Windows.Controls.GroupBox groupBox)
        {
            scrollControl = true;
            scrollViewer.ScrollChanged -= scrollViewer_ScrollChanged;
            RepaintGroupBoxes();
            scrollViewer.ScrollToEnd();
            groupBox.BringIntoView();
            ((StackPanel)groupBox.Content).Background = System.Windows.Media.Brushes.Moccasin;
            scrollViewer.ScrollChanged += scrollViewer_ScrollChanged;
        }

        private void scrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(scrollControl)
            {
                scrollControl = false;
            }
            else
            {
                RepaintGroupBoxes();
            }
        }

        private void ShowNotification(string taskName, Tasks.EnumTaskStatusTask tasKStatus)
        {
            notificationIcon.Icon = SystemIcons.Information;
            notificationIcon.BalloonTipTitle = "Cambio de estatus Tarea";
            notificationIcon.BalloonTipText = "Tarea " + taskName + ", estado: " + tasKStatus.ToString();
            if (tasKStatus == Tasks.EnumTaskStatusTask.APROBADA || tasKStatus == Tasks.EnumTaskStatusTask.PENDIENTE)
            {
                notificationIcon.BalloonTipIcon = ToolTipIcon.Info;
            }
            else
            {
                notificationIcon.BalloonTipIcon = ToolTipIcon.Error;
            }           
            notificationIcon.Visible = true;
            notificationIcon.ShowBalloonTip(500000);
        }

        private void upDownUpdate_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                switch (e.Key)
                {
                    case Key.OemComma:
                    case Key.D0:
                    case Key.NumPad0:
                    case Key.D1:
                    case Key.NumPad1:
                    case Key.D2:
                    case Key.NumPad2:
                    case Key.D3:
                    case Key.NumPad3:
                    case Key.D4:
                    case Key.NumPad4:
                    case Key.D5:
                    case Key.NumPad5:
                    case Key.D6:
                    case Key.NumPad6:
                    case Key.D7:
                    case Key.NumPad7:
                    case Key.D8:
                    case Key.NumPad8:
                    case Key.D9:
                    case Key.NumPad9:
                        e.Handled = false;
                        break;
                    default:

                        e.Handled = true;
                        break;
                }
            }
            catch
            {
                ((Xceed.Wpf.Toolkit.DecimalUpDown)sender).CommitInput();
                e.Handled = true;
            }
        }

        private void UpdateLabelStatus(System.Windows.Controls.Label label, Tasks.EnumTaskStatusTask status, System.Windows.Media.Brush color)
        {
            label.Content = status.ToString();
            label.Foreground = color;
        }

        private void UpdateLabelStatusModeratorMessage(System.Windows.Controls.Label label, string text)
        {
            label.Content = text;
        }

        private void UpdateTabCounter(Tasks.EnumTaskStatusTask status, OPERATIONS_ADD_SUBTRAC option)
        {
            if(option == OPERATIONS_ADD_SUBTRAC.ADD)
            {
                if (status == Tasks.EnumTaskStatusTask.PENDIENTE)
                {
                    int counter = Int32.Parse(tabNumberPendingTask.Text.Trim('(').Trim(')'));
                    counter++;
                    tabNumberPendingTask.Text = "(" + counter.ToString() + ")";
                }
                else if (status == Tasks.EnumTaskStatusTask.APROBADA)
                {
                    int counter = Int32.Parse(tabNumbeAprovedTask.Text.Trim('(').Trim(')'));
                    counter++;
                    tabNumbeAprovedTask.Text = "(" + counter.ToString() + ")";
                }
                else if (status == Tasks.EnumTaskStatusTask.DENEGADA)
                {
                    int counter = Int32.Parse(tabNumberDeniedTask.Text.Trim('(').Trim(')'));
                    counter++;
                    tabNumberDeniedTask.Text = "(" + counter.ToString() + ")";
                }
            }
            else if (option == OPERATIONS_ADD_SUBTRAC.SUBTRAC)
            {
                if (status == Tasks.EnumTaskStatusTask.PENDIENTE)
                {
                    int counter = Int32.Parse(tabNumberPendingTask.Text.Trim('(').Trim(')'));
                    counter--;
                    tabNumberPendingTask.Text = "(" + counter.ToString() + ")";
                }
                else if (status == Tasks.EnumTaskStatusTask.APROBADA)
                {
                    int counter = Int32.Parse(tabNumbeAprovedTask.Text.Trim('(').Trim(')'));
                    counter--;
                    tabNumbeAprovedTask.Text = "(" + counter.ToString() + ")";
                }
                else if (status == Tasks.EnumTaskStatusTask.DENEGADA)
                {
                    int counter = Int32.Parse(tabNumberDeniedTask.Text.Trim('(').Trim(')'));
                    counter--;
                    tabNumberDeniedTask.Text = "(" + counter.ToString() + ")";
                }
            }
        }

        private string AddDecimalsToQuantity(string value)
        {
            if (value.Contains("."))
            {
                return value;
            }
            else
            {
                return value + ".00";
            }
        }

        // walk up the visual tree to find object of type T, starting from initial object
        public static T FindUpVisualTree<T>(DependencyObject initial) where T : DependencyObject
        {
            DependencyObject current = initial;

            while (current != null && current.GetType() != typeof(T))
            {
                current = System.Windows.Media.VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

        private async void buttonSolicitudeUnblock_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxUnblockUser.SelectedIndex != -1)
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = false;
                comboBoxUnblockUser.IsEnabled = false;
                System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Usuario: " + comboBoxUnblockUser.SelectedValue.ToString();
                NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                if (result.Item1 && result.Item3.statusOperation)
                {
                    if ((string)labelStatusUnblock.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                    {
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    }
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                    UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                    UpdateLabelStatusModeratorMessage(labelMessageUnblock, MODERATOR_MESSAGE_EMPTY);
                    ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UNBLOCK_USER, groupBoxUpper));
                    ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = true;
                    comboBoxUnblockUser.IsEnabled = true;
                    comboBoxUnblockUser.SelectedIndex = -1;
                    UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    UpdateLabelStatusModeratorMessage(labelMessageUnblock, MODERATOR_MESSAGE_EMPTY);
                    if (result.Item3.statusOperation == false)
                    {
                        System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un elemento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void buttonSolicitudeUnblockFiscalMachine_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxUnblockFiscalMachine.SelectedIndex != -1)
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = false;
                comboBoxUnblockFiscalMachine.IsEnabled = false;
                System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Caja: " + ((MyDataComboBox)comboBoxUnblockFiscalMachine.SelectedValue).DisplayValue.ToString();
                NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                if (result.Item1 && result.Item3.statusOperation)
                {
                    if ((string)labelStatusUnblockFiscalMachine.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                    {
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    }
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                    UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                    UpdateLabelStatusModeratorMessage(labelMessageUnblockFiscalMachine, MODERATOR_MESSAGE_EMPTY);
                    ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UNBLOCK_FISCAL_MACHINE, groupBoxUpper));
                    ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = true;
                    comboBoxUnblockFiscalMachine.IsEnabled = true;
                    comboBoxUnblockFiscalMachine.SelectedIndex = -1;
                    UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    UpdateLabelStatusModeratorMessage(labelMessageUnblockFiscalMachine, MODERATOR_MESSAGE_EMPTY);
                    if (result.Item3.statusOperation == false)
                    {
                        System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un elemento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void buttonSolicitudeResendCloser_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            datePickerResendCloser.IsEnabled = false;
            System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
            string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Fecha: " + datePickerResendCloser.SelectedDate.Value.ToString("yyyy-MM-dd");
            NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

            Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
            if (result.Item1 && result.Item3.statusOperation)
            {
                if ((string)labelStatusResendCloser.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                {
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                }
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                UpdateLabelStatusModeratorMessage(labelMessageResendCloser, MODERATOR_MESSAGE_EMPTY);
                ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.RESEND_CLOSER, groupBoxUpper));
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
            }
            else
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = true;
                datePickerResendCloser.IsEnabled = true;
                UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageResendCloser, MODERATOR_MESSAGE_EMPTY);
                if (result.Item3.statusOperation == false)
                {
                    System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void buttonSolicitudeCloseCashMachine_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxCloseCashMachine.SelectedIndex != -1)
            {
                Tuple<bool, string> existRegister = await mysql.CloseCashMachineIfExistRegister(datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).Value.ToString());

                if(existRegister.Item1)
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    comboBoxCloseCashMachine.IsEnabled = false;
                    datePickerCloseCashMachine.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Caja: " + ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).DisplayValue + ". Fecha: " + datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd");
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                                  string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result.Item1 && result.Item3.statusOperation)
                    {
                        if ((string)labelStatusCloseCashMachine.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageCloseCashMachine, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.CLOSE_CASH_MACHINE, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        comboBoxCloseCashMachine.IsEnabled = true;
                        datePickerCloseCashMachine.IsEnabled = true;
                        UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageCloseCashMachine, MODERATOR_MESSAGE_EMPTY);
                        if (result.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(existRegister.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un elemento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void buttonSolicitudeEqualsFiscalNeto_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxEqualsFiscalNeto.SelectedIndex != -1)
            {
                Tuple<bool, string> existRegister = await mysql.CloseCashMachineIfExistRegister(datePickerEqualsFiscalNeto.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxEqualsFiscalNeto.SelectedItem).Value.ToString());

                if (existRegister.Item1)
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    comboBoxEqualsFiscalNeto.IsEnabled = false;
                    datePickerEqualsFiscalNeto.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Caja: " + ((MyDataComboBox)comboBoxEqualsFiscalNeto.SelectedItem).DisplayValue + ". Fecha: " + datePickerEqualsFiscalNeto.SelectedDate.Value.ToString("yyyy-MM-dd");
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                                    string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result.Item1 && result.Item3.statusOperation)
                    {
                        if ((string)labelStatusEqualsFiscalNeto.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageEqualsFiscalNeto, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.EQUALS_FISCAL_NETO, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        comboBoxEqualsFiscalNeto.IsEnabled = true;
                        datePickerEqualsFiscalNeto.IsEnabled = true;
                        UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageEqualsFiscalNeto, MODERATOR_MESSAGE_EMPTY);
                        if (result.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(existRegister.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un elemento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void buttonSolicitudeUpdateAmountPettyCashFund_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            decimalUpDownUpdateAmountPettyCashFund.IsEnabled = false;
            System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
            string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + AddDecimalsToQuantity(decimalUpDownUpdateAmountPettyCashFund.Value.ToString().Replace(',', '.'));
            NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

            Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
            if (result.Item1 && result.Item3.statusOperation)
            {
                if ((string)labelStatusUpdateAmountPettyCashFund.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                {
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                }
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountPettyCashFund, MODERATOR_MESSAGE_EMPTY);
                ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UPDATE_AMOUNT_PETTY_CASH_FUND, groupBoxUpper));
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
            }
            else
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = true;
                decimalUpDownUpdateAmountPettyCashFund.IsEnabled = true;
                UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountPettyCashFund, MODERATOR_MESSAGE_EMPTY);
                if (result.Item3.statusOperation == false)
                {
                    System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void buttonSolicitudeUpdateEuro_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            decimalUpDownUpdateEuro.IsEnabled = false;
            System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
            string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + AddDecimalsToQuantity(decimalUpDownUpdateEuro.Value.ToString().Replace(',', '.'));
            NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

            Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
            if (result.Item1 && result.Item3.statusOperation)
            {
                if ((string)labelStatusUpdateEuro.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                {
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                }
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateEuro, MODERATOR_MESSAGE_EMPTY);
                ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UPDATE_EURO, groupBoxUpper));
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
            }
            else
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = true;
                decimalUpDownUpdateEuro.IsEnabled = true;
                UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateEuro, MODERATOR_MESSAGE_EMPTY);
                if (result.Item3.statusOperation == false)
                {
                    System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void buttonSolicitudeUpdateDollar_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            decimalUpDownUpdateDollar.IsEnabled = false;
            System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
            string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + AddDecimalsToQuantity(decimalUpDownUpdateDollar.Value.ToString().Replace(',', '.'));
            NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

            Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
            if (result.Item1 && result.Item3.statusOperation)
            {
                if ((string)labelStatusUpdateDollar.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                {
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                }
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateDollar, MODERATOR_MESSAGE_EMPTY);
                ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UPDATE_DOLLAR, groupBoxUpper));
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
            }
            else
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = true;
                decimalUpDownUpdateDollar.IsEnabled = true;
                UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateDollar, MODERATOR_MESSAGE_EMPTY);
                if (result.Item3.statusOperation == false)
                {
                    System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void buttonSolicitudeDisableEnableDiscountSap_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxDisableEnableDiscountSap.SelectedIndex != -1)
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = false;
                comboBoxDisableEnableDiscountSap.IsEnabled = false;
                System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Opcion: " + ((MyDataComboBox)comboBoxDisableEnableDiscountSap.SelectedValue).DisplayValue.ToString();
                NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                if (result.Item1 && result.Item3.statusOperation)
                {
                    if ((string)labelStatusDisableEnableDiscountSap.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                    {
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    }
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                    UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                    UpdateLabelStatusModeratorMessage(labelMessageDisableEnableDiscountSap, MODERATOR_MESSAGE_EMPTY);
                    ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.DISABLE_ENABLE_DISCOUNT_SAP, groupBoxUpper));
                    ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = true;
                    comboBoxDisableEnableDiscountSap.IsEnabled = true;
                    comboBoxDisableEnableDiscountSap.SelectedIndex = -1;
                    UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    UpdateLabelStatusModeratorMessage(labelMessageDisableEnableDiscountSap, MODERATOR_MESSAGE_EMPTY);
                    if (result.Item3.statusOperation == false)
                    {
                        System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar una accion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void buttonSolicitudeUpdateAmountMaxPerInvoice_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            decimalUpDownUpdateAmountMaxPerInvoice.IsEnabled = false;
            System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
            string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + AddDecimalsToQuantity(decimalUpDownUpdateAmountMaxPerInvoice.Value.ToString().Replace(',', '.'));
            NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

            Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
            if (result.Item1 && result.Item3.statusOperation)
            {
                if ((string)labelStatusUpdateAmountMaxPerInvoice.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                {
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                }
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountMaxPerInvoice, MODERATOR_MESSAGE_EMPTY);
                ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UPDATE_AMOUNT_MAX_PER_INVOICE, groupBoxUpper));
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
            }
            else
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = true;
                decimalUpDownUpdateAmountMaxPerInvoice.IsEnabled = true;
                UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountMaxPerInvoice, MODERATOR_MESSAGE_EMPTY);
                if (result.Item3.statusOperation == false)
                {
                    System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void buttonSolicitudeDisableEnableCloseEmail_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxDisableEnableCloseEmail.SelectedIndex != -1)
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = false;
                comboBoxDisableEnableCloseEmail.IsEnabled = false;
                System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Opcion:" + ((MyDataComboBox)comboBoxDisableEnableCloseEmail.SelectedValue).DisplayValue.ToString();
                NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                if (result.Item1 && result.Item3.statusOperation)
                {
                    if ((string)labelStatusDisableEnableCloseEmail.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                    {
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    }
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                    UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                    UpdateLabelStatusModeratorMessage(labelMessageDisableEnableCloseEmail, MODERATOR_MESSAGE_EMPTY);
                    ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.DISABLE_ENABLE_CLOSE_EMAIL, groupBoxUpper));
                    ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = true;
                    comboBoxDisableEnableCloseEmail.IsEnabled = true;
                    comboBoxDisableEnableCloseEmail.SelectedIndex = -1;
                    UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    UpdateLabelStatusModeratorMessage(labelMessageDisableEnableCloseEmail, MODERATOR_MESSAGE_EMPTY);
                    if (result.Item3.statusOperation == false)
                    {
                        System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar una accion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void buttonSolicitudeFailureUpdatePricesDisccountsTotalPos_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            integerUpDownFailureUpdatePricesDisccountsTotalPos.IsEnabled = false;
            System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
            string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + integerUpDownFailureUpdatePricesDisccountsTotalPos.Value;
            NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

            Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
            if (result.Item1 && result.Item3.statusOperation)
            {
                if ((string)labelStatusFailureUpdatePricesDisccountsTotalPos.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                {
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                }
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                UpdateLabelStatusModeratorMessage(labelMessageFailureUpdatePricesDisccountsTotalPos, MODERATOR_MESSAGE_EMPTY);
                ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.FAILURE_UPDATE_PRICES_DISCCOUNTS_TOTAL_POS, groupBoxUpper));
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
            }
            else
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = true;
                integerUpDownFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
                UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageFailureUpdatePricesDisccountsTotalPos, MODERATOR_MESSAGE_EMPTY);
                if (result.Item3.statusOperation == false)
                {
                    System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void buttonSolicitudeGlobalDiscount_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            decimalUpDownGlobalDiscount.IsEnabled = false;
            System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
            string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + AddDecimalsToQuantity(decimalUpDownGlobalDiscount.Value.ToString().Replace(',', '.'));
            NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

            Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
            if (result.Item1 && result.Item3.statusOperation)
            {
                if ((string)labelStatusGlobalDiscount.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                {
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                }
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscount, MODERATOR_MESSAGE_EMPTY);
                ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.GLOBAL_DISCOUNT, groupBoxUpper));
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
            }
            else
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = true;
                decimalUpDownGlobalDiscount.IsEnabled = true;
                UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscount, MODERATOR_MESSAGE_EMPTY);
                if (result.Item3.statusOperation == false)
                {
                    System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void buttonSolicitudeGlobalDiscountPerSector_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxGlobalDiscountPerSector.SelectedItems.Count != 0)
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = false;
                decimalUpDownGlobalDiscountPerSector.IsEnabled = false;
                listBoxGlobalDiscountPerSector.IsEnabled = false;
                System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + AddDecimalsToQuantity(decimalUpDownGlobalDiscountPerSector.Value.ToString().Replace(',', '.')) + ". Sectores: " + GetSelectedItemsOfListBox(listBoxGlobalDiscountPerSector);
                NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                if (result.Item1 && result.Item3.statusOperation)
                {
                    if ((string)labelStatusGlobalDiscountPerSector.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                    {
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    }
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                    UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                    UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerSector, MODERATOR_MESSAGE_EMPTY);
                    ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_SECTOR, groupBoxUpper));
                    ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = true;
                    decimalUpDownGlobalDiscountPerSector.IsEnabled = true;
                    listBoxGlobalDiscountPerSector.IsEnabled = true;
                    listBoxGlobalDiscountPerSector.UnselectAll();
                    UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerSector, MODERATOR_MESSAGE_EMPTY);
                    if (result.Item3.statusOperation == false)
                    {
                        System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar por lo menos un sector.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonUploadTextGlobalDiscountPerArticle_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = FilesManager.GetPathFileDialog();
            if (!string.IsNullOrEmpty(pathFile))
            {
                textBoxUploadTextGlobalDiscountPerArticle.Text = pathFile;
            }
        }

        private async void buttonSolicitudeGlobalDiscountPerArticle_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUploadTextGlobalDiscountPerArticle.Text))
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Tuple<bool, string> result = FilesManager.GetArticlesFromFileArticleByLine(textBoxUploadTextGlobalDiscountPerArticle.Text, FilesManager.ListOfArticlesGlobalDiscountPerArticle);

                if (result.Item1 == false)
                {
                    textBoxUploadTextGlobalDiscountPerArticle.Text = string.Empty;
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    buttonUploadTextGlobalDiscountPerArticle.IsEnabled = false;
                    decimalUpDownGlobalDiscountPerArticle.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Nuevo monto: " + AddDecimalsToQuantity(decimalUpDownGlobalDiscountPerArticle.Value.ToString().Replace(',', '.')) + ". Cantidad de Articulos: " + FilesManager.ListOfArticlesGlobalDiscountPerArticle.Count;
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                        string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result3 = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result3.Item1 && result3.Item3.statusOperation)
                    {
                        if ((string)labelStatusGlobalDiscountPerArticle.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerArticle, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result3.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_ARTICLE, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        buttonUploadTextGlobalDiscountPerArticle.IsEnabled = true;
                        decimalUpDownGlobalDiscountPerArticle.IsEnabled = true;
                        textBoxUploadTextGlobalDiscountPerArticle.Text = string.Empty;
                        FilesManager.ListOfArticlesGlobalDiscountPerArticle.Clear();
                        UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerArticle, MODERATOR_MESSAGE_EMPTY);
                        if (result3.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result3.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result3.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void buttonUploadTextDiscountSectorExcludingArticle_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = FilesManager.GetPathFileDialog();
            if (!string.IsNullOrEmpty(pathFile))
            {
                textBoxUploadTExtDiscountSectorExcludingArticle.Text = pathFile;
            }
        }

        private async void buttonSolicitudeDiscountSectorExcludingArticle_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUploadTExtDiscountSectorExcludingArticle.Text))
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Tuple<bool, string> result = FilesManager.GetArticlesFromFileArticleByLine(textBoxUploadTExtDiscountSectorExcludingArticle.Text, FilesManager.ListOfArticlesDiscountSectorExcludingArticle);

                if (result.Item1 == false)
                {
                    textBoxUploadTExtDiscountSectorExcludingArticle.Text = string.Empty;
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (listBoxDiscountSectorExcludingArticle.SelectedItems.Count <= 0)
                {
                    System.Windows.MessageBox.Show("Error debe seleccionar por lo menos un sector.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    buttonUploadTextDiscountSectorExcludingArticle.IsEnabled = false;
                    decimalUpDownDiscountSectorExcludingArticle.IsEnabled = false;
                    listBoxDiscountSectorExcludingArticle.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Nuevo monto: " + AddDecimalsToQuantity(decimalUpDownDiscountSectorExcludingArticle.Value.ToString().Replace(',', '.')) + ". Sectores: " + GetSelectedItemsOfListBox(listBoxDiscountSectorExcludingArticle) + ". Cantidad de Articulos: " + FilesManager.ListOfArticlesDiscountSectorExcludingArticle.Count;
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                        string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result3 = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result3.Item1 && result3.Item3.statusOperation)
                    {
                        if ((string)labelStatusDiscountSectorExcludingArticle.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageDiscountSectorExcludingArticle, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result3.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.DISCOUNT_SECTOR_EXCLUDING_ARTICLE, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        textBoxUploadTExtDiscountSectorExcludingArticle.Text = string.Empty;
                        buttonUploadTextDiscountSectorExcludingArticle.IsEnabled = true;
                        decimalUpDownDiscountSectorExcludingArticle.IsEnabled = true;
                        listBoxDiscountSectorExcludingArticle.IsEnabled = true;
                        listBoxDiscountSectorExcludingArticle.UnselectAll();
                        FilesManager.ListOfArticlesDiscountSectorExcludingArticle.Clear();
                        UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageDiscountSectorExcludingArticle, MODERATOR_MESSAGE_EMPTY);
                        if (result3.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result3.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result3.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private async void buttonSolicitudeDisableEnableSectorByDate_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxDisableEnableSectorByDate.SelectedIndex != -1)
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = false;
                comboBoxDisableEnableSectorByDate.IsEnabled = false;
                System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Opcion: " + ((MyDataComboBox)comboBoxDisableEnableSectorByDate.SelectedValue).DisplayValue.ToString();
                NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                if (result.Item1 && result.Item3.statusOperation)
                {
                    if ((string)labelStatusDisableEnableSectorByDate.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                    {
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    }
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                    UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                    UpdateLabelStatusModeratorMessage(labelMessageDisableEnableSectorByDate, MODERATOR_MESSAGE_EMPTY);
                    ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.DISABLE_ENABLE_SECTOR_BY_DATE, groupBoxUpper));
                    ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = true;
                    comboBoxDisableEnableSectorByDate.IsEnabled = true;
                    comboBoxDisableEnableSectorByDate.SelectedIndex = -1;
                    UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    UpdateLabelStatusModeratorMessage(labelMessageDisableEnableSectorByDate, MODERATOR_MESSAGE_EMPTY);
                    if (result.Item3.statusOperation == false)
                    {
                        System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar una accion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonUploadTextUpdateSectorByArticle_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = FilesManager.GetPathFileDialog();
            if (!string.IsNullOrEmpty(pathFile))
            {
                textBoxUploadTextUpdateSectorByArticle.Text = pathFile;
            }
        }

        private async void buttonSolicitudeUpdateSectorByArticle_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUploadTextUpdateSectorByArticle.Text))
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Tuple<bool, string> result = FilesManager.GetArticlesCommaField(textBoxUploadTextUpdateSectorByArticle.Text, FilesManager.ListOfArticleCommaSectorUpdateSectorByArticle);
                if (result.Item1 == false)
                {
                    textBoxUploadTextUpdateSectorByArticle.Text = string.Empty;
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    buttonUploadTextUpdateSectorByArticle.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Cantidad de Articulos: " + FilesManager.ListOfArticleCommaSectorUpdateSectorByArticle.Count;
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                        string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result3 = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result3.Item1 && result3.Item3.statusOperation)
                    {
                        if ((string)labelStatusUpdateSectorByArticle.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageUpdateSectorByArticle, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result3.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UPDATE_SECTOR_BY_ARTICLE, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        buttonUploadTextUpdateSectorByArticle.IsEnabled = true;
                        textBoxUploadTextUpdateSectorByArticle.Text = string.Empty;
                        FilesManager.ListOfArticleCommaSectorUpdateSectorByArticle.Clear();
                        UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageUpdateSectorByArticle, MODERATOR_MESSAGE_EMPTY);
                        if (result3.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result3.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result3.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void buttonUploadTextUpdateArticlesDiscounts_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = FilesManager.GetPathFileDialog();
            if (!string.IsNullOrEmpty(pathFile))
            {
                textBoxUploadTextUpdateArticlesDiscounts.Text = pathFile;
            }
        }

        private async void buttonSolicitudeUpdateArticlesDiscounts_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUploadTextUpdateArticlesDiscounts.Text))
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Tuple<bool, string> result = FilesManager.GetArticlesCommaField(textBoxUploadTextUpdateArticlesDiscounts.Text, FilesManager.ListUpdateArticleCommaDiscountUpdateArticlesDiscounts);
                if (result.Item1 == false)
                {
                    textBoxUploadTextUpdateArticlesDiscounts.Text = string.Empty;
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    buttonUploadTextUpdateArticlesDiscounts.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Cantidad de Articulo,Descuento: " + FilesManager.ListUpdateArticleCommaDiscountUpdateArticlesDiscounts.Count;
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                        string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result3 = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result3.Item1 && result3.Item3.statusOperation)
                    {
                        if ((string)labelStatusUpdateArticlesDiscounts.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageUpdateArticlesDiscounts, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result3.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.ARTICLES_DISCOUNTS, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        buttonUploadTextUpdateArticlesDiscounts.IsEnabled = true;
                        textBoxUploadTextUpdateArticlesDiscounts.Text = string.Empty;
                        FilesManager.ListUpdateArticleCommaDiscountUpdateArticlesDiscounts.Clear();
                        UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageUpdateArticlesDiscounts, MODERATOR_MESSAGE_EMPTY);
                        if (result3.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result3.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result3.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void buttonUploadTextUpdateGendersPerArticles_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = FilesManager.GetPathFileDialog();
            if (!string.IsNullOrEmpty(pathFile))
            {
                textBoxUploadTextUpdateGendersPerArticles.Text = pathFile;
            }
        }

        private async void buttonSolicitudeUpdateGendersPerArticles_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUploadTextUpdateGendersPerArticles.Text))
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Tuple<bool, string> result = FilesManager.GetArticlesCommaField(textBoxUploadTextUpdateGendersPerArticles.Text, FilesManager.ListUpdateArticleCommaGenderUpdateArticlesGenders);
                if (result.Item1 == false)
                {
                    textBoxUploadTextUpdateGendersPerArticles.Text = string.Empty;
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    buttonUploadTextUpdateGendersPerArticles.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Cantidad de Articulo,Genero: " + FilesManager.ListUpdateArticleCommaGenderUpdateArticlesGenders.Count;
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                        string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result3 = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result3.Item1 && result3.Item3.statusOperation)
                    {
                        if ((string)labelStatusUpdateGendersPerArticles.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageUpdateGendersPerArticles, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result3.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UPDATE_GENDER_ARTICLES, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        buttonUploadTextUpdateGendersPerArticles.IsEnabled = true;
                        textBoxUploadTextUpdateGendersPerArticles.Text = string.Empty;
                        FilesManager.ListUpdateArticleCommaGenderUpdateArticlesGenders.Clear();
                        UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageUpdateGendersPerArticles, MODERATOR_MESSAGE_EMPTY);
                        if (result3.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result3.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result3.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void buttonUploadTextUpdateCategoriesPerArticles_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = FilesManager.GetPathFileDialog();
            if (!string.IsNullOrEmpty(pathFile))
            {
                textBoxUploadTextUpdateCategoriesPerArticles.Text = pathFile;
            }
        }

        private async void buttonSolicitudeUpdateCategoriesPerArticles_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUploadTextUpdateCategoriesPerArticles.Text))
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Tuple<bool, string> result = FilesManager.GetArticlesCommaField(textBoxUploadTextUpdateCategoriesPerArticles.Text, FilesManager.ListUpdateArticleCommaCategoryUpdateArticlesCategories);
                if (result.Item1 == false)
                {
                    textBoxUploadTextUpdateCategoriesPerArticles.Text = string.Empty;
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    buttonUploadTextUpdateCategoriesPerArticles.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Cantidad de Articulo,Catgoria: " + FilesManager.ListUpdateArticleCommaCategoryUpdateArticlesCategories.Count;
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                        string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result3 = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result3.Item1 && result3.Item3.statusOperation)
                    {
                        if ((string)labelStatusUpdateCategoriesPerArticles.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageUpdateCategoriesPerArticles, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result3.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.UPDATE_CATEGORY_ARTICLES, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        buttonUploadTextUpdateCategoriesPerArticles.IsEnabled = true;
                        textBoxUploadTextUpdateCategoriesPerArticles.Text = string.Empty;
                        FilesManager.ListUpdateArticleCommaCategoryUpdateArticlesCategories.Clear();
                        UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageUpdateCategoriesPerArticles, MODERATOR_MESSAGE_EMPTY);
                        if (result3.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result3.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result3.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private async void buttonSolicitudeGlobalDiscountPerGender_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxGlobalDiscountPerGender.SelectedItems.Count != 0)
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = false;
                decimalUpDownGlobalDiscountPerGender.IsEnabled = false;
                listBoxGlobalDiscountPerGender.IsEnabled = false;
                System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + AddDecimalsToQuantity(decimalUpDownGlobalDiscountPerGender.Value.ToString().Replace(',', '.')) + ". Generos: " + GetSelectedItemsOfListBox(listBoxGlobalDiscountPerGender);
                NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                if (result.Item1 && result.Item3.statusOperation)
                {
                    if ((string)labelStatusGlobalDiscountPerGender.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                    {
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    }
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                    UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                    UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerGender, MODERATOR_MESSAGE_EMPTY);
                    ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_GENDERS, groupBoxUpper));
                    ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = true;
                    decimalUpDownGlobalDiscountPerGender.IsEnabled = true;
                    listBoxGlobalDiscountPerGender.IsEnabled = true;
                    listBoxGlobalDiscountPerGender.UnselectAll();
                    UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerGender, MODERATOR_MESSAGE_EMPTY);
                    if (result.Item3.statusOperation == false)
                    {
                        System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar por lo menos un genero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void buttonSolicitudeGlobalDiscountPerCategories_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxGlobalDiscountPerCategories.SelectedItems.Count != 0)
            {
                ((System.Windows.Controls.Button)sender).IsEnabled = false;
                decimalUpDownGlobalDiscountPerCategories.IsEnabled = false;
                listBoxGlobalDiscountPerCategories.IsEnabled = false;
                System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Valor: " + AddDecimalsToQuantity(decimalUpDownGlobalDiscountPerCategories.Value.ToString().Replace(',', '.')) + ". Generos: " + GetSelectedItemsOfListBox(listBoxGlobalDiscountPerCategories);
                NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                          string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                if (result.Item1 && result.Item3.statusOperation)
                {
                    if ((string)labelStatusGlobalDiscountPerCategories.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                    {
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                    }
                    UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                    UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                    UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerCategories, MODERATOR_MESSAGE_EMPTY);
                    ListOfTask.TasksQueue.Enqueue(new Tasks(result.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_CATEGORIES, groupBoxUpper));
                    ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = true;
                    decimalUpDownGlobalDiscountPerCategories.IsEnabled = true;
                    listBoxGlobalDiscountPerCategories.IsEnabled = true;
                    listBoxGlobalDiscountPerCategories.UnselectAll();
                    UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerCategories, MODERATOR_MESSAGE_EMPTY);
                    if (result.Item3.statusOperation == false)
                    {
                        System.Windows.MessageBox.Show(result.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar por lo menos una categoria.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonUploadTextSetIva_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = FilesManager.GetPathFileDialog();
            if (!string.IsNullOrEmpty(pathFile))
            {
                textBoxUploadTextSetIva.Text = pathFile;
            }
        }

        private async void buttonSolicitudeSetIva_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUploadTextSetIva.Text))
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Tuple<bool, string> result = FilesManager.GetArticlesFromFileArticleByLine(textBoxUploadTextSetIva.Text, FilesManager.ListOfArticlesSetIva);
                if (result.Item1 == false)
                {
                    textBoxUploadTextSetIva.Text = string.Empty;
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    buttonUploadTextSetIva.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Cantidad de Articulos: " + FilesManager.ListOfArticlesSetIva.Count;
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                        string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result3 = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result3.Item1 && result3.Item3.statusOperation)
                    {
                        if ((string)labelStatusSetIva.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageSetIva, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result3.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.SET_IVA, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        buttonUploadTextSetIva.IsEnabled = true;
                        textBoxUploadTextSetIva.Text = string.Empty;
                        FilesManager.ListOfArticlesSetIva.Clear();
                        UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageSetIva, MODERATOR_MESSAGE_EMPTY);
                        if (result3.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result3.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result3.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        private void buttonUploadTextSetExento_Click(object sender, RoutedEventArgs e)
        {
            string pathFile = FilesManager.GetPathFileDialog();
            if (!string.IsNullOrEmpty(pathFile))
            {
                textBoxUploadTextSetExento.Text = pathFile;
            }
        }

        private async void buttonSolicitudeSetExento_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUploadTextSetExento.Text))
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un archivo.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Tuple<bool, string> result = FilesManager.GetArticlesFromFileArticleByLine(textBoxUploadTextSetExento.Text, FilesManager.ListOfArticlesSetExento);
                if (result.Item1 == false)
                {
                    textBoxUploadTextSetExento.Text = string.Empty;
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ((System.Windows.Controls.Button)sender).IsEnabled = false;
                    buttonUploadTextSetExento.IsEnabled = false;
                    System.Windows.Controls.GroupBox groupBoxUpper = FindUpVisualTree<System.Windows.Controls.GroupBox>((System.Windows.Controls.Button)sender);
                    string description = ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", "") + ". Cantidad de Articulos: " + FilesManager.ListOfArticlesSetExento.Count;
                    NetworkClassLibrary.Models.TaskModel taskModel = new NetworkClassLibrary.Models.TaskModel(initialData.StoreEmployee.data[0].stremp_store_id, initialData.StoreEmployee.data[0].stremp_id, (int)Tasks.EnumTaskStatusTask.PENDIENTE,
                                                                                                        string.Empty, DateTime.Now, description, string.Empty, (int)Tasks.EnumTaskStatusLocal.NONE, string.Empty, 1, DateTime.Now, false);

                    Tuple<bool, string, NetworkClassLibrary.Models.TaskAnswerModel> result3 = await NetworkClassLibrary.Post.InsertTask(initialData.IP_WEB_SERVICE + "/api/Task/MakeTask", taskModel);
                    if (result3.Item1 && result3.Item3.statusOperation)
                    {
                        if ((string)labelStatusSetExento.Content == Tasks.EnumTaskStatusTask.DENEGADA.ToString())
                        {
                            UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                            QuickNavigationDenied(new MyDataComboBox((string)groupBoxUpper.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                        }
                        UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.ADD);
                        UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.PENDIENTE, System.Windows.Media.Brushes.DarkOrange);
                        UpdateLabelStatusModeratorMessage(labelMessageSetExento, MODERATOR_MESSAGE_EMPTY);
                        ListOfTask.TasksQueue.Enqueue(new Tasks(result3.Item3.data, string.Empty, string.Empty, Tasks.EnumTaskStatusTask.PENDIENTE, Tasks.EnumTaskGroup.SET_EXENTO, groupBoxUpper));
                        ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)groupBoxUpper.Header).Children[1]).Text.Replace(" - ", ""), Tasks.EnumTaskStatusTask.PENDIENTE);
                    }
                    else
                    {
                        ((System.Windows.Controls.Button)sender).IsEnabled = true;
                        buttonUploadTextSetExento.IsEnabled = true;
                        textBoxUploadTextSetExento.Text = string.Empty;
                        FilesManager.ListOfArticlesSetExento.Clear();
                        UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                        UpdateLabelStatusModeratorMessage(labelMessageSetExento, MODERATOR_MESSAGE_EMPTY);
                        if (result3.Item3.statusOperation == false)
                        {
                            System.Windows.MessageBox.Show(result3.Item3.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(result3.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }

        //Ciclo infinito para procesar las tareas en cola
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(true)
            {
                Thread.Sleep(Int32.Parse(initialData.TIME_INTERVAL_CHECKING_TASK));

                Tasks task = new Tasks();

                while (ListOfTask.TasksQueue.Count > 0)
                {
                    if (ListOfTask.TasksQueue.TryDequeue(out task))
                    {
                        var doTask = NetworkClassLibrary.Post.GetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/GetStatusTask", task.Id.ToString());
                        doTask.Wait();
                        Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = doTask.Result;
                        if (result.Item1)
                        {
                            SetStatusTaskAndToken(task, result.Item3.data[0].task_status_id, result.Item3.data[0].task_token, result.Item3.data[0].task_moderator_message);
                            backgroundWorker.ReportProgress(0, task);
                        }
                        else
                        {
                            task.Status = Tasks.EnumTaskStatusTask.ERROR;
                            backgroundWorker.ReportProgress(0, task);
                        }
                    }

                    Thread.Sleep(Int32.Parse(initialData.TIME_INTERVAL_CHECKING_TASK));
                }
            }
        }

        private void SetStatusTaskAndToken(Tasks task, int newStatus, string token, string moderatorMessage)
        {
            switch(newStatus)
            {
                case (int)Tasks.EnumTaskStatusTask.CERRADA:
                    task.Status = Tasks.EnumTaskStatusTask.CERRADA;
                    task.Token = token;
                    task.ModeratorMessage = moderatorMessage;
                    break;
                case (int)Tasks.EnumTaskStatusTask.PENDIENTE:
                    task.Status = Tasks.EnumTaskStatusTask.PENDIENTE;
                    task.Token = token;
                    task.ModeratorMessage = moderatorMessage;
                    break;
                case (int)Tasks.EnumTaskStatusTask.APROBADA:
                    task.Status = Tasks.EnumTaskStatusTask.APROBADA;
                    task.Token = token;
                    task.ModeratorMessage = moderatorMessage;
                    break;
                case (int)Tasks.EnumTaskStatusTask.DENEGADA:
                    task.Status = Tasks.EnumTaskStatusTask.DENEGADA;
                    task.Token = token;
                    task.ModeratorMessage = moderatorMessage;
                    break;
                default:
                    task.Status = Tasks.EnumTaskStatusTask.NONE;
                    task.Token = token;
                    task.ModeratorMessage = moderatorMessage;
                    break;
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Tasks task = ((Tasks)e.UserState);

            //Si esta pendiente coloco la tarea de nuevo en cola de proceso
            if (task.Status == Tasks.EnumTaskStatusTask.PENDIENTE)
            {
                ListOfTask.TasksQueue.Enqueue(task);
            }
            else if (task.Group == Tasks.EnumTaskGroup.UNBLOCK_USER && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUnblock, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUnblockUser.Tag = task;
                buttonExecuteUnblockUser.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UNBLOCK_USER && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUnblock, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUnblockUser.IsEnabled = true;
                comboBoxUnblockUser.IsEnabled = true;
                comboBoxUnblockUser.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UNBLOCK_USER && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUnblock, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUnblockUser.IsEnabled = true;
                comboBoxUnblockUser.IsEnabled = true;
                comboBoxUnblockUser.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UNBLOCK_USER && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUnblock, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUnblockUser.IsEnabled = true;
                comboBoxUnblockUser.IsEnabled = true;
                comboBoxUnblockUser.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UNBLOCK_FISCAL_MACHINE && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUnblockFiscalMachine, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUnblockFiscalMachine.Tag = task;
                buttonExecuteUnblockFiscalMachine.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UNBLOCK_FISCAL_MACHINE && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUnblockFiscalMachine, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUnblockFiscalMachine.IsEnabled = true;
                comboBoxUnblockFiscalMachine.IsEnabled = true;
                comboBoxUnblockFiscalMachine.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UNBLOCK_FISCAL_MACHINE && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUnblockFiscalMachine, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUnblockFiscalMachine.IsEnabled = true;
                comboBoxUnblockFiscalMachine.IsEnabled = true;
                comboBoxUnblockFiscalMachine.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UNBLOCK_FISCAL_MACHINE && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUnblockFiscalMachine, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUnblockFiscalMachine.IsEnabled = true;
                comboBoxUnblockFiscalMachine.IsEnabled = true;
                comboBoxUnblockFiscalMachine.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.RESEND_CLOSER && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageResendCloser, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteResendCloser.Tag = task;
                buttonExecuteResendCloser.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.RESEND_CLOSER && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageResendCloser, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeResendCloser.IsEnabled = true;
                datePickerResendCloser.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.RESEND_CLOSER && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageResendCloser, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeResendCloser.IsEnabled = true;
                datePickerResendCloser.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.RESEND_CLOSER && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageResendCloser, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeResendCloser.IsEnabled = true;
                datePickerResendCloser.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.CLOSE_CASH_MACHINE && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageCloseCashMachine, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteCloseCashMachine.Tag = task;
                buttonExecuteCloseCashMachine.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.CLOSE_CASH_MACHINE && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageCloseCashMachine, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeCloseCashMachine.IsEnabled = true;
                datePickerCloseCashMachine.IsEnabled = true;
                comboBoxCloseCashMachine.IsEnabled = true;
                comboBoxCloseCashMachine.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.CLOSE_CASH_MACHINE && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageCloseCashMachine, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeCloseCashMachine.IsEnabled = true;
                datePickerCloseCashMachine.IsEnabled = true;
                comboBoxCloseCashMachine.IsEnabled = true;
                comboBoxCloseCashMachine.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.CLOSE_CASH_MACHINE && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageCloseCashMachine, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeCloseCashMachine.IsEnabled = true;
                datePickerCloseCashMachine.IsEnabled = true;
                comboBoxCloseCashMachine.IsEnabled = true;
                comboBoxCloseCashMachine.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.EQUALS_FISCAL_NETO && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageEqualsFiscalNeto, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteEqualsFiscalNeto.Tag = task;
                buttonExecuteEqualsFiscalNeto.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.EQUALS_FISCAL_NETO && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageEqualsFiscalNeto, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeEqualsFiscalNeto.IsEnabled = true;
                datePickerEqualsFiscalNeto.IsEnabled = true;
                comboBoxEqualsFiscalNeto.IsEnabled = true;
                comboBoxEqualsFiscalNeto.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.EQUALS_FISCAL_NETO && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageEqualsFiscalNeto, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeEqualsFiscalNeto.IsEnabled = true;
                datePickerEqualsFiscalNeto.IsEnabled = true;
                comboBoxEqualsFiscalNeto.IsEnabled = true;
                comboBoxEqualsFiscalNeto.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.EQUALS_FISCAL_NETO && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageEqualsFiscalNeto, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeEqualsFiscalNeto.IsEnabled = true;
                datePickerEqualsFiscalNeto.IsEnabled = true;
                comboBoxEqualsFiscalNeto.IsEnabled = true;
                comboBoxEqualsFiscalNeto.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_AMOUNT_PETTY_CASH_FUND && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountPettyCashFund, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUpdateAmountPettyCashFund.Tag = task;
                buttonExecuteUpdateAmountPettyCashFund.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_AMOUNT_PETTY_CASH_FUND && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountPettyCashFund, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateAmountPettyCashFund.IsEnabled = true;
                decimalUpDownUpdateAmountPettyCashFund.IsEnabled = true;
                decimalUpDownUpdateAmountPettyCashFund.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_AMOUNT_PETTY_CASH_FUND && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountPettyCashFund, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateAmountPettyCashFund.IsEnabled = true;
                decimalUpDownUpdateAmountPettyCashFund.IsEnabled = true;
                decimalUpDownUpdateAmountPettyCashFund.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_AMOUNT_PETTY_CASH_FUND && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountPettyCashFund, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateAmountPettyCashFund.IsEnabled = true;
                decimalUpDownUpdateAmountPettyCashFund.IsEnabled = true;
                decimalUpDownUpdateAmountPettyCashFund.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_EURO && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateEuro, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUpdateEuro.Tag = task;
                buttonExecuteUpdateEuro.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_EURO && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateEuro, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateEuro.IsEnabled = true;
                decimalUpDownUpdateEuro.IsEnabled = true;
                decimalUpDownUpdateEuro.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_EURO && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateEuro, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateEuro.IsEnabled = true;
                decimalUpDownUpdateEuro.IsEnabled = true;
                decimalUpDownUpdateEuro.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_EURO && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateEuro, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateEuro.IsEnabled = true;
                decimalUpDownUpdateEuro.IsEnabled = true;
                decimalUpDownUpdateEuro.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_DOLLAR && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateDollar, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUpdateDollar.Tag = task;
                buttonExecuteUpdateDollar.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_DOLLAR && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateDollar, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateDollar.IsEnabled = true;
                decimalUpDownUpdateDollar.IsEnabled = true;
                decimalUpDownUpdateDollar.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_DOLLAR && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateDollar, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateDollar.IsEnabled = true;
                decimalUpDownUpdateDollar.IsEnabled = true;
                decimalUpDownUpdateDollar.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_DOLLAR && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateDollar, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateDollar.IsEnabled = true;
                decimalUpDownUpdateDollar.IsEnabled = true;
                decimalUpDownUpdateDollar.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_DISCOUNT_SAP && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableDiscountSap, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteDisableEnableDiscountSap.Tag = task;
                buttonExecuteDisableEnableDiscountSap.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_DISCOUNT_SAP && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableDiscountSap, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableDiscountSap.IsEnabled = true;
                comboBoxDisableEnableDiscountSap.IsEnabled = true;
                comboBoxDisableEnableDiscountSap.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_DISCOUNT_SAP && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableDiscountSap, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableDiscountSap.IsEnabled = true;
                comboBoxDisableEnableDiscountSap.IsEnabled = true;
                comboBoxDisableEnableDiscountSap.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_DISCOUNT_SAP && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableDiscountSap, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableDiscountSap.IsEnabled = true;
                comboBoxDisableEnableDiscountSap.IsEnabled = true;
                comboBoxDisableEnableDiscountSap.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_AMOUNT_MAX_PER_INVOICE && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountMaxPerInvoice, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUpdateAmountMaxPerInvoice.Tag = task;
                buttonExecuteUpdateAmountMaxPerInvoice.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_AMOUNT_MAX_PER_INVOICE && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountMaxPerInvoice, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateAmountMaxPerInvoice.IsEnabled = true;
                decimalUpDownUpdateAmountMaxPerInvoice.IsEnabled = true;
                decimalUpDownUpdateAmountMaxPerInvoice.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_AMOUNT_MAX_PER_INVOICE && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountMaxPerInvoice, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateAmountMaxPerInvoice.IsEnabled = true;
                decimalUpDownUpdateAmountMaxPerInvoice.IsEnabled = true;
                decimalUpDownUpdateAmountMaxPerInvoice.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_AMOUNT_MAX_PER_INVOICE && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountMaxPerInvoice, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateAmountMaxPerInvoice.IsEnabled = true;
                decimalUpDownUpdateAmountMaxPerInvoice.IsEnabled = true;
                decimalUpDownUpdateAmountMaxPerInvoice.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_CLOSE_EMAIL && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableCloseEmail, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteDisableEnableCloseEmail.Tag = task;
                buttonExecuteDisableEnableCloseEmail.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_CLOSE_EMAIL && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableCloseEmail, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableCloseEmail.IsEnabled = true;
                comboBoxDisableEnableCloseEmail.IsEnabled = true;
                comboBoxDisableEnableCloseEmail.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_CLOSE_EMAIL && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableCloseEmail, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableCloseEmail.IsEnabled = true;
                comboBoxDisableEnableCloseEmail.IsEnabled = true;
                comboBoxDisableEnableCloseEmail.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_CLOSE_EMAIL && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableCloseEmail, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableCloseEmail.IsEnabled = true;
                comboBoxDisableEnableCloseEmail.IsEnabled = true;
                comboBoxDisableEnableCloseEmail.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.FAILURE_UPDATE_PRICES_DISCCOUNTS_TOTAL_POS && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageFailureUpdatePricesDisccountsTotalPos, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteFailureUpdatePricesDisccountsTotalPos.Tag = task;
                buttonExecuteFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.FAILURE_UPDATE_PRICES_DISCCOUNTS_TOTAL_POS && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageFailureUpdatePricesDisccountsTotalPos, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
                integerUpDownFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
                integerUpDownFailureUpdatePricesDisccountsTotalPos.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.FAILURE_UPDATE_PRICES_DISCCOUNTS_TOTAL_POS && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageFailureUpdatePricesDisccountsTotalPos, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
                integerUpDownFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
                integerUpDownFailureUpdatePricesDisccountsTotalPos.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.FAILURE_UPDATE_PRICES_DISCCOUNTS_TOTAL_POS && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageFailureUpdatePricesDisccountsTotalPos, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
                integerUpDownFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
                integerUpDownFailureUpdatePricesDisccountsTotalPos.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscount, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteGlobalDiscount.Tag = task;
                buttonExecuteGlobalDiscount.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscount, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscount.IsEnabled = true;
                decimalUpDownGlobalDiscount.IsEnabled = true;
                decimalUpDownGlobalDiscount.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscount, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscount.IsEnabled = true;
                decimalUpDownGlobalDiscount.IsEnabled = true;
                decimalUpDownGlobalDiscount.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscount, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscount.IsEnabled = true;
                decimalUpDownGlobalDiscount.IsEnabled = true;
                decimalUpDownGlobalDiscount.Value = 0;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_SECTOR && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerSector, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteGlobalDiscountPerSector.Tag = task;
                buttonExecuteGlobalDiscountPerSector.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_SECTOR && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerSector, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerSector.IsEnabled = true;
                decimalUpDownGlobalDiscountPerSector.IsEnabled = true;
                decimalUpDownGlobalDiscountPerSector.Value = 0;
                listBoxGlobalDiscountPerSector.IsEnabled = true;
                listBoxGlobalDiscountPerSector.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_SECTOR && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerSector, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerSector.IsEnabled = true;
                decimalUpDownGlobalDiscountPerSector.IsEnabled = true;
                decimalUpDownGlobalDiscountPerSector.Value = 0;
                listBoxGlobalDiscountPerSector.IsEnabled = true;
                listBoxGlobalDiscountPerSector.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_SECTOR && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerSector, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerSector.IsEnabled = true;
                decimalUpDownGlobalDiscountPerSector.IsEnabled = true;
                decimalUpDownGlobalDiscountPerSector.Value = 0;
                listBoxGlobalDiscountPerSector.IsEnabled = true;
                listBoxGlobalDiscountPerSector.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteGlobalDiscountPerArticle.Tag = task;
                buttonExecuteGlobalDiscountPerArticle.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerArticle.IsEnabled = true;
                decimalUpDownGlobalDiscountPerArticle.IsEnabled = true;
                decimalUpDownGlobalDiscountPerArticle.Value = 0;
                textBoxUploadTextGlobalDiscountPerArticle.Text = string.Empty;
                buttonUploadTextGlobalDiscountPerArticle.IsEnabled = true;
                FilesManager.ListOfArticlesGlobalDiscountPerArticle.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerArticle.IsEnabled = true;
                decimalUpDownGlobalDiscountPerArticle.IsEnabled = true;
                decimalUpDownGlobalDiscountPerArticle.Value = 0;
                textBoxUploadTextGlobalDiscountPerArticle.Text = string.Empty;
                buttonUploadTextGlobalDiscountPerArticle.IsEnabled = true;
                FilesManager.ListOfArticlesGlobalDiscountPerArticle.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerArticle.IsEnabled = true;
                decimalUpDownGlobalDiscountPerArticle.IsEnabled = true;
                decimalUpDownGlobalDiscountPerArticle.Value = 0;
                textBoxUploadTextGlobalDiscountPerArticle.Text = string.Empty;
                buttonUploadTextGlobalDiscountPerArticle.IsEnabled = true;
                FilesManager.ListOfArticlesGlobalDiscountPerArticle.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISCOUNT_SECTOR_EXCLUDING_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageDiscountSectorExcludingArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteDiscountSectorExcludingArticle.Tag = task;
                buttonExecuteDiscountSectorExcludingArticle.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISCOUNT_SECTOR_EXCLUDING_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageDiscountSectorExcludingArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDiscountSectorExcludingArticle.IsEnabled = true;
                decimalUpDownDiscountSectorExcludingArticle.IsEnabled = true;
                decimalUpDownDiscountSectorExcludingArticle.Value = 0;
                textBoxUploadTExtDiscountSectorExcludingArticle.Text = string.Empty;
                buttonUploadTextDiscountSectorExcludingArticle.IsEnabled = true;
                FilesManager.ListOfArticlesDiscountSectorExcludingArticle.Clear();
                listBoxDiscountSectorExcludingArticle.IsEnabled = true;
                listBoxDiscountSectorExcludingArticle.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISCOUNT_SECTOR_EXCLUDING_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageDiscountSectorExcludingArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDiscountSectorExcludingArticle.IsEnabled = true;
                decimalUpDownDiscountSectorExcludingArticle.IsEnabled = true;
                decimalUpDownDiscountSectorExcludingArticle.Value = 0;
                textBoxUploadTExtDiscountSectorExcludingArticle.Text = string.Empty;
                buttonUploadTextDiscountSectorExcludingArticle.IsEnabled = true;
                FilesManager.ListOfArticlesDiscountSectorExcludingArticle.Clear();
                listBoxDiscountSectorExcludingArticle.IsEnabled = true;
                listBoxDiscountSectorExcludingArticle.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISCOUNT_SECTOR_EXCLUDING_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageDiscountSectorExcludingArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDiscountSectorExcludingArticle.IsEnabled = true;
                decimalUpDownDiscountSectorExcludingArticle.IsEnabled = true;
                decimalUpDownDiscountSectorExcludingArticle.Value = 0;
                textBoxUploadTExtDiscountSectorExcludingArticle.Text = string.Empty;
                buttonUploadTextDiscountSectorExcludingArticle.IsEnabled = true;
                FilesManager.ListOfArticlesDiscountSectorExcludingArticle.Clear();
                listBoxDiscountSectorExcludingArticle.IsEnabled = true;
                listBoxDiscountSectorExcludingArticle.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_SECTOR_BY_DATE && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableSectorByDate, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteDisableEnableSectorByDate.Tag = task;
                buttonExecuteDisableEnableSectorByDate.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_SECTOR_BY_DATE && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableSectorByDate, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableSectorByDate.IsEnabled = true;
                comboBoxDisableEnableSectorByDate.IsEnabled = true;
                comboBoxDisableEnableSectorByDate.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_SECTOR_BY_DATE && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableSectorByDate, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableSectorByDate.IsEnabled = true;
                comboBoxDisableEnableSectorByDate.IsEnabled = true;
                comboBoxDisableEnableSectorByDate.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.DISABLE_ENABLE_SECTOR_BY_DATE && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageDisableEnableSectorByDate, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeDisableEnableSectorByDate.IsEnabled = true;
                comboBoxDisableEnableSectorByDate.IsEnabled = true;
                comboBoxDisableEnableSectorByDate.SelectedIndex = -1;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_SECTOR_BY_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateSectorByArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUpdateSectorByArticle.Tag = task;
                buttonExecuteUpdateSectorByArticle.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_SECTOR_BY_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateSectorByArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateSectorByArticle.IsEnabled = true;
                textBoxUploadTextUpdateSectorByArticle.Text = string.Empty;
                buttonUploadTextUpdateSectorByArticle.IsEnabled = true;
                FilesManager.ListOfArticleCommaSectorUpdateSectorByArticle.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_SECTOR_BY_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateSectorByArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateSectorByArticle.IsEnabled = true;
                textBoxUploadTextUpdateSectorByArticle.Text = string.Empty;
                buttonUploadTextUpdateSectorByArticle.IsEnabled = true;
                FilesManager.ListOfArticleCommaSectorUpdateSectorByArticle.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_SECTOR_BY_ARTICLE && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateSectorByArticle, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateSectorByArticle.IsEnabled = true;
                textBoxUploadTextUpdateSectorByArticle.Text = string.Empty;
                buttonUploadTextUpdateSectorByArticle.IsEnabled = true;
                FilesManager.ListOfArticleCommaSectorUpdateSectorByArticle.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.ARTICLES_DISCOUNTS && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateArticlesDiscounts, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUpdateyArticlesDiscounts.Tag = task;
                buttonExecuteUpdateyArticlesDiscounts.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.ARTICLES_DISCOUNTS && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateArticlesDiscounts, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateArticlesDiscounts.IsEnabled = true;
                textBoxUploadTextUpdateArticlesDiscounts.Text = string.Empty;
                buttonUploadTextUpdateArticlesDiscounts.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaDiscountUpdateArticlesDiscounts.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.ARTICLES_DISCOUNTS && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateArticlesDiscounts, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateArticlesDiscounts.IsEnabled = true;
                textBoxUploadTextUpdateArticlesDiscounts.Text = string.Empty;
                buttonUploadTextUpdateArticlesDiscounts.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaDiscountUpdateArticlesDiscounts.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.ARTICLES_DISCOUNTS && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateArticlesDiscounts, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateArticlesDiscounts.IsEnabled = true;
                textBoxUploadTextUpdateArticlesDiscounts.Text = string.Empty;
                buttonUploadTextUpdateArticlesDiscounts.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaDiscountUpdateArticlesDiscounts.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_GENDER_ARTICLES && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateGendersPerArticles, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUpdateGendersPerArticles.Tag = task;
                buttonExecuteUpdateGendersPerArticles.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_GENDER_ARTICLES && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateGendersPerArticles, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateGendersPerArticles.IsEnabled = true;
                textBoxUploadTextUpdateGendersPerArticles.Text = string.Empty;
                buttonUploadTextUpdateGendersPerArticles.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaGenderUpdateArticlesGenders.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_GENDER_ARTICLES && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateGendersPerArticles, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateGendersPerArticles.IsEnabled = true;
                textBoxUploadTextUpdateGendersPerArticles.Text = string.Empty;
                buttonUploadTextUpdateGendersPerArticles.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaGenderUpdateArticlesGenders.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_GENDER_ARTICLES && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateGendersPerArticles, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateGendersPerArticles.IsEnabled = true;
                textBoxUploadTextUpdateGendersPerArticles.Text = string.Empty;
                buttonUploadTextUpdateGendersPerArticles.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaGenderUpdateArticlesGenders.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_CATEGORY_ARTICLES && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateCategoriesPerArticles, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteUpdateCategoriessPerArticles.Tag = task;
                buttonExecuteUpdateCategoriessPerArticles.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_CATEGORY_ARTICLES && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateCategoriesPerArticles, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateCategoriesPerArticles.IsEnabled = true;
                textBoxUploadTextUpdateCategoriesPerArticles.Text = string.Empty;
                buttonUploadTextUpdateCategoriesPerArticles.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaCategoryUpdateArticlesCategories.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_CATEGORY_ARTICLES && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateCategoriesPerArticles, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateCategoriesPerArticles.IsEnabled = true;
                textBoxUploadTextUpdateCategoriesPerArticles.Text = string.Empty;
                buttonUploadTextUpdateCategoriesPerArticles.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaCategoryUpdateArticlesCategories.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.UPDATE_CATEGORY_ARTICLES && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageUpdateCategoriesPerArticles, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeUpdateCategoriesPerArticles.IsEnabled = true;
                textBoxUploadTextUpdateCategoriesPerArticles.Text = string.Empty;
                buttonUploadTextUpdateCategoriesPerArticles.IsEnabled = true;
                FilesManager.ListUpdateArticleCommaCategoryUpdateArticlesCategories.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_GENDERS && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerGender, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteGlobalDiscountPerGender.Tag = task;
                buttonExecuteGlobalDiscountPerGender.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_GENDERS && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerGender, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerGender.IsEnabled = true;
                decimalUpDownGlobalDiscountPerGender.IsEnabled = true;
                decimalUpDownGlobalDiscountPerGender.Value = 0;
                listBoxGlobalDiscountPerGender.IsEnabled = true;
                listBoxGlobalDiscountPerGender.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_GENDERS && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerGender, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerGender.IsEnabled = true;
                decimalUpDownGlobalDiscountPerGender.IsEnabled = true;
                decimalUpDownGlobalDiscountPerGender.Value = 0;
                listBoxGlobalDiscountPerGender.IsEnabled = true;
                listBoxGlobalDiscountPerGender.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_GENDERS && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerGender, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerGender.IsEnabled = true;
                decimalUpDownGlobalDiscountPerGender.IsEnabled = true;
                decimalUpDownGlobalDiscountPerGender.Value = 0;
                listBoxGlobalDiscountPerGender.IsEnabled = true;
                listBoxGlobalDiscountPerGender.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_CATEGORIES && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerCategories, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteGlobalDiscountPerCategories.Tag = task;
                buttonExecuteGlobalDiscountPerCategories.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_CATEGORIES && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerCategories, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerCategories.IsEnabled = true;
                decimalUpDownGlobalDiscountPerCategories.IsEnabled = true;
                decimalUpDownGlobalDiscountPerCategories.Value = 0;
                listBoxGlobalDiscountPerCategories.IsEnabled = true;
                listBoxGlobalDiscountPerCategories.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_CATEGORIES && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerCategories, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerCategories.IsEnabled = true;
                decimalUpDownGlobalDiscountPerCategories.IsEnabled = true;
                decimalUpDownGlobalDiscountPerCategories.Value = 0;
                listBoxGlobalDiscountPerCategories.IsEnabled = true;
                listBoxGlobalDiscountPerCategories.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.GLOBAL_DISCOUNT_PER_CATEGORIES && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerCategories, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeGlobalDiscountPerCategories.IsEnabled = true;
                decimalUpDownGlobalDiscountPerCategories.IsEnabled = true;
                decimalUpDownGlobalDiscountPerCategories.Value = 0;
                listBoxGlobalDiscountPerCategories.IsEnabled = true;
                listBoxGlobalDiscountPerCategories.UnselectAll();
            }
            else if (task.Group == Tasks.EnumTaskGroup.SET_IVA && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageSetIva, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteSetIva.Tag = task;
                buttonExecuteSetIva.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.SET_IVA && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageSetIva, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeSetIva.IsEnabled = true;
                textBoxUploadTextSetIva.Text = string.Empty;
                buttonUploadTextSetIva.IsEnabled = true;
                FilesManager.ListOfArticlesSetIva.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.SET_IVA && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageSetIva, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeSetIva.IsEnabled = true;
                textBoxUploadTextSetIva.Text = string.Empty;
                buttonUploadTextSetIva.IsEnabled = true;
                FilesManager.ListOfArticlesSetIva.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.SET_IVA && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageSetIva, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeSetIva.IsEnabled = true;
                textBoxUploadTextSetIva.Text = string.Empty;
                buttonUploadTextSetIva.IsEnabled = true;
                FilesManager.ListOfArticlesSetIva.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.SET_EXENTO && task.Status == Tasks.EnumTaskStatusTask.APROBADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.APROBADA);
                QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.APROBADA, System.Windows.Media.Brushes.Green);
                UpdateLabelStatusModeratorMessage(labelMessageSetExento, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonExecuteSetExento.Tag = task;
                buttonExecuteSetExento.IsEnabled = true;
            }
            else if (task.Group == Tasks.EnumTaskGroup.SET_EXENTO && task.Status == Tasks.EnumTaskStatusTask.DENEGADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.DENEGADA);
                QuickNavigationDenied(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.DENEGADA, OPERATIONS_ADD_SUBTRAC.ADD);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.DENEGADA, System.Windows.Media.Brushes.Red);
                UpdateLabelStatusModeratorMessage(labelMessageSetExento, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeSetExento.IsEnabled = true;
                textBoxUploadTextSetExento.Text = string.Empty;
                buttonUploadTextSetExento.IsEnabled = true;
                FilesManager.ListOfArticlesSetExento.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.SET_EXENTO && task.Status == Tasks.EnumTaskStatusTask.CERRADA)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.CERRADA);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageSetExento, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeSetExento.IsEnabled = true;
                textBoxUploadTextSetExento.Text = string.Empty;
                buttonUploadTextSetExento.IsEnabled = true;
                FilesManager.ListOfArticlesSetExento.Clear();
            }
            else if (task.Group == Tasks.EnumTaskGroup.SET_EXENTO && task.Status == Tasks.EnumTaskStatusTask.ERROR)
            {
                ShowNotification(((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[1]).Text.Trim('-').Trim(), Tasks.EnumTaskStatusTask.ERROR);
                UpdateTabCounter(Tasks.EnumTaskStatusTask.PENDIENTE, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
                UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                UpdateLabelStatusModeratorMessage(labelMessageSetExento, string.IsNullOrWhiteSpace(task.ModeratorMessage) ? MODERATOR_MESSAGE_EMPTY : task.ModeratorMessage);
                buttonSolicitudeSetExento.IsEnabled = true;
                textBoxUploadTextSetExento.Text = string.Empty;
                buttonUploadTextSetExento.IsEnabled = true;
                FilesManager.ListOfArticlesSetExento.Clear();
            }
        }

        private async void buttonExecuteUnblock_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UnblockUser(comboBoxUnblockUser.SelectedValue.ToString());

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUnblock, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUnblockUser.IsEnabled = true;
            comboBoxUnblockUser.IsEnabled = true;
            comboBoxUnblockUser.SelectedIndex = -1;
            UpdateLabelStatusModeratorMessage(labelMessageUnblock, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUnblockFiscalMachine_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UnblockFailureFiscalMachine(((MyDataComboBox)comboBoxUnblockFiscalMachine.SelectedValue).Value.ToString());

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUnblockFiscalMachine, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUnblockFiscalMachine.IsEnabled = true;
            comboBoxUnblockFiscalMachine.IsEnabled = true;
            comboBoxUnblockFiscalMachine.SelectedIndex = -1;
            UpdateLabelStatusModeratorMessage(labelMessageUnblockFiscalMachine, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteResendCloser_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.ResendCloser(datePickerResendCloser.SelectedDate.Value.ToString("yyyy-MM-dd"));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusResendCloser, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeResendCloser.IsEnabled = true;
            datePickerResendCloser.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageResendCloser, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteCloseCashMachine_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string, double> resultSum = await mysql.CloseCashMachineSumSales(datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).Value.ToString());

                if (resultSum.Item1)
                {
                    if (resultSum.Item3 == 0)
                    {
                        Tuple<bool, string> result = await mysql.CloseCashMachine(datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).Value.ToString());
                        if (result.Item1)
                        {
                            resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                            System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                            System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        Tuple<bool, string> result = await mysql.CloseCashMachineZReport(datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).Value.ToString());
                        if (result.Item1)
                        {
                            resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                            System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                            System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = resultSum.Item2 });
                    System.Windows.MessageBox.Show(resultSum.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusCloseCashMachine, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeCloseCashMachine.IsEnabled = true;
            comboBoxCloseCashMachine.IsEnabled = true;
            comboBoxCloseCashMachine.SelectedIndex = -1;
            datePickerCloseCashMachine.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageCloseCashMachine, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteEqualsFiscalNeto_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.EqualsFicalNeto(datePickerEqualsFiscalNeto.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxEqualsFiscalNeto.SelectedItem).Value.ToString());

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusEqualsFiscalNeto, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeEqualsFiscalNeto.IsEnabled = true;
            comboBoxEqualsFiscalNeto.IsEnabled = true;
            comboBoxEqualsFiscalNeto.SelectedIndex = -1;
            datePickerEqualsFiscalNeto.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageEqualsFiscalNeto, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUpdateAmountPettyCashFund_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UpdateAmountPettyCashFund(AddDecimalsToQuantity(decimalUpDownUpdateAmountPettyCashFund.Value.ToString().Replace(',', '.')));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUpdateAmountPettyCashFund, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUpdateAmountPettyCashFund.IsEnabled = true;
            decimalUpDownUpdateAmountPettyCashFund.Value = 0;
            decimalUpDownUpdateAmountPettyCashFund.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountPettyCashFund, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUpdateEuro_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UpdateEuroRate(AddDecimalsToQuantity(decimalUpDownUpdateEuro.Value.ToString().Replace(',', '.')));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUpdateEuro, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUpdateEuro.IsEnabled = true;
            decimalUpDownUpdateEuro.Value = 0;
            decimalUpDownUpdateEuro.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageUpdateEuro, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUpdateDollar_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UpdateDollarRate(AddDecimalsToQuantity(decimalUpDownUpdateDollar.Value.ToString().Replace(',', '.')));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUpdateDollar, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUpdateDollar.IsEnabled = true;
            decimalUpDownUpdateDollar.Value = 0;
            decimalUpDownUpdateDollar.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageUpdateDollar, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteDisableEnableDiscountSap_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.EnbaleDisableGlobalDisccountSap(((MyDataComboBox)comboBoxDisableEnableDiscountSap.SelectedValue).Value.ToString());

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusDisableEnableDiscountSap, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeDisableEnableDiscountSap.IsEnabled = true;
            comboBoxDisableEnableDiscountSap.IsEnabled = true;
            comboBoxDisableEnableDiscountSap.SelectedIndex = -1;
            UpdateLabelStatusModeratorMessage(labelMessageDisableEnableDiscountSap, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUpdateAmountMaxPerInvoice_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UpdateAmountMaxPerInvoice(AddDecimalsToQuantity(decimalUpDownUpdateAmountMaxPerInvoice.Value.ToString().Replace(',', '.')));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUpdateAmountMaxPerInvoice, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUpdateAmountMaxPerInvoice.IsEnabled = true;
            decimalUpDownUpdateAmountMaxPerInvoice.Value = 0;
            decimalUpDownUpdateAmountMaxPerInvoice.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageUpdateAmountMaxPerInvoice, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteDisableEnableCloseEmail_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.DisableEnableCloseEmail(((MyDataComboBox)comboBoxDisableEnableCloseEmail.SelectedValue).Value.ToString());

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusDisableEnableCloseEmail, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeDisableEnableCloseEmail.IsEnabled = true;
            comboBoxDisableEnableCloseEmail.IsEnabled = true;
            comboBoxDisableEnableCloseEmail.SelectedIndex = -1;
            UpdateLabelStatusModeratorMessage(labelMessageDisableEnableCloseEmail, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteFailureUpdatePricesDisccountsTotalPos_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.FailureUpdatePricesDisccountsTotalPos(integerUpDownFailureUpdatePricesDisccountsTotalPos.Value.ToString());

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusFailureUpdatePricesDisccountsTotalPos, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
            integerUpDownFailureUpdatePricesDisccountsTotalPos.Value = 0;
            integerUpDownFailureUpdatePricesDisccountsTotalPos.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageFailureUpdatePricesDisccountsTotalPos, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteGlobalDiscount_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.GlobalDiscount(AddDecimalsToQuantity(decimalUpDownGlobalDiscount.Value.ToString().Replace(',', '.')));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusGlobalDiscount, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeGlobalDiscount.IsEnabled = true;
            decimalUpDownGlobalDiscount.Value = 0;
            decimalUpDownGlobalDiscount.IsEnabled = true;
            UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscount, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteGlobalDiscountPerSector_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.GlobalDiscountPerSectors(AddDecimalsToQuantity(decimalUpDownGlobalDiscountPerSector.Value.ToString().Replace(',', '.')), GetSelectedItemsOfListBox(listBoxGlobalDiscountPerSector));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusGlobalDiscountPerSector, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeGlobalDiscountPerSector.IsEnabled = true;
            decimalUpDownGlobalDiscountPerSector.Value = 0;
            decimalUpDownGlobalDiscountPerSector.IsEnabled = true;
            listBoxGlobalDiscountPerSector.IsEnabled = true;
            listBoxGlobalDiscountPerSector.UnselectAll();
            UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerSector, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteGlobalDiscountPerArticle_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.GlobalDiscountPerArticles(AddDecimalsToQuantity(decimalUpDownGlobalDiscountPerArticle.Value.ToString().Replace(',', '.')), FilesManager.ListOfArticlesGlobalDiscountPerArticle);

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusGlobalDiscountPerArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeGlobalDiscountPerArticle.IsEnabled = true;
            decimalUpDownGlobalDiscountPerArticle.Value = 0;
            decimalUpDownGlobalDiscountPerArticle.IsEnabled = true;
            textBoxUploadTextGlobalDiscountPerArticle.Text = string.Empty;
            buttonUploadTextGlobalDiscountPerArticle.IsEnabled = true;
            FilesManager.ListOfArticlesGlobalDiscountPerArticle.Clear();
            UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerArticle, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteDiscountSectorExcludingArticle_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.DiscountSectorExcludingArticle(AddDecimalsToQuantity(decimalUpDownDiscountSectorExcludingArticle.Value.ToString().Replace(',', '.')), GetSelectedItemsOfListBox(listBoxDiscountSectorExcludingArticle), FilesManager.GetArticlesQuotesAndCommas(FilesManager.ListOfArticlesDiscountSectorExcludingArticle));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusDiscountSectorExcludingArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeDiscountSectorExcludingArticle.IsEnabled = true;
            decimalUpDownDiscountSectorExcludingArticle.Value = 0;
            decimalUpDownDiscountSectorExcludingArticle.IsEnabled = true;
            textBoxUploadTExtDiscountSectorExcludingArticle.Text = string.Empty;
            buttonUploadTextDiscountSectorExcludingArticle.IsEnabled = true;
            FilesManager.ListOfArticlesDiscountSectorExcludingArticle.Clear();
            listBoxDiscountSectorExcludingArticle.IsEnabled = true;
            listBoxDiscountSectorExcludingArticle.UnselectAll();
            UpdateLabelStatusModeratorMessage(labelMessageDiscountSectorExcludingArticle, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteDisableEnableSectorByDate_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.DisableEnableSectorByDate(((MyDataComboBox)comboBoxDisableEnableSectorByDate.SelectedValue).Value.ToString());

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusDisableEnableSectorByDate, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeDisableEnableSectorByDate.IsEnabled = true;
            comboBoxDisableEnableSectorByDate.IsEnabled = true;
            comboBoxDisableEnableSectorByDate.SelectedIndex = -1;
            UpdateLabelStatusModeratorMessage(labelMessageDisableEnableSectorByDate, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUpdateSectorByArticle_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UpdateSectorByArticle(FilesManager.ListOfArticleCommaSectorUpdateSectorByArticle);

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUpdateSectorByArticle, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUpdateSectorByArticle.IsEnabled = true;
            textBoxUploadTextUpdateSectorByArticle.Text = string.Empty;
            buttonUploadTextUpdateSectorByArticle.IsEnabled = true;
            FilesManager.ListOfArticleCommaSectorUpdateSectorByArticle.Clear();
            UpdateLabelStatusModeratorMessage(labelMessageUpdateSectorByArticle, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUpdateyArticlesDiscounts_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UpdateDiscocuntsByArticles(FilesManager.ListUpdateArticleCommaDiscountUpdateArticlesDiscounts);

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUpdateArticlesDiscounts, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUpdateArticlesDiscounts.IsEnabled = true;
            textBoxUploadTextUpdateArticlesDiscounts.Text = string.Empty;
            buttonUploadTextUpdateArticlesDiscounts.IsEnabled = true;
            FilesManager.ListUpdateArticleCommaDiscountUpdateArticlesDiscounts.Clear();
            UpdateLabelStatusModeratorMessage(labelMessageUpdateArticlesDiscounts, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUpdateGendersOfArticles_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UpdateGendersByArticles(FilesManager.ListUpdateArticleCommaGenderUpdateArticlesGenders);

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUpdateGendersPerArticles, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUpdateGendersPerArticles.IsEnabled = true;
            textBoxUploadTextUpdateGendersPerArticles.Text = string.Empty;
            buttonUploadTextUpdateGendersPerArticles.IsEnabled = true;
            FilesManager.ListUpdateArticleCommaGenderUpdateArticlesGenders.Clear();
            UpdateLabelStatusModeratorMessage(labelMessageUpdateGendersPerArticles, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteUpdatCategoriessPerArticles_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.UpdateCategoriesByArticles(FilesManager.ListUpdateArticleCommaCategoryUpdateArticlesCategories);

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusUpdateCategoriesPerArticles, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeUpdateCategoriesPerArticles.IsEnabled = true;
            textBoxUploadTextUpdateCategoriesPerArticles.Text = string.Empty;
            buttonUploadTextUpdateCategoriesPerArticles.IsEnabled = true;
            FilesManager.ListUpdateArticleCommaCategoryUpdateArticlesCategories.Clear();
            UpdateLabelStatusModeratorMessage(labelMessageUpdateCategoriesPerArticles, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteGlobalDiscountPerGender_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.GlobalDiscountPerGenders(AddDecimalsToQuantity(decimalUpDownGlobalDiscountPerGender.Value.ToString().Replace(',', '.')), GetSelectedItemsOfListBoxWhitQuotes(listBoxGlobalDiscountPerGender));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusGlobalDiscountPerGender, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeGlobalDiscountPerGender.IsEnabled = true;
            decimalUpDownGlobalDiscountPerGender.Value = 0;
            decimalUpDownGlobalDiscountPerGender.IsEnabled = true;
            listBoxGlobalDiscountPerGender.IsEnabled = true;
            listBoxGlobalDiscountPerGender.UnselectAll();
            UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerGender, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteGlobalDiscountPerCategories_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.GlobalDiscountPerCategories(AddDecimalsToQuantity(decimalUpDownGlobalDiscountPerCategories.Value.ToString().Replace(',', '.')), GetSelectedItemsOfListBoxWhitQuotes(listBoxGlobalDiscountPerCategories));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusGlobalDiscountPerCategories, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeGlobalDiscountPerCategories.IsEnabled = true;
            decimalUpDownGlobalDiscountPerCategories.Value = 0;
            decimalUpDownGlobalDiscountPerCategories.IsEnabled = true;
            listBoxGlobalDiscountPerCategories.IsEnabled = true;
            listBoxGlobalDiscountPerCategories.UnselectAll();
            UpdateLabelStatusModeratorMessage(labelMessageGlobalDiscountPerCategories, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteSetIva_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.SetIva(FilesManager.GetArticlesQuotesAndCommas(FilesManager.ListOfArticlesSetIva));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusSetIva, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeSetIva.IsEnabled = true;
            textBoxUploadTextSetIva.Text = string.Empty;
            buttonUploadTextSetIva.IsEnabled = true;
            FilesManager.ListOfArticlesSetIva.Clear();
            UpdateLabelStatusModeratorMessage(labelMessageSetIva, MODERATOR_MESSAGE_EMPTY);
        }

        private async void buttonExecuteSetExento_Click(object sender, RoutedEventArgs e)
        {
            ((System.Windows.Controls.Button)sender).IsEnabled = false;
            UpdateTabCounter(Tasks.EnumTaskStatusTask.APROBADA, OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tasks task = (Tasks)((System.Windows.Controls.Button)sender).Tag;
            QuickNavigationAproved(new MyDataComboBox((string)task.GroupBox.Tag, ((System.Windows.Controls.TextBlock)((System.Windows.Controls.StackPanel)task.GroupBox.Header).Children[0]).Text), OPERATIONS_ADD_SUBTRAC.SUBTRAC);
            Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> resultSetStatus;

            Tuple<bool, string> tokenResult = NetworkClassLibrary.Token.IsTokenAlive(task.Token);

            if (tokenResult.Item1)
            {
                Tuple<bool, string> result = await mysql.SetExento(FilesManager.GetArticlesQuotesAndCommas(FilesManager.ListOfArticlesSetExento));

                if (result.Item1)
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.EXITOSA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = result.Item2 });
                    System.Windows.MessageBox.Show(result.Item2, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                if (resultSetStatus.Item1)
                {
                    UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.CERRADA, System.Windows.Media.Brushes.Black);
                }
                else
                {
                    Logger.WriteToLog(resultSetStatus.Item2);
                    UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);
                    System.Windows.MessageBox.Show("Error al cerrar la tarea, " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                UpdateLabelStatus(labelStatusSetExento, Tasks.EnumTaskStatusTask.ERROR, System.Windows.Media.Brushes.Black);

                resultSetStatus = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = tokenResult.Item2 });

                if (resultSetStatus.Item1)
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Tarea cerrada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show(tokenResult.Item2 + " Adicionalmente Error al cerrar la tarea: " + resultSetStatus.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(resultSetStatus.Item2);
                }
            }

            buttonSolicitudeSetExento.IsEnabled = true;
            textBoxUploadTextSetExento.Text = string.Empty;
            buttonUploadTextSetExento.IsEnabled = true;
            FilesManager.ListOfArticlesSetExento.Clear();
            UpdateLabelStatusModeratorMessage(labelMessageSetExento, MODERATOR_MESSAGE_EMPTY);
        }

        //Cerrar las tareas pendientes y las aprobadas.
        private async void Window_Closing(object sender, CancelEventArgs e)
        {
            string messaage_local_status = "Aplicacion cerrada y tarea pendiente, se procedio a cerrar la tarea.";
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                
                while (ListOfTask.TasksQueue.Count > 0)
                {
                    Tasks task = new Tasks();
                    if (ListOfTask.TasksQueue.TryDequeue(out task))
                    {
                        Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status});
                        if (result.Item1)
                        {
                            ; //Do nothing
                        }
                        else
                        {
                            Logger.WriteToLog(result.Item2);
                        }
                    }
                }

                await CloseTaskNotExecuted();
            }
        }

        private async Task CloseTaskNotExecuted()
        {
            string messaage_local_status = "Aplicacion cerrada y tarea aprobada no ejecutada, se procedio a cerrar la tarea.";
            if(buttonExecuteUnblockUser.IsEnabled)
            {
                buttonExecuteUnblockUser.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUnblockUser.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUnblockFiscalMachine.IsEnabled)
            {
                buttonExecuteUnblockFiscalMachine.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUnblockFiscalMachine.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteResendCloser.IsEnabled)
            {
                buttonExecuteResendCloser.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteResendCloser.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteCloseCashMachine.IsEnabled)
            {
                buttonExecuteCloseCashMachine.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteCloseCashMachine.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteEqualsFiscalNeto.IsEnabled)
            {
                buttonExecuteEqualsFiscalNeto.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteEqualsFiscalNeto.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteEqualsFiscalNeto.IsEnabled)
            {
                buttonExecuteEqualsFiscalNeto.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteEqualsFiscalNeto.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUpdateAmountPettyCashFund.IsEnabled)
            {
                buttonExecuteUpdateAmountPettyCashFund.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUpdateAmountPettyCashFund.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUpdateEuro.IsEnabled)
            {
                buttonExecuteUpdateEuro.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUpdateEuro.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUpdateDollar.IsEnabled)
            {
                buttonExecuteUpdateDollar.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUpdateDollar.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteDisableEnableDiscountSap.IsEnabled)
            {
                buttonExecuteDisableEnableDiscountSap.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteDisableEnableDiscountSap.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUpdateAmountMaxPerInvoice.IsEnabled)
            {
                buttonExecuteUpdateAmountMaxPerInvoice.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUpdateAmountMaxPerInvoice.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteDisableEnableCloseEmail.IsEnabled)
            {
                buttonExecuteDisableEnableCloseEmail.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteDisableEnableCloseEmail.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteFailureUpdatePricesDisccountsTotalPos.IsEnabled)
            {
                buttonExecuteFailureUpdatePricesDisccountsTotalPos.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteFailureUpdatePricesDisccountsTotalPos.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteGlobalDiscount.IsEnabled)
            {
                buttonExecuteGlobalDiscount.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteGlobalDiscount.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteGlobalDiscountPerSector.IsEnabled)
            {
                buttonExecuteGlobalDiscountPerSector.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteGlobalDiscountPerSector.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteGlobalDiscountPerArticle.IsEnabled)
            {
                buttonExecuteGlobalDiscountPerArticle.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteGlobalDiscountPerArticle.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteDiscountSectorExcludingArticle.IsEnabled)
            {
                buttonExecuteDiscountSectorExcludingArticle.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteDiscountSectorExcludingArticle.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteDisableEnableSectorByDate.IsEnabled)
            {
                buttonExecuteDisableEnableSectorByDate.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteDisableEnableSectorByDate.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUpdateSectorByArticle.IsEnabled)
            {
                buttonExecuteUpdateSectorByArticle.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUpdateSectorByArticle.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUpdateyArticlesDiscounts.IsEnabled)
            {
                buttonExecuteUpdateyArticlesDiscounts.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUpdateyArticlesDiscounts.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUpdateGendersPerArticles.IsEnabled)
            {
                buttonExecuteUpdateGendersPerArticles.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUpdateGendersPerArticles.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteUpdateCategoriessPerArticles.IsEnabled)
            {
                buttonExecuteUpdateCategoriessPerArticles.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteUpdateCategoriessPerArticles.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteGlobalDiscountPerSector.IsEnabled)
            {
                buttonExecuteGlobalDiscountPerSector.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteGlobalDiscountPerSector.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteGlobalDiscountPerCategories.IsEnabled)
            {
                buttonExecuteGlobalDiscountPerCategories.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteGlobalDiscountPerCategories.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteSetIva.IsEnabled)
            {
                buttonExecuteSetIva.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteSetIva.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }
            if (buttonExecuteSetExento.IsEnabled)
            {
                buttonExecuteSetExento.IsEnabled = false;
                Tasks task = (Tasks)buttonExecuteSetExento.Tag;
                Tuple<bool, string, NetworkClassLibrary.Models.TaskStatusAnswerModel> result = await NetworkClassLibrary.Post.SetStatusTask(initialData.IP_WEB_SERVICE + "/api/Task/SetStatusTask", new NetworkClassLibrary.Models.TaskStatusModel { id = task.Id, task_status_id = (int)Tasks.EnumTaskStatusTask.CERRADA, task_status_local = (int)Tasks.EnumTaskStatusLocal.FALLIDA, task_status_local_message = messaage_local_status });
                if (result.Item1)
                {
                    ; //Do nothing
                }
                else
                {
                    System.Windows.MessageBox.Show("Error al cerrar tarea no ejecutada, ver el logger, " + result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.WriteToLog(result.Item2);
                }
            }

            await Task.Run(() => Thread.Sleep(200));
        }

        private async void buttonExecuteUnblockUserOffline_Click(object sender, RoutedEventArgs e)
        {
            buttonExecuteUnblockUserOffline.IsEnabled = false;
            if (comboBoxUnblockUser.SelectedIndex != -1)
            {
                CheckPasswordWindow checkPasswordWindow = new CheckPasswordWindow();
                if (checkPasswordWindow.ShowDialog() == true && checkPasswordWindow.DialogResult.Value == true)
                {
                    Tuple<bool, string> result = await mysql.UnblockUser(comboBoxUnblockUser.SelectedValue.ToString());

                    if (result.Item1)
                    {
                        System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un elemento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            buttonExecuteUnblockUserOffline.IsEnabled = true;
        }

        private async void buttonExecuteUnblockFiscalMachineOffline_Click(object sender, RoutedEventArgs e)
        {
            buttonExecuteUnblockFiscalMachineOffline.IsEnabled = false;
            if (comboBoxUnblockFiscalMachine.SelectedIndex != -1)
            {
                CheckPasswordWindow checkPasswordWindow = new CheckPasswordWindow();
                if (checkPasswordWindow.ShowDialog() == true && checkPasswordWindow.DialogResult.Value == true)
                {
                    Tuple<bool, string> result = await mysql.UnblockFailureFiscalMachine(((MyDataComboBox)comboBoxUnblockFiscalMachine.SelectedValue).Value.ToString());

                    if (result.Item1)
                    {
                        System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un elemento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            buttonExecuteUnblockFiscalMachineOffline.IsEnabled = true;
        }

        private async void buttonExecuteCloseCashMachineOffline_Click(object sender, RoutedEventArgs e)
        {
            buttonExecuteCloseCashMachineOffline.IsEnabled = false;
            if (comboBoxCloseCashMachine.SelectedIndex != -1)
            {
                Tuple<bool, string> existRegister = await mysql.CloseCashMachineIfExistRegister(datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).Value.ToString());

                if (existRegister.Item1)
                {

                    CheckPasswordWindow checkPasswordWindow = new CheckPasswordWindow();
                    if (checkPasswordWindow.ShowDialog() == true && checkPasswordWindow.DialogResult.Value == true)
                    {
                        Tuple<bool, string, double> resultSum = await mysql.CloseCashMachineSumSales(datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).Value.ToString());

                        if (resultSum.Item1)
                        {
                            if (resultSum.Item3 == 0)
                            {
                                Tuple<bool, string> result = await mysql.CloseCashMachine(datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).Value.ToString());
                                if (result.Item1)
                                {
                                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                            else
                            {
                                Tuple<bool, string> result = await mysql.CloseCashMachineZReport(datePickerCloseCashMachine.SelectedDate.Value.ToString("yyyy-MM-dd"), ((MyDataComboBox)comboBoxCloseCashMachine.SelectedItem).Value.ToString());
                                if (result.Item1)
                                {
                                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                }
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(resultSum.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }                        
                }
                else
                {
                    System.Windows.MessageBox.Show(existRegister.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Error debe seleccionar un elemento.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            buttonExecuteCloseCashMachineOffline.IsEnabled = true;
        }


        private async void buttonExecuteUpdateEuroOffline_Click(object sender, RoutedEventArgs e)
        {
            buttonExecuteUpdateEuroOffline.IsEnabled = false;

            CheckPasswordWindow checkPasswordWindow = new CheckPasswordWindow();
            if (checkPasswordWindow.ShowDialog() == true && checkPasswordWindow.DialogResult.Value == true)
            {
                Tuple<bool, string> result = await mysql.UpdateEuroRate(AddDecimalsToQuantity(decimalUpDownUpdateEuro.Value.ToString().Replace(',', '.')));

                if (result.Item1)
                {
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            buttonExecuteUpdateEuroOffline.IsEnabled = true;
        }

        private async void buttonExecuteUpdateDollarOffline_Click(object sender, RoutedEventArgs e)
        {
            buttonExecuteUpdateDollarOffline.IsEnabled = false;

            CheckPasswordWindow checkPasswordWindow = new CheckPasswordWindow();
            if (checkPasswordWindow.ShowDialog() == true && checkPasswordWindow.DialogResult.Value == true)
            {
                Tuple<bool, string> result = await mysql.UpdateDollarRate(AddDecimalsToQuantity(decimalUpDownUpdateDollar.Value.ToString().Replace(',', '.')));

                if (result.Item1)
                {
                    System.Windows.MessageBox.Show("Tarea Ejecutada Exitosamente.", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    System.Windows.MessageBox.Show(result.Item2, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            buttonExecuteUpdateDollarOffline.IsEnabled = true;
        }

        

        
    }
}
