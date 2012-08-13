using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ShowTime.Services.Guessers
{
    public interface IShowAttributeGuesser
    {
        ShowGuesserResults GuessShowName(IEpisodeFileSystemEntry episode);
    }

    public class ShowGuesserResults
    {
        private const string UNKNOWN_SHOW_NAME = "";
        public static ShowGuesserResults UNKNOWN_RESULTS = new ShowGuesserResults(UNKNOWN_SHOW_NAME);

        public readonly string ShowName;
        public bool Successful
        {
            get { return ShowName != UNKNOWN_SHOW_NAME; }
        }

        public ShowGuesserResults(string showName)
        {
            this.ShowName = showName;
        }
    }

    public class ShowGuesser : IShowAttributeGuesser
    {
        public ShowGuesserResults GuessShowName(IEpisodeFileSystemEntry episode)
        {
            // Assumes that the episode contains the name of the show, 
            // and that the parent or grandparent directory is named after the show
            // e.g. Videos\Bones\Bones.S02E10.avi
            // e.g. Videos\Bones\Season 8\Bones.S02E10.avi
            string episodeName = SanitiseName(episode.NameWithoutExtension);

            string parentDirectoryName = SanitiseName(episode.ParentDirectoryName);
            if (episodeName.Contains(parentDirectoryName))
                return new ShowGuesserResults(episode.ParentDirectoryName);

            string grandParentDirectoryName = SanitiseName(episode.GrandParentDirectoryName);
            if (episodeName.Contains(grandParentDirectoryName))
                return new ShowGuesserResults(episode.GrandParentDirectoryName);

            return ShowGuesserResults.UNKNOWN_RESULTS;
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
