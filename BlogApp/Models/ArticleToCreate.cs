using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class ArticleToCreate
    {
        public int ArticleId { get; set; }

        [Required]
        public string? ArticleTitle { get; set; }

        public DateTime? ArticleCreatedDate { get; set; } = DateTime.Now;

        public string? ArticleContent { get; set; }

        public int? ArticleCategory { get; set; }

        public IFormFile? ArticleThumbNail { get; set; }

        public string? ArticleThumbnailType { get; set; }

        public string IsArticleDeleted { get; set; } = "N";

        public DateTime ArticleDeletedDate { get; set; }

        public string? ShowInHome { get; set; }
    }
}
