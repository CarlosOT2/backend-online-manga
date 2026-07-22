using back_end.Data;
using back_end.Database.DbAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using back_end.Shared.Core;
using back_end.Models;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace back_end.Database.DbAccess
{
    public class Title : ITitle
    {
        private readonly AppDbContext _context;

        public Title(AppDbContext context)
        {
            _context = context;
        }

        public class TitleQueryOptions
        {
            public bool IncludeAuthors { get; set; } = false;
            public bool IncludeArtists { get; set; } = false;
            public bool IncludeGenres { get; set; } = false;
            public bool IncludeThemes { get; set; } = false;
            public bool IncludeAlternativeNames { get; set; } = false;
            public bool IncludeChapters { get; set; } = false;
            public bool IncludeChaptersTranslation { get; set; } = false;
        }
        private IQueryable<DTOs.Title> BuildQuery(TitleQueryOptions? options = null)
        {
            options ??= new TitleQueryOptions();

            return _context.Titles
            .AsNoTracking()
            .AsSplitQuery()
            .Select(t => new DTOs.Title
            {
                id = t.id,
                name = t.name,
                synopsis = t.synopsis,
                publicationDate = t.publicationDate,
                img = t.img,
                CreatedAt = t.CreatedAt,

                Status = t.Status.id,
                ContentRating = t.ContentRating.id,
                Demographic = t.Demographic.id,

                authors = options.IncludeAuthors
                ? t.Author.Select(a => a.name)
                : null,

                artists = options.IncludeArtists
                ? t.Artist.Select(a => a.name)
                : null,

                genres = options.IncludeGenres
                ? t.Genre.Select(g => g.id)
                : null,

                themes = options.IncludeThemes
                ? t.Theme.Select(th => th.id)
                : null,

                alternativenames = options.IncludeAlternativeNames
                ? t.AlternativeNames.Select(alt => new DTOs.Title.AlternativeNameDTO
                {
                    name = alt.name,
                    languageId = alt.LanguageId
                })
                : null,

                chapters = options.IncludeChapters
                ? t.Chapters.Select(c => new DTOs.Title.ChaptersDTO
                {
                    id = c.id,
                    number = c.number,
                    UpdatedAt = c.UpdatedAt,

                    translations = options.IncludeChaptersTranslation
                    ? c.ChapterTranslation.Select(ct => new DTOs.Title.ChapterTranslationDTO
                    {
                        id = ct.id,
                        chapterTitle = ct.chapterTitle,
                        uploadedAt = ct.uploadedAt,
                        viewCount = ct.viewCount,
                        ScanGroupName = ct.ScanGroup.name,
                        LanguageId = ct.LanguageId
                    })
                    : null
                })
                : null
            });
        }

        private IQueryable<DTOs.ChapterLatestUpdates> BuildQueryLatest()
        {
            return _context.ChapterTranslations
            .AsNoTracking()
            .Select(ct => new DTOs.ChapterLatestUpdates
            {
                ChapterTranslationId = ct.id,
                ChapterNumber = ct.Chapter.number,
                uploadedAt = ct.uploadedAt,
                viewCount = ct.viewCount,
                LanguageId = ct.LanguageId,
                ScanGroupName = ct.ScanGroup.name,
                TitleId = ct.Chapter.Title.id,
                TitleName = ct.Chapter.Title.name,
                TitleImg = ct.Chapter.Title.img
            });
        }

        private async Task<List<T>> RunQuery<T>(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public async Task<Result<List<DTOs.Title>>> GetTitleById(int id)
        {
            try
            {
                IQueryable<DTOs.Title> query = BuildQuery(new TitleQueryOptions
                {
                    IncludeAuthors = true,
                    IncludeArtists = true,
                    IncludeGenres = true,
                    IncludeThemes = true,
                    IncludeAlternativeNames = true,
                    IncludeChapters = true,
                    IncludeChaptersTranslation = true
                }
                );
                query = query.Where(t => t.id == id);
                List<DTOs.Title> title = await RunQuery(query);

                return Result<List<DTOs.Title>>.Success(title);
            }
            catch (Exception ex)
            {
                return Result<List<DTOs.Title>>.Failure(ex.Message);
            }
        }
        public async Task<Result<List<DTOs.ChapterLatestUpdates>>> GetTitleLatestUpdates(int limit)
        {
            try
            {
                if (limit <= 0)
                    return Result<List<DTOs.ChapterLatestUpdates>>.Failure("The limit must be above than zero.");

                if (limit > 100)
                    return Result<List<DTOs.ChapterLatestUpdates>>.Failure("The limit cannot exceed 100.");

                IQueryable<DTOs.ChapterLatestUpdates> query = BuildQueryLatest()
                   .OrderByDescending(ct => ct.uploadedAt)
                   .Take(limit);

                List<DTOs.ChapterLatestUpdates> result = await RunQuery(query);

                return Result<List<DTOs.ChapterLatestUpdates>>.Success(result);
            }
            catch (Exception ex)
            {
                return Result<List<DTOs.ChapterLatestUpdates>>.Failure(ex.Message);
            }
        }
        public async Task<Result<List<DTOs.Title>>> GetTitleRecentlyAdded(int limit, bool compact)
        {
            try
            {
                if (limit <= 0)
                    return Result<List<DTOs.Title>>.Failure("The limit must be above than zero.");

                if (limit > 100)
                    return Result<List<DTOs.Title>>.Failure("The limit cannot exceed 100.");

                IQueryable<DTOs.Title> query = compact ? BuildQuery()
                : BuildQuery(new TitleQueryOptions
                {
                    IncludeThemes = true,
                    IncludeGenres = true,
                    IncludeAuthors = true,
                    IncludeArtists = true,
                });

                query = query
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(limit);

                List<DTOs.Title> title = await RunQuery(query);
                return Result<List<DTOs.Title>>.Success(title);
            }
            catch (Exception ex)
            {
                return Result<List<DTOs.Title>>.Failure(ex.Message);
            }
        }
        public async Task<Result<List<DTOs.Title>>> GetFeaturedTitles(int limit)
        {
            try
            {
                if (limit > 100)
                    return Result<List<DTOs.Title>>.Failure("The limit cannot exceed 100.");

                IQueryable<DTOs.Title> query = BuildQuery(new TitleQueryOptions
                {
                    IncludeThemes = true,
                    IncludeGenres = true,
                    IncludeAuthors = true,
                    IncludeArtists = true
                });
                query = query.Take(limit);

                List<DTOs.Title> titles = await RunQuery(query);
                return Result<List<DTOs.Title>>.Success(titles);
            }
            catch (Exception ex)
            {
                return Result<List<DTOs.Title>>.Failure(ex.Message);
            }
        }
        public async Task<Result<List<DTOs.Title>>> GetTitlesByFilters(
            string? name,
            string? author,
            string? artist,

            int[]? genresIds,
            int[]? themesIds,
            int[]? demographicIds,
            int[]? statusIds,
            int[]? contentRatingIds,
            int? publicationYear,

            int[]? excludeGenresIds,
            int[]? excludeThemesIds
            )
        {
            try
            {
                //? Variables
                IQueryable<DTOs.Title> query = BuildQuery(new TitleQueryOptions
                {
                    IncludeAuthors = true,
                    IncludeArtists = true,
                    IncludeGenres = true,
                    IncludeThemes = true,
                    IncludeAlternativeNames = true
                }
                );

                //? Verification
                IEnumerable<int> conflictingGenres = genresIds?.Intersect(excludeGenresIds ?? []) ?? [];
                IEnumerable<int> conflictingThemes = themesIds?.Intersect(excludeThemesIds ?? []) ?? [];

                if (conflictingGenres.Any() || conflictingThemes.Any())
                    return Result<List<DTOs.Title>>.Failure("Invalid filters: same id cannot be included and excluded simultaneously");

                //# Include
                //? Name
                if (!string.IsNullOrWhiteSpace(name))
                    query = query.Where(t => EF.Functions.ILike(t.name, $"%{name}%"));

                //? Author
                if (!string.IsNullOrWhiteSpace(author))
                    query = query.Where(t => t.authors!.Any(aut => EF.Functions.ILike(aut, $"%{author}%")));

                //? Artist
                if (!string.IsNullOrWhiteSpace(artist))
                    query = query.Where(t => t.artists!.Any(art => EF.Functions.ILike(art, $"%{artist}%")));

                //? Genres
                if (genresIds?.Length >= 1)
                    query = query.Where(t => genresIds.All(id => t.genres!.Contains(id)));

                //? Themes
                if (themesIds?.Length >= 1)
                    query = query.Where(t => themesIds.All(id => t.themes!.Contains(id)));

                //? Demographic
                if (demographicIds?.Length >= 1)
                    query = query.Where(t => demographicIds.Contains(t.Demographic));

                //? Status
                if (statusIds?.Length >= 1)
                    query = query.Where(t => statusIds.Contains(t.Status));

                //? Content Rating
                if (contentRatingIds?.Length >= 1)
                    query = query.Where(t => contentRatingIds.Contains(t.ContentRating));

                //? Publication Year
                if (publicationYear.HasValue)
                    query = query.Where(t => t.publicationDate.Year == publicationYear.Value);

                //# Exclude 
                if (excludeGenresIds?.Length >= 1)
                    query = query.Where(t => !excludeGenresIds.Any(id => t.genres!.Contains(id)));

                if (excludeThemesIds?.Length >= 1)
                    query = query.Where(t => !excludeThemesIds.Any(id => t.themes!.Contains(id)));

                List<DTOs.Title> titles = await RunQuery(query);
                return Result<List<DTOs.Title>>.Success(titles);
            }
            catch (Exception ex)
            {
                return Result<List<DTOs.Title>>.Failure(ex.Message);
            }
        }
    }
}
