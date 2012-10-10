using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShowTime.Model;
using ShowTime.Services.CloseActions;

namespace ShowTime
{
    public class VideoPlayRequest
    {
        public EpisodeId EpisodeId { get; private set; }
        public Bookmark Bookmark { get; private set; }

        public VideoPlayRequest(EpisodeId episodeId, Bookmark bookmark = null)
        {
            this.EpisodeId = episodeId;
            this.Bookmark = bookmark;
        }
    }

    public class VideoPlayRequestHandler
    {
        private IDataStore dataStore;
        private IPlayer videoPlayer;
        private IEpisodeCloseActionsCalculatorProvider closeActionCalculatorProvider;
        private IEpisodeCloseActionsExecutor closeActionExecutor;

        public VideoPlayRequestHandler(
            IPlayer videoPlayer,
            IDataStore dataStore, 
            IEpisodeCloseActionsCalculatorProvider closeActionCalculatorProvider,
            IEpisodeCloseActionsExecutor closeActionExecutor)
        {
            this.videoPlayer = videoPlayer;
            this.dataStore = dataStore;
            this.closeActionCalculatorProvider = closeActionCalculatorProvider;
            this.closeActionExecutor = closeActionExecutor;

            videoPlayer.Closed += playWindow_Closed;
        }

        public void PlayVideo(VideoPlayRequest request)
        {
            var episodeToPlay = dataStore.EpisodeRepository.Find(request.EpisodeId);
            if (episodeToPlay == null)
            {
                //TODO: Log error
                return;
            }

            videoPlayer.Play(episodeToPlay, request.Bookmark);
        }

        private void playWindow_Closed(object sender, EpisodePlayerClosedEventArgs e)
        {
            //var episode = (sender as IPlayer).Episode;
            var episode = e.Episode;

            var actionCalculator = closeActionCalculatorProvider.GetActionsCalculator(episode, e);
            closeActionExecutor.ExecuteActions(episode, e, actionCalculator);
        }
    }
}
