using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShowTime.Services.EpisodeDetailsBuilders
{
    public interface IShowAttributeBuilder
    {
        ShowAttributeBuilderResults GuessShowName(IEpisodeFileSystemEntry episode);
    }

    public class ShowAttributeBuilderResults
    {
        private const string UNKNOWN_SHOW_NAME = "";
        public static ShowAttributeBuilderResults UNKNOWN_RESULTS = new ShowAttributeBuilderResults(UNKNOWN_SHOW_NAME);

        public readonly string ShowName;
        public bool Successful
        {
            get { return ShowName != UNKNOWN_SHOW_NAME; }
        }

        public ShowAttributeBuilderResults(string showName)
        {
            this.ShowName = showName;
        }
    }

    public class ShowAttributeBuilder : IShowAttributeBuilder
    {
        public ShowAttributeBuilderResults GuessShowName(IEpisodeFileSystemEntry episode)
        {
            // Assumes that the episode contains the name of the show, 
            // and that the parent or grandparent directory is named after the show
            // e.g. Videos\Bones\Bones.S02E10.avi
            // e.g. Videos\Bones\Season 8\Bones.S02E10.avi
            string episodeName = SanitiseName(episode.NameWithoutExtension);

            string parentDirectoryName = SanitiseName(episode.ParentDirectoryName);
            if (episodeName.Contains(parentDirectoryName))
                return new ShowAttributeBuilderResults(episode.ParentDirectoryName);

            string grandParentDirectoryName = SanitiseName(episode.GrandParentDirectoryName);
            if (episodeName.Contains(grandParentDirectoryName))
                return new ShowAttributeBuilderResults(episode.GrandParentDirectoryName);

            return ShowAttributeBuilderResults.UNKNOWN_RESULTS;
        }

        /// <summary>
        /// Remove whitespace and punctuation, converts all to lower case
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string SanitiseName(string input)
        {
            Regex whitespaceRegex = new Regex("\\s");
            input = whitespaceRegex.Replace(input, "");

            Regex punctuationRegex = new Regex("[^A-Za-z0-9]");
            input = punctuationRegex.Replace(input, "");

            return input.ToLower();
        }
    }
}
