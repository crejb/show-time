using System;
namespace ShowTime
{
    public interface IDataStore
    {
        ShowTime.Repositories.IRepository<ShowTime.Model.EpisodeId, ShowTime.Model.Episode> EpisodeRepository { get; }
        ShowTime.Repositories.IRepository<ShowTime.Model.SeasonId, ShowTime.Model.Season> SeasonRepository { get; }
        ShowTime.Repositories.IRepository<ShowTime.Model.TVShowId, ShowTime.Model.TVShow> TVShowRepository { get; }
        ShowTime.Repositories.IRepository<ShowTime.Model.BookmarkId, ShowTime.Model.Bookmark> BookmarkRepository { get; }
    }
}
