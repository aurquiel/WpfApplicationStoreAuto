using LogLibraryClassLibrary;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media;

namespace WpfApplicationStoreAuto
{
    /// <summary>
    /// Interaction logic for CheckPasswordOfflineWindow.xaml
    /// </summary>
    public partial class CheckPasswordWindow : Window
    {
        public CheckPasswordWindow()
        {
            InitializeComponent();
        }

        private void TextBoxStatusUpdate(string text, Brush colorBrush)
        {
            textBoxStatus.Dispatcher.Invoke(() =>
            {
                textBoxStatus.Foreground = colorBrush;
                textBoxStatus.Text = text;
            });
        }

        private void buttonCheckPasswordOffline_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string password = ConfigurationManager.AppSettings["EXECUTE_TASK_OFFLINE_PASSWORD"].ToString();
                if(password == passwordBoxCheckPasswordOffline.Password)
                {
                    this.DialogResult = true;
                    TextBoxStatusUpdate("Contraseña verificada", Brushes.Green);
                    Close();
                }
                else
                {
                    TextBoxStatusUpdate("Contraseña invalida", Brushes.Red);
                }
            }
            catch(Exception ex)
            {
                Logger.WriteToLog("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower());
                TextBoxStatusUpdate("Metodo: " + ex.TargetSite + ", Error: " + ex.Message.ToLower(), Brushes.Red);
            }
        }

        private void buttonCheckPasswordOfflineClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
