using Emma.ViewModels.BaseObservable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Emma.ViewModels.TextboxCommands
{
    public class TextBoxUpdates : ObservableObject
    {
        private BitmapImage? _emmastextbox = null;
        private String? _speech = null;
        public BitmapImage? TextBoxImage
        {
            get
            {
                if (_emmastextbox == null)
                    return null;
                return new BitmapImage(new Uri("/Emma;component/images/Text_box.png", UriKind.Relative));
            }
            set
            {
                _emmastextbox = value;
                OnPropertyChanged("TextBoxImage");
            }
        }
        public String? Speech
        {
            get
            {
                if (_speech == null) {
                    return "";
                }
                return _speech;
            }
            set
            {
                _speech = value;
                OnPropertyChanged("Speech");
            }
        }
    }


}
