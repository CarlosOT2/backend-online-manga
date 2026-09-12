namespace back_end.DTOs
{
    public class Title
    {
        public class ChaptersDTO
        {
            public int id { get; set; }
            public decimal number { get; set; }
            public DateTime UpdatedAt { get; set; }
            public List<ChapterTranslationDTO>? translations { get; set; } = new List<ChapterTranslationDTO>();
        }
        public class ChapterTranslationDTO
        {
            public int id { get; set; }
            public string? chapterTitle { get; set; }
            public string ScanGroupName { get; set; } = null!;
            public bool isOfficial { get; set; }
            public DateTime uploadedAt { get; set; }
            public int viewCount { get; set; }
            public int LanguageId { get; set; }
        }

        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string synopsis { get; set; } = string.Empty;
        public DateOnly publicationDate { get; set; } = DateOnly.MinValue;
        public string img { get; set; } = string.Empty;
        public long viewCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;

        public int Status { get; set; } = -1;
        public int ContentRating { get; set; } = -1;
        public int Demographic { get; set; } = -1;

        public List<int>? genres { get; set; } = new List<int>();
        public List<int>? themes { get; set; } = new List<int>();
        public List<string>? authors { get; set; } = new List<string>();
        public List<string>? artists { get; set; } = new List<string>();
        public List<AlternativeNameDTO>? alternativenames { get; set; } = new List<AlternativeNameDTO>();
        public List<ChaptersDTO>? chapters { get; set; } = new List<ChaptersDTO>();    
    }
}
