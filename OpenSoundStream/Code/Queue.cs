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
			Tracks = new LinkedList<Track>();
			LastPlayed = new Stack<Track>();
		}

		public LinkedList<Track> Tracks { get; set; }

		public Stack<Track> LastPlayed { get; private set; }

		public bool Shuffle { get; set; }

		public bool Repeat { get; set; }

		public Track ActiveTrack { get; set; }

		public Track SelectNextTrack()
		{
			AddLastTrack();
			ActiveTrack = DequeueNextTrack();
			return ActiveTrack;
		}

		public Track SelectLastTrack()
		{
			ActiveTrack = PopLastPlayed();
			AddTrackToQueueFirstPos(ActiveTrack);
			return ActiveTrack;
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
			if(Tracks.First == null)
			{
				foreach (var item in LastPlayed)
				{
					Tracks.AddFirst(item);
				}
				// Remove null from Stack
				Tracks.RemoveFirst();
				LastPlayed.Clear();

				return null;
			}
			else
			{
				Track nextTrack = Tracks.First.Value;
				Tracks.RemoveFirst();
				return nextTrack;
			}
		}
	}
}