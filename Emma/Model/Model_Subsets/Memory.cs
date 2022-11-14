using System.IO;
using System.Threading.Tasks;

namespace Emma.Model.Model_Subsets
{
    public class Memory
    {
        public string[]? saved_data =null;
        public string current_file;
        public string directory;
        public string date;
        private string time;
        private settings settings;
        //constructor
        public Memory(settings set,string dir)
        {
            directory= dir;
            UpdateList();
            settings = set;
        }
        //update stored list
        public void UpdateList()
        {
            //Get all files in stored Memory
            saved_data = Directory.GetFiles(directory + "/Emma/Data/Stored_Memory/");
            //If files contain more then a week of data, delete file.
            if (saved_data.Length > 7)
            {
                string[][] temp = new string[saved_data.Length][];
                for (int i = 0; i < saved_data.Length; i++)
                    temp[i] = saved_data[i].Replace(directory + "/Emma/Data/Stored_Memory/Run_Data_", string.Empty).Replace(".txt",string.Empty).Split("_");
                bool change;
                while (true)
                {
                    change = false;
                    for (int i = 0; i < temp.Length-1; i++) {
                        if (int.Parse(temp[i][2]) >= int.Parse(temp[i+1][2])&& int.Parse(temp[i][0]) >= int.Parse(temp[i + 1][0])&& int.Parse(temp[i][1]) > int.Parse(temp[i + 1][1])) {
                                    string[] hold = temp[i];
                                    string test = saved_data[i+1];
                                    string hold2 = saved_data[i];
                                    temp[i] = temp[i + 1];
                                    saved_data[i] = saved_data[i + 1];
                                    temp[i + 1] = hold;
                                    saved_data[i + 1] = hold2;
                                    change = true; 
                        }
                    }
                    if (change == false)
                        break;
                }
                int j = 1;
                string[] temp2 =new string[7];
                for (int i = saved_data.Length - 1; i > -1; i--)
                {
                    if (i > saved_data.Length - 7)
                    {
                        temp2[6 - j] = saved_data[i];
                        j++;
                    }
                    else
                        File.Delete(saved_data[i]);
                }
            }

        }
        //Create Run Data per day
        public void CreateRunData() {
            bool file_exist = false;
            directory = App.basemodel.runtimedata.directroy;
            date = App.basemodel.runtimedata.date.Replace("/","_");
            time = App.basemodel.runtimedata.time;
            //If file exist save start time
            for (int i = 0; i < saved_data.Length; i++) {
                if (saved_data[i].Contains(date)) {
                    file_exist = true;
                    current_file = directory + "/Emma/Data/Stored_Memory/Run_Data_" + date + ".txt";
                    File.AppendAllText(current_file, "\n"+time+": Starting run"+"\n");
                }
            }
            //If file does not exist, create
            if (!file_exist) {
                current_file = directory + "/Emma/Data/Stored_Memory/Run_Data_" + date + ".txt";
                System.IO.StreamWriter writer = new System.IO.StreamWriter(directory + "/Emma/Data/Stored_Memory/Run_Data_" + date + ".txt");
                writer.Write(time+ ": Starting run"+"\n");
                writer.Close();
                saved_data[saved_data.Length - 1] = current_file;
            }
        }
        //Write to file
        public async void SaveData(string value)
        {
            //save rundata setting is off
            if (settings.get_saveRunData() == false)
                return;
            while (true)
            {
                try
                {
                    current_file = directory + "/Emma/Data/Stored_Memory/Run_Data_" + date + ".txt";
                    File.AppendAllText(current_file, time + ": " + value + "\n");
                    break;
                }
                catch { await Task.Delay(100); }
            }
        }
    }
}
