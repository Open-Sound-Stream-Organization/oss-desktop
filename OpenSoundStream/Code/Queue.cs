﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OpenSoundStream
{
	public class MusicQueue : INotifyPropertyChanged
	{
		#region Variables
		private Track activeTrack;
		private LinkedListNode<Track> activeNode;

		public LinkedList<Track> Queue { get; set; }
		public LinkedList<Track> ActivePlayableContainer { get; set; }

		public LinkedList<Track> PriorityQueue { get; set; }

		public bool Shuffle { get; set; }

		private static Random rng = new Random();

		public bool RepeatQueue { get; set; }

		public bool RepeatTrack { get; set; }

		public LinkedListNode<Track> ActiveNode { get => activeNode; set { activeNode = value; OnPropertyChanged(); } }

		public Track ActiveTrack { get => activeTrack; set { activeTrack = value; OnPropertyChanged(); } }

		public event PropertyChangedEventHandler PropertyChanged;
        #endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public MusicQueue()
		{
			Queue = new LinkedList<Track>();
			PriorityQueue = new LinkedList<Track>();
			ActivePlayableContainer = new LinkedList<Track>();
		}

		/// <summary>
		/// Select next track
		/// </summary>
		/// <param name="mp"></param>
        public void NextTrack(Musicplayer mp)
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
					mp.Stop();
					mp.State = PlayerState.Stop;
					ActiveNode = Queue.First;
					ActiveTrack = Queue.First.Value;
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

		/// <summary>
		/// Select previous track
		/// </summary>
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

		/// <summary>
		/// Add track to priorityQueue -> next track
		/// </summary>
		/// <param name="track"></param>
		public void AddTrackToQueueFirstPos(Track track)
		{
			PriorityQueue.AddLast(track);
		}

		/// <summary>
		/// Add track to queue last position
		/// </summary>
		/// <param name="track"></param>
		public void AddTrackToQueueLastPos(Track track)
		{
			Queue.AddLast(track);
		}

		/// <summary>
		/// Remove track from queue
		/// </summary>
		/// <param name="track"></param>
		public void RemoveTrackFromQueue(Track track)
		{
			Queue.Remove(track);
		}


		/// <summary>
		/// Load Playlist, Album or all tracks in Queue
		/// </summary>
		/// <param name="pc"></param>
		public void LoadPlayableContainerInQueue(PlayableContainer pc)
		{
			Queue = new LinkedList<Track>();
			foreach (Track track in pc.Tracks)
			{
				Queue.AddLast(track);
			}
			if(Queue.First != null) 
			{ 
				ActiveTrack = Queue.First.Value;
				ActiveNode = Queue.First;
			}
		}

		/// <summary>
		/// Load all albums from artist in queue
		/// </summary>
		/// <param name="artist"></param>
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

		/// <summary>
		/// Select track in playlist, album or all tracks
		/// </summary>
		/// <param name="track"></param>
		/// <param name="pc"></param>
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

		
		/// <summary>
		/// Shuffle playablecontainer
		/// </summary>
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

		/// <summary>
		/// Unshuffle playablecontainer
		/// </summary>
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

		/// <summary>
		/// Find active node in queue
		/// </summary>
		/// <param name="track"></param>
		public void FindActiveNode(Track track)
		{
			LinkedListNode<Track> actualNode;
			for (actualNode = Queue.First; actualNode != Queue.Last; actualNode = actualNode.Next)
			{
				if (actualNode.Value.id == track.id)
				{
					ActiveNode = actualNode;
					break;
				}
			}

			if(actualNode.Value.id == Queue.Last.Value.id)
			{
				ActiveNode = actualNode;
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