using System.Collections.ObjectModel;
using System.Linq;

namespace OpenSoundStream.ViewModel
{
    //TODO
    public static class SortList
    {
        /// <summary>
        /// Order tracks alphabetically after Artists 
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public static ObservableCollection<TrackMetadata> sortArtists(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Artist));
        }

        /// <summary>
        /// Order tracks alphabetically after Title 
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public static ObservableCollection<TrackMetadata> sortTitle(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Title));
        }

        /// <summary>
        /// Order tracks alphabetically after Album 
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public static ObservableCollection<TrackMetadata> sortAlbums(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Album));
        }

        /// <summary>
        /// Order tracks alphabetically after Genre 
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public static ObservableCollection<TrackMetadata> sortGenre(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Genre));
        }

        /// <summary>
        /// Order tracks after Yeat 
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public static ObservableCollection<TrackMetadata> sortYear(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Year));
        }

        /// <summary>
        /// Order tracks after Length 
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public static ObservableCollection<TrackMetadata> sortLength(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Length));
        }

        /// <summary>
        /// Order tracks after Track Number 
        /// </summary>
        /// <param name="tracks"></param>
        /// <returns></returns>
        public static ObservableCollection<TrackMetadata> sortNumber(ObservableCollection<TrackMetadata> tracks)
        {
            return new ObservableCollection<TrackMetadata>(tracks.OrderBy(x => x.Number));
        }
    }
}
