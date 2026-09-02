using back_end.Data;
using back_end.Database.DbAccess.Interfaces;
using back_end.Shared.Core;
using Microsoft.EntityFrameworkCore;
using static back_end.Database.DbAccess.Title;

namespace back_end.Database.DbAccess
{
    public class ChapterTranslation : IChapterTranslation
    {
        private readonly AppDbContext _context;

        public ChapterTranslation(AppDbContext context)
        {
            _context = context;
        }

        private IQueryable<DTOs.ChapterTranslation> BuildQuery()
        {
            return _context.ChapterTranslations
                .AsNoTracking()
                .Select(ct => new DTOs.ChapterTranslation
                {
                    id = ct.id,
                    ChapterNumber = ct.Chapter.number,
                    LanguageId = ct.LanguageId,

                    Pages = ct.Page.Select(p => new DTOs.ChapterTranslationPage
                         {
                              id = p.id,
                              PageNumber = p.pageNumber,
                              ImageUrl = p.imageUrl
                         })
                });
        }

        private async Task<List<T>> RunQuery<T>(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }
        
        public async Task<Result<DTOs.ChapterTranslation>> GetChapterTranslation(int id)
        {
            try
            {
                IQueryable<DTOs.ChapterTranslation> query = BuildQuery();
                query = query.Where(ct => ct.id == id);
                List<DTOs.ChapterTranslation> ct = await RunQuery(query);
                return Result<DTOs.ChapterTranslation>.Success(ct.FirstOrDefault());
            }
            catch (Exception ex)
            {
                return Result<DTOs.ChapterTranslation>.Failure(ex.Message);
            }
        }
    }
}
