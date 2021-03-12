using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamGitBlogMaker
{
    public class Music
    {
        public string MusicName { get; set; }
        public string MusicPath { get; set; }
        public string LyricContent { get; set; }
        public Music(string musicName, string musicPath, string lyricContent)
        {
            MusicName = musicName;
            MusicPath = musicPath;
            LyricContent = lyricContent;
        }
    }
}