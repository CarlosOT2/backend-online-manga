using back_end.Shared.Core;

namespace back_end.Database.DbAccess.Interfaces
{
    public interface IChapterTranslation
    {
        Task<Result<DTOs.ChapterTranslation>> GetChapterTranslation(int id);
    }
}
