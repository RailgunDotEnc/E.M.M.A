using Emma.ViewModels.Commands;
using Emma.views;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Emma.ViewModels
{
    public class ViewModelUI
    {
        public ButtonPressedCommand CC { get; private set; }
        public ButtonPressedCommand Min { get; private set; }
        public ButtonPressedCommand OF { get; private set; }
        public ButtonPressedCommand OMem { get; private set; }
        public ButtonPressedCommand MenuB { get; private set; }
        public ButtonPressedCommand OSettings { get; private set; }
        public static PropertyUpdate? Property { get; private set; }
        

        public ViewModelUI()
        {

            CC = new ButtonPressedCommand(Close_App);
            Min = new ButtonPressedCommand(Min_App);
            OF = new ButtonPressedCommand(Folder_Open);
            OMem = new ButtonPressedCommand(Open_Memory);
            MenuB = new ButtonPressedCommand(Menu_Button);
            OSettings = new ButtonPressedCommand(Settings_Button);
            Property = new PropertyUpdate();
            //Connect Runtime data and property update
            App.basemodel.runtimedata.addpropertyclass(Property);
            App.basemodel.runtimedata.UpdateDiagnostics();
            //Connect Weather data and property update
            App.basemodel.weather.addpropertyclass(Property);
            //Connect Wifi Data and property update
            App.basemodel.Wifi_Data.addpropertyclass(Property);
            App.basemodel.Wifi_Data.check_wifi();
            Menu_Button();
        }

        public static async void Close_App()
        {
            App.basemodel.runtimedata.emma_running = false;
            App.basemodel.memory.SaveData("Program closed");
            await Task.Delay(200);
            Application.Current.Shutdown();
        }
        public static void Min_App()
        {
            App.basemodel.animation.setImage("sleeping");
            App.basemodel.runtimedata.emma_sleeping = true;
            App.basemodel.runtimedata.min_sleep = true;
            var window = Application.Current.MainWindow;
            window.WindowState = WindowState.Minimized;
        }
        public static void Folder_Open()
        {
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            var CurrentDirectory = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            CurrentDirectory = CurrentDirectory.Replace("\\Emma\\bin\\Debug\\net6.0-windows", String.Empty);
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
            Process.Start("explorer.exe", CurrentDirectory);
        }
        public static void Open_Memory() {

            string fileName = App.basemodel.memory.current_file;
            Process.Start("notepad.exe", fileName);
        }
        public static void Menu_Button() {
            if (Property.Menu == Visibility.Visible)
            {
                Property.Menu = Visibility.Collapsed;
                Property.Time = null;
                Property.Menu_Image = null;
            }
            else {
                Property.Menu = Visibility.Visible;
                App.basemodel.runtimedata.UpdateDiagnostics();
                Property.Menu_Image = new BitmapImage(new Uri("/Emma;component/images/frame.png", UriKind.Relative));
            }
        }
        public static void Settings_Button() {
            EmmaSettingsWindow settings = new EmmaSettingsWindow();
            settings.Show();
        }
        
    }
}
