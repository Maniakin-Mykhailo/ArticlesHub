using System.ComponentModel.DataAnnotations;

namespace ArticlesHub.Models
{
    public class Image
    {
        public int Id { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public int? ArticleId { get; set; }
        public Article? Article { get; set; }
    }
}
