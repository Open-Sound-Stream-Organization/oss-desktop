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
		private LinkedListNode<Track> activeNode;

		public MusicQueue()
		{
			Queue = new LinkedList<Track>();
			PriorityQueue = new LinkedList<Track>();
			ActivePlayableContainer = new LinkedList<Track>();
		}
		public LinkedList<Track> Queue { get; set; }
		public LinkedList<Track> ActivePlayableContainer { get; set; }

		public LinkedList<Track> PriorityQueue { get; set; }

		public bool Shuffle { get; set; }

		public bool RepeatQueue { get; set; }

		public bool RepeatTrack { get; set; }

		public LinkedListNode<Track> ActiveNode { get => activeNode; set { activeNode = value; OnPropertyChanged(); } }

		public Track ActiveTrack { get => activeTrack; set { activeTrack = value; OnPropertyChanged(); } }

		public event PropertyChangedEventHandler PropertyChanged;

		public void NextTrack()
		{
			LinkedListNode<Track> currentTrack = ActiveNode;
			LinkedListNode<Track> nextTrack = null;

			if(currentTrack == null)
			{
				nextTrack = Queue.First;
			}
			else
			{
				nextTrack = currentTrack.Next;
			}

			if (PriorityQueue.Count() == 0)
			{
				if (nextTrack != null)
				{
					ActiveNode = nextTrack;
					ActiveTrack = ActiveNode.Value;
				}
				else
				{
					ActiveNode = null;
					ActiveTrack = null;
				}
			}
			else
			{
				nextTrack = PriorityQueue.First;
				if (nextTrack != null)
				{
					ActiveTrack = nextTrack.Value;
					PriorityQueue.RemoveFirst();
				}
			}
		}

		public void PrevTrack()
		{
			LinkedListNode<Track> currentTrack = ActiveNode;

			if (currentTrack == null)
				return;

			ActiveNode = ActiveNode.Previous;
			if(ActiveNode == null)
			{
				ActiveNode = Queue.First;
			}
			ActiveTrack = ActiveNode.Value;
		}

		public void AddTrackToQueueFirstPos(Track track)
		{
			PriorityQueue.AddLast(track);
		}

		public void AddTrackToQueueLastPos(Track track)
		{
			Queue.AddLast(track);
		}

		public void RemoveTrackFromQueue(Track track)
		{
			Queue.Remove(track);
		}

		public void LoadPlayableContainerInQueue(PlayableContainer pc)
		{
			Queue = new LinkedList<Track>();
			foreach (Track track in pc.Tracks)
			{
				Queue.AddLast(track);
			}
			ActiveNode = null;
		}

		public void LoadArtistInQueue(Artist artist) 
		{
			Queue = new LinkedList<Track>();
			foreach (Album album in artist.Albums)
			{
				foreach (Track track in album.Tracks)
				{
					Queue.AddLast(track);
				}
			}
		}

		public void SelectTrackInQueue(Track track, PlayableContainer pc)
		{
			Queue = pc.Tracks;

			try
			{
				ActiveNode = Queue.Find(track);
			}
			catch(Exception e)
			{
				throw e;
			}

			ActiveTrack = ActiveNode.Value;
		}

		private static Random rng = new Random();

		public void ShuffleQueue()
		{
			if(Queue.Count() == 0)
			{
				return;
			}
			int n = Queue.Count;
			ActivePlayableContainer = Queue;
			List<Track> list = Queue.ToList<Track>();
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				Track value = list[k];
				list[k] = list[n];
				list[n] = value;
			}

			Queue = new LinkedList<Track>(list);
			ActiveNode = Queue.Find(ActiveNode.Value);
			ActiveTrack = ActiveNode.Value;
		}
		public void UnshuffleQueue()
		{
			if(ActivePlayableContainer.Count() == 0)
			{
				return;
			}
			Queue = ActivePlayableContainer;
			ActiveNode = Queue.Find(ActiveNode.Value);
			ActiveTrack = ActiveNode.Value;
		}

		// Create the OnPropertyChanged method to raise the event
		// The calling member's name will be used as the parameter.
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}