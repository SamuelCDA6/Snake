using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Media;
using System.IO;

namespace Snake
{
    public class AudioManager
    {
        private SoundPlayer _AudioPlayer;

        public SoundPlayer AudioPlayer { get => _AudioPlayer; private set => _AudioPlayer = value; }

        public AudioManager()
        {
            if (OperatingSystem.IsWindows())
            {
                AudioPlayer = new SoundPlayer();
            }
        }

        public AudioManager(string path)
        {
            if (OperatingSystem.IsWindows())
            {
                if (File.Exists(path))
                {
                    AudioPlayer = new SoundPlayer(path);
                    AudioPlayer.LoadAsync();
                }
                else
                {
                    AudioPlayer = new SoundPlayer();
                }
            }
        }



        public void PlayMusic()
        {
            if (OperatingSystem.IsWindows() && AudioPlayer.SoundLocation != null)
            {
                AudioPlayer.PlayLooping();
            }
        }

        public void PlaySound()
        {
            if (OperatingSystem.IsWindows() && AudioPlayer.SoundLocation != null)
            {
                AudioPlayer.Play();
            }
        }

        public void StopSound()
        {
            if (OperatingSystem.IsWindows() && AudioPlayer.SoundLocation != null)
            {
                AudioPlayer.Stop();
            }
        }

        public void ChangeSound(string path)
        {
            if (OperatingSystem.IsWindows() && File.Exists(path))
            {
                AudioPlayer.SoundLocation = path;
                AudioPlayer.Load();
            }
        }
    }
}
