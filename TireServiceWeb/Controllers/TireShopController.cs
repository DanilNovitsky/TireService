using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TireServiceWeb.Context;
using TireServiceWeb.Models;

namespace TireServiceWeb.Controllers
{
    public class TireShopController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TireShopController> _logger;
        public TireShopController(ApplicationDbContext db, ILogger<TireShopController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: /TireShop
        public async Task<IActionResult> Index(int? customerId)
        {
            _logger.LogInformation("Customer ID Index: {customerId}", customerId);
            IEnumerable<TireShop> tireShops = customerId.HasValue
                ? await _db.TireShops.Include(t => t.Customer).Where(t => t.CustomerId == customerId.Value).ToListAsync()
                : await _db.TireShops.Include(t => t.Customer).ToListAsync();
            TempData["id"] = customerId;
            return View(tireShops);
        }

        // GET: /TireShop/Create
        public IActionResult Create(int customerId)
        {
            _logger.LogInformation("Customer ID Create Get: {customerId}", customerId);
            ViewBag.CustomerId = customerId;
            return View(new TireShop { CustomerId = customerId });
        }

        // POST: /TireShop/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TireShop tireShop)
        {
            _logger.LogInformation("Tire shop created: {Name}, Customer ID: {CustomerId}", tireShop.Name, tireShop.CustomerId);
            if (ModelState.IsValid)
            {
                _db.TireShops.Add(tireShop);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tireShop);
        }

        // GET: /TireShop/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var tireShop = await _db.TireShops.FindAsync(id);
            if (tireShop == null)
            {
                return NotFound();
            }

            return View(tireShop);
        }

        // POST: /TireShop/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TireShop tireShop)
        {
            if (ModelState.IsValid)
            {
                _db.TireShops.Update(tireShop);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tireShop);
        }

        // GET: /TireShop/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var tireShop = await _db.TireShops.Include(t => t.Customer).FirstOrDefaultAsync(t => t.Id == id);
            if (tireShop == null)
            {
                return NotFound();
            }

            return View(tireShop);
        }

        // POST: /TireShop/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int id)
        {
            var tireShop = await _db.TireShops.FindAsync(id);
            if (tireShop == null)
            {
                return NotFound();
            }

            _db.TireShops.Remove(tireShop);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
