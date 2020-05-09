using System.Collections.Generic;
//TODO 

namespace OpenSoundStream
{
	public abstract class PlayableContainer
	{
		public string name { get; set; }

		public LinkedList<Track> Tracks { get; set; }
	}
}