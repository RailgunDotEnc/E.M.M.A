
import speech_recognition as sr
import uvicorn
from fastapi import FastAPI
import socket
from geopy.geocoders import Nominatim
import requests,geocoder

#uvicorn Pi_API:app --reload
def speech_to_text():
    listener=sr.Recognizer()
    my_mic = sr.Microphone()
    print("\n[Listening]")
    try:
        with my_mic as source: #Basic voice assistant things
            listener.adjust_for_ambient_noise(source, duration = 1)
            voice=listener.listen(source, phrase_time_limit=5) #Where the code is getting stuck with bluetooth 
            command=(listener.recognize_google(voice)).lower() #Updates command string
    ######################################################
            return command
        ###########################################################
        #No sound was heard or understood 
    except sr.UnknownValueError:
            return "No Sound"

def getWeather():
    g = geocoder.ip('me')
    # calling the nominatim tool
    geoLoc = Nominatim(user_agent="GetLoc")
    # passing the coordinates
    locname = str(geoLoc.reverse(f"{g.latlng[0]}, {g.latlng[1]}"))
    # printing the address/location name
    locname=(locname.split(', '))
    api='[API KEY]'
    city_name=locname[2]
    complete_api_link=f"https://api.openweathermap.org/data/2.5/weather?q={city_name}&appid={api}"
    api_link=requests.get(complete_api_link)
    api_data=api_link.json()
    
    temp_city=((api_data["main"]['temp'])-273.15)
    temp_city=int((temp_city*9/5)+32)
    weather_desc=api_data['weather'][0]['description']
    return api_data

app = FastAPI()

h_name = socket.gethostname()
IP_addres = socket.gethostbyname(h_name)

@app.get("/api")
async def index():
    text=speech_to_text()
    print(text)
    return {"message": text}

@app.get("/api/weather")
async def index():
    print("Getting weather")
    message=getWeather()
    return {"message": message}


@app.get("/api/ping")
async def ping():
    print("Ping = true")
    return {"message": "true"}

if __name__ == '__main__':
    uvicorn.run("Pi_API:app", host=f"{IP_addres}", port=4000)
    