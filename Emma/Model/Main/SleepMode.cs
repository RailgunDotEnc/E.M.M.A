using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System;
using Newtonsoft.Json;

namespace Emma.Model.Main
{
    public class SleepMode
    {
        private BaseModel Current;
        string? response = null;

        //Constructor
        public SleepMode(BaseModel value) {
            Current = value;
        }

        //Sleeping check list
        public async Task sleeping()
        {
            var window = Application.Current.MainWindow;
            string? response = null;
            bool apiOn;
            Thread apiThread = new Thread(Current.speech_text.Listen);
            Current.animation.setImage("sleeping");
            Current.text_box.set_speech("Zzzzz...");
            await Task.Delay(1000);

            //Check if api is on
            apiOn=await check_ping();

            if (apiOn)
            {
                //Check sound for hey emma
                response = await Listening(apiThread);
                dynamic hold= JsonConvert.DeserializeObject(response);
                response = hold["message"];
                response = response.ToLower();
                if (response.Contains("hey emma"))
                    wakeup(window);
            }
            

            //Breaks sleep if emma is no longer in min from min sleep
            if (Current.runtimedata.min_sleep == true && window.WindowState != WindowState.Minimized)
                wakeup(window);
        }

        //Check api for sound
        public async Task<string> Listening(Thread apiThread) {
            string? response = null;
            Current.runtimedata.listening_button_on();
            apiThread.Start();
            await Task.Delay(400);

            while (true)
            {
                response = Current.speech_text.GetResponse();
                if (response != null)
                { break; }
                if (Current.runtimedata.emma_running == false)
                {
                    apiThread.Interrupt();
                }
                await Task.Delay(100);
            }
            Current.runtimedata.listening_button_off();
            return response;
        }

        //Wake up process
        public void wakeup(Window window) {
            Current.runtimedata.emma_sleeping = false;
            Current.runtimedata.min_sleep = false;
            window.WindowState = WindowState.Normal;
        }

        //Check if api is on
        public async Task<bool> check_ping() {
            Thread apiPing = new Thread(Current.speech_text.Ping);
            string check;
            apiPing.Start();
            while (true)
            {
                check = Current.speech_text.GetResponse();
                if (check != null)
                { break; }
                await Task.Delay(100);
            }
            apiPing = null;
            if(check.Contains("true"))
                return true;
            return false;
        }
    }
}
