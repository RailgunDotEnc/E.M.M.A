using Emma.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Emma.Model.Model_Subsets
{
    public class Wifi_Data
    {
        #region variables
        private float past_strength = -0.00f;
        private float wifi_strength = 0.00f;
        private bool wifi_on = false;
        private string network="";
        string[] split_output;
        private BitmapImage? _wifi = new BitmapImage(new Uri("/Emma;component/images/icons/no_wifi.png", UriKind.Relative));
        private BitmapImage low_wifi= new BitmapImage(new Uri("/Emma;component/images/icons/low_wifi.png", UriKind.Relative));
        private BitmapImage mid_wifi= new BitmapImage(new Uri("/Emma;component/images/icons/medium_wifi.png", UriKind.Relative));
        private BitmapImage high_wifi = new BitmapImage(new Uri("/Emma;component/images/icons/high_wifi.png", UriKind.Relative));
        private string IP_private;
        //private string IP_public;
        //private bool wifi_secured = false;
        //private bool vpn_active = false;
        //private bool remote_accessed = false;
        private PropertyUpdate? propertyupdate =null;
        #endregion

        //Constructor
        public Wifi_Data()
        {
            check_wifi();
            IP_private = check_IP().ToString();
        }

        //Get access to ProperyUpdate class
        public void addpropertyclass(PropertyUpdate value)
        {
            propertyupdate = value;
        }

        //Scan for all nessesary wifi info
        public void check_wifi(){
            string command = "netsh wlan show interfaces";

            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Verb = "runas",
                Arguments = "/C " + command,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true

            };


            var cmd = Process.Start(startInfo);
            string output = cmd.StandardOutput.ReadToEnd();
            output = output.Replace("\r", String.Empty);
            cmd.WaitForExit();
            split_output = output.Split("\n");

            update_wifi_data();
        }

        //Organize data from cmd
        public void update_wifi_data() {
            try
            {
                //Check if wifi is conected
                //8
                if (split_output[8].Contains(" State"))
                {
                    if (split_output[8].Contains(" connected"))
                        wifi_on = true;
                    else
                    {
                        wifi_on = false;
                        wifi_strength = 0.0f;
                    }
                }

                if (wifi_on == true)
                    //Get wifi's name
                    //9
                    if (split_output[9].Contains(" SSID"))
                        network = split_output[9].Substring(split_output[9].IndexOf(":") + 1);
                if (split_output[20].Contains(" Signal"))
                {
                    //Get wifi strength
                    //20
                    string temp = split_output[20].Substring(split_output[20].IndexOf(":") + 1);
                    temp = temp.Replace("%", String.Empty);
                    wifi_strength = float.Parse(temp);
                }
            }
            catch (Exception e) {
                wifi_on = false;
                wifi_strength = 0.0f;
            }
            update_wifi_symbol();
        }

        //Update wifi image by signle strength
        public void update_wifi_symbol() {
            if (!(past_strength > 1 && past_strength < 50) && (wifi_strength > 1 && wifi_strength < 50))
                _wifi = low_wifi;
            else if (!(past_strength >= 50 && past_strength < 80) && (wifi_strength >= 50 && wifi_strength < 80))
                _wifi = mid_wifi;
            else if (!(past_strength >= 80) && (wifi_strength >= 80))
                _wifi = high_wifi;
            else
                _wifi = null;
            if (propertyupdate != null)
            {
                propertyupdate.Wifi = _wifi;
            }
        }

        //Scan IP address
        private string check_IP() {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            IEnumerable<string> IP= (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
            return IP.First();
            
        }

        //return variables
        public float get_wifi_strength() { 
            return wifi_strength;
        }
        public string get_network()
        {
            return network;
        }
        public bool get_wifi_on() {
            return wifi_on;
        }
        public string get_ip_private()
        {
            return IP_private;
        }
    }
}
