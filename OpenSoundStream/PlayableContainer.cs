using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSoundStream
{
	public abstract class PlayableContainer
	{
		public string name { get; set; }

		public LinkedList<Track> Tracks { get; set; }
	}
}