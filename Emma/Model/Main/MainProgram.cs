using Emma.API;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Emma.Model.Main
{
    public class MainProgram
    {
        #region variables
        private BaseModel Current;
        private bool wifi_on = false;
        private bool emma_sleeping = false;
        private string? response =null;
        private bool listening=false;
        private bool commanded_activated=false;
        private MainCommands maincommands;
        private SleepMode sleepmode;
        #endregion

        //Constructor
        public MainProgram(BaseModel value)
        {
            Current = value;
            maincommands = new MainCommands(Current);
            Task.Run(wifi_loop);
            Task.Run(time_loop);
            run();
            sleepmode = new SleepMode(Current);
        }

        //run Main program
        public async void run()
        {
            Thread mainthread = Thread.CurrentThread;
            mainthread.Name = "Main Thread";
            var window=Application.Current.MainWindow;

            while (true)
            {
                //Set up variables
                await Build_Variables();
                await Task.Delay(2000);
                Current.memory.SaveData("Start");
                //Emma doing routine
                if (wifi_on == true && Current.runtimedata.emma_sleeping == false && App.basemodel.runtimedata.dragged == false && window.WindowState != WindowState.Minimized)
                    await Tasks(Current.runtimedata.threads[0]);

                //Action if there is no wifi
                else if (App.basemodel.Wifi_Data.get_wifi_on() == false)
                {
                    Current.animation.setImage("error");
                    Current.text_box.set_speech("No wifi");
                }
                
                //Result if app istold to sleep
                if (Current.runtimedata.emma_sleeping == true)
                    emma_sleeping=await Sleeping();

                await Task.Delay(2000);

                //Clean cut when program ends
                if (Current.runtimedata.emma_running == false)
                    break;

                //Laugh and clean cycle
                await Clean_Variables();
            }
        }

        //Emma's tasks
        public async Task Tasks(Thread apiThread) {

            Current.memory.SaveData("Starting Tasks");
             //Call api and set up listening
             response = await Listening(apiThread);
            
            //Options depending on what the api response was
            if (response.Contains("emma") && Current.runtimedata.emma_sleeping == false)
                commanded_activated = await Act(response);

            //If there is a connection error to api
            else if (response.Contains("error") && Current.runtimedata.emma_sleeping == false)
                await APIConnectionError(response);

            //if there is no sound
            else if (response.Equals("no sound") && Current.runtimedata.emma_sleeping == false)
                await NoSoundError(response);
            else
                await NoCommandError(response);
            Current.memory.SaveData("Done with Tasks");
        }

        //No sound error
        public async Task NoSoundError(string response)
        {
            Current.animation.setImage("mad");
            Current.runtimedata.attemps++;
            Current.text_box.set_speech(response);
            //if to many failed attemps, go to sleep mode
            if (Current.runtimedata.attemps >= Current.Emmasettings.get_listeningAttempts())
            {
                Current.memory.SaveData("Error[No Sound]");
                Current.runtimedata.emma_sleeping = true;
            }
            await Task.Delay(1000);
        }

        //API Connection error
        public async Task APIConnectionError(string response) {
            Current.animation.setImage("error");
            Current.runtimedata.attemps++;
            Current.text_box.set_speech(response);
            await Task.Delay(1000);
            //if to many failed attemps, go to sleep mode
            if (Current.runtimedata.attemps >= Current.Emmasettings.get_listeningAttempts())
            {
                Current.memory.SaveData("Error[Fail to connect to api]");
                Current.runtimedata.emma_sleeping = true;
            }
        }

        //No Command error
        public async Task NoCommandError(string response) {
            Current.text_box.set_speech("You said "+response);
            await Task.Delay(1000);
        }

        //Build variables
        public async Task Build_Variables() {
            Current.runtimedata.threads.Add(new Thread(() => Current.speech_text.Listen()));
            //Base image set
            Current.animation.setImage(null);
            Current.text_box.set_speech("Hi");
            //Check if wifi is on
            wifi_on = Current.Wifi_Data.get_wifi_on();
        }

        //Clean variables
        public async Task Clean_Variables() {
            Current.animation.setImage("laugh");
            Current.runtimedata.threads.RemoveAt(0);
            commanded_activated = false;
            maincommands.clean_command();
            listening = false;
            response = null;
            await Task.Delay(1000);
            Current.memory.SaveData("End");
        }

        //Call and activate command
        public async Task<Boolean> Act(string response)
        {
            commanded_activated = maincommands.Action(response);
            while (true)
            {
                if (maincommands.get_last_command() != null)
                    break;
                if (Current.runtimedata.emma_sleeping == true)
                    break;
                await Task.Delay(100);
            }
            if (commanded_activated)
            {
                //Conformation wheather called worked or not
                Current.animation.setImage("done");
                return true;
            }
            return false;
        }

        //Sleep mode
        public async Task<Boolean> Sleeping() {
            emma_sleeping = true;
            Current.memory.SaveData("Sleeping");
            while (emma_sleeping == true)
            {
                await sleepmode.sleeping();
                if (Current.runtimedata.emma_sleeping == false)
                    break;
                await Task.Delay(5000);
                emma_sleeping = Current.runtimedata.emma_sleeping;
            }
            Current.memory.SaveData("Wake up");
            return false;
        }

        //Call api to get sound
        public async Task<string> Listening(Thread apiThread) {
            bool Ping = await check_ping();
            Current.memory.SaveData("Start listening aciton");
            if (Ping == true)
            {
                listening = true;
                apiThread.Start();
                Current.animation.setImage("listening");
                Current.runtimedata.listening_button_on();
                await Task.Delay(500);
                while (true)
                {
                    response = Current.speech_text.GetResponse();
                    if (response != null)
                        break;
                    if (Current.runtimedata.emma_running == false)
                        break;
                    await Task.Delay(100);
                }

                Current.runtimedata.listening_button_off();
                Current.memory.SaveData(response);
                if (response != null)
                {
                    dynamic hold = JsonConvert.DeserializeObject(response);
                    response = hold["message"];
                    response = response.ToLower();
                }
                else if(response==null)
                    response = "";
                return response;
            }
            else
                return "connection error";
        }

        //check wifi
        public async void wifi_loop()
        {
            bool dragged;
            while (true)
            {
                if (Current.runtimedata.emma_running == true)
                {
                    Thread wifi_thread = new Thread(Current.Wifi_Data.check_wifi);
                    dragged = App.basemodel.runtimedata.dragged;
                    Current.memory.SaveData("Wifi Check");
                    wifi_thread.Start();
                }
                else break;
                await Task.Delay(5000);
            }
        }

        //Check time
        public async void time_loop()
        {
            string temp;
            while (true)
            {

                if (Current.runtimedata.emma_running != false)
                {
                    Current.memory.SaveData("Check time");
                    Current.runtimedata.UpdateDiagnostics();
                    emma_sleeping = Current.runtimedata.emma_sleeping;
                    if (Current.runtimedata.emma_running == false)
                        break;
                    await check_weather();
                }
                else break;
                
                await Task.Delay(10000);
            }
        }

        //Check weather
        public async Task check_weather() {
            API_Call API= new API_Call(Current.runtimedata.directroy);
            Thread weather = new Thread(API.Weather);
            string check;
            weather.Start();
            while (true)
            {
                check = API.GetResponse();
                if (check != null)
                { break; }
                await Task.Delay(100);
            }

            dynamic hold = JsonConvert.DeserializeObject(check);
            weather = null;
            return;
        }

        //Check api
        public async Task<bool> check_ping()
        {
            Current.memory.SaveData("Check api ping");
            API_Call apc = new API_Call();
            Thread apiPing = new Thread(apc.Ping);
            string check;
            apiPing.Start();
            //infinite loop??????????????????????????????????????
            while (true)
            {
                check = apc.GetResponse();
                if (check != null)
                { break; }
                await Task.Delay(100);
            }
            apiPing = null;
            if (check.Contains("true"))
                return true;
            return false;
        }
    }
}
