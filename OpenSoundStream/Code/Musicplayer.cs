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
		public MusicQueue Musicqueue { get; set; }

		public System.Windows.Media.MediaPlayer Mediaplayer { get; set; }

		public PlayerState State { get; set; }

		public Musicplayer()
		{
			Musicqueue = new MusicQueue();
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
			Musicqueue.SelectNextTrack();

			if (Musicqueue.ActiveTrack != null)
			{
				Mediaplayer.Open(Musicqueue.ActiveTrack.Filepath);
				if(State == PlayerState.Play)
					Play();
			}
			else if (Musicqueue.Repeat && Musicqueue.LastPlayed.Count > 0)
			{
				Musicqueue.Queue = new LinkedList<Track>(Musicqueue.LastPlayed.Reverse());
				Musicqueue.LastPlayed = new Stack<Track>();
				Mediaplayer.Open(Musicqueue.SelectNextTrack().Filepath);
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
			Musicqueue.SelectLastTrack();
			if (Musicqueue.ActiveTrack != null)
			{
				Mediaplayer.Open(Musicqueue.ActiveTrack.Filepath);
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

			if (Musicqueue.ActiveTrack == null)
			{
				NextTrack();

				if (Musicqueue.ActiveTrack == null)
					Stop();
			}

			Mediaplayer.Play();
			State = PlayerState.Play;
		}

		public void PlayTrack(Track track)
		{
			Musicqueue.ActiveTrack = track;
			Mediaplayer.Open(Musicqueue.ActiveTrack.Filepath);
			Play();
		}

		public void SetTimelinePosition()
		{
			throw new System.NotImplementedException();
		}
	}
}