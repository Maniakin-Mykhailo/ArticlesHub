using ArticlesHub.Data;
using ArticlesHub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ArticlesHub.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticlesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            return _context.Articles != null ?
                        View(await _context.Articles.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Articles'  is null.");
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.Include(a => a.Images)
                                                 .FirstOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Articles/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Text,Author")] Article article, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();

                foreach (var imageFile in images)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        var image = new Image
                        {
                            FileName = fileName,
                            FilePath = "/images/" + fileName,
                            ArticleId = article.Id
                        };

                        _context.Images.Add(image);
                    }
                }

                await _context.SaveChangesAsync();

                // Загрузка связанных изображений в свойство Images статьи
                article.Images = _context.Images.Where(i => i.ArticleId == article.Id).ToList();

                return RedirectToAction(nameof(Index));
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            // Получение связанных изображений статьи
            article.Images = await _context.Images.Where(i => i.ArticleId == id).ToListAsync();

            if (article.Images == null)
            {
                article.Images = new List<Image>(); // Инициализация свойства Images
            }

            return View(article);
        }

        // POST: Articles/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Text,Author")] Article article, List<IFormFile> images)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();

                    // Обработка загруженных изображений
                    foreach (var imageFile in images)
                    {
                        if (imageFile != null && imageFile.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                            using (var stream = new FileStream(imagePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }

                            var image = new Image
                            {
                                FileName = fileName,
                                FilePath = "/images/" + fileName,
                                ArticleId = article.Id
                            };

                            _context.Images.Add(image);
                        }
                    }

                    await _context.SaveChangesAsync();

                    // Загрузка связанных изображений в свойство Images статьи
                    article.Images = await _context.Images.Where(i => i.ArticleId == article.Id).ToListAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }


        // GET: Articles/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Articles == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Articles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Articles' is null.");
            }

            var article = await _context.Articles
                .Include(a => a.Images) // Включаем связанные картинки
                .FirstOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                return NotFound();
            }

            // Удаляем все связанные изображения
            foreach (var image in article.Images)
            {
                await DeleteImage(image.Id); // Вызываем метод DeleteImage для удаления каждого изображения
            }

            // Удаляем статью
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }




        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image != null)
            {
                // Удаление изображения из базы данных
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();

                // Удаление файла изображения с сервера
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", image.FileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                return RedirectToAction(nameof(Edit), new { id = image.ArticleId }); // Перенаправляем обратно на страницу редактирования статьи
            }

            return NotFound();
        }

        [AllowAnonymous]
        public IActionResult GetImage(int id)
        {
            var image = _context.Images.FirstOrDefault(i => i.Id == id);
            if (image != null)
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", image.FileName);
                return PhysicalFile(imagePath, "image/jpeg");
            }

            return NotFound();
        }
    }
}
