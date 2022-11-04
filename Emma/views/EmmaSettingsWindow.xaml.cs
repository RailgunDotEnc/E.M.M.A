using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Emma.views
{
    /// <summary>
    /// Interaction logic for EmmaSettingsWindow.xaml
    /// </summary>
    public partial class EmmaSettingsWindow : Window
    {
        private string[] new_settings = {" "," "," "," "," "," "," "," "};
        private string[] old_settings = { " ", " ", " ", " ", " ", " ", " ", " " };
        public EmmaSettingsWindow()
        {
            InitializeComponent();
            set_settings();
        }
        public void set_settings() {
            old_settings=App.basemodel.Emmasettings.get_settings_list();
            update_window(old_settings);
        }
        //update window settins
        public void update_window(string[] list) {
            if (list[0].Contains("true"))
            {
                Startup.Content = "[On]";
                Startup.Foreground = Brushes.Green;
            }
            else {
                Startup.Content = "[Off]";
                Startup.Foreground = Brushes.Red;
            }
            if (list[1].Contains("true"))
            {
                Email.Content = "[On]";
                Email.Foreground = Brushes.Green;
            }
            else
            {
                Email.Content = "[Off]";
                Email.Foreground = Brushes.Red;
            }
            if (list[2].Contains("true"))
            {
                Weather.Content = "[On]";
                Weather.Foreground = Brushes.Green;
            }
            else
            {
                Weather.Content = "[Off]";
                Weather.Foreground = Brushes.Red;
            }
            if (list[3].Contains("true"))
            {
                Folder.Content = "[On]";
                Folder.Foreground = Brushes.Green;
            }
            else
            {
                Folder.Content = "[Off]";
                Folder.Foreground = Brushes.Red;
            }
            if (list[4].Contains("true"))
            {
                RunData.Content = "[On]";
                RunData.Foreground = Brushes.Green;
            }
            else
            {
                RunData.Content = "[Off]";
                RunData.Foreground = Brushes.Red;
            }
            if (list[5].Contains("true"))
            {
                Notification.Content = "[On]";
                Notification.Foreground = Brushes.Green;
            }
            else
            {
                Notification.Content = "[Off]";
                Notification.Foreground = Brushes.Red;
            }
            Attemps.Text = App.basemodel.Emmasettings.get_listeningAttempts().ToString();
            Volume.Text = App.basemodel.Emmasettings.get_volume().ToString();
            old_settings[6] = Attemps.Text.ToString();
            old_settings[7] = Volume.Text.ToString();
            for (int i = 0; i < old_settings.Length; i++)
                new_settings[i] = old_settings[i];
            Save.Content = "[Save]";
        }
        //button actions
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Startup.Content.ToString().Contains("On"))
            {
                Startup.Content = "[Off]";
                Startup.Foreground = Brushes.Red;
                new_settings[0] = "startUp:false";
            }
            else {
                Startup.Content = "[On]";
                Startup.Foreground = Brushes.Green;
                new_settings[0] = "startUp:true";
            }
        }

        private void Email_Click(object sender, RoutedEventArgs e)
        {
            if (Email.Content.ToString().Contains("On"))
            {
                Email.Content = "[Off]";
                Email.Foreground = Brushes.Red;
                new_settings[1] = "emailCheck:false";
            }
            else
            {
                Email.Content = "[On]";
                Email.Foreground = Brushes.Green;
                new_settings[1] = "emailCheck:true";
            }

        }

        private void Weather_Click(object sender, RoutedEventArgs e)
        {
            if (Weather.Content.ToString().Contains("On"))
            {
                Weather.Content = "[Off]";
                Weather.Foreground = Brushes.Red;
                new_settings[2] = "weather:false";
            }
            else
            {
                Weather.Content = "[On]";
                Weather.Foreground = Brushes.Green;
                new_settings[2] = "weather:true";
            }
        }

        private void Folder_Click(object sender, RoutedEventArgs e)
        {
            if (Folder.Content.ToString().Contains("On"))
            {
                Folder.Content = "[Off]";
                Folder.Foreground = Brushes.Red;
                new_settings[3] = "orginizeFolder:false";
            }
            else
            {
                Folder.Content = "[On]";
                Folder.Foreground = Brushes.Green;
                new_settings[3] = "orginizeFolder:true";
            }
        }

        private void RunData_Click(object sender, RoutedEventArgs e)
        {
            if (RunData.Content.ToString().Contains("On"))
            {
                RunData.Content = "[Off]";
                RunData.Foreground = Brushes.Red;
                new_settings[4] = "saveRunData:false";
            }
            else
            {
                RunData.Content = "[On]";
                RunData.Foreground = Brushes.Green;
                new_settings[4] = "saveRunData:true";
            }
        }

        private void Notification_Click(object sender, RoutedEventArgs e)
        {
            if (Notification.Content.ToString().Contains("On"))
            {
                Notification.Content = "[Off]";
                Notification.Foreground = Brushes.Red;
                new_settings[5] = "reminder:false";
            }
            else
            {
                Notification.Content = "[On]";
                Notification.Foreground = Brushes.Green;
                new_settings[5] = "reminder:true";
            }
        }

        //Close everything
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //Revert to old_settings
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < new_settings.Length; i++)
            {
                if (new_settings[i] != old_settings[i] || Attemps.Text.ToString() == old_settings[6] || old_settings[7] == Volume.Text.ToString())
                {
                    update_window(old_settings);
                    return;
                }
            }
            Revert.Content = "[None]";
            await Task.Delay(500);
            Revert.Content = "[Revert]";

        }
        //Save update
        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < new_settings.Length; i++)
            {
                if (new_settings[i] != old_settings[i] || Attemps.Text.ToString()!= old_settings[6] || old_settings[7] != Volume.Text.ToString())
                {
                    try
                    {
                        int.Parse(Attemps.Text.ToString());
                        int.Parse(Volume.Text.ToString());
                        Save.Content = "[Saving...]";
                        new_settings[6] = Attemps.Text.ToString();
                        new_settings[7] = Volume.Text.ToString();
                        for (int j = 0; j < new_settings.Length; j++) {
                            old_settings[j] = new_settings[j];
                            await Task.Delay(50);
                        }
                        App.basemodel.Emmasettings.update_file(new_settings);
                        Save.Content = "[Save]";
                        return;
                    }
                    catch (Exception ex)
                    {
                        Save.Content = "[Error]";
                        update_window(old_settings);
                        return;
                    }
                }
            }
            Save.Content = "[No Changes]";
            await Task.Delay(500);
            Save.Content = "[Save]";

        }
    }
}
