using System;
using System.Windows.Media.Imaging;
using Emma.ViewModels.ImageCommands;

namespace Emma.Model.Model_Subsets
{
    public class Animation
    {
        #region variables
        //List of images
        BitmapImage done =new BitmapImage(new Uri("/Emma;component/images/new_done.png", UriKind.Relative));
        BitmapImage error = new BitmapImage(new Uri("/Emma;component/images/new_error.png", UriKind.Relative));
        BitmapImage laugh = new BitmapImage(new Uri("/Emma;component/images/new_laugh.png", UriKind.Relative));
        BitmapImage listening = new BitmapImage(new Uri("/Emma;component/images/new_listening.png", UriKind.Relative));
        BitmapImage mad = new BitmapImage(new Uri("/Emma;component/images/new_mad.png", UriKind.Relative));
        BitmapImage sad = new BitmapImage(new Uri("/Emma;component/images/new_sad.png", UriKind.Relative));
        BitmapImage searching = new BitmapImage(new Uri("/Emma;component/images/new_searching.png", UriKind.Relative));
        BitmapImage sleeping = new BitmapImage(new Uri("/Emma;component/images/new_sleeping.png", UriKind.Relative));
        //Get view model access
        ImageUpdate? imageupdate =null;
        //Current image to use
        BitmapImage? current = null;
        #endregion
        //Constructor
        public void imageupdate_set(ImageUpdate value) { 
            imageupdate = value;
        }

        //Update image for gui
        void updateimage(BitmapImage? value) {
            current= value;
            imageupdate.EmmasImage = current;  
        }

        //String to bitmap 
        public void setImage(string? value =null) {
            if (value == null)
                updateimage(null);
            else if (value.Equals("laugh"))
                updateimage(laugh);
            else if (value.Equals("done"))
                updateimage(done);
            else if (value.Equals("error"))
                updateimage(error);
            else if (value.Equals("listening"))
                updateimage(listening);
            else if (value.Equals("mad"))
                updateimage(mad);
            else if (value.Equals("sad"))
                updateimage(sad);
            else if (value.Equals("searching"))
                updateimage(searching);
            else if (value.Equals("sleeping"))
                updateimage(sleeping);
        }
        
    }
}
