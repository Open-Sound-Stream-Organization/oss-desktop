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

        public Musicplayer()
        {
            Mediaplayer.MediaEnded += EndOfTrackReached;
        }

        public void SetVolume(double volume)
        {
            if (volume < 0.0 || volume > 1.0)
                throw new ArgumentOutOfRangeException();

            Mediaplayer.Volume = volume;
        }

        public void NextTrack()
        {
            Track nextTrack = Queue.NextTrack;
            if (nextTrack != null)
                Mediaplayer.Open(Queue.NextTrack.Filepath);
        }

        private void EndOfTrackReached(object sender, EventArgs e)
        {
            Track nextTrack = Queue.NextTrack;
            if (nextTrack != null)
                Mediaplayer.Open(Queue.NextTrack.Filepath);
            else if(Queue.Repeat)
            {
                Queue.Tracks = new LinkedList<Track>(Queue.LastPlayed.Reverse());
                Mediaplayer.Open(Queue.NextTrack.Filepath);
            } 
        }

        public void PrevTrack()
        {
            Mediaplayer.Open(Queue.LastTrack.Filepath);
        }

        public void Stop()
        {
            if (State != PlayerState.Stop)
            {
                Mediaplayer.Stop();
                State = PlayerState.Stop;
            }
        }

        public void Pause()
        {
            if (State != PlayerState.Pause)
            {
                Mediaplayer.Pause();
                State = PlayerState.Pause;
            }
        }

        public void Play()
        {
            if (State != PlayerState.Play)
            {
                if (Mediaplayer.HasAudio == false)
                    throw new Exception("Audio kann nicht abgespielt werden");
                Mediaplayer.Play();
                State = PlayerState.Play;
            }
        }

        public void PlayTrack(Track track)
        {
            Queue.ActiveTrack = track;

            if (State != PlayerState.Play)
                Play();
        }

        public void SetTimelinePosition()
        {
            throw new System.NotImplementedException();
        }
    }
}