using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StackOverflow.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StackOverflow.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostsController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentuser = await _userManager.FindByIdAsync(userId);
            return View(_db.Posts.Where(x => x.User.Id == currentuser.Id));
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(int Id)
        {
            var thisPost = _db.Posts.Include(x => x.Answers)
                              .ThenInclude(r => r.User)
                              .FirstOrDefault(x => x.PostId == Id);
            return View(thisPost);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            post.User = currentUser;
            _db.Posts.Add(post);
            _db.SaveChanges();
            return RedirectToAction("Details", new {Id = post.PostId});
        }

        public IActionResult NewAnswer()
        {
            return View();
        }

    }
}
