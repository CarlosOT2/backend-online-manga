using static back_end.DTOs.Title;

namespace back_end.DTOs
{
    public class FastTitle
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;

        public int Status { get; set; } = -1;
        public int ContentRating { get; set; } = -1;
        public int Demographic { get; set; } = -1;

        public string img { get; set; } = string.Empty;
        public List<AlternativeNameDTO>? alternativenames { get; set; } = new List<AlternativeNameDTO>();
    }
}
