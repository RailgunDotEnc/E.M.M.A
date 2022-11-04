using Emma.ViewModels.ImageCommands;


namespace Emma.ViewModels
{
    public class ViewModelQue
    {
        public static ImageUpdate? Images { get; private set; }
        public ViewModelQue() {
            Images = new ImageUpdate();
            //Link Animation model
            App.basemodel.animation.imageupdate_set(Images);
        }
    }
}
