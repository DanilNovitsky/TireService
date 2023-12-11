using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TireServiceWeb.Context;
using TireServiceWeb.Models;

namespace TireServiceWeb.Controllers
{
    public class TireController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<TireController> _logger;

        public TireController(ApplicationDbContext db, ILogger<TireController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: /Tire
        public async Task<IActionResult> Index(int? tireShopId)
        {
            _logger.LogInformation("tireShop ID Index: {tireShopId}", tireShopId);
            IEnumerable<Tire> tires = tireShopId.HasValue
                ? await _db.Tires.Include(t => t.TireShop).Where(t => t.TireShopId == tireShopId.Value).ToListAsync()
                : await _db.Tires.Include(t => t.TireShop).ToListAsync();
            TempData["tireShopId"] = tireShopId;
            return View(tires);
        }

        // GET: /Tire/Create
        public IActionResult Create(int tireShopId)
        {
            _logger.LogInformation("Customer ID Create Get: {customerId}", tireShopId);
            ViewBag.CustomerId = tireShopId;
            return View(new Tire { TireShopId = tireShopId });
        }

        // POST: /Tire/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tire tire)
        {
            if (ModelState.IsValid)
            {
                _db.Tires.Add(tire);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tire);
        }

        // GET: /Tire/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var tire = await _db.Tires.FindAsync(id);
            if (tire == null)
            {
                return NotFound();
            }

            return View(tire);
        }

        // POST: /Tire/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Tire tire)
        {
            if (ModelState.IsValid)
            {
                _db.Tires.Update(tire);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tire);
        }

        // GET: /Tire/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var tire = await _db.Tires.Include(t => t.TireShop).FirstOrDefaultAsync(t => t.Id == id);
            if (tire == null)
            {
                return NotFound();
            }

            return View(tire);
        }

        // POST: /Tire/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int id)
        {
            var tire = await _db.Tires.FindAsync(id);
            if (tire == null)
            {
                return NotFound();
            }

            _db.Tires.Remove(tire);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: /Tire/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var tire = await _db.Tires
                .Include(t => t.TireShop)
                .ThenInclude(ts => ts.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tire == null)
            {
                return NotFound();
            }

            return View(tire);
        }
    }
}
