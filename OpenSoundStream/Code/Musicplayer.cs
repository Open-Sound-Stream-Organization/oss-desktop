using OpenSoundStream.Code.DataManager;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Timers;

namespace OpenSoundStream
{
    public class Musicplayer : ReactiveObject
    {
        #region Variables

        private Timer timer;

        public MusicQueue Musicqueue { get; set; }

        public System.Windows.Media.MediaPlayer Mediaplayer { get; set; }

        public PlayerState State { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public Musicplayer()
        {
            Musicqueue = new MusicQueue();
            Mediaplayer = new System.Windows.Media.MediaPlayer();
            State = PlayerState.Stop;

            Mediaplayer.MediaEnded += NextTrack;
        }

        #region Methods

        /// <summary>
        /// Set music player volume
        /// </summary>
        /// <param name="volume"></param>
        public void SetVolume(double volume)
        {
            if (volume < 0.0 || volume > 1.0)
                throw new ArgumentOutOfRangeException();

            Mediaplayer.Volume = volume;
        }

        /// <summary>
        /// Play next track
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void NextTrack(object sender = null, EventArgs e = null)
        {
            Musicqueue.NextTrack(this);

            // Check if there is a next track 
            if (Musicqueue.RepeatTrack && Musicqueue.ActiveNode != null)
            {
                Musicqueue.ActiveNode = Musicqueue.ActiveNode.Previous;
            }
            else if (Musicqueue.RepeatQueue && Musicqueue.ActiveNode == Musicqueue.Queue.Last)
            {
                Musicqueue.ActiveNode = Musicqueue.Queue.First;
                Musicqueue.ActiveTrack = Musicqueue.ActiveNode.Value;
                NextTrack();
            }

            Mediaplayer.Open(Musicqueue.ActiveTrack.Filepath);
            if (State == PlayerState.Play)
                Play();
        }

        /// <summary>
        /// Play Previous Track
        /// </summary>
        public void PrevTrack()
        {
            Musicqueue.PrevTrack();

            // Check if there is a previous track
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

        /// <summary>
        /// Stop musicplayer
        /// </summary>
        public void Stop()
        {
            if (State != PlayerState.Stop)
            {
                Mediaplayer.Stop();
                State = PlayerState.Stop;
            }
        }

        /// <summary>
        /// Pause musicplayer
        /// </summary>
        public void Pause()
        {
            if (State != PlayerState.Pause)
            {
                Mediaplayer.Pause();
                State = PlayerState.Pause;
            }
        }

        /// <summary>
        /// Start musicplayer
        /// </summary>
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

        public void SetActiveTrackInPlayableContainer(Track track)
        {
            Musicqueue.FindActiveNode(track);
            Musicqueue.ActiveTrack = track;
            Mediaplayer.Open(new Uri(@"file:///" + track.audio));
        }

        public void SetActiveTrack(Track track)
        {
            PlayableContainer tracks = new Playlist("All Tracks");
            foreach (Track item in TracksManager.db_GetAllTracks())
            {
                tracks.Tracks.AddLast(item);
            }
            Musicqueue.LoadPlayableContainerInQueue(tracks);

            LinkedListNode<Track> test = Musicqueue.Queue.Find(track);

            SetActiveTrackInPlayableContainer(track);
        }

        /// <summary>
        /// Sets a 15min sleeptimer
        /// </summary>
        public void SetSleepTimer()
        {
            timer = new Timer();
            timer.Interval = 900000;
            timer.AutoReset = false;
            timer.Elapsed += new ElapsedEventHandler(stop_Timer);
            timer.Start();
        }

        /// <summary>
        /// Deactive Sleeptimer
        /// </summary>
        public void StopSleepTimer()
        {
            timer.Stop();
        }

        /// <summary>
        /// Stops Tracktimer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_Timer(object sender, EventArgs e)
        {
            Stop();
        }

        #endregion
    }
}