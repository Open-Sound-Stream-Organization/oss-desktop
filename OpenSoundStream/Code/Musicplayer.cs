using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class Musicplayer
    {

        public MusicQueue Queue { get; set; }

        public System.Windows.Media.MediaPlayer Mediaplayer { get; set; }

        public PlayerState State { get; set; }

        public void SetVolume(double volume)
        {
            throw new System.NotImplementedException();
        }

        public void NextTrack()
        {
            throw new System.NotImplementedException();
        }

        public void PrevTrack()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void Pause()
        {
            throw new System.NotImplementedException();
        }

        public void Play()
        {
            throw new System.NotImplementedException();
        }

        public void SetTimelinePosition()
        {
            throw new System.NotImplementedException();
        }
    }
}