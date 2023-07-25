using BlogApp.Data;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Controllers
{
    public class CreateArticlesController : Controller
    {

        private readonly ApplicationDbContext _context;

        [Obsolete]
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

        [Obsolete]
        public CreateArticlesController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }

        public IActionResult CreateNewArticle(int id)
        {
            if(id == 0) { return View(); }
            else
            {
                var articleDetails = _context.AllArticles.Find(id);

                var articletoedit = new ArticleToCreate
                {
                    ArticleTitle = articleDetails.ArticleTitle,
                    ArticleId = articleDetails.ArticleId,
                    ArticleContent = articleDetails.ArticleContent,
                    ArticleCategory = articleDetails.ArticleCategory,
                    ArticleCreatedDate = articleDetails.ArticleCreatedDate,
                    ArticleDeletedDate = articleDetails.ArticleDeletedDate,
                    IsArticleDeleted = articleDetails.IsArticleDeleted
                };


                return View(articletoedit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Obsolete]
        public IActionResult CreateNewArticle(ArticleToCreate articleobj, bool updatemode)
        {
            string thumbnailfileName = "";
            if (ModelState.IsValid)
            {
                var contentList = JsonSerializer.Deserialize<IList<ArticleContent>>(articleobj.ArticleContent);

                // saving thumbnail
                if(articleobj.ArticleThumbNail != null)
                {
                    string thumbnailpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    //create folder if not exist
                    if (!Directory.Exists(thumbnailpath))
                        Directory.CreateDirectory(thumbnailpath);

                    FileInfo thumbnailfileInfo = new FileInfo(articleobj.ArticleThumbNail.FileName);
                    thumbnailfileName = articleobj.ArticleTitle.ToString().Replace(" ", "_") + "_thumbnail" + thumbnailfileInfo.Extension;

                    string thumbnailfileNameWithPath = Path.Combine(thumbnailpath, thumbnailfileName);

                    using (var stream = new FileStream(thumbnailfileNameWithPath, FileMode.Create))
                    {
                        articleobj.ArticleThumbNail.CopyTo(stream);
                    }
                }
                // saving thumbnail


                foreach (var content in contentList)
                {
                    if(content.element_type == "image")
                    {
                        var base64index = content.image_base64.IndexOf("base64,") + 7;
                        byte[] imagebytes = Convert.FromBase64String((content.image_base64.Substring(base64index)));
                        string path = Path.Combine(Environment.WebRootPath, "images");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        var fileName = articleobj.ArticleTitle.ToString().Replace(" ","_") + "_" + content.content_id.ToString() + "." + content.image_type;

                        var filePath = Path.Combine(path, fileName);

                        var contents = new StreamContent(new MemoryStream(imagebytes));

                        using (FileStream fs = System.IO.File.Create(filePath))

                        {

                            contents.CopyToAsync(fs);

                        }

                        content.image_base64 = fileName;

                    }
                }

                articleobj.ArticleContent = JsonSerializer.Serialize(contentList);

                if (!updatemode)
                {
                    _context.AllArticles.Add(new Article 
                    { 
                    ArticleTitle = articleobj.ArticleTitle,
                    ArticleCategory = articleobj.ArticleCategory,
                    ArticleContent = articleobj.ArticleContent,
                    ArticleCreatedDate = articleobj.ArticleCreatedDate,
                    ArticleDeletedDate = articleobj.ArticleDeletedDate,
                    ArticleId = articleobj.ArticleId,
                    ArticleThumbNail = thumbnailfileName,
                    IsArticleDeleted = articleobj.IsArticleDeleted
                    });
                }
                else
                {
                    _context.AllArticles.Update(new Article
                    {
                        ArticleTitle = articleobj.ArticleTitle,
                        ArticleCategory = articleobj.ArticleCategory,
                        ArticleContent = articleobj.ArticleContent,
                        ArticleCreatedDate = articleobj.ArticleCreatedDate,
                        ArticleDeletedDate = articleobj.ArticleDeletedDate,
                        ArticleId = articleobj.ArticleId,
                        ArticleThumbNail = thumbnailfileName,
                        IsArticleDeleted = articleobj.IsArticleDeleted
                    });
                }

                _context.SaveChanges();
                TempData["ResultOk"] = "Record Added Successfully !";
                return RedirectToAction("CreateNewArticle");
            }
            else
            {
                return View(articleobj);

            }
        }
    }
}
