using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Model
{
    public class Season : Entity<SeasonId>
    {
        public TVShowId TVShowId { get; private set; }
        public SeasonId Id { get; private set; }
        public int Number { get; private set; }

        public Season(TVShowId tvShowId, int number)
        {
            this.Id = SeasonId.CreateId(tvShowId, number);

            this.TVShowId = tvShowId;
            this.Number = number;
        }

        public override bool Equals(System.Object other)
        {
            Season that = other as Season;
            if ((object)that == null)
            {
                return false;
            }

            return this.Id.Equals(that.Id);
        }

        public bool Equals(Season other)
        {
            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class SeasonId
    {
        public static SeasonId CreateId(TVShowId showId, int seasonNumber)
        {
            return new SeasonId(showId, seasonNumber);
        }

        public readonly TVShowId ShowId;
        public readonly int SeasonNumber;

        private SeasonId(TVShowId showId, int seasonNumber)
        {
            this.ShowId = showId;
            this.SeasonNumber = seasonNumber;
        }

        public override bool Equals(System.Object other)
        {
            SeasonId that = other as SeasonId;
            if ((object)that == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.ShowId.Equals(that.ShowId) && this.SeasonNumber == that.SeasonNumber;
        }

        public bool Equals(SeasonId other)
        {
            return this.ShowId.Equals(other.ShowId) && this.SeasonNumber == other.SeasonNumber;
        }

        public override int GetHashCode()
        {
            return this.ShowId.GetHashCode() ^ this.SeasonNumber.GetHashCode();
        }
    }
}
