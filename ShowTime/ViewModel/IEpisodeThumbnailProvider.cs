using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;

namespace ShowTime.ViewModel
{
    public interface IEpisodeThumbnailProvider
    {
        string GetThumbnailFilanemeForEpisode(Episode episode);
    }

    public class MockEpisodeThumbnailProvider : IEpisodeThumbnailProvider
    {
        public string GetThumbnailFilanemeForEpisode(Episode episode)
        {
            return @"C:\ShowTimeData\Thumbnails\Test.jpg";
        }
    }
}
