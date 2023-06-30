namespace ArticlesHub.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
