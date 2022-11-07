using Emma.API;
using Emma.Model.Model_Subsets;
using System.Windows;

namespace Emma.Model
{
    
    public class BaseModel
    {
        #region Variables
        public Wifi_Data Wifi_Data { get; private set; }
        public RunTimeData runtimedata { get;private set;}
        public Memory memory { get; private set; }
        public Animation animation { get; private set; }
        public API_Call speech_text { get; private set; }
        public TextBox text_box { get; private set; }
        public settings Emmasettings { get; private set; }
        public Weather weather { get; private set; }
        public Sound sound { get; private set; }
        #endregion

        //Constructor
        public BaseModel() {
            runtimedata = new RunTimeData();
            Emmasettings = new settings(runtimedata.directroy);
            if (Emmasettings.get_startup() == false) { 
                Application.Current.Shutdown();
            }
            Wifi_Data = new Wifi_Data();
            memory = new Memory();
            animation=new Animation();
            speech_text = new API_Call(runtimedata.directroy);
            text_box = new TextBox();
            weather = new Weather(speech_text,runtimedata.directroy);
            sound = new Sound(runtimedata.directroy);
        }

    }
}
