using System;
using System.IO;

namespace Emma.Model.Model_Subsets
{
    public class settings
    {
        #region variables
        private string direct;
        private string[] settings_list = {" "," "," "," "," "," ","0","0"};
        #endregion

        public settings(string value) {
            direct = value;
            read_settings();
        }
        //Read from file
        public void read_settings() {  
            using (StreamReader file = new StreamReader(direct+ "/Emma/Data/EmmasSettings.txt"))
            {
                int counter = 0;
                string ln;

                while ((ln = file.ReadLine()) != null)
                {
                    settings_list[counter]= ln;
                    counter++;
                }
                settings_list[6] = settings_list[6].Split(":")[1];
                settings_list[7] = settings_list[7].Split(":")[1];
                file.Close();

            }
        }
        //write settings to file
        public void write_settings() {
            using (StreamWriter writer = new StreamWriter(direct + "/Data/EmmasSettings.txt"))
            {
                for (int counter = 0; counter < settings_list.Length; counter++) {
                    writer.WriteLine(settings_list[counter]);
                }  
            }
        }

        //Return specific settings
        public bool get_startup() {
            if (settings_list[0].Contains("true"))
                return true;
            else
                return false;
        }
        public bool get_emailCheck() {
            if (settings_list[1].Contains("true"))
                return true;
            else
                return false;
        }
        public bool get_weather() {
            if (settings_list[2].Contains("true"))
                return true;
            else
                return false;
        }
        public bool get_organizeFolders()
        {
            if (settings_list[3].Contains("true"))
                return true;
            else
                return false;
        }
        public bool get_saveRunData() {
            if (settings_list[4].Contains("true"))
                return true;
            else
                return false;
        }
        public bool get_reminder() {
            if (settings_list[5].Contains("true"))
                return true;
            else
                return false;
        }
        public int get_listeningAttempts()
        {
          return int.Parse(settings_list[6]);
        }
        public int get_volume() {
            return int.Parse(settings_list[7]);
        }

        //Get all current settings
        public string[] get_settings_list() { 
            return settings_list;
        }

        //Update settings and save
        public void update_file(String[] list) {
            for (int i = 0; i < list.Length; i++) {
                settings_list[i] = list[i];
            }
            StreamWriter sw = new StreamWriter(direct + "/Emma/Data/EmmasSettings.txt");
            for (int i = 0; i < list.Length-2; i++)
            {
                sw.WriteLine(list[i]);
            }
            sw.WriteLine("listeningAttempts:" + settings_list[6]);
            sw.WriteLine("volume:" + settings_list[7]);
            sw.Close();
        }
    }
}
