using Emma.ViewModels.BaseObservable;
using System;
using System.Windows.Media.Imaging;

namespace Emma.ViewModels.ImageCommands
{
    public class ImageUpdate : ObservableObject
    {
        public ImageUpdate() { 
        }
        private BitmapImage? _emmasimage = null;
        public BitmapImage EmmasImage
        {
            get
            {
                if (_emmasimage == null)
                    return new BitmapImage(new Uri("/Emma;component/images/new_base.png", UriKind.Relative));
                return _emmasimage;
            }
            set
            {
                _emmasimage = value;
                OnPropertyChanged("EmmasImage");
            }
        }
    }
}
