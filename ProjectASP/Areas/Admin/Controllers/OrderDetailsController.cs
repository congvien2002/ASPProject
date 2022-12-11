using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectASP.Models;

namespace ProjectASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderDetailsController : Controller
    {
        private readonly ProjectContext _context;

        public OrderDetailsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderDetails
        public async Task<IActionResult> Index()
        {
            var projectContext = _context.OrderDetails.Include(o => o.Account).Include(o => o.Product);
            return View(await projectContext.ToListAsync());
        }

        // GET: Admin/OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Account)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            List<OrderDetail> products = _context.OrderDetails.Include(o=>o.Product).Where(o => o.AccountID == orderDetail.AccountID && o.TypeOrder == orderDetail.TypeOrder).ToList();
            ViewBag.Products = products;
            float total = 0;
            foreach(OrderDetail o in products)
            {
                total += (o.Product.SalePrice > 0 ? o.Product.SalePrice : o.Product.Price) * o.Quantity;
            }
            ViewBag.Total = total;
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Create
        public IActionResult Create()
        {
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName");
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: Admin/OrderDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,AccountID,ProductID,Quantity,TypeOrder,Address,Status")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName", orderDetail.AccountID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "MadeIn", orderDetail.ProductID);
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName", orderDetail.AccountID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "MadeIn", orderDetail.ProductID);
            return View(orderDetail);
        }

        // POST: Admin/OrderDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,AccountID,ProductID,Quantity,TypeOrder,Address,Status")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderID))
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
            ViewData["AccountID"] = new SelectList(_context.Accounts, "AccountID", "AccountName", orderDetail.AccountID);
            ViewData["ProductID"] = new SelectList(_context.Products, "ProductID", "MadeIn", orderDetail.ProductID);
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetails
                .Include(o => o.Account)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: Admin/OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderDetail = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
            return _context.OrderDetails.Any(e => e.OrderID == id);
        }
    }
}
