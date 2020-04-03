using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSoundStream
{
	public class Musicplayer
	{

		public MusicQueue Queue { get; set; }

		public System.Windows.Media.MediaPlayer Mediaplayer { get; set; }

		public PlayerState State { get; set; }

		public Musicplayer()
		{
			Queue = new MusicQueue();
			Mediaplayer = new System.Windows.Media.MediaPlayer();
			State = PlayerState.Stop;

			Mediaplayer.MediaEnded += NextTrack;
		}

		public void SetVolume(double volume)
		{
			if (volume < 0.0 || volume > 1.0)
				throw new ArgumentOutOfRangeException();

			Mediaplayer.Volume = volume;
		}

		public void NextTrack(object sender = null, EventArgs e = null)
		{
			Queue.SelectNextTrack();

			if (Queue.ActiveTrack != null)
			{
				Mediaplayer.Open(Queue.ActiveTrack.Filepath);
				if(State == PlayerState.Play)
					Play();
			}
			else if (Queue.Repeat && Queue.LastPlayed.Count > 0)
			{
				Queue.Tracks = new LinkedList<Track>(Queue.LastPlayed.Reverse());
				Mediaplayer.Open(Queue.SelectNextTrack().Filepath);
				if (State == PlayerState.Play)
					Play();
			}
			else
			{
				Stop();
			}
		}

		public void PrevTrack()
		{
			Queue.SelectLastTrack();
			if (Queue.ActiveTrack != null)
			{
				Mediaplayer.Open(Queue.ActiveTrack.Filepath);
				if (State == PlayerState.Play)
					Play();
			}
			else
			{
				Stop();
			}
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

			if (Queue.ActiveTrack == null)
			{
				NextTrack();

				if (Queue.ActiveTrack == null)
					Stop();
			}

			Mediaplayer.Play();
			State = PlayerState.Play;
		}

		public void PlayTrack(Track track)
		{
			Queue.ActiveTrack = track;
			Mediaplayer.Open(Queue.ActiveTrack.Filepath);
			Play();
		}

		public void SetTimelinePosition()
		{
			throw new System.NotImplementedException();
		}
	}
}