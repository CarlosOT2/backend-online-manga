using back_end.Shared.Core;

namespace back_end.Database.DbAccess.Interfaces
{
    public interface IChapterTranslation
    {
        Task<Result<List<DTOs.ChapterTranslation>>> GetChapterTranslation(int id);
    }
}
