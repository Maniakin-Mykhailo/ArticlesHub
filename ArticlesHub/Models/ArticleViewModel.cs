using System.ComponentModel.DataAnnotations;

namespace ArticlesHub.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public Article Article { get; set; }
        public List<Image> Images { get; set; }

        //[Display(Name = "Remove Images")]
        public List<int> RemoveImages { get; set; }

        [Display(Name = "New Images")]
        public List<IFormFile>? NewImages { get; set; }
    }
}
