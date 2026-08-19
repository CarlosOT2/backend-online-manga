using static back_end.DTOs.Title;

namespace back_end.DTOs
{
    public class ChapterTranslation
    {
        public int id { get; set; }
        public decimal ChapterNumber { get; set; }
        public int LanguageId { get; set; }
        public IEnumerable<ChapterTranslationPage> Pages { get; set; } = new List<ChapterTranslationPage>();
    }

    public class ChapterTranslationPage
    {
        public int id { get; set; }
        public int PageNumber { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}