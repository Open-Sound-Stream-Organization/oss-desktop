using System;

namespace OpenSoundStream
{
    public class Metadata
    {
        public double BPM { get; set; }

        public string CoverFile { get; set; }

        public string Genre { get; set; }

        public int Year { get; set; }

        public TimeSpan Length { get; set; }
    }
}