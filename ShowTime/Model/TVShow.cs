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

        public TVShow(string name, string description)
        {
            this.Id = TVShowId.CreateId(name);

            this.Name = name;
            this.Description = description;
        }

        public override bool Equals(System.Object other)
        {
            TVShow that = other as TVShow;
            if ((object)that == null)
            {
                return false;
            }

            return this.Id.Equals(that.Id);
        }

        public bool Equals(TVShow other)
        {
            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }

    public class TVShowId
    {
        public static TVShowId CreateId(string name)
        {
            return new TVShowId(name);
        }

        public readonly string Name;

        private TVShowId(string name)
        {
            this.Name = name;
        }

        public override bool Equals(System.Object other)
        {
            TVShowId that = other as TVShowId;
            if ((object)that == null)
            {
                return false;
            }

            return string.CompareOrdinal(this.Name, that.Name) == 0;
        }

        public bool Equals(TVShowId other)
        {
            return string.CompareOrdinal(this.Name, other.Name) == 0;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}
