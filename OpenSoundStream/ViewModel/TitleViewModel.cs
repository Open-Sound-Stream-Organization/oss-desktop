using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.ComponentModel;
using OpenSoundStream.Code.DataManager;

namespace OpenSoundStream.ViewModel
{
    public class TitleViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Variables and Binding Properties

        private static MainViewModel mainViewModel = MainViewModel.mainViewModel;
		private static ObservableCollection<TrackMetadata> _tracks = new ObservableCollection<TrackMetadata>();
		public static ObservableCollection<TrackMetadata> Tracks { get { return _tracks; } }
		public RelayCommand<TrackMetadata> ListViewCommand { get; private set; }

        #endregion

        public TitleViewModel()
		{
			this.ListViewCommand = new RelayCommand<TrackMetadata>((item) => this.playSelectedTrack(item));
		}

		/// <summary>
		/// Plays a selected track from ListView
		/// </summary>
		/// <param name="selectedTrack"></param>
		private void playSelectedTrack(TrackMetadata selectedTrack)
		{
			MainViewModel.musicplayer.SetActiveTrack(TracksManager.db_GetAllTracks().Find(x => x.title == selectedTrack.Title));
			mainViewModel.playMusic();
		}
	}
}
