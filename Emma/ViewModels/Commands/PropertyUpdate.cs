using System;
using System.Windows.Media.Imaging;
using System.Windows;
using Emma.ViewModels.BaseObservable;

namespace Emma.ViewModels.Commands
{
    public class PropertyUpdate:ObservableObject
    {
        private string _display=App.basemodel.runtimedata.display;
        private string _time=App.basemodel.runtimedata.time;
        private BitmapImage? _wifi = null;
        private BitmapImage? _listening = null;
        private BitmapImage? _weather_image = null;
        private Visibility _menu = Visibility.Visible;
        private BitmapImage? _menu_image = new BitmapImage(new Uri("/Emma;component/images/frame.png", UriKind.Relative));
        
        public string Display
        {
            get
            {
                if (string.IsNullOrEmpty(_display))
                    return "Unk";
                return _display;
            }
            set
            {
                _display = value;
                OnPropertyChanged("Display");
            }
        }
        public string Time
        {
            get
            {
                if (string.IsNullOrEmpty(_time))
                    return "";
                return _time;
            }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }
        public BitmapImage Wifi
        {
            get
            {
                if (_wifi == null)
                    return new BitmapImage(new Uri("/Emma;component/images/icons/no_wifi.png", UriKind.Relative));
                return _wifi;
            }
            set
            {
                _wifi = value;
                OnPropertyChanged("Wifi");
            }
        }
        public BitmapImage Listening
        {
            get
            {
                if (_listening == null)
                    return new BitmapImage(new Uri("/Emma;component/images/icons/off.png", UriKind.Relative));
                return _listening;
            }
            set
            {
                _listening = value;
                OnPropertyChanged("Listening");
            }
        }
        public Visibility Menu
        {
            get
            {
                return _menu;
            }
            set
            {
                _menu = value;
                OnPropertyChanged("Menu");
            }
        }
        public BitmapImage Menu_Image
        {
            get
            {
                if (_menu_image == null)
                    return null;
                return new BitmapImage(new Uri("/Emma;component/images/frame.png", UriKind.Relative)); 
            }
            set
            {
                _menu_image = value;
                OnPropertyChanged("Menu_Image");
            }
        }
        public BitmapImage Weatehr_Image
        {
            get
            {
                if (_weather_image == null)
                    return new BitmapImage(new Uri("/Emma;component/images/icons/temp.png", UriKind.Relative));
                return new BitmapImage(new Uri("/Emma;component/images/icons/rain.png", UriKind.Relative));
            }
            set
            {
                _weather_image = value;
                OnPropertyChanged("Weatehr_Image");
            }
        }


    }
}
