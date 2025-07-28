using System;

namespace smelite_app.ViewModels.Blog
{
    public class BlogPostListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? AuthorName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublished { get; set; }
    }
}
