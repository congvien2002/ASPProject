using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectASP.Models;
using X.PagedList;

namespace ProjectASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ProjectContext _context;

        public CategoriesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageItem = 9;
            var categoryContext = _context.Categories.OrderByDescending(c => c.CategoryID).ToPagedListAsync(page, pageItem);
            return View(await categoryContext);
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            int count = _context.Products.Where(p => p.CategoryID == id).Count();
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.TotalProduct = count;
            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryID,CategoryName")] Category category)
        {
            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
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
            return View(category);
        }


        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }
        public async Task<IActionResult> SortByID(int id, int page = 1)
        {
            int pageItem = 9;
            var categoryContext = await _context.Categories.OrderBy(c => c.CategoryID).ToPagedListAsync(page, pageItem);
            if (id == 0)
            {
                categoryContext = await _context.Categories.OrderByDescending(c => c.CategoryID).ToPagedListAsync(page, pageItem);
                ViewBag.Toggle = 1;
            }
            else
            {
                ViewBag.Toggle = 0;
            }
            return View("Index", categoryContext);
        }

        public async Task<IActionResult> SortByName(int id, int page = 1)
        {
            int pageItem = 9;
            var categoryContext = await _context.Categories.OrderBy(c => c.CategoryName).ToPagedListAsync(page, pageItem);
            if (id == 0)
            {
                categoryContext = await _context.Categories.OrderByDescending(c => c.CategoryName).ToPagedListAsync(page, pageItem);
                ViewBag.ToggleName = 1;
            }
            else
            {
                ViewBag.ToggleName = 0;
            }
            return View("Index", categoryContext);
        }
    }
}
