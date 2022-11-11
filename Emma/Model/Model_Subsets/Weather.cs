using Emma.API;
using Emma.ViewModels.Commands;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        public string last_weather="";
        private settings settings;
        //Image icons for weather
        public BitmapImage cloud= new BitmapImage(new Uri("/Emma;component/images/icons/cloudy.png", UriKind.Relative));
        public BitmapImage sun = new BitmapImage(new Uri("/Emma;component/images/icons/sunny.png", UriKind.Relative));
        public BitmapImage snow=new BitmapImage(new Uri("/Emma;component/images/icons/snow.png", UriKind.Relative));
        public BitmapImage rain= new BitmapImage(new Uri("/Emma;component/images/icons/rain.png", UriKind.Relative));
        public BitmapImage thunder= new BitmapImage(new Uri("/Emma;component/images/icons/thunderstorm.png", UriKind.Relative));

        public API_Call API;
        private PropertyUpdate propertyupdate;

        public Weather(API_Call api,string directory, settings set)
        {
            API = api;
            Directory = directory;
            settings = set;
        }

        //Get access to Properties update
        public void addpropertyclass(PropertyUpdate value)
        {
            propertyupdate = value;
        }

        public async Task updateWeather()
        {
            //if weather setting is off
            if (settings.get_weather() == false)
            {
                if (!last_weather.Equals("null"))
                {
                    last_weather = "null";
                    propertyupdate.Weatehr_Image = null;
                }
                return;
            }
            //Start updating weather
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
            check_property();
            weather = null;
            return;
        }
        //Update image icon for weather
        public void check_property() {
            if (!last_weather.Equals(weatherType))
            {
                if (weatherType.ToLower().Contains("rain"))
                {
                    last_weather = weatherType;
                    propertyupdate.Weatehr_Image = rain;
                }
                else if (weatherType.ToLower().Contains("sun"))
                {
                    last_weather = weatherType;
                    propertyupdate.Weatehr_Image = sun;
                }
                else if (weatherType.ToLower().Contains("snow"))
                {
                    last_weather = weatherType;
                    propertyupdate.Weatehr_Image = snow;
                }
                else if (weatherType.ToLower().Contains("thunder"))
                {
                    last_weather = weatherType;
                    propertyupdate.Weatehr_Image = thunder;
                }
                else if (weatherType.ToLower().Contains("cloud"))
                {
                    last_weather = weatherType;
                    propertyupdate.Weatehr_Image = cloud;
                }
                else
                {
                    if (!last_weather.Equals("null"))
                    {
                        last_weather = "null";
                        App.basemodel.memory.SaveData("Weather Error: "+weatherType);
                        propertyupdate.Weatehr_Image = null;
                    }
                }
            }
        }
    }
}
