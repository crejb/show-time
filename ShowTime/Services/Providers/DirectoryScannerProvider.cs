using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Services.EpisodeDetailsBuilders;

namespace ShowTime.Services.Providers
{
    public interface IDirectoryScannerProvider
    {
        IDirectoryScanner GetDirectoryScanner(string directory);
    }

    public class DirectoryScannerProvider : IDirectoryScannerProvider
    {
        private readonly IEpisodeDetailsBuilder episodeBuilder;

        public DirectoryScannerProvider(IEpisodeDetailsBuilder episodeBuilder)
        {
            this.episodeBuilder = episodeBuilder;
        }

        public IDirectoryScanner GetDirectoryScanner(string directory)
        {
            return new DirectoryScanner(directory, episodeBuilder);
        }
    }
}
