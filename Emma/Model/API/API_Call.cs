using System;
using System.Net;
using System.IO;
//uvicorn Pi_API:app --reload
namespace Emma.API
{
    public class API_Call
    {
        #region variables
        private String? Response;
        private HttpWebRequest httpRequest;
        private string current_dir;
        #endregion

        //Consructor
        public API_Call(string value)
        {
            current_dir = value;
        }
        public API_Call() { }

        //Activate api to listen
        public void Listen() {
            try
            {
                var url = "http://" + App.basemodel.Wifi_Data.get_ip_private() + ":4000/api";
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Accept = "application/json";
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Response = result;
                    return;
                }
            }
            //No connection error
            catch (System.Net.WebException ex)
            {
                Response="{message: connection error}";
            }
            catch (Exception ex) {
                App.basemodel.memory.SaveData(ex.Message);
            }
            if (Response == null)
                Response = "{message: Unknown error}";
            return;
        }

        //Get weather
        public void Weather()
        {
            try
            {
                var url = "http://" + App.basemodel.Wifi_Data.get_ip_private() + ":4000/api/weather";
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Accept = "application/json";
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Response = result;
                    return;
                }
            }
            //No connection error
            catch (System.Net.WebException ex)
            {
                Response = "{message: connection error}";
            }
            catch (Exception ex)
            {
                App.basemodel.memory.SaveData(ex.Message);
            }
            if (Response == null)
                Response = "{message: Unknown error}";
            return;
        }

        //CHeck if ping is on
        public void Ping()
        {
            try
            {
                var url = "http://" + App.basemodel.Wifi_Data.get_ip_private() + ":4000/api/ping";
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Accept = "application/json";
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    Response = result;
                    return;
                }
            }
            catch (Exception ex)
            {
                App.basemodel.memory.SaveData(ex.Message);
            }
            Response = "error";
            return;
        }

        //Return response
        public string GetResponse() {
            if (Response == null)
                return null;
            else
            {
                string hold = Response;
                Response = null;
                return hold;
            }
        }
    }

    
}
