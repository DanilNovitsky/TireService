using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System.Collections.Generic;
using System.Diagnostics;
using TireServiceWeb.Context;
using TireServiceWeb.Models;
using TireServiceWeb.Services;
using TireServiceWeb.Utility;

namespace TireServiceWeb.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ICustomerService _customerService;

        public CustomerController(ApplicationDbContext db, ICustomerService customerService)
        {
            _db = db;
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Customer> objCustomer = await _db.Customers.ToListAsync();
            return View(objCustomer);
        }

        //GET: /Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST: /Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer obj)
        {
            if (!await _customerService.IsEmailUniqueAsync(obj.Email))
            {
                ModelState.AddModelError("email", "Email is already in use");
            }
            if (!await _customerService.IsPhoneNumberUniqueAsync(obj.PhoneNumber))
            {
                ModelState.AddModelError("phoneNumber", "Phone is already in use");
            }
            if (ModelState.IsValid)
            {
                obj.Password = HashUtility.HashPassword(obj.Password);

                _db.Customers.Add(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Customer created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        //GET: /Customer/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var customerFromDb = await _db.Customers.FindAsync(id);

            if (customerFromDb == null)
            {
                return NotFound();
            }

            return View(customerFromDb);
        }

        //POST: /Customer/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Customer obj)
        {
            if (!await _customerService.IsEmailUniqueAsync(obj.Email, obj.Id))
            {
                ModelState.AddModelError("Email", "Email is already in use");
            }
            if (!await _customerService.IsPhoneNumberUniqueAsync(obj.PhoneNumber, obj.Id))
            {
                ModelState.AddModelError("PhoneNumber", "Phone is already in use");
            }

            if (ModelState.IsValid)
            {
                obj.Password = HashUtility.HashPassword(obj.Password);

                _db.Customers.Update(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Customer updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        // GET: /Customer/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var customerFromDb = await _db.Customers.FindAsync(id);
            if (customerFromDb == null)
            {
                return NotFound();
            }

            return View(customerFromDb);
        }

        // POST: /Customer/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePOST(int? id)
        {
            var customerFromDb = await _db.Customers.FindAsync(id);
            if (customerFromDb == null)
            {
                return NotFound();
            }

            _db.Customers.Remove(customerFromDb);
            await _db.SaveChangesAsync();
            TempData["success"] = "Customer deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
