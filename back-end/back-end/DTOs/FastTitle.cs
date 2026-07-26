using static back_end.DTOs.Title;

namespace back_end.DTOs
{
    public class FastTitle
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string img { get; set; } = string.Empty;
        public IEnumerable<AlternativeNameDTO>? alternativenames { get; set; } = new List<AlternativeNameDTO>();
    }
}
