using System.ComponentModel.DataAnnotations;

namespace smelite_app.ViewModels.Blog
{
    public class BlogPostViewModel
    {
        public int? Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? CoverImageUrl { get; set; }

        public bool IsPublished { get; set; } = true;
    }
}
