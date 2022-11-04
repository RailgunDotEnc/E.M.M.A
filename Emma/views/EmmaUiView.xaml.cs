using Emma.ViewModels;
using System;

using System.Windows.Controls;

using System.Windows.Input;

using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading.Tasks;

namespace Emma.views
{
    /// <summary>
    /// Interaction logic for EmmaUiView.xaml
    /// </summary>
    public partial class EmmaUiView : UserControl
    {
        public EmmaUiView()
        {
            InitializeComponent();
            DataContext = new ViewModelUI();
            Menu_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/open_frame.png", UriKind.Relative));
            Menu_Button.Margin = new Thickness(128, -5, 0, 463);
            App.basemodel.runtimedata.menu_up = true;

        }
        //Effect of pressing a button


        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Min_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/minimize_d.png", UriKind.Relative));
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            Folder_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/folder_d.png", UriKind.Relative));
        }

        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            Mem_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/memory_d.png", UriKind.Relative));
        }

        private void Image_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            Settings_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/settings_d.png", UriKind.Relative));
        }
        //When mouse is moved but button was not activated

        private void Button_MouseLeave_1(object sender, MouseEventArgs e)
        {
            Min_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/minimize.png", UriKind.Relative));
        }

        private void Button_MouseLeave_2(object sender, MouseEventArgs e)
        {
            Folder_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/folder.png", UriKind.Relative));
        }

        private void Button_MouseLeave_3(object sender, MouseEventArgs e)
        {
            Mem_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/memory.png", UriKind.Relative));
        }

        private void Button_MouseLeave_4(object sender, MouseEventArgs e)
        {
            Settings_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/settings.png", UriKind.Relative));
        }

        private async void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await Task.Delay(150);
            if (App.basemodel.runtimedata.menu_up == false)
            {
                Menu_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/open_frame.png", UriKind.Relative));
                Menu_Button.Margin=new Thickness(128,-5, 0, 463);
                App.basemodel.runtimedata.menu_up = true;
            }
            else {
                Menu_Button.Source = new BitmapImage(new Uri("/Emma;component/images/icons/close_frame.png", UriKind.Relative));
                Menu_Button.Margin = new Thickness(128, 324, 0, 163);
                App.basemodel.runtimedata.menu_up = false;
            }
                
        }


    }
}
