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
            this.Id = SeasonId.CreateId();

            this.TVShowId = tvShowId;
            this.Number = number;
        }
    }

    public class SeasonId
    {
        private static long idCounter = 1;

        public static SeasonId CreateId()
        {
            return new SeasonId(idCounter++);
        }

        public readonly long Id;

        private SeasonId(long id)
        {
            this.Id = id;
        }

        public override bool Equals(System.Object other)
        {
            SeasonId that = other as SeasonId;
            if ((object)that == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Id == that.Id;
        }

        public bool Equals(SeasonId other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
