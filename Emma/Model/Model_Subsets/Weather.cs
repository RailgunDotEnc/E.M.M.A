using Emma.API;
using Emma.ViewModels.Commands;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Emma.Model.Model_Subsets
{
    public class Weather
    {
        public string weatherType;
        public string weatherDes;
        public double tempK=-100;
        public int tempC=-100;
        public int tempF=-100;
        public string Directory;
        public API_Call API;
        private PropertyUpdate propertyupdate;

        public Weather(API_Call api,string directory) {
            API = api;
            Directory = directory;
        }

        //Get access to Properties update
        public void addpropertyclass(PropertyUpdate value)
        {
            propertyupdate = value;
        }

        public async Task updateWeather()
        {
            API_Call API = new API_Call(Directory);
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
            hold = hold["message"];
            weatherType = hold.weather[0].main;
            weatherDes= hold.weather[0].description;
            string holdtemp = hold.main.temp;
            holdtemp = holdtemp.Substring(0,6);
            tempK = double.Parse(holdtemp);
            tempC = (int)(tempK - 273.15);
            tempF = (int)((tempK - 273.15) * 1.8 + 32);

            weather = null;
            return;
        }

    }
}
