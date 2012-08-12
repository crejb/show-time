using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Model
{
    public class TVShow : Entity<TVShowId>
    {
        public TVShowId Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public TVShow(TVShowId id, string name, string description)
        {
            this.Id = id;

            this.Name = name;
            this.Description = description;
        }

        public TVShow(string name, string description)
        {
            this.Id = TVShowId.CreateId();

            this.Name = name;
            this.Description = description;
        }


    }

    public class IdTracker
    {
        private long idCounter = 1;
        private HashSet<long> usedIds = new HashSet<long>();

        public long GetNextId()
        {
            while (usedIds.Contains(idCounter))
                idCounter++;

            usedIds.Add(idCounter);
            return idCounter;
        }

        public long GetId(long id)
        {
            if (usedIds.Contains(id))
                throw new Exception();

            usedIds.Add(id);
            return id;
        }
    }

    public class TVShowId
    {
        private static IdTracker idTracker = new IdTracker();

        public static TVShowId CreateId()
        {
            return new TVShowId(idTracker.GetNextId());
        }

        public static TVShowId CreateId(long id)
        {
            return new TVShowId(idTracker.GetId(id));
        }

        public readonly long Id;

        private TVShowId(long id)
        {
            this.Id = id;
        }

        public override bool Equals(System.Object other)
        {
            TVShowId that = other as TVShowId;
            if ((object)that == null)
            {
                return false;
            }

            // Return true if the fields match:
            return this.Id == that.Id;
        }

        public bool Equals(TVShowId other)
        {
            return this.Id == other.Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
