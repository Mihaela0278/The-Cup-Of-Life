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
        public async Task<IActionResult> Index()
        {
            var theCupOfLifeContext = _context.Posts.Include(p => p.Tag).Include(p => p.User);
            return View(await theCupOfLifeContext.ToListAsync());
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var post = await _context.Posts
                .Include(p => p.Tag)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name");

            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ImageUrl,Content,TagId")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Id = Guid.NewGuid();
                post.UserId = _userManager.GetUserId(User);
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", post.TagId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null || post.UserId != _userManager.GetUserId(User))
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", post.TagId);

            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,ImageUrl,Content,TagId")] Post post)
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
                    await _context.SaveChangesAsync();
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

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var post = await _context.Posts
                .Include(p => p.Tag)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null || post.UserId != _userManager.GetUserId(User))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(Guid id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
