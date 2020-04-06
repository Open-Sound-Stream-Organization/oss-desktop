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
			Queue = new LinkedList<Track>();
			LastPlayed = new Stack<Track>();
		}

		public LinkedList<Track> Queue { get; set; }

		public Stack<Track> LastPlayed { get; set; }

		public bool Shuffle { get; set; }

		public bool Repeat { get; set; }

		public Track ActiveTrack { get; set; }

		public Track SelectNextTrack()
		{
			if (ActiveTrack != null)
				LastPlayed.Push(ActiveTrack);
			ActiveTrack = DequeueNextTrack();
			return ActiveTrack;
		}

		public Track SelectLastTrack()
		{
			if (ActiveTrack != null)
			{
				Queue.AddFirst(ActiveTrack);
			}
			if(LastPlayed.Count >0)
			{
				ActiveTrack = LastPlayed.Pop();
				AddTrackToQueueFirstPos(ActiveTrack);
				return ActiveTrack;
			}
			return null;
		}

		public void AddTrackToQueueFirstPos(Track track)
		{
			Queue.AddFirst(track);
		}

		public void AddTrackToQueueLastPos(Track track)
		{
			Queue.AddLast(track);
		}

		public void RemoveTrackFromQueue(Track track)
		{
			Queue.Remove(track);
		}

		private Track DequeueNextTrack()
		{
			if (Queue.First == null)
			{
				foreach (var item in LastPlayed)
				{
					Queue.AddFirst(item);
				}
				LastPlayed.Clear();

				return Queue.First.Value;
			}
			else
			{
				Track nextTrack = Queue.First.Value;
				Queue.RemoveFirst();
				return nextTrack;
			}
		}

		public void LoadPlaylistInQueue(Playlist playlist)
		{
			foreach (Track track in playlist.Tracks)
			{
				Queue.AddLast(track);
			}
		}
	}
}