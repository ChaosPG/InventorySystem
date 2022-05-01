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
    public class OrdersController : Controller
    {
        private readonly Data.InventorySystemContext _context;

        public OrdersController(Data.InventorySystemContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Orders
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var order = from s in _context.Orders.Include(p => p.Customer)
                        select s;

            switch (sortOrder)
            {
                case "Date":
                    order = order.OrderBy(s => s.Date_of_Order);
                    break;
                case "date_desc":
                    order = order.OrderByDescending(s => s.Date_of_Order);
                    break;
                default:
                    order = order.OrderByDescending(s => s.Date_of_Order);
                    break;
            }

            var inventorySystemContext = _context.Orders.Include(p => p.Customer);
            int pageSize = 10;
            return View(await PaginatedList<Orders>.CreateAsync(order.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [Authorize]
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Order_ID == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        [Authorize]
        // GET: Orders/Create
        public IActionResult Create()
        {
            //ViewData["Customer_ID"] = new SelectList(_context.Customer, "Customer_ID", "Customer_ID");
            ViewData["Customer_ID"] = new SelectList(_context.Customer, "Customer_ID", "Customer_FirstName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Order_ID,Date_of_Order,Order_Details,Customer_ID")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Customer_ID"] = new SelectList(_context.Customer, "Customer_ID", "Customer_ID", orders.Customer_ID);
            return View(orders);
        }

        [Authorize]
        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            ViewData["Customer_ID"] = new SelectList(_context.Customer, "Customer_ID", "Customer_FirstName", orders.Customer_ID);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Order_ID,Date_of_Order,Order_Details,Customer_ID")] Orders orders)
        {
            if (id != orders.Order_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.Order_ID))
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
            ViewData["Customer_ID"] = new SelectList(_context.Customer, "Customer_ID", "Customer_ID", orders.Customer_ID);
            return View(orders);
        }

        [Authorize]
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.Order_ID == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.Order_ID == id);
        }

        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Order_ID"),
                                        new DataColumn("Date_of_Order"),
                                        new DataColumn("Order_Details"),
                                        new DataColumn("Customer_ID") });

            var orders = from order in this._context.Orders.Take(10)
                             select order;

            foreach (var order in orders)
            {
                dt.Rows.Add(order.Order_ID, order.Date_of_Order, order.Order_Details,order.Customer_ID);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx");
                }
            }
        }
    }
}
