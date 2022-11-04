using Emma.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Emma.Model.Model_Subsets
{
    public class RunTimeData
    {
        #region variables
        public bool emma_sleeping = false;
        public bool min_sleep = false;
        public bool emma_running = true;
        public string display = "0";
        public string time = "0:00 AM";
        public string date = "00/00/00";
        public int attemps = 0;
        public String? directroy = System.AppContext.BaseDirectory.Replace("\\Emma\\bin\\Debug\\net6.0-windows", string.Empty);
        private BitmapImage? listening = null;
        public bool menu_up = false;
        public bool dragged = false;
        public WindowState window;
        public List<string> RunningExe = new List<string>();
        public List<Thread> threads = new List<Thread>();
        PropertyUpdate? propertyupdate = null;
        #endregion

        //Constructor
        public RunTimeData()
        {
            //Set up time
            date = DateTime.Now.ToString("M/d/yyyy");
            time = DateTime.Now.ToString("hh:mm tt");
            if (time[0].Equals('0'))
                time = time.Substring(1);
        }
        
        //Get access to Properties update
        public void addpropertyclass(PropertyUpdate value)
        {
            propertyupdate = value;
        }

        //Update all computer Statistics
        public void UpdateDiagnostics()
        {
            
            //Update moniter count
            ManagementObjectSearcher monitorObjectSearch = new ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor");
            int Counter = CheckActiveMonitors();
            display = Counter.ToString();
            propertyupdate.Display = display;
            //Update time
            time=CheckTime();
            if(menu_up==false)
                propertyupdate.Time = time;
            //Get ALl running EXE programs
            EXEFinder();
        }

        //Update listening image to on
        public void listening_button_on() {
            listening = new BitmapImage(new Uri("/Emma;component/images/icons/check.png", UriKind.Relative));
            propertyupdate.Listening = listening;
        }

        //Update listeing image to off
        public void listening_button_off() {
            listening = new BitmapImage(new Uri("/Emma;component/images/icons/off.png", UriKind.Relative));
            propertyupdate.Listening = listening;
        }

        //Check monitor count
        private int CheckActiveMonitors()
        {
            int Counter = 0;
            System.Management.ManagementObjectSearcher monitorObjectSearch = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_DesktopMonitor");
            foreach (ManagementObject Monitor in monitorObjectSearch.Get())
            {
                UInt16 Status = 0;
                try
                {
                    Status = (UInt16)Monitor["Availability"];
                }
                catch (Exception ex)
                {
                    //Error handling if you want to
                    continue;
                }
                if (Status == 3)
                    Counter++;

            }
            return Counter;
        }

        //Check current time
        private string CheckTime() {
            time = DateTime.Now.ToString("hh:mm tt");
            if (time[0].Equals('0'))
                time = time.Substring(1);
            return time;
        }

        //Get Running programs
        private void EXEFinder()
        {
            string hold;
            Process[] processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                if (!String.IsNullOrEmpty(p.ProcessName))
                {
                    hold = p.ProcessName.ToLower();
                    //not include certain programs
                    if (!hold.Contains("killer") && !hold.Contains("handler") && !hold.Contains("service") && !hold.Contains("runtime") && !hold.Contains("microsoft") && !hold.Contains("nvidia") && !hold.Contains("host") && !hold.Contains("scv") && !hold.Contains("dell") && !RunningExe.Contains(p.ProcessName))
                        RunningExe.Add(p.ProcessName);
                }
            }
        }

    }

}