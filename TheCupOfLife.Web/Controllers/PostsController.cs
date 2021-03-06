using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheCupOfLife.Data;
using TheCupOfLife.Data.Models;

namespace TheCupOfLife.Web.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly TheCupOfLifeContext _context;

        private readonly UserManager<User> _userManager;

        public PostsController(TheCupOfLifeContext context, UserManager<User> userManager)
        {
            _context = context;

            _userManager = userManager;

        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var theCupOfLifeContext = _context.Posts.Include(p => p.Tag).Include(p => p.User);
            return View(theCupOfLifeContext.ToList());
        }

        [AllowAnonymous]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var post = _context.Posts
                .Include(p => p.Tag)
                .Include(p => p.User)
                .FirstOrDefault(m => m.Id == id);
            if (post == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Title,ImageUrl,Content,TagId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Id = Guid.NewGuid();
                post.UserId = _userManager.GetUserId(User);
                _context.Posts.Add(post);
                _context.SaveChanges();
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", post.TagId);
            return View(post);
        }

        [HttpGet]
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var post = _context.Posts.Find(id);

            if (!(post == null || User.IsInRole(Roles.ADMIN.ToString()) || post.UserId == _userManager.GetUserId(User)))
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", post.TagId);

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,Title,ImageUrl,Content,TagId")] Post post)
        {
            if (id != post.Id)
            {
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    post.UserId = _userManager.GetUserId(User);
                    _context.Update(post);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", post.TagId);

            return View(post);
        }

        [HttpGet]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var post = _context.Posts
                .Include(p => p.Tag)
                .Include(p => p.User)
                .FirstOrDefault(m => m.Id == id);
            if (post == null || !User.IsInRole(Roles.ADMIN.ToString()))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var post = _context.Posts.Find(id);
            _context.Posts.Remove(post);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(Guid id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
