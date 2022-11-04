using Emma.ViewModels.TextboxCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
