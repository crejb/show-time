using System;
namespace ShowTime
{
    public interface IDataStore
    {
        ShowTime.Repositories.IRepository<ShowTime.Model.Episode, ShowTime.Model.EpisodeId> EpisodeRepository { get; }
        ShowTime.Repositories.IRepository<ShowTime.Model.Season, ShowTime.Model.SeasonId> SeasonRepository { get; }
        ShowTime.Repositories.IRepository<ShowTime.Model.TVShow, ShowTime.Model.TVShowId> TVShowRepository { get; }
    }
}
