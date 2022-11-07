using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Forms;
using VisioForge.Libs.NAudio.CoreAudioApi;
using Emma.Model.Driver;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Emma.Model.Main
{
    public class MainCommands
    {
        #region variables
        private BaseModel Current;
        private string? last_command = null;
        private MMDevice _playbackDevice;
        #endregion

        //Constructor
        public MainCommands(BaseModel value) { 
            Current = value;
        }

        //Command actions
        public async Task<Boolean> Action(String command) {
            last_command = command;
            //Emergancy cancel
            if (command.Contains("cancel") || command.Contains("stop"))
            {
                last_command = "Cancelled";
                return false;
            }
            //Shut down computer
            else if (command.Contains("Shutdown computer") || command.Contains("shut down computer"))
            {
                Current.runtimedata.emma_running = false;
                Current.memory.SaveData("ShutDown Computer");
                Current.sound.voice("bye");
                await Task.Delay(500);
                Process.Start("shutdown", "/s /t 0");
                System.Windows.Application.Current.Shutdown();
                return true;
            }
            //Restart computer
            else if (command.Contains("restart computer"))
            {
                Current.sound.voice("bye");
                await Task.Delay(500);
                Process.Start("shutdown", "/r /t 0");
                return true;
            }
            //shutdown emma
            else if (command.Contains("shutdown") || command.Contains("go away"))
            {
                Current.sound.voice("bye");
                await Task.Delay(500);
                Current.runtimedata.emma_running = false;
                System.Windows.Application.Current.Shutdown();
                return true;
            }
            //sleep command
            else if (command.Contains("sleep"))
            {
                Current.runtimedata.emma_sleeping = true;
                return true;
            }
            //Minimize the window as if clicking button [Testing needed]
            else if (command.Contains("minimize"))
            {
                App.basemodel.animation.setImage("sleeping");
                App.basemodel.runtimedata.emma_sleeping = true;
                App.basemodel.runtimedata.min_sleep = true;
                var window = System.Windows.Application.Current.MainWindow;
                window.WindowState = WindowState.Minimized;
                return true;
            }
            //Move emma [tesint needed]
            else if (command.Contains("move"))
            {
                var window = System.Windows.Application.Current.MainWindow;
                if (command.Contains("top right") || command.Contains("top-right"))
                {
                    window.Left = 1000;
                    window.Top = 5;
                }
                else if (command.Contains("bottom right") || command.Contains("bottom-right"))
                {
                    window.Left = 1000;
                    window.Top = 400;
                }
                else if (command.Contains("bottom left") || command.Contains("bottom-left"))
                {
                    window.Left = 0;
                    window.Top = 400;
                }
                else if (command.Contains("top left") || command.Contains("top-left"))
                {
                    window.Left = 0;
                    window.Top = 5;
                }
                else
                {
                    last_command = "none";
                    return false;
                }
                return true;
            }
            //Web search
            else if (command.Contains("look up") || command.Contains("search"))
            {
                string search = "";
                if (command.Contains("look up"))
                    search = command.Substring(command.IndexOf("look up ") + 8);
                else
                    search = command.Substring(command.IndexOf("search ") + 7);
                search = search.Replace(" ", "+");
                System.Diagnostics.Process.Start("cmd", "/c start https://www.google.no/search?q=" + search);
                return true;
            }
            //Change volume
            else if (command.Contains("change volume") || command.Contains("lower volume") || command.Contains("raise volume"))
            {
                string number = "";
                for (int i = 0; i < command.Length; i++)
                {
                    if (command[i] >= '0' && command[i] <= '9')
                        number = number + command[i];
                }
                if (number.Equals(""))
                    return false;
                AudioManager.SetMasterVolume(int.Parse(number));
                return true;
            }
            //EXE Close [testing needed]
            else if (command.Contains("kill") || command.Contains("close"))
            {
                string killprogram = "";
                string commandsub = command.Substring(command.IndexOf("kill")+5);

                for (int i = 0; i < App.basemodel.runtimedata.RunningExe.Count; i++)
                {
                    killprogram = App.basemodel.runtimedata.RunningExe[i];
                    if (commandsub.Contains(killprogram.ToLower()))
                    {
                        App.basemodel.text_box.set_speech("Killed Program: " + killprogram);
                        string test = "/C taskkill /IM " + killprogram + ".exe /F";
                        System.Diagnostics.Process.Start("CMD.exe", "/C taskkill /IM " + killprogram + ".exe /F");
                        return true;
                    }
                    
                }
                App.basemodel.text_box.set_speech("Could not kill program ...");
                return false;
            }
            //Screen shot [testing needed]
            else if (command.Contains("screen shot")) {
                last_command = "screen shot";
                //Create a new bitmap.
                var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height,
                 PixelFormat.Format32bppArgb);
                // Create a graphics object from the bitmap.
                var g = Graphics.FromImage(bmpScreenshot);
                // Take the screenshot from the upper left corner to the right bottom corner.
                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,Screen.PrimaryScreen.Bounds.Y,0,0,Screen.PrimaryScreen.Bounds.Size,
                CopyPixelOperation.SourceCopy);
                string directory = App.basemodel.runtimedata.directroy;
                string date = App.basemodel.runtimedata.date.Replace("/", "_");
                string time = App.basemodel.runtimedata.time;
                Random random = new Random();
                //Save to screen shots folder
                bmpScreenshot.Save(directory + "\\Emma\\Data\\ScreenShots\\Screenshot1"+date+"_"+time+"_"+random.Next(0,10000)+".png", ImageFormat.Png);
                return true;
            }
            //No commands were achived
            else
            {
                Current.memory.SaveData("Command Does not exist[" + command + "]");
                last_command = "none";
                return false;
            }
        }
        
        //Clean command when done
        public void clean_command() {
            last_command = null;
        }

        //Get last command
        public string? get_last_command() { 
            return last_command;
        }


    }
}
