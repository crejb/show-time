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
            this.Id = EpisodeId.CreateId();

            this.TVShowId = tvShowId;
            this.SeasonId = seasonId;
            this.Number = number;
            this.Title = title;
            this.Description = description;
            this.Filename = filename;
        }
    }

    public class EpisodeId
    {
        private static long idCounter = 1;

        public static EpisodeId CreateId()
        {
            return new EpisodeId(idCounter++);
        }

        public readonly long Id;

        private EpisodeId(long id)
        {
            this.Id = id;
        }

        public override bool Equals(System.Object other)
        {
            EpisodeId that = other as EpisodeId;
            if ((object)that == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Id == that.Id;
        }

        public bool Equals(EpisodeId other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public interface Entity<TKey>
    {
        TKey Id { get; }
    }
}
