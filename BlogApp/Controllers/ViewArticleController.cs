using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BlogApp.Controllers
{
    public class ViewArticleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ViewArticleController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var articleDetails = _context.AllArticles.Find(id);
            //var contentList = JsonSerializer.Deserialize<IList<ArticleContent>>(articleDetails.ArticleContent);
            return View(articleDetails);
        }
    }
}
