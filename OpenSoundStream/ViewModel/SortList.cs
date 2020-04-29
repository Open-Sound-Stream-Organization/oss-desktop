using System.Collections.ObjectModel;
using System.Linq;

namespace OpenSoundStream.ViewModel
{
    public static class SortList
    {
        public static ObservableCollection<TrackMetadata> sortArtists(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Artist));
        }

        public static ObservableCollection<TrackMetadata> sortAlbums(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Album));
        }

        public static ObservableCollection<TrackMetadata> sortGenre(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Genre));
        }

        public static ObservableCollection<TrackMetadata> sortYear(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Year));
        }

        public static ObservableCollection<TrackMetadata> sortLength(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Length));
        }

        public static ObservableCollection<TrackMetadata> sortNumber(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Number));
        }
    }
}
