using Emma.ViewModels.TextboxCommands;

namespace Emma.ViewModels
{
    public class ViewModelTextBox
    {
        public static TextBoxUpdates? TextBox { get; private set; }
        public ViewModelTextBox()
        {
            TextBox = new TextBoxUpdates();
            App.basemodel.text_box.set_ViewModel(TextBox);
        }
    }
}
