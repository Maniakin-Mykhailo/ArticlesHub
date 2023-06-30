using ArticlesHub.Models;
using System.ComponentModel.DataAnnotations;

public class Article
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Text { get; set; }
    [Required]
    public string Author { get; set; }

    public List<Image>? Images { get; set; }
}
