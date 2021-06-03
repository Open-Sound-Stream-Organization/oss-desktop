namespace OpenSoundStream
{
    public class OpenSoundStreamManager
    {
        public OpenSoundStreamManager()
        {
            DatabaseHandler = new DatabaseHandler();
            Musicplayer = new Musicplayer();
        }

        public static DatabaseHandler DatabaseHandler { get; set; }

        public static Musicplayer Musicplayer { get; set; }

    }
}