using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace InventorySystem.Controllers
{
    public class CustomersController : Controller
    {
        private readonly Data.InventorySystemContext _context;

        public CustomersController(Data.InventorySystemContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Customers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "cus_firstname" : "";
            ViewData["CurrentFilter"] = searchString;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var customer = from s in _context.Customer
                             select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                customer = customer.Where(s => s.Customer_FirstName.Contains(searchString)
                                       || s.Customer_LastName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "cus_firstname":
                    customer = customer.OrderByDescending(s => s.Customer_FirstName);
                    break;
                case "cus_lastname":
                    customer = customer.OrderBy(s => s.Customer_LastName);
                    break;
                default:
                    customer = customer.OrderBy(s => s.Customer_FirstName);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Customer>.CreateAsync(customer.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await categories.AsNoTracking().ToListAsync());
        }

        [Authorize]
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Customer_ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [Authorize]
        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Customer_ID,Customer_FirstName,Customer_LastName,Customer_Address,Customer_Phone,Customer_Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        [Authorize]
        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Customer_ID,Customer_FirstName,Customer_LastName,Customer_Address,Customer_Phone,Customer_Email")] Customer customer)
        {
            if (id != customer.Customer_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Customer_ID))
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
            return View(customer);
        }

        [Authorize]
        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .FirstOrDefaultAsync(m => m.Customer_ID == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.Customer_ID == id);
        }

        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Customer_ID"),
                                        new DataColumn("Customer_FirstName"),
                                        new DataColumn("Customer_LastName"),
                                        new DataColumn("Customer_Address"),
                                        new DataColumn("Customer_Phone"),
                                        new DataColumn("Customer_Email") });

            var customers = from customer in this._context.Customer.Take(10)
                             select customer;

            foreach (var customer in customers)
            {
                dt.Rows.Add(customer.Customer_ID, customer.Customer_FirstName, customer.Customer_LastName, customer.Customer_Address,customer.Customer_Phone,customer.Customer_Email);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Customers.xlsx");
                }
            }
        }
    }
}
