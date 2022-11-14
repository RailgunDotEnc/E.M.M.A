using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;

namespace Emma.Model.Model_Subsets
{
    public class Sound
    {
        private MediaPlayer sleep;
        private MediaPlayer hello;
        private MediaPlayer joy;
        private MediaPlayer question;
        private MediaPlayer error;
        private MediaPlayer mad;
        private MediaPlayer done;
        private MediaPlayer bye;

        private string directory;

        public Sound(string value) { 
            directory = value;
        }
        private void Play(MediaPlayer speech)
        {
            int volume=App.basemodel.Emmasettings.get_volume();
            SetVolume(volume,speech);
            speech.Play();
        }
        private void SetVolume(int volume,MediaPlayer speech)
        {
            // MediaPlayer volume is a float value between 0 and 1.
            speech.Volume = volume / 100.0f;
        }

        public void voice(string value) {
            string speech = "";
            if (value.Equals("tired"))
            {
                sleep = new MediaPlayer();
                sleep.Open(new Uri(directory + "/Emma/Voice/FCS12_013_Healthy turn1.wav"));
                Play(sleep);
                speech="I don't wanna work~";
            }
            else if (value.Equals("hello"))
            {
                //Opening sound
                hello = new MediaPlayer();
                string test=Environment.GetEnvironmentVariable("userdir");
                hello.Open(new Uri(directory + "/Emma/Voice/FCS12_111_Call strongly1.wav"));
                Play(hello);
                speech= "Hello-?";
            }
            else if (value.Equals("joy"))
            {
                //joy
                joy = new MediaPlayer();
                joy.Open(new Uri(directory + "/Emma/Voice/FCS12_101_Amazed1.wav"));
                Play(joy);
                speech= "Wow...";
            }
            else if (value.Equals("what")) {
                //Listtening que
                question = new MediaPlayer();
                question.Open(new Uri(directory + "/Emma/Voice/FCS12_060_Reply1.wav"));
                Play(question);
                speech = "Wha~t?";
            }
            else if (value.Equals("error"))
            {
                //error
                error = new MediaPlayer();
                error.Open(new Uri(directory + "/Emma/Voice/FCS12_023_Damage3.wav"));
                Play(error);
                speech = "Ueh";
            }
            else if (value.Equals("mad"))
            {
                //mad
                mad = new MediaPlayer();
                mad.Open(new Uri(directory + "/Emma/Voice/FCS12_071_Suspicious2.wav"));
                Play(mad);
                speech = "*staaare*";
            }
            else if (value.Equals("done"))
            {
                //done
                done = new MediaPlayer();
                done.Open(new Uri(directory + "/Emma/Voice/FCS12_073_Joy2.wav"));
                Play(done);
                speech = "All right!";
            }
            else if (value.Equals("bye"))
            {
                //bye
                bye = new MediaPlayer();
                bye.Open(new Uri(directory + "/Emma/Voice/FCS12_122_Good bye2.wav"));
                Play(bye);
                speech = "Bye bye~";
            }

            App.basemodel.text_box.set_speech(speech);
        }
    }
}
