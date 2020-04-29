using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSoundStream.ViewModel
{
	public class TrackMetadata
	{
		public int? Number { get; set; }
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Album { get; set; }
		public string Length { get; set; }
		public string Genre { get; set; }
		public string Year { get; set; }
	}
}
