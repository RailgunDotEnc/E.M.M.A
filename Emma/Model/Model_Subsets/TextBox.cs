using Emma.ViewModels.TextboxCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Emma.Model.Model_Subsets
{
    public class TextBox
    {
        #region variables
        private TextBoxUpdates TBU=null;
        public bool TextboxStatus = false;
        private String? speech = null;
        #endregion

        //Get access to view model
        public void set_ViewModel(TextBoxUpdates value) { 
            TBU=value;
        }
        //Turn on textbox
        public void Textbox_on() {
            TBU.TextBoxImage = new BitmapImage(new Uri("/Emma;component/images/Text_box.png", UriKind.Relative));
            TextboxStatus = true;
        }

        //Turn off textbox
        public void Textbox_off() {
            TBU.TextBoxImage = null;
            TextboxStatus = false;
        }

        //Print text
        public async void set_speech(String value) {
            Textbox_on();
            speech = value;
            TBU.Speech = speech;
            await Task.Delay(1000);
            clean_speech();
        }

        //Delete everyting
        private void clean_speech() {
            TBU.Speech = null;
            Textbox_off();
        }
    }
}
