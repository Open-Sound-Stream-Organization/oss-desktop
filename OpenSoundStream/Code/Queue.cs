using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class MusicQueue
    {
        public MusicQueue()
        {
            throw new System.NotImplementedException();
        }

        public Queue<Track> Tracks { get; set; }

        public bool Shuffle { get; set; }

        public bool Repeat { get; set; }

        public Track ActiveTrack { get; set; }

        public void AddTrack()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveTrack()
        {
            throw new System.NotImplementedException();
        }
    }
}