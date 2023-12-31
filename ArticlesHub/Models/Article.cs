﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArticlesHub.Models
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Author { get; set; }

        public ICollection<Image>? Images { get; set; }
    }
}

