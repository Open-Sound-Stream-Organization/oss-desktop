using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
    public class OpenSoundStreamManager
    {
        public DatabaseHandler DatabaseHandler { get; set; }

        public NetworkHandler NetworkHandler { get; set; }

        public Musicplayer Musicplayer { get; set; }

    }
}