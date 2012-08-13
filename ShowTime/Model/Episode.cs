using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ShowTime.Model
{
    public class Episode : Entity<EpisodeId>
    {
        public TVShowId TVShowId { get; private set; }
        public SeasonId SeasonId { get; private set; }
        public EpisodeId Id { get; private set; }

        public int Number { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string Filename { get; private set; }

        public Episode(TVShowId tvShowId, SeasonId seasonId, int number, string title, string description, string filename)
        {
            this.Id = EpisodeId.CreateId(tvShowId, seasonId, number);

            this.TVShowId = tvShowId;
            this.SeasonId = seasonId;
            this.Number = number;
            this.Title = title;
            this.Description = description;
            this.Filename = filename;
        }

        public override bool Equals(System.Object other)
        {
            Episode that = other as Episode;
            if ((object)that == null)
            {
                return false;
            }

            return this.Id.Equals(that.Id);
        }

        public bool Equals(Episode other)
        {
            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class EpisodeId
    {
        public static EpisodeId CreateId(TVShowId showId, SeasonId seasonId, int episodeNumber)
        {
            return new EpisodeId(showId, seasonId, episodeNumber);
        }

        public readonly TVShowId ShowId;
        public readonly SeasonId SeasonId;
        public readonly int EpisodeNumber;

        private EpisodeId(TVShowId showId, SeasonId seasonId, int episodeNumber)
        {
            this.ShowId = showId;
            this.SeasonId = seasonId;
            this.EpisodeNumber = episodeNumber;
        }

        public override bool Equals(System.Object other)
        {
            EpisodeId that = other as EpisodeId;
            if ((object)that == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.ShowId.Equals(that.ShowId) &&
                this.SeasonId.Equals(that.SeasonId) &&
                this.EpisodeNumber == that.EpisodeNumber;
        }

        public bool Equals(EpisodeId other)
        {
            return this.ShowId.Equals(other.ShowId) &&
                this.SeasonId.Equals(other.SeasonId) &&
                this.EpisodeNumber == other.EpisodeNumber;
        }

        public override int GetHashCode()
        {
            return this.ShowId.GetHashCode() ^ this.SeasonId.GetHashCode() ^ this.EpisodeNumber.GetHashCode();
        }
    }

    public interface Entity<TKey>
    {
        TKey Id { get; }
    }
}
