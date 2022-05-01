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
    public class PaymentsController : Controller
    {
        private readonly Data.InventorySystemContext _context;

        public PaymentsController(Data.InventorySystemContext context)
        {
            _context = context;
        }

        [Authorize]
        // GET: Payments
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "payment_type" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var payment = from s in _context.Payment
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                payment = payment.Where(s => s.Payment_Type.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "payment_type":
                    payment = payment.OrderByDescending(s => s.Payment_Type);
                    break;
                default:
                    payment = payment.OrderBy(s => s.Payment_Type);
                    break;
            }
            int pageSize = 10;
            return View(await PaginatedList<Payment>.CreateAsync(payment.AsNoTracking(), pageNumber ?? 1, pageSize));

            //return View(await categories.AsNoTracking().ToListAsync());
        }

        [Authorize]
        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .FirstOrDefaultAsync(m => m.Bill_Number == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        [Authorize]
        // GET: Payments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Bill_Number,Payment_Type,Other_Details")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(payment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

        [Authorize]
        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Bill_Number,Payment_Type,Other_Details")] Payment payment)
        {
            if (id != payment.Bill_Number)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.Bill_Number))
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
            return View(payment);
        }

        [Authorize]
        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .FirstOrDefaultAsync(m => m.Bill_Number == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.Bill_Number == id);
        }

        [HttpPost]
        public IActionResult Export()
        {
            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[3] { new DataColumn("Bill_Number"),
                                        new DataColumn("Payment_Type"),
                                        new DataColumn("Other_Details") });

            var payments = from payment in this._context.Payment.Take(10)
                             select payment;

            foreach (var payment in payments)
            {
                dt.Rows.Add(payment.Bill_Number, payment.Payment_Type, payment.Other_Details);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Payments.xlsx");
                }
            }
        }
    }
}
