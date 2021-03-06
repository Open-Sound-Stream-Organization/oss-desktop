﻿using System;

namespace OpenSoundStream
{
    public class Track
    {
        public Track(string title, Uri filepath)
        {
            //TODO
            this.title = title;
            Filepath = filepath;
            Metadata = new Metadata();
        }

        public Album Album { get; set; }
        public Metadata Metadata { get; set; }
        public Uri Filepath { get; set; }
        public int? id { get; set; }
        public string mbid { get; set; }
        public string title { get; set; }
        public string resource_uri { get; set; }
        public string album { get; set; }
        public string audio { get; set; }
        public string[] artists { get; set; }
        public string[] tags { get; set; }

    }
}