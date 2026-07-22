namespace back_end.DTOs
{
    public class ChapterLatestUpdates
    {
        public int TitleId { get; set; }
        public string TitleName { get; set; } = string.Empty;
        public string TitleImg { get; set; } = string.Empty;
        public int ChapterTranslationId { get; set; }
        public decimal ChapterNumber { get; set; }
        public string ScanGroupName { get; set; } = null!;
        public DateTime uploadedAt { get; set; }
        public int viewCount { get; set; }
        public int LanguageId { get; set; }
    }
}
