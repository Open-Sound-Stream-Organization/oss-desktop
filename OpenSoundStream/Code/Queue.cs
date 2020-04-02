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

		public LinkedList<Track> Tracks { get; set; }

		public Stack<Track> LastPlayed { get; private set; }

		public bool Shuffle { get; set; }

		public bool Repeat { get; set; }

		public Track ActiveTrack { get; set; }

		public Track NextTrack
		{
			get
			{
				AddLastTrack();
				return DequeueNextTrack();
			}
		}

		public Track LastTrack
		{
			get
			{
				Track lastTrack = PopLastPlayed();
				AddTrackToQueueFirstPos(lastTrack);
				return lastTrack;
			}
		}

		public void AddTrackToQueueFirstPos(Track track)
		{
			Tracks.AddFirst(track);
		}

		public void AddTrackToQueueLastPos(Track track)
		{
			Tracks.AddLast(track);
		}

		public void RemoveTrackFromQueue(Track track)
		{
			Tracks.Remove(track);
		}

		public void AddLastTrack()
		{
			LastPlayed.Push(ActiveTrack);
		}

		public Track PopLastPlayed()
		{
			if(ActiveTrack != null)
			{
				Tracks.AddFirst(ActiveTrack);
			}
			return LastPlayed.Pop();
		}

		public Track DequeueNextTrack()
		{
			//TODO Nullprüfung einbauen
			Track nextTrack = Tracks.First.Value;
			Tracks.RemoveFirst();
			return nextTrack;
		}
	}
}