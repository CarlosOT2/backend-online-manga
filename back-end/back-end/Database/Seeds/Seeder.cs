using back_end.Data;
using back_end.Database.DbAccess;
using Microsoft.EntityFrameworkCore;

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
                        chapterTitle = "Test",
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

        public async Task Run(int rows)
        {
            await _DbSeeds.Clear();
            await _DbSeeds.Static();

            await _DbSeeds.Run<Models.Title>("Titles", rows, new Models.Title
            {
                name = "Manga",
                synopsis = "A 34-year-old NEET gets killed in a traffic accident and finds himself in a world of magic. " +
                "Rather than waking up as a full-grown mage, he gets reincarnated as a newborn baby, retaining the memories of his past life. " +
                "Before he can even properly move his body, " +
                "he resolves to never make the same mistakes he made in his first life ever again and instead live a life with no regrets with the new one that was given " +
                "to him. Because he has the knowledge of a middle-aged man, by the age of two, he has already become a prodigy and possesses power unthinkable for " +
                "anyone his age and even older. Thus begins the chronicles of Rudeus Greyrat, son of swordsman Paul and healer Zenith, as he enters a new world to " +
                "become the strongest mage known to man, with powers rivaling even the gods themselves.",
                // this prop will generate random dates starting from this date
                publicationDate = new DateOnly(1950, 1, 1),
                img = "image",
                // this prop will generate random dates starting from this date
                CreatedAt = new DateTime(2000, 1, 1, 0, 0, 0),
            });
            await _DbSeeds.Run<Models.Artist>("Artists", rows, new Models.Artist { name = "Artist" });
            await _DbSeeds.Run<Models.Author>("Authors", rows, new Models.Author { name = "Author" });
            await _DbSeeds.Run<Models.AlternativeName>("AlternativeNames", rows, new Models.AlternativeName { name = "Alternative Name" });
            await _DbSeeds.Run<Models.ScanGroup>("ScanGroups", rows, new Models.ScanGroup { name = "ScanGroup", websiteUrl = "teste" });
            await SeedChapters();
            await SeedChapterTranslations();
        }
    }
}