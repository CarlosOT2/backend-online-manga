using back_end.Data;
using back_end.Database.DbAccess;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace back_end.Database.Seeds
{
    public class Seeder
    {
        private readonly AppDbContext _context;
        private readonly DbSeeds _DbSeeds;

        public Seeder(AppDbContext context)
        {
            _context = context;
            _DbSeeds = new DbSeeds(context);
        }

        // Chapter seeding is done manually because each title requires multiple chapters
        // with unique and ordered numbers — something that couldn't be easily implemented
        // in the generic DbSeeds.Run<T>() method.
        private async Task SeedChapters()
        {
            Random random = Random.Shared;
            List<Models.Title> titles = await _context.Titles.ToListAsync();
            List<Models.Chapter> chapters = new List<Models.Chapter>()!;

            int id = 1;
            foreach (Models.Title title in titles)
            {
                int totalChapters = random.Next(1, 201);
                SortedSet<decimal> chapterNumbers = new SortedSet<decimal>()!;

                while (chapterNumbers.Count < totalChapters)
                {
                    double roll = random.NextDouble();
                    decimal number = roll < 0.05 ? random.Next(151, 201)
                                   : roll < 0.25 ? random.Next(51, 151)
                                   : random.Next(1, 51);

                    decimal fraction = random.NextDouble() < 0.7 ? 0m : random.Next(1, 10) / 10m;
                    chapterNumbers.Add(number + fraction);
                }

                foreach (decimal number in chapterNumbers)
                    chapters.Add(new Models.Chapter { id = id++, TitleId = title.id, number = number });
            }

            _context.Chapters.AddRange(chapters);
            await _context.SaveChangesAsync();
        }
        private async Task SeedChapterTranslations()
        {
            Random random = Random.Shared;
            List<Models.Chapter> chapters = await _context.Chapters.ToListAsync();
            List<Models.ScanGroup> scanGroups = await _context.ScanGroups.ToListAsync();
            List<Models.Language> languages = await _context.Languages.ToListAsync();

            List<Models.ChapterTranslation> translations = new List<Models.ChapterTranslation>();
            int id = 1;

            DateTime startDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            int totalDays = (DateTime.UtcNow - startDate).Days;

            List<string> chapterTitles = await LoadSeedJson("ChapterTitleNames");

            foreach (Models.Chapter chapter in chapters)
            {
                int qntTranslactions = random.Next(1, 5);
                HashSet<(int scanGroupId, int languageId)> usedCombinations = new();
                DateTime latestUploadForChapter = DateTime.MinValue;

                int attempts = 0;
                while (usedCombinations.Count < qntTranslactions && attempts < 50)
                {
                    attempts++;
                    int scanGroupId = scanGroups[random.Next(scanGroups.Count)].id;
                    int languageId = languages[random.Next(languages.Count)].id;

                    if (!usedCombinations.Add((scanGroupId, languageId)))
                        continue;

                    DateTime uploadedAt = startDate.AddDays(random.Next(totalDays)).AddSeconds(random.Next(0, 86400));

                    translations.Add(new Models.ChapterTranslation
                    {
                        id = id++,
                        chapterTitle = chapterTitles[random.Next(chapterTitles.Count)],
                        ChapterId = chapter.id,
                        ScanGroupId = scanGroupId,
                        LanguageId = languageId,
                        uploadedAt = uploadedAt,
                        viewCount = random.Next(0, 10000)
                    });

                    if (uploadedAt > latestUploadForChapter)
                        latestUploadForChapter = uploadedAt;
                }

                chapter.UpdatedAt = latestUploadForChapter;
            }

            _context.ChapterTranslations.AddRange(translations);
            await _context.SaveChangesAsync();
        }
        private async Task SeedPages()
        {
            List<Models.ChapterTranslation> chapterTranslations = await _context.ChapterTranslations.ToListAsync();
            List<Models.Page> pages = new List<Models.Page>();
            List<string> imageUrls = await LoadSeedJson("ImagesUrl");

            int id = 1;

            foreach (Models.ChapterTranslation chapterTranslation in chapterTranslations)
            {
                int numPages = Random.Shared.Next(1, 101);

                int currentPageNum = 1;
                while(currentPageNum <= numPages)
                {
                    pages.Add(new Models.Page
                    {
                        id = id++,
                        pageNumber = currentPageNum,
                        imageUrl = imageUrls[(currentPageNum - 1) % imageUrls.Count],
                        ChapterTranslationId = chapterTranslation.id
                    });

                    currentPageNum++;
                }
            }

            _context.Pages.AddRange(pages);
            await _context.SaveChangesAsync();
        }

        private async Task<List<string>> LoadSeedJson(string keyName)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Database", "Seeds", "Seed.json");
            string json = await File.ReadAllTextAsync(path);
            using JsonDocument doc = JsonDocument.Parse(json);
            JsonElement seedArray = doc.RootElement.GetProperty(keyName);

            List<string> seedData = new();
            foreach (JsonElement item in seedArray.EnumerateArray())
                seedData.Add(item.GetString()!);

            return seedData;
        }
        private async Task<string> LoadSeedJsonWithPipe(string keyName)
        {
            List<string> seedData = await LoadSeedJson(keyName);
            return string.Join('|', seedData);
        }

        public async Task Run(int rows)
        {
            await _DbSeeds.Clear();
            await _DbSeeds.Static();

            await _DbSeeds.Run<Models.Title>("Titles", rows, new Models.Title
            {
                name = await LoadSeedJsonWithPipe("Names"),
                synopsis = await LoadSeedJsonWithPipe("Synopsis"),
                // this prop will generate random dates starting from this date
                publicationDate = new DateOnly(2005, 1, 1),
                img = await LoadSeedJsonWithPipe("Images"),
                // this prop will generate random dates starting from this date
                CreatedAt = new DateTime(2000, 1, 1, 0, 0, 0)
            });
            await _DbSeeds.Run<Models.Artist>("Artists", rows, new Models.Artist { name = "Artist" });
            await _DbSeeds.Run<Models.Author>("Authors", rows, new Models.Author { name = "Author" });
            await _DbSeeds.Run<Models.AlternativeName>("AlternativeNames", rows, new Models.AlternativeName { name = "Alternative Name" });
            await _DbSeeds.Run<Models.ScanGroup>("ScanGroups", rows, new Models.ScanGroup { name = "ScanGroup", websiteUrl = "teste" });
            await SeedChapters();
            await SeedChapterTranslations();
            await SeedPages();
        }
    }
}