using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace OpenSoundStream
{
	public class MusicQueue : INotifyPropertyChanged
	{
		private Track activeTrack;

		public MusicQueue()
		{
			Queue = new LinkedList<Track>();
			LastPlayed = new Stack<Track>();
		}

		public LinkedList<Track> Queue { get; set; }

		public Stack<Track> LastPlayed { get; set; }

		public bool Shuffle { get; set; }

		public bool Repeat { get; set; }

		public Track ActiveTrack { get => activeTrack; set { activeTrack = value; OnPropertyChanged(); } }

		public event PropertyChangedEventHandler PropertyChanged;

		public void NextTrack()
		{
			Track currentTrack = ActiveTrack;
			LinkedListNode<Track> nextTrack = Queue.First;

			if (nextTrack != null)
			{
				if (currentTrack != null)
					LastPlayed.Push(currentTrack);

				ActiveTrack = nextTrack.Value;
				Queue.RemoveFirst();
			}
		}

		public void PrevTrack()
		{
			Track currentTrack = ActiveTrack;
			Track lastTrack = null;

			if (currentTrack == null)
				return;

			try
			{
				lastTrack = LastPlayed.Pop();
			}
			catch (Exception ex)
			{
				//TODO Fehlermanagement
			}

			if (lastTrack != null)
			{
				ActiveTrack = lastTrack;
				Queue.AddFirst(currentTrack);
			}
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

		public void LoadPlaylistInQueue(Playlist playlist)
		{
			foreach (Track track in playlist.Tracks)
			{
				Queue.AddLast(track);
			}
		}

		// Create the OnPropertyChanged method to raise the event
		// The calling member's name will be used as the parameter.
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}