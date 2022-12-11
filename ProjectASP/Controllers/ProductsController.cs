using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectASP.Models;

namespace ProjectASP.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProjectContext _context;


        public ProductsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(int page = 1)
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Category = categories;
            var projectContext = _context.Products.Include(p => p.Category).Skip((page - 1) * 9).Take(9);
            double totalPage = _context.Products.Count() / 9;
            if(_context.Products.Count() - totalPage*9 > 0)
            {
                totalPage += 1;
            }
            ViewBag.TotalPage = totalPage;
            ViewBag.CurrentPage = page;
            return View(await projectContext.ToListAsync());
        }

        public async Task<IActionResult> GetbyCategory(int? id)
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Category = categories;
            List<Product> products = await _context.Products.Where(p => p.CategoryID == id).ToListAsync();
            return View("Index",products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Category = categories;
            List<Product> products = _context.Products.Where(p=>p.CategoryID == product.CategoryID).OrderBy(p => p.ProductID).Skip(0).Take(5).ToList();
            ViewBag.Related = products;
            ViewBag.Product = product;
            return View();
        }

        public async Task<IActionResult> AddCart([Bind("product_id","quantity","account_id")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
            }
                return RedirectToAction("Index");
        }

        
        public IActionResult Search(string search)
        {
            List<Product> products = _context.Products.Where(p => p.ProductName.Contains(search)).ToList();
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Category = categories;
            return View("Index", products);
        }

        public IActionResult FilterByPrice(int type)
        {
            List<Product> products;
            if (type == 1)
            {
                products = _context.Products.Where(p => (p.SalePrice > 0 ? p.SalePrice : (p.SalePrice > 0 ? p.SalePrice : p.Price ) ) > 0 && (p.SalePrice > 0 ? p.SalePrice : (p.SalePrice > 0 ? p.SalePrice : p.Price )) <= 3000000).ToList();
            }
            else if(type == 2)
            {
                products = _context.Products.Where(p => (p.SalePrice > 0 ? p.SalePrice : (p.SalePrice > 0 ? p.SalePrice : p.Price )) > 3000000 && (p.SalePrice > 0 ? p.SalePrice : (p.SalePrice > 0 ? p.SalePrice : p.Price )) <= 5000000).ToList();
            }
            else if (type == 3)
            {
                products = _context.Products.Where(p => (p.SalePrice > 0 ? p.SalePrice : p.Price ) > 5000000 && (p.SalePrice > 0 ? p.SalePrice : p.Price ) <= 10000000).ToList();
            }
            else if (type == 4)
            {
                products = _context.Products.Where(p => (p.SalePrice > 0 ? p.SalePrice : p.Price ) > 10000000 && (p.SalePrice > 0 ? p.SalePrice : p.Price ) <= 15000000).ToList();
            }
            else 
            {
                products = _context.Products.Where(p => (p.SalePrice > 0 ? p.SalePrice : p.Price ) > 15000000).ToList();
            }
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Category = categories;
            return View("Index",products);
        }

        public IActionResult OrderBy(string orderby)
        {
            List<Product> products;
            if(orderby.Equals("name-ascending"))
            {
                products = _context.Products.OrderBy(p => p.ProductName).ToList();
            }else if (orderby.Equals("name-descending"))
            {
                products = _context.Products.OrderByDescending(p => p.ProductName).ToList();
            }
            else if (orderby.Equals("price-ascending"))
            {
                products = _context.Products.OrderBy(p => (p.SalePrice > 0 ? p.SalePrice : p.Price )).ToList();
            }
            else if (orderby.Equals("price-descending"))
            {
                products = _context.Products.OrderByDescending(p => (p.SalePrice > 0 ? p.SalePrice : p.Price )).ToList();
            }
            else if (orderby.Equals("created-descending"))
            {
                products = _context.Products.OrderByDescending(p => p.DayMaking).ToList();
            }
            else if (orderby.Equals("created-ascending"))
            {
                products = _context.Products.OrderBy(p => p.DayMaking).ToList();
            }
            else if (orderby.Equals("best-sale"))
            {
                products = _context.Products.Where(p => p.SalePrice > 0).OrderByDescending(p => (p.SalePrice > 0 ? p.SalePrice : p.Price ) - p.SalePrice).ToList();
            }
            else
            {
                products = _context.Products.ToList();
            }
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Category = categories;
            return View("Index",products);
        }
    }
}
