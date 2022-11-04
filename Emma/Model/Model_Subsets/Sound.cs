using System;
using System.Windows.Media;

namespace Emma.Model.Model_Subsets
{
    public class Sound
    {
        private MediaPlayer m_mediaPlayer;
        public void Play(string filename)
{
            m_mediaPlayer = new MediaPlayer();
            m_mediaPlayer.Open(new Uri(filename));
            m_mediaPlayer.Play();
        }
        public void SetVolume(int volume)
        {
            // MediaPlayer volume is a float value between 0 and 1.
            m_mediaPlayer.Volume = volume / 100.0f;
        }
    }
}
