using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Services.Providers
{
    public interface ITVShowDiscovererProvider
    {
        ITVShowDiscoverer GetTVShowDiscoverer(string directory);
    }

    public class TVShowDiscovererProvider : ITVShowDiscovererProvider
    {
        private readonly IDataStore dataStore;
        private readonly IDirectoryScannerProvider directoryScannerProvider;

        public TVShowDiscovererProvider(IDataStore dataStore, IDirectoryScannerProvider directoryScannerProvider)
        {
            this.dataStore = dataStore;
            this.directoryScannerProvider = directoryScannerProvider;
        }

        public ITVShowDiscoverer GetTVShowDiscoverer(string directory)
        {
            var directoryScanner = directoryScannerProvider.GetDirectoryScanner(directory);
            directoryScanner.LoadData(dataStore);
            return new TVShowDiscoverer(dataStore, directoryScanner);
        }
    }
}
