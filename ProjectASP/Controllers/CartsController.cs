using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectASP.Models;
using Microsoft.AspNetCore.Http;

namespace ProjectASP.Controllers
{
    public class CartsController : Controller
    {
        private readonly ProjectContext _context;

        public CartsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("Customer") != null)
            {
                var projectContext = _context.Carts.Where(c => c.AccountID == HttpContext.Session.GetInt32("CustomerID")).Include(c => c.Product);
                List<Category> categories = _context.Categories.Take(7).ToList();
                ViewBag.Category = categories;
                float total = 0;
                foreach (var item in projectContext.ToList())
                {
                    total += item.Quantity * (item.Product.SalePrice == 0 ? item.Product.Price : item.Product.SalePrice);
                }
                ViewBag.Total = total;
                return View(await projectContext.ToListAsync());
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartID,ProductID,AccountID,Quantity")] Cart cart)
        {
            if(cart.AccountID == -1)
            {
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                var cart_exist = _context.Carts.Where(c => c.AccountID == cart.AccountID && c.ProductID == cart.ProductID).FirstOrDefault();
                if (cart_exist != null)
                {
                    cart_exist.Quantity += cart.Quantity;
                    _context.Update(cart_exist);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _context.Add(cart);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            };
            return RedirectToAction("Index","Products");
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName", cart.AccountID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "MadeIn", cart.ProductID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartID,ProductID,AccountID,Quantity")] Cart cart)
        {
            if (id != cart.CartID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartID))
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName", cart.AccountID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "MadeIn", cart.ProductID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            
            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartID == id);
        }

        public async Task<IActionResult> CheckOut(int id)
        {
            List<Category> categories = _context.Categories.Take(7).ToList();
            ViewBag.Category = categories;
            List<Cart> carts = _context.Carts.Where(c => c.AccountID == id).Include(c => c.Product).ToList();
            Account account = _context.Accounts.Where(a => a.AccountID == id).First();
            ViewBag.Account = account;
            float total = 0;
            foreach (var item in carts)
            {
                total += item.Quantity * (item.Product.SalePrice == 0 ? item.Product.Price : item.Product.SalePrice);
            }
            ViewBag.Total = total;
            return View(carts);
        }
        public IActionResult Pay(int AccountID,string address,int type)
        {
            List<Cart> carts = _context.Carts.Where(c => c.AccountID == AccountID).Include(c => c.Product).ToList();
            foreach(Cart c in carts)
            {
                OrderDetail o = new OrderDetail();
                o.AccountID = c.AccountID;
                o.ProductID = c.ProductID;
                o.Quantity = c.Quantity;
                o.TypeOrder = type;
                o.Address = address;
                if(type == 3)
                {
                    o.Status = 1;
                }
                else
                {
                    o.Status = 0;
                }
                _context.OrderDetails.Add(o);
               
                _context.Carts.Remove(c);

                _context.SaveChanges();
            }
            List<Category> categories = _context.Categories.Take(7).ToList();
            ViewBag.Category = categories;
            return View();
        }
    }
}
